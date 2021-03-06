Project Silver Fox
===================

A WPF Application to save favourite windows services and enable quick management of them
------------------------------------------------------------------------------------------

Project Silver Fox (PSF) - Release name: Lehko Services Manager

PSF is a learning and demonstration project that's still in progress. Initially inspired by the wish to have a filter/search on the windows services.msc program. Users can browse all the services and save frequently used ones (persisted to file) to quickly stop/start and change start-up settings. PSF should be useful for game/work purposes and replace a collection of batch files or manual searching through rows of services. Future functionality considered includes managing services on other computers and IIS. 
PSF main features will give users these options:

* Display list of saved services, including status and startup type.
* Enable starting and stopping of multiple selected services.
* Enable changing the startup type of multiple selected services.
* Change the description of saved services.
* Refresh the running values of selected/all services.
* Remove saved services from the favourites list.
* Display a searchable (Regex) list of all services on PC and allow saving of selected services
* (via app.config) Change location of saved services xml file.
* (via app.config) Change application background theme (BaseLight/BaseDark)
* (via app.config) Change the application accent

The application uses GalaSoft.MvvmLight (https://mvvmlight.codeplex.com/) and MahApps.Metro (http://mahapps.com/) for the UI.
Administrative privileges is required by the application to change service values.

## Design Sketches

|Main Page Sketch  |   Add Page Sketch   |
|:-----------------------:|:-------------------------:|
|![scetch1](https://cloud.githubusercontent.com/assets/14425937/15806345/3b7a495a-2b39-11e6-8b5e-3d8edcc727ed.png)|![scetch2](https://cloud.githubusercontent.com/assets/14425937/15806346/3b7faf30-2b39-11e6-8307-ab0a1e8cb7d9.png)|


## Project Demo
|Main Page                                  |                                        Add Page                     |
|:-------------------------------------:|:------------------------------------------------------------------------------------:|
|![blue1](https://cloud.githubusercontent.com/assets/14425937/15806674/8d0ce942-2b40-11e6-9c08-b392fb9bbe58.PNG)|![blue2](https://cloud.githubusercontent.com/assets/14425937/15806673/8d0adfe4-2b40-11e6-8e85-9c34f40d0829.PNG)|



|Main Page                                 |                                        Add Page                   |
|:--------------------------------------------------------:|:------------------------------------------------------------------------------------:|
|![orange1](https://cloud.githubusercontent.com/assets/14425937/15806675/8d0d81f4-2b40-11e6-8848-41cd94f2eb10.PNG)|![orange2](https://cloud.githubusercontent.com/assets/14425937/15806676/8d104f06-2b40-11e6-8eed-efc728755f74.PNG)|


## Motivation
I started Project Silver Fox (Lehko Services Manager) to gain knowledge about software development, C# programming language and WPF. My main goal is to make the application work and practically use the concepts that I study. Public hosting also allows interviewers to see a sample of my coding abilities.


## Installation
TBC - Nothing yet, build from VS


## License
PSF is licensed under the  [GNU license](https://github.com/OoopOoop/SilverFox/files/299384/license.txt)
