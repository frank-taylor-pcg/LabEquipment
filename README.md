# LabEquipment
A Unity simulation for various lab equipment

# Current structure
Anything I don't mention was pulled in automatically during setup of one thing or another

### Materials
Collection of materials I created to texture things

### Prefabs
Collection of reusable components
  
### Scenes
Collection of scenes

#### SampleScene
Contains results of my boredom during a day of nothing but meetings. I mocked out a cart, complete with trays and 'barcodes' and I was starting to greybox out the frame. Contains no actual functionality.

#### Scenes/ScannerTest
The beginnings of a tray elevator, complete with a stack of 25 barcodes to scan.  Not currently functional as I was just moving it up but wanted to flesh out the actual 'stage' or 'axis' component to use here.  Also contains prefabs for the tower safety light and the door request and system reset buttons.

#### Scenes/AxisTesting
This is the most complicated scene. This one creates stages that can have teachpoints set for them to allow us to effectively emulate real world linear stages. They can be communicated with via the on-screen buttons or if you have an MQTT broker running on your machine you can communicate with the stages via MQTT messages.  I'm running Mosquitto because that's what the field team was using the last time I spoke with them. Any broker should work, however.

### Shaders
A place for shaders that will be used to render special effects. The current shader I'm working on is a text rendering shader to show the text value of the barcodes

### Textures
Currently only contains a bitmap font I plan to use with the text shader

### Setting up node-red to send messages
To communicate with this scene via MQTT you can use node-red to send the messages. I've included a folder with a JSON export of a very simple flow that sends the appropriate messages.  You should be able to import the flow into node-red and get it working pretty quickly.  If that doesn't work, however you can recreate the flow very quickly by adding 4 Inject nodes.  For each Inject node, double click it to open the properties and change the msg.payload type to string by clicking the dropdown on the right-side box (it looks like msg.payload = [Click this box]). Set the values to the 4 inject nodes to "Home", "TeachPoint1", "TeachPoint2", and "MaxExtents" (case-insensitive).
