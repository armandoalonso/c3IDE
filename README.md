# c3IDE - Construct3 Plugin IDE

This is a project I have been working on for a while, it's purpose is to help me maintain and speed up the development of c3 addons. it's not meant to be a easy plugin maker, it just organizes the workflow of making addons in a different way. it's still missing alot of features, a modern code editor has, but I am planning to keep improving it, and adding things like syntax highlight, linting, and other specific thing more geared towards the c3 SDK. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-preview.gif)
*creating a simple logging plugin and running it in construct3*

# Current state

As of right now only Single Global Plugins are support, this will change in the future when i get around to adding support for it. There is an embedded web server which takes care of hosting the plugin for you. It's still not ready for an alpha realease. you could fork the repo and compile it, to test it out. When i think it's in a good state, I will create a release for it, and open it up to pull request if people want to improve on it. I plan on writing some more documentation on the specifics of teh IDE and i plan on converting some of my addon's so they can be used a reference.   

# Known Issues

-[ ] aces allow creation of params before an action is created, this crashes the app
