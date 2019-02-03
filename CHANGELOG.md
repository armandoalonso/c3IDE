
# c3IDE - CHANGE LOG

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
