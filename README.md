# c3IDE - Construct3 Plugin IDE

This is a project I have been working on for a while, it's purpose is to help me maintain and speed up the development of c3 addons. it's not meant to be a easy plugin maker, it just organizes the workflow of making addons in a different way. it's still missing alot of features, a modern code editor has, but I am planning to keep improving it, and adding things like syntax highlight, linting, and other specific thing more geared towards the c3 SDK. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-preview.gif)
*creating a simple logging plugin and running it in construct3*

# Installation

You can get the setup exe ![HERE](https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe), This will install the latest version, When you open the app, if there is a new update you will be promted with an update. 

# Current state

The project is really close to being complete for thes first stage, Only about 20% of the SDK is missing from the auto completion list. Most of the features are already implemented, there is only a bit of polish left like adding context menu and shortcuts to improve workflow. And i am also missing documentation and examples projects. There are a few things i will work on after the 1.0 release, such as themes, and other plugin export types such as effect/ custom importers. 

#Auto Completion 

I take a niave approach to auto completion, There are 2 flavors of auto completion mixed in, The first is user based auto completion (think sublime text), all user entered tokens are added to the completion list with a low piority. The second flavor of auto completion is scrapped data from the construct 3 documentation based on the defined interfaces. what that means is I parse the document for any of the defined types used in construct (ex. IInstanceBase, SDK, C3...) then from this types you will get a set of methods and properties that would be available, we also don't just look for interfaces we also look at method calls, and we have a mapping that based on those call returns a list of interfaces for the return types and parameters. so calling GetObjectType() expose all the members of IObjectType. this approach is naive becuase there is no reflection or AST to infer types. if one of the token is found on the document, we just populate the list of completions, I have tried to add as much documentation to the tool tip to help associate the call with the proper interface, but if you are not fimilar with the C3 documentation you could run into issue, make sure the object you are using to call the method actually has that method exposed.        

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-auto-complete.gif)
*context based auto completion*

# Import / Export Projects

You can export you projects, by default it will get stored in *C:\Users\{USER}\AppData\Roaming\C3IDE_DATA\Exports*
You can import projects by dragging the .c3ide file on the dashboard window.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-import-export.gif)
*importing an exported .c3ide file into the app*

# Example Addon Projects 

* ![Simple Logging Plugin ](https://github.com/armandoalonso/c3IDE/blob/master/doc/examples_projects/LogPlugin.c3ide)

