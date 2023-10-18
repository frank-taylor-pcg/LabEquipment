This project is my experiments with build a machine simulator for us using Unity. It started when I was stuck in meetings for the bulk of a day (~6 hours that day) and was bored out of my mind, but wasn't able to focus on other work. So I started mocking out the shape of a cart for the offloader. Then inspiration struck and I spent the last 2 hours of that day and most of the next 12 hours not sleeping as I started figuring this stuff out.

## Scenes
Each scene has different components I was playing with. I've listed the scenes and any relevant information you'll need for each one below

### SampleScene
That scene doesn't really have any functionality at the moment and just has the beginnings of a rough mockup of a tray cart and the outline of an offloader.

### ScannerTest
This scene contains my initial test of the barcode scanning functionality. Movement of the tray elevator is fixed and the results of barcode scans are printed to the console. I also have a tower light and the light up button components in this scene with some random pulses being sent to them to test them.

### AxisTesting
This is the most complicated scene. This one creates stages that can have teachpoints set for them to allow us to effectively emulate real world linear stages. They can be communicated with via the on-screen buttons or if you have an MQTT broker running on your machine you can communicate with the stages via MQTT messages.  I'm running Mosquitto because that's what the field team was using the last time I spoke with them. Any broker should work, however.

## Setting up node-red to send messages
To communicate with this scene via MQTT you can use node-red to send the messages. I've included a folder with a JSON export of a very simple flow that sends the appropriate messages.  You should be able to import the flow into node-red and get it working in seconds.  If that doesn't work, however you can recreate the flow very quickly. I've included a step-by-step below, but I recommend working through the following segments of the node-red documentation first to avoid wasting time:

- [Getting Started](https://nodered.org/docs/getting-started/)
- [Running locally](https://nodered.org/docs/getting-started/local)
- [Creating your first flow](https://nodered.org/docs/tutorials/first-flow)

## Creating the node-red Test Axis Stages flow from scratch
Start by adding 4 `inject` nodes.  For each `inject` node, double click it to open the properties and change the `msg.payload type` to `string` by clicking the dropdown on the right-side box (it looks like msg.payload = [Click this box]). Set the values to the 4 `inject nodes` to "Home", "TeachPoint1", "TeachPoint2", and "MaxExtents" (case-insensitive).

Next drag in an `mqtt out` node. Double-click it to open the properties and set your server to `localhost:1883` (the port may vary based on your broker). The topic should be set to `unity/machine` (case sensitive I believe). You can change the name if you like, it just changes how the node looks in the flow editor.

Once this is done wire all 4 `inject` nodes outputs to the input of the `mqtt out` node.  Deploy your changes with the red Deploy button in the upper right and you're good to go. With node-red and the AxisTesting scene running, you can click the square buttons on the left side of the `inject` nodes to send messages to MQTT that get picked up by the MqttCoordinator object in Unity. MqttCoordinator then dispatches messages to each stage telling it to move to the appropriate teachpoint.