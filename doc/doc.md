#c3IDE Documentation 

## Introduction
c3IDE is a Construct3 plugin development tool. It lets you you quickly iterate and test your plugins. And there are a few improvment to help speed up your workflow.

## Setup & Installation
You can download and install the application from this link [https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe](https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe "Setup & Install"). The application is hosted on Github. The application updates automaticlly when a new version is detected.

## Application Layout
The application is composed of multiple windows, You can navigate to the multiple windows using the menu on the top left corner. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-mainmenu.gif)

##Dashboard

The Dashboard is the main window that starts up when you first load load application, From this window you can create, manage, and export all the addons you have created with c3IDE.

### Creating a new addon

To create a new addon, you have to fill out the information under Addon Data, and then press the "Create Addon" button.

- ** Addon Name** : This is the name of the Addon, This value can include spaces. 
- **Addon Class Name**: This is one part of the the Addon Identifier and is used to prefix a few of the javascript class. This value cannot have any spaces
- **Company Name**: This is the name of your company. This value will be combined with the class name to form the addon's ID *Company_Class*. 
- **Author** - Is the name of the author that will be placed in the Addon.json files
- **Version** - Is the current version of your addon.
- **Addon Type** - This will control what template will be used in creating the addon. You can choose from *SingleGlobal*, *MultiInstance*, *Drawing*, *Behavior*
- **Addon Icon** - This is an svg file that will be used as the addon icon inside of construct 3. You can change the addon by dragging an svg file over the existing icon.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-change-icon.gif)

### Loading an existing addon

In order to start editing an addon, it must be loaded there are two ways you can load load an exisiting add.on 

- You can select the addon from the addon list, and click on the "Load Addon" button.
- You can double click on an addon from the addon list

### Importing / Exporting your project

You can Import or Export your project,  this create a *.c3ideproj* file, which allows to you archieve different version of a plugin you are working on. It also allows you to easily transfer your projects across computers. 

##### Export
You can export a project by selecting the project in the addon list and clicking on the "Export Addon" button. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-export-project.gif)

When you export a project by default it will be saved to your *\AppData\Roaming\C3IDE_DATA\Exports* folder, this can be changed in the options.  The name of the exported file will be the defined class followed by a datetime stamp.

##### Import
The only way to Import a project, is by dragging the *.c3ideproj* file into the addon list. You can click on the "Open Export Folder" to quickly navigate to the you specificed Export Location.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-import-project.gif)

### Creating the final c3addon file
From the dashboard you can package you addon into the c3addon file format used by construct to install addons. You need to select the addon from the addon list, and then click the "Create C3Addon File" button. This process will compile you addon and generate the files needed. Inorder to make sure you can create a valid c3addon file, Test out your addon from the Test Window. In the Testing Window it will compile your addon and give you feedback on the process and if any failures occured. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-creating-c3addon.gif)

When you create you c3addon file, It will be saved in *AppData\Roaming\C3IDE_DATA\C3Addons* folder. The c3addon file name will contain the class name followed by the version.


