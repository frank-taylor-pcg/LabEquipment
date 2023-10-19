using System;
using System.Threading;
using System.Threading.Tasks;

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

public class ManagedMqttClient
{
	private MqttFactory? _factory;
	private IMqttClient? _client;
	private MqttClientOptions? _options;
	
	// I modified the Console.WriteLines I had in here to just add to this. I'm not sure the best way to handle that,
	// but at least this way the message is viewable if I need it.
	public string Message { get; set; }

	public bool Initialize()
	{
		_factory = new MqttFactory();
		_client = _factory.CreateMqttClient();
		_options = new MqttClientOptionsBuilder()
			.WithTcpServer("127.0.0.1")
			.Build();

		return _factory != null && _client != null && _options != null;
	}

	public bool IsConnected => _client?.IsConnected ?? false;

	public Task<MqttClientConnectResult> ConnectWithTimeout(int milliseconds = 250)
	{
		// How do I cleanly exit this and report a failure?
		if (_client is null || _options is null)
			return Task.FromResult(new MqttClientConnectResult());

		try
		{
			using(CancellationTokenSource timeoutToken = new CancellationTokenSource(TimeSpan.FromMilliseconds(milliseconds)))
			{
				return _client.ConnectAsync(_options, timeoutToken.Token);
			}
		}
		catch (OperationCanceledException)
		{
			Message = "Timeout when connecting to MQTT broker";
		}

		return Task.FromResult(new MqttClientConnectResult());
	}

	public void SetMessageHandler(Func<MqttApplicationMessageReceivedEventArgs, Task> del)
	{
		if (_client is null) return;

		_client.ApplicationMessageReceivedAsync += del;
	}

	public async void Publish(string topic, string msg)
	{
		if (!IsConnected)
		{
			Message = "Can't publish a message because the MQTT client is not connected!";
			return;
		}

		MqttApplicationMessage? message = new MqttApplicationMessageBuilder()
			.WithTopic(topic)
			.WithPayload(msg)
			.WithQualityOfServiceLevel(MqttQualityOfServiceLevel.ExactlyOnce)
			.Build();

		await _client!.PublishAsync(message, CancellationToken.None);
	}

	public async void Subscribe(string topic)
	{
		if (_factory is null) return;

		MqttClientSubscribeOptions? subscribeOptions = _factory!
			.CreateSubscribeOptionsBuilder()
			.WithTopicFilter(x => x.WithTopic(topic))
			.Build();

		Task<MqttClientSubscribeResult> result = _client!.SubscribeAsync(subscribeOptions, CancellationToken.None);

		await result;

		Message = $"MqttClient successfully subscribed to the [{topic}] topic";
	}
}
