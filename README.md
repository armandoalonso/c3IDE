# c3IDE - Construct 3 Plugin Development Environment 

## Introduction
c3IDE is a Construct3 plugin development tool. It lets you you quickly iterate and test your plugins. And there are a few improvment to help speed up your workflow.

## Setup & Installation
You can download and install the application from this link [https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe](https://github.com/armandoalonso/c3IDE/raw/master/c3IDE/publish/setup.exe "Setup & Install"). The application is hosted on Github. The application updates automaticlly when a new version is detected.

## Change Log
You can see detailed change log here : [https://github.com/armandoalonso/c3IDE/blob/master/CHANGELOG.md](https://github.com/armandoalonso/c3IDE/blob/master/CHANGELOG.md "Change Log").

## Application Layout
The application is composed of multiple windows, You can navigate to the multiple windows using the menu on the top left corner. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-mainmenu.gif)

## Sample Projects
I have include a few sample projects that you can import into the application, to see how the add is layed out. These exported projects and be found *AppData\Roaming\C3IDE_DATA\Exports* folder they will be prefixed with Example_, I will be adding more examples as I keep updating the application.  

## Dashboard

The Dashboard is the main window that starts up when you first load load application, From this window you can create, manage, and export all the addons you have created with c3IDE.

### Creating a new addon

To create a new addon, you have to fill out the information under Addon Data, and then press the "Create Addon" button.

- **Addon Name** : This is the name of the Addon, This value can include spaces. 
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

#### Export
You can export a project by selecting the project in the addon list and clicking on the "Export Addon" button. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-export-project.gif)

When you export a project by default it will be saved to your *\AppData\Roaming\C3IDE_DATA\Exports* folder, this can be changed in the options.  The name of the exported file will be the defined class followed by a datetime stamp.

#### Import
The only way to Import a project, is by dragging the *.c3ideproj* file into the addon list. You can click on the "Open Export Folder" to quickly navigate to the you specificed Export Location.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-import-project.gif)

### Creating the final c3addon file
From the dashboard you can package you addon into the c3addon file format used by construct to install addons. You need to select the addon from the addon list, and then click the "Create C3Addon File" button. This process will compile you addon and generate the files needed. Inorder to make sure you can create a valid c3addon file, Test out your addon from the Test Window. In the Testing Window it will compile your addon and give you feedback on the process and if any failures occured. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-creating-c3addon.gif)

When you create you c3addon file, It will be saved in *AppData\Roaming\C3IDE_DATA\C3Addons* folder. The c3addon file name will contain the class name followed by the version.

## Addon
The addon window contains 2 tabs, One tab manages the addon.json which contains all the metadata about your plugin. The other tab is the Third Party Files tab. You can you the tab to add external librarys or your own extra javascript files.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-addon-format.gif)

You can add extra javascript files to your peoject by clicking on the "Add New File" button, Or dragging a js file over the Third Party File list. Adding (or removing) a  javascript file will automatcially update the addon.json file

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-thirdparty-js.gif)

##Plugin / Behavior
The plugin/behavior will contain 2 tabs, each tab will manage the plugin.js/behavior.js files in the addon. The edit time tab manages the file on the root of the addon, and the run time tab manages the file in the c3runtime folder.

### Edit time
In the edit time plugin file you can specify the plugin properties. A qucik way to create plugin properties is to right click and choose "Insert New Plugin Property" this will bring up the property wizard which will guide you through creating a new property.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-new-prop.gif)

If you have added extra javascript files to your project, you can right click and choose "Generate File Dependeny" to generate the correct code to link those added files to your addon.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-file-dep.gif)

### Run time
The run time tab will contain allow you to edit the run time plugin file. 

## Type & Instance 
Both the type and instance window allow you to edit the edit time and run time files for both the type.js and the instance.js.  These editors have auto completion and basic syntax highlighting for javascript. 

## ACEs
The ACEs windows let you edit the actions, conditions, and expressions for your addon. These windows are divided into 4 sections, You have ace list which let's you navigate your defined aces, you have the ACE.json view which contains the section of json for ACE.json file, the Language section which contains the language portion of your addon, and the Code view which contains the implementation of your currently selected ace.

### Navigation
Using the navigation button above the Ace List, you can expand and collapse the different sections of the editors

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-ace-nav.gif)

### Creating New Action
To create a new action, you can click on the "Add New Action" button below the the Action List (or you can right click on the ace list, and select create "Create New Action"). This will bring up the Action Creation Wizard. The wizard will guide you through setting up a new action.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-add-act.gif)

- **Action ID** : This is a unique identifier for the action, it cannot have any spaces, all spaces will be converted into dashes automactially.
- **Action Category** : This will be the category used to group your action. You can change the category by right clicking on the action in the action list and selecting "Change Category". In the lanagauge window you will be able to assign a description to this category.   
- **Action List Name** : This is the text that will show up in construct when browsing the aces of your addon.
- **Highlight** : Set to true to highlight the ACE in the condition/action/expression picker dialogs. This should only be used for the most regularly used ACEs, to help users pick them out from the list easily.
- **Display Text** : the text that appears in the event sheet. You can use simple BBCode tags like [b] and [i], and use {0}, {1} etc. as parameter placeholders. (There must be one parameter placeholder per parameter.) For behaviors only, the placeholder {my} is substituted for the behavior name and icon.
- **Description** : a description of the action or condition, which appears as a tip at the top of the condition/action picker dialog.

### Creating New Condition
To create a new condition, you can click on the "Add New condition" button below the the condition List (or you can right click on the ace list, and select create "Create New condition"). This will bring up the condition Creation Wizard. The wizard will guide you through setting up a new condition.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-add-cnd.gif)

- **Condition ID** : This is a unique identifier for the condition, it cannot have any spaces, all spaces will be converted into dashes automactially.
- **Condition Category** : This will be the category used to group your condition. You can change the category by right clicking on the condition in the condition list and selecting "Change Category". In the lanagauge window you will be able to assign a description to this category.   
- **Condition List Name** : This is the text that will show up in construct when browsing the aces of your addon.
- **Highlight** : Set to true to highlight the ACE in the action/condition/expression picker dialogs. This should only be used for the most regularly used ACEs, to help users pick them out from the list easily.
- **Is Static** : Normally, the condition runtime method is executed once per picked instance. If the condition is marked static, the runtime method is executed once only, on the object type class. This means the runtime method must also implement the instance picking entirely itself, including respecting negation and OR blocks.
- **Is Trigger** : Specifies a trigger condition. This appears with an arrow in the event sheet. Instead of being evaluated every tick, triggers only run when they are explicity triggered by a runtime call.
- **Is FakeTrigger** : Specifies a fake trigger. This appears identical to a trigger in the event sheet, but is actually evaluated every tick. This is useful for conditions which are true for a single tick, such as for APIs which must poll a value every tick
- **Is Looping** : Display an icon in the event sheet to indicate the condition loops. This should only be used with conditions which implement re-triggering
- **Is Invertible** : Allow the condition to be inverted in the event sheet. Set to false to disable invert.
- **Is Compatible With Trigger** : Allow the condition to be used in the same branch as a trigger. Set to false if the condition does not make sense when used in a trigger, such as the Trigger once condition.
- **Display Text** : the text that appears in the event sheet. You can use simple BBCode tags like [b] and [i], and use {0}, {1} etc. as parameter placeholders. (There must be one parameter placeholder per parameter.) For behaviors only, the placeholder {my} is substituted for the behavior name and icon.
- **Description** : a description of the condition or condition, which appears as a tip at the top of the condition/action picker dialog.

### Creating New Expression
To create a new expression, you can click on the "Add New expression" button below the the expression List (or you can right click on the ace list, and select create "Create New expression"). This will bring up the expression Creation Wizard. The wizard will guide you through setting up a new expression.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-add-exp.gif)

- **Expression ID** : This is a unique identifier for the expression, it cannot have any spaces, all spaces will be converted into dashes automactially.
- **Expression Category** : This will be the category used to group your expression. You can change the category by right clicking on the expression in the expression list and selecting "Change Category". In the lanagauge window you will be able to assign a description to this category.   
- **Return Type** : One of "number", "string", "any". The runtime function must return the corresponding type, and "any" must still return either a number or a string.
- **Is Variadic Parameters** : If true, Construct 3 will allow the user to enter any number of parameters beyond those defined. In other words the parameters (if any) listed in "params" are required, but this flag enables adding further "any" type parameters beyond the end.
- **Translated name** : the translated name of the expression name. In the en-US file, this should simply match the expression name from the expression definition. This key mainly exists so it can be changed in other languages, making it possible to translate expressions in some contexts. Note when actually typing an expression the non-translated expression name must always be used.
- **Description** : the description that appears in the expressions dictionary, which lists all available expressions.

### Creating ACE Parameters
When defining you ACEs you can easily add new parameters, by right clicking in any of the ACE editors and selecting "Insert _ Parameter". This will bring up the ACE Parameter wizard and guid you through setting up a new paramter. Adding a paramter will modify the ACE.json, Langauge and Code section to include the new parameter, there is no automated way to reorder your parameters as of now.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-add-param.gif)

- **Parameter ID** : A string with a unique identifier for this parameter. This is used to refer to the parameter in the language file. No spaces are allowed, All spaces entered will be coberted into dashes.
- **Parameter Type** : This will determine the data type of the paramter.  
- **Initial Value** : A string which is used as the initial expression for expression-based parameters. Note this is still a string for "number" type parameters. It can contain any valid expression for the parameter, such as "1 + 1". For "combo" parameters, this is the initial item ID.
- **Parameter Name** : This will be the parameter name in the language file.
- **Parameter Description** : This will be the parameter desc in the language file.

## Language
The langauge window helps filling out the missing peices of the language file. There are 2 sections one for Plugin Properties defined in the edit time plugin.js, and one for the Categories defined through the ACEs. If you update The plugin properties or the ACE categories, you need to come to this window and regenerate these jsons. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-lang.gif)

## Test 
The Testing window lets you compile and host your addon with the built in web server. When you click "Test C3 Addon" button, it will compile your addon and display logs on the left section of the testing window. All addons are compiled to *AppData\Roaming\C3IDE_DATA\Server\Test\${addon class name}* by default.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-preview.gif)
*creating and testing a simple plugin*

The web server will host all the files located in *AppData\Roaming\C3IDE_DATA\Server\Test\* so if you have compiled multiple addon's you can test them at the same time. The current URL of the compiled addon will be located at the bottom of the testing windows, clicking on the url will copy it to your clipboard so you can use it construct 3.

When you are done testing your addon, you can click on the "Stop Web Server" button to shutdown the webserver. 

During compliation there are a few validation that take place to prevent a few of the error you could get when installing your addon, such as language placeholder not matching, duplicate id's and a few others.

## Options
The Option window has some global options that e=affect how the application behaves.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/c3IDE-options.gif)

- **Compile Path** : The path that will be used when testing and compiling you addon
- **Export Path** : The path where exported project from the dashboard will appear
- **Data Path** : (READONLY) The data path points to the root where the application will store all it's data
- **C3Addon Path** : (READONLY) The C3Addon Path is the path where .c3addon files will be created 
- **Default Company** : This is the company that will be used when starting a new addon
- **Default Author** : This is the author that will be used when starting a new addon
- **Editor Font Size** : This value effect the font size of all the code editors
- **Editor Font Family** : This value effects the font that will be used in all editors
- **Editor Theme** : This value effects the theme of application, as of now there is only one theme, but more are planned for the future
