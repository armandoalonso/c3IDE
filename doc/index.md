# C3IDE Documentation #

## Introduction 

c3IDE is a Constrcut 3 Plugin creation tool,which helps streamline the process of creating addons. It lets you quickly iterate and test your addons, and has several quailty of life improvements designed to help you speed up your workflow.

## Setup / Installation / Updates 
c3IDE is an open source project developed using WPF and C#. you can find the link to the git hub repository [here](https://github.com/armandoalonso/c3IDE). To install c3IDE you can download the msi from [itch.io](https://piranha305.itch.io/c3ide). As new updates are released the application will prompt you to update.

## Application Layout 
c3IDE is composed of multiple windows, you can navigate the windows using the left side menu.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/1.png)

## Dashboard 

The dashboard is the main window, from here you can create and manage all you addons. The dashboard will display basic information about all your addons. You will see a small thumbnail of the **addon's icon**. It will display the **name**, the **version**, the **last modified date**, the **addon key** which is used to uniquely identify a particular addon, and the addon **type**.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/2.png)

On the right hand side of the dashboard there will be a few buttons that will allow you to create, load, delete, and duplicate selected addons.you can also right click an addon to bring up the context menu. from the context menu you will have several options on the dashboard. 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/3.png)

## Creating a New Addon

When you create a new addon, you will be taken to the create new addon window. In this window you can input basic information about your addon.

- **Addon Name** : This is the name of the Addon. 
- **Addon Class Name**: This is one part of the the Addon Identifier and is used to prefix a few of the javascript classes.  
- **Author** - This is the name of the author. This value will be combined with the class name to form the addon's ID *Author_Class*, and is the name of the author that will be placed in the Addon.json files
- **Description** - This is a small description of your addon.
- **Addon Type** - This will control what template will be used in creating the addon. You can choose from *SingleGlobal*, *Drawing*, *Behavior*, *Effect*
- - **Addon Category** - This category will determine where which section in construct you addon will be. these value depend on the type of addon you have chosen.
- **Addon Icon** - This is an svg file that will be used as the addon icon inside of construct 3. You can change the addon by dragging an svg file over the existing icon.


![](https://github.com/armandoalonso/c3IDE/blob/master/doc/4.gif)

After your addon has been created, you will be taken back to the dashboard. if you have created an effect, your menus will change to help streamline effect creation. 

## Addon Window
The addon window has 2 tabs, the **addon.json** tab and the **third party files** tab

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/5.png)

###Addon.json
The addon.json tab contains the **addon.json** file which contains mete data about your addon. There is not much to do in this tab as most of this information will be created for you.

###Third Party Files
The third party files tab allows you at add 3rd party js libraries, css files, or any other files you need bundled with your addon. Files that can be viewed as plain text will be visible in the text editor. you can also add binary files those files will just be copied over as is, when your addon is compiled.

- You can create a blank file by clicking the add new file button. this will allow you to text or copy over the contents.
- You can also drag files into the listbox to import them into the project.

 ![](https://github.com/armandoalonso/c3IDE/blob/master/doc/6.gif)

Adding a new third party file will automatically update your addon.json to include the file.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/7.png)

## Plugin/Behavior Window
The plugin/behavior window controls **plugin.js** file or the **behavior.js** file. There are 2 tabs, the edit time tab and the run time tab. 

###Edit time
The edit time tab has the plugin.js/behavior.js code on the root of your addon.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/8.png)

From the Edit time tab you can add new addon properties. you can click on the *New Plugin Property Button* below the text editor section, or you can Right click to bring up the context menu and select new *New Plugin property* from there.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/9.gif)

After clicking on the new plugin property you will be prompted with the new property dialog box, where you can fill out basic property information.  

**If you have any third party files** you can click on the *generate file dependency button* and this generate the code snippets for all you included files.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/10.gif)

###Run time
The run time tab has the plugin.js/behavior.js code from the c3runtime folder.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/11.png)

## Type & Instance

The type and instance windows provide a way to edit the edit time & run time version of **type.js** and **instance.js**

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/12.png)

##Actions, Conditions, Expressions (ACES)
The action, condition, and expression windows let you manage **ace.json**, the **language files** and **java script** which contains the ace code. All 3 ace windows are the same, they just manage different parts of your addon.

Each window will have a list which will display the ace id and the category in parenthesis. The window is broken into 3 section (**Ace.json**, **Langauge**, **Code**). 

###Ace.json 
this section contains the snippet of json that will be used in the ace.json file

###Lanaguge
this section contains the snippet of json that will be included in the en-US.json language file 

###Code
this section contains the block of code that will be used in the corresponding run time java script file.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/13.png)

There are 4 buttons above the Ace list these buttons manage the view. you can also manipulate the size of the different sections by dragging the section borders to increase the size of the sections. clicking the default button will reset the layout size. clicking on the json, lang, code button will expand that section to take up the entire size available.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/14.png)

##Creating a New ACE

When you craete anew ace you will be prompted with a new ACE dialog, here you will fill out basic information about your action/condition/expression.

### Action
![](https://github.com/armandoalonso/c3IDE/blob/master/doc/15.png)

- **Action ID** : This is a unique identifier for the action, it cannot have any spaces, all spaces will be converted into dashes automatically.
- **Action Category** : This will be the category used to group your action. You can change the category by right clicking on the action in the action list and selecting "Change Category". In the language window you will be able to assign a description to this category.   
- **Action List Name** : This is the text that will show up in construct when browsing the aces of your addon.
- **Highlight** : Set to true to highlight the ACE in the condition/action/expression picker dialogs. This should only be used for the most regularly used ACEs, to help users pick them out from the list easily.
- **Async Action** : Setting the action async will append the async keyword to the script in the code section.
- **Display Text** : the text that appears in the event sheet. You can use simple BBCode tags like [b] and [i], and use {0}, {1} etc. as parameter placeholders. (There must be one parameter placeholder per parameter.) For behaviors only, the placeholder {my} is substituted for the behavior name and icon.
- **Description** : a description of the action or condition, which appears as a tip at the top of the condition/action picker dialog.

### Condition
![](https://github.com/armandoalonso/c3IDE/blob/master/doc/16.png)

- **Condition ID** : This is a unique identifier for the condition, it cannot have any spaces, all spaces will be converted into dashes automatically.
- **Condition Category** : This will be the category used to group your condition. You can change the category by right clicking on the condition in the condition list and selecting "Change Category". In the language window you will be able to assign a description to this category.   
- **Condition List Name** : This is the text that will show up in construct when browsing the aces of your addon.
- **Highlight** : Set to true to highlight the ACE in the action/condition/expression picker dialogs. This should only be used for the most regularly used ACEs, to help users pick them out from the list easily.
- **Is Static** : Normally, the condition runtime method is executed once per picked instance. If the condition is marked static, the runtime method is executed once only, on the object type class. This means the runtime method must also implement the instance picking entirely itself, including respecting negation and OR blocks.
- **Is Trigger** : Specifies a trigger condition. This appears with an arrow in the event sheet. Instead of being evaluated every tick, triggers only run when they are explicitly triggered by a runtime call.
- **Is FakeTrigger** : Specifies a fake trigger. This appears identical to a trigger in the event sheet, but is actually evaluated every tick. This is useful for conditions which are true for a single tick, such as for APIs which must poll a value every tick
- **Is Looping** : Display an icon in the event sheet to indicate the condition loops. This should only be used with conditions which implement re-triggering
- **Is Invertible** : Allow the condition to be inverted in the event sheet. Set to false to disable invert.
- **Is Compatible With Trigger** : Allow the condition to be used in the same branch as a trigger. Set to false if the condition does not make sense when used in a trigger, such as the Trigger once condition.
- **Display Text** : the text that appears in the event sheet. You can use simple BBCode tags like [b] and [i], and use {0}, {1} etc. as parameter placeholders. (There must be one parameter placeholder per parameter.) For behaviors only, the placeholder {my} is substituted for the behavior name and icon.
- **Description** : a description of the condition or condition, which appears as a tip at the top of the condition/action picker dialog.

### Expression 
![](https://github.com/armandoalonso/c3IDE/blob/master/doc/17.png)

- **Expression ID** : This is a unique identifier for the expression, it cannot have any spaces, all spaces will be converted into dashes automatically.
- **Expression Category** : This will be the category used to group your expression. You can change the category by right clicking on the expression in the expression list and selecting "Change Category". In the language window you will be able to assign a description to this category.   
- **Return Type** : One of "number", "string", "any". The runtime function must return the corresponding type, and "any" must still return either a number or a string.
- **Is Variadic Parameters** : If true, Construct 3 will allow the user to enter any number of parameters beyond those defined. In other words the parameters (if any) listed in "params" are required, but this flag enables adding further "any" type parameters beyond the end.
- **Translated name** : the translated name of the expression name. In the en-US file, this should simply match the expression name from the expression definition. This key mainly exists so it can be changed in other languages, making it possible to translate expressions in some contexts. Note when actually typing an expression the non-translated expression name must always be used.
- **Description** : the description that appears in the expressions dictionary, which lists all available expressions.

## ACE Parameters
When defining you ACEs you can easily add new parameters, clicking the add parameter button or by right clicking in any of the ACE editors and selecting "Insert _ Parameter". This will bring up the ACE Parameter wizard and guide you through setting up a new parameter. Adding a parameter will modify the ACE.json, Language and Code section to include the new parameter.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/18.png)

- **Parameter ID** : A string with a unique identifier for this parameter. This is used to refer to the parameter in the language file. No spaces are allowed, All spaces entered will be converted into dashes.
- **Parameter Type** : This will determine the data type of the parameter.  
- **Initial Value** : A string which is used as the initial expression for expression-based parameters. Note this is still a string for "number" type parameters. It can contain any valid expression for the parameter, such as "1 + 1". For "combo" parameters, this is the initial item ID.
- **Parameter Name** : This will be the parameter name in the language file.
- **Parameter Description** : This will be the parameter desc in the language file.

## Language
The language window helps filling out the missing pieces of the language file. There are 2 sections one for Plugin Properties defined in the edit time plugin.js, and one for the Categories defined through the ACEs. If you update The plugin properties or the ACE categories, you need to come to this window and regenerate these jsons. 

##Test
The Testing window lets you compile and host your addon with the built in web server. When you click "Test C3 Addon" button, it will compile your addon and display logs on the left section of the testing window. All addons are compiled to *AppData\Roaming\C3IDE_DATA\Server\Test\${addon class name}* by default. During compliation there are a few validation that take place to prevent a few of the error you could get when installing your addon, such as language placeholder not matching, duplicate id's and a few others.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/19.png)

The test window is broken into 4 section, **Test**, **Build**, **Project**, **Version**

###Test
- Test C3 Addon : this button will compile you addon and start the web server to host you test addon. *AppData\Roaming\C3IDE_DATA\Server\Test\${addon class name}* is the default directory where the addon will be compiled to for test but this can be changed in the options.
- Start Web Server : this will not compile any addons and just start the web server pointing to the default addon compile directory.
- Stop Web Server : this button will stop the web server if it is running
- Lint JavaScript - this feature is currently under development.
- Validate Addon - this will perform basic validations which will catch some common errors 

###Build
- Compile Only : this button compiles your addon without starting a web server.
- Open Compile Folder : this button opens up the folder where all addons are compiled to in windows explorer
- Create C3Addon File - this button packages up all your addon files into a .c3addon file which can be imported into construct 3. each time you package your addon the build version will increment by 1

###Project
- Export Addon : the export addon button will export your project (as single file or multi file) .c3ide projects, these projects can then be imported back into c3IDE to create new addon entries
- Open Export Folder : opens the folder where all addon projects will be exported to

###Version
This section handles the version of your addon. you can change the major, minor, revision and build versions for your addon.

###Addon URL
This is the url of the currently loaded and compiled addon *http://localhost:8080/{addon class name}/addon.json*
 
> When the web server is started all addons that have been compiled will be available to construct 3 

###Open Construct
Below the addon url are 2 button which will open construct 3 (in the web or the desktop version)


##Import/Export
You can export/import .c3ide project files. theer are currently 2 formats that can be used. 

- Single-File Project : compress your entire addon into one json file that be be imported into the ide
- Multi-File Project : saves your project in a folder consisting of multiple files (better with version control)   

###Export
You can export your project from the Dashboard or from the Test Window
![](https://github.com/armandoalonso/c3IDE/blob/master/doc/20.png)

###Import
In order to import your project, you have to drag the c3ide project file into the dashboard. this will import your addon. there is an option that will always generate a new addon key every time you import to not overwrite your existing addons.

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/21.png)

##Search & Replace
There is a search and replace window that will scan your projects for instances of text. and you can do a mass replace. in order to bring up the search and replace window select any text inside an editor and press **F1** 

![](https://github.com/armandoalonso/c3IDE/blob/master/doc/22.gif)

##Options
The Option window has some global options that affect how the application behaves.
 
![](https://github.com/armandoalonso/c3IDE/blob/master/doc/23.png)

- **Compile Path** : The path that will be used when testing and compiling you addon
- **Export Path** : The path where exported project from the dashboard will appear
- **Data Path** : (READONLY) The data path points to the root where the application will store all it's data
- **C3Addon Path** : The C3Addon Path is the path where .c3addon files will be created 
- **Construct Desktop Path** : This is the path used to open the desktop version construct with the open construct buttons 
- **Default Author** : This is the author that will be used when starting a new addon
- **Editor Font Size** : This value effect the font size of all the code editors
- **Editor Font Family** : This value effects the font that will be used in all editors
- **Editor Theme** : This drop down includes all of c3IDE themes, If you have any preferences let me know and i will add more. 

- **Open Construct 3 On Web** : this options will change how the open construct buttons behave 
- **Pin Main menu** : This option changes if the menu is collapsed or expanded 
- **Export Single File Project** : This changes how export behaves weather the export is a single file or multi file
- **Remove Console Logs (Compile)** : When this option is enabled when your project is compiled all console.log statements will be commented out 
- **Include Timestamp On Export** : This option will append a time stamp to an exported project file or folder
- **Compile On Save** : This option will compile the currently selected addon, when a save operation is performed using the save button or the save shortcut (ctrl-s). this will bring up a compile log dialog, which will show any compilation errors  
- **Overwrite Addon Id On Import** : When this option is set if you import an addon with the same addon key, the addon will be overwritten, if it's not set it will create a duplicate addon (with a different addon key)  

##Contact
If you run into any issues (or feature request) report it [HERE](https://github.com/armandoalonso/c3IDE/issues)

This project is open source, It is written using WPF & C# the Git hub repository can be found [HERE](https://github.com/armandoalonso/c3IDE) 

Follow me on twitter [@Piranha_305](https://twitter.com/piranha_305)

Check out my games on [itch.io](https://piranha305.itch.io/)

If you just want to chat, you can find me in the [Construct Community Discord ](https://discordapp.com/channels/116497549237551109/253490735268102144) **piranha305#8396**

   



 