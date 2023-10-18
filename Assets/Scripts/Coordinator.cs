using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MQTTnet.Client;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

public class Coordinator : MonoBehaviour
{
	private const string MqttClientAlreadyConnected = "MQTT client already connected";
	private const string MqttClientFailedToInitialize = "Failed to initialize MQTT client";
	private const string MqttClientFailedToConnect = "Failed to connect to MQTT broker";
	private const string MqttClientSetUpSuccessfully = "MQTT client set up successfully";
	
	ManagedMqttClient _client = new ManagedMqttClient();
	
	private readonly List<string> _subscribeTopics = new()
	{
		"unity/machine",
	};
	
	// TODO: There HAS to be a better way to do this -- some kind of NamedEvent class, but how can I make that editable in the Inspector window?
	public UnityEvent HomeEvents;
	public UnityEvent TeachPoint1Events;
	public UnityEvent TeachPoint2Events;
	public UnityEvent MaxExtentEvents;
	
	private string topic = null;
	private string message = null;
	
	// Start is called before the first frame update
	void Start()
	{
		print(SetupMqtt());
	}
	
	// Update is called once per frame
	void Update()
	{
		if (message == null) return;
		
		switch (message)
		{
			case "Home":
			{
				print("Move to Home");
				HomeEvents.Invoke();
				break;
			}
			case "TeachPoint1":
			{
				print("Move to teachpoint 1");
				TeachPoint1Events.Invoke();
				break;
			}
			case "TeachPoint2":
			{
				print("Move to teachpoint 2");
				TeachPoint2Events.Invoke();
				break;
			}
			case "MaxExtent":
			{
				print("Move to max extents");
				MaxExtentEvents.Invoke();
				break;
			}
			default:
			{
				print($"Received: {message} from {topic}");
				break;
			}
		}
		// Reset the message so we don't keep firing the events
		message = null;
	}
	
	// These allow the UI to move the stages through the coordinator
	public void MoveToMaxExtents() => MaxExtentEvents.Invoke();
	public void MoveToTeachPoint1() => TeachPoint1Events.Invoke();
	public void MoveToTeachPoint2() => TeachPoint2Events.Invoke();
	public void MoveToHome() => HomeEvents.Invoke();
	
	// Handles the MQTT client setup and returns a message indicating the success or failure
	private string SetupMqtt()
	{
		if (_client.IsConnected) return MqttClientAlreadyConnected;

		if (!_client.Initialize()) return MqttClientFailedToInitialize;

		_client.ConnectWithTimeout().Wait();
		if (!_client.IsConnected) return MqttClientFailedToConnect;

		_client.SetMessageHandler(HandleMessage);
		_subscribeTopics.ForEach(_client.Subscribe);
		
		return MqttClientSetUpSuccessfully;		
	}
	
	private Task HandleMessage(MqttApplicationMessageReceivedEventArgs e)
	{
		message = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment.Array);
		string topic = e.ApplicationMessage.Topic;
		
		return Task.CompletedTask;
	}
}
