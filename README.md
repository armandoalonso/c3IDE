# c3IDE - Construct3 Plugin IDE

This is a project I have been working on for a while, it's purpose is to help me maintain and speed up the development of c3 addons. it's not meant to be a easy plugin maker, it just organizes the workflow of making addons in a different way. it's still missing alot of features, a modern code editor has, but I am planning to keep improving it, and adding things like syntax highlight, linting, and other specific thing more geared towards the c3 SDK. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-preview.gif)
*creating a simple logging plugin and running it in construct3*

# Installation

You can get the setup exe ![HERE](https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe), This will install the latest version, When you open the app, if there is a new update you will be promted with an update. 

# Current state

As of right now only Single Global Plugins are support, this will change in the future when i get around to adding support for it. There is an embedded web server which takes care of hosting the plugin for you. There is currently an alpha version which I will be updating regularly. The major things i have left todo is add the rest of the templates, and finish the auto completion, there are also a few small options i want to look into like themes for the editors. but before that i want to get all the functionaility in.

# Import / Export Projects

You can export you projects, by default it will get stored in *C:\Users\{USER}\AppData\Roaming\C3IDE_DATA\Exports*
You can import projects by dragging the .c3ide file on the dashboard window.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-import-export.gif)
*importing an exported .c3ide file into the app*

# Example Addon Projects 

* ![Simple Logging Plugin ](https://github.com/armandoalonso/c3IDE/blob/master/doc/examples_projects/LogPlugin.c3ide)

