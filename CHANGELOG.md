
# c3IDE - CHANGE LOG

### Build# 1.0.1.27 (02/17/2019)
* fix issue with javascript arrow function and js beauitifer.
* default template now dont contain any pre-defined properties 

### Build# 1.0.1.25 (02/12/19)
* add variadic parameter to action type (this feature is undocumented in c3 sdk but might be useful) using it comes with a compiler warning
* added add param to context menu in list box for all ace types

### Build# 1.0.1.24 (02/12/19)
* fix bug with empty string param values
* fix bug with search not looking at current file being searched

### Build# 1.0.1.23 (02/11/19)
* add global search and replace (highlight word and hit F1 to bring up the search and replace window) (effect addon does not have search and replace yet, this will be in the next update, where i will focus a bit more on the effect workflow)
* fixed an issue with the export
* moved the export buttons to the test windows (this ensure that you can only export the currently loaded addon)
* fixed issue with aces not saving properly when navigating the windows
* forced class name to not have spaces on creation
* removed update button while i rework a better way to replace addon metadata. this should be much easier now that i have the search and replace system in place

### Build# 1.0.1.22 (02/07/19)
* fix issue with update plugin when values were null or empty
* enforce all addon metadata values to not be empty (description was previously nullable which caused issues down stream)
* fix validate addon for effects
* include addon validator logic in the compilation 
* fix issue with c3addon path being readonly, it is no longer readonly

### Build# 1.0.1.20 (02/06/19)
* added extra buttons to compile window, to start web server/open construct, refactored how compile logging wroks 
* remove spaces from author, since it's used as part of the addon id   

### Build# 1.0.1.17 (02/06/19)
* add compile on save option - this option will compile the selected addon when a change is saved (ctrl-s), this will bring up a compilation log popout window, this allows for quicker iteration. you can go in start the web server, start making changes and compile on the fly, this avoids having to manually recompile the files everytime.    

#### Build# 1.0.1.16 (02/05/19) 
* add new c3ide icons and file association, now there are 2 new ways to pass arguments into the application, it will accept a path to .c3ide file or a guid to identify the addon to load. 
* added option to pin menu, which will leave menu open 

#### Build# 1.0.1.3  (02/02/2019)
* refactor auto completion, there were some issue that need to be fixed, as a quick fix, i disbaled the embedded documentaion until i can recolve the issue,. as of now auto completion still works but its not context based. it works more like how sublime text works.
* updated syntax highlighting across the board for all themes, now json and js are highlighted differently
* added ayu light and ayu mirage themesn (https://github.com/ayu-theme/ayu-colors )
* added effect template, and changed parts of the ui to be dynamic based on addon type
* updated the addon compiler to support effects
* start working on the infustrature to create find and replace, and improve the editors workflow
* added a label on the dashboard to show plugin type
* fix issue with aces category being overwritten

#### Build# 1.0.1.2  (01/29/2019)
* (hotfix) fix issue with start server 

#### Build# 1.0.1.1  (01/29/2019)
* Fixed bug with addon compiler, now the process will terminate when there is an error
* Added a validate addon button test window, (as of now this only check json)
* Fixed bug when navigating from the aces windows, sometimes the changes were not being saved
* Added a compile only button to test window, which just regenerates the addon without starting the server
* Added a start web server button to test window, which just starts the web server with no compilation 
* Started working on eventing backend (not user facing)

#### Build# 1.0.1.0  (01/27/2019)
* upgrade minor build, for newer build numbers
* (hotfix) now when you duplicate aces, ace/lang scriptName property also get updated 

#### Build# 1.0.0.19  (01/27/2019)
* Added duplicate action function to context menu.
* Added duplicate condition function to context menu.
* Added duplicate expression function to context menu.
