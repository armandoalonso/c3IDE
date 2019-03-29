
# c3IDE - CHANGE LOG

### Build# 1.1.0.19 (03/29/2019) 
* add changelog to solution, fix about window to link to new changelog
* remove old change log button on options window

### Build# 1.1.0.18 (03/29/2019) 
* (hotfix) fix issue with thrid party file being null, when it expected byte array
* disable linting, while a new version is being developed (current version was not very usbale), going to be using esprima to create javascript AST and go from there
* add commands for actions hidden in context menus
* add fix to application start up, when opening directly from c3ide project file, now you should be get prompt  
* change order of dashboadr, now last modified items will always be at the top 
* fix bug with pinning not being respected when restarting the app
* add about window to main menu, window has version information, and links 
* add option to remove console.log statements on compile (only comments them out);

### Build# 1.1.0.8 (03/29/2019)
* add search/filter to dashboard
* display ace category next to ace id in listbox 

### Build# 1.1.0.7 (03/28/2019)
* add option to export project as mutiple files for version control / diff
* add option to overwrite addon id on import
* added better version tracking, update test window to show major/minor/revision/build, increment on c3addon build
 
### Build# 1.1.0.6 (03/28/2019)
* fix issue with effect bool casing

### Build# 1.1.0.5 (03/27/2019)
* re-structure the addon creation process - now there are 2 seperate windows one for the dashboard and one for the creation of the addon.
* fix bug with gloabl search and replace, not finding text in the current window.
* change how 3rd party files are imported, so now non-text files are accepted and converted to base64.
* add very simple js linting (not working as expected, it needs love) 
* re implement effect pages to streamline the creation of effects
* implement a menu manager, we no longer use to isolated menus (effect/main), menu is now controlled by the manager
* fix issue with adding params being sorted in inverse ordder

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
