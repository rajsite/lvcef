lvcef
=====

****************

Overview
--------

Chrome Embedded Framework (CEF) Toolkit for LabVIEW. The toolkit utilizes the Xilium.CefGlue .NET interface for CEF 3 and provides an alternate implementation of the Client example with the following properties:

* A separate render process is used instead of the browser process
* LVCef does not require the creation of classes to override callbacks. Instead, functionality is implemented through .NET delegates wherever possible
* The class structure stays very close to the Xilium.CefGlue structure to make referencing the original CEF documentation straightforward
* The .NET control does very little automatic initialization giving the implementer as much flexibility as possible
* Extensive debugging statements are included to enable monitoring of the life cycle of the control, see Debugging LVCef .NET Assemblies

While the goal of the project is high interoperability with the LabVIEW Development Environment, the LVCef client implementation is generic enough to be utilized in other applications. Some use cases would be in plug-in like environments (where dedicated render processes are required, etc.) or for trying to get a clearer grasp of using the CEF API.

### Current Library Versions ###

* CEF 3.1916.1721 Windows 32-bit 
* Xilium.CefGlue Commit f9edd93 

Getting Started
---------

Instructions for getting up and running an example as fast as possible:

1. Download a copy of the LVCef source. For a description of the different components of the project see Source Project Layout

2. Open the /dotnet/lvcef.sln project in Visual Studio 2012. The source has been tested to run in Visual Studio 2012 on Windows 7. It may be possible to use other versions of Visual Studio as well.

    While LVCef currently uses .NET 3.5 it is not strictly necessary and it may be possible to use newer versions of .NET, etc.

3. Follow the instructions in **Important Build Notes** to correctly configure the projects

4. Build all of the projects in the /dotnet/lvcef.sln

5. Before executing the LVCef.TestControlAppSimple demo start a local web server by running the /html/runwebserver.bat script. The script requires python to execute, see Source Project Layout and the /html section for more information.

6. With the web server running launch the LVCef.TestControlAppSimple demo to see a Windows Form Application run and utilize the LVCefControl to render a web application.

7. To run the example in LabVIEW, first open LabVIEW in the correct working directory by running the script at the /lv/runwithpaths.bat location. The script requires a specific version of LabVIEW to execute, see Source Project Layout and the /lv section for more information.

8. LabVIEW will launch and open the lvcef.lvproj LabVIEW Project. Before running any VIs perform a Mass compile of the project. see **Important Build Notes**

9. Run the Init VI once (it should only be run once while the LabVIEW.exe process is running. Only run Init VI again if LabVIEW is completely shutdown and restarted. Do not run Init VI between execution of different VIs if the LabVIEW.exe process stays running between VIs)

10. Run the Example VIs in the lvcef.proj file to have the LVCefControl render a web application on the LabVIEW front panel. To understand more about what is going on see the Required Reading section and the Source Project Layout section

Required Reading
-----

CEF is a very large codebase with a diverse feature set that is easy to get lost in. The following documents are required reading to gain the background needed to utilize LVCef to its fullest in your own custom HTML5 powered applications:

* [Chrome Embedded Framework: General Usage] (https://code.google.com/p/chromiumembedded/wiki/GeneralUsage)

Source Project Layout
--------------

Developers wishing to use LVCef in a project should refer to the Deployed Project Layout section. Developers wishing to inspect the source, modify, and build LVCev should continue in this section.

The projects use relative paths as much as possible to make building and testing code as easy as possible for LVCef developers. The following project structure does not reflect the layout of the deployed LVCef project on a users system (see Deployed Project Layout).

In addition, not all project configuration could be modified for relative paths and some manual configuration is required to successfully build and run LVCef. Please see **Important Build Notes** for more information.

The following top-level directories are used to organize the LVCef project:

>    /cef

The /cef directory contains a Release binary of the Chrome Embedded Framework for the CEF version specified in Current Library Versions. The Release binaries have been Included in the repository for simplicity as CEF is not made available in a package management system.

For debugging purposes, the release symbols, as well as the debug binary and debug symbols, can be found at the [CEF Builds] (http://cefbuilds.com/) page. Take note of the Branch number as well as the Revision number when downloading binaries and symbol files.

> /xilium.cefglue

This directory contains the commit revision specified in Current Library Versions for the [Xilium.CefGlue project] (https://bitbucket.org/xilium/xilium.cefglue/). The repository is included for simplicity as a different version control system is utilized than LVCef. To see the version of CEF that Xilium.CefGlue is targeting you can reference the following file: 

        /xilium.cefglue/CefGlue/Interop/version.g.cs

As of the commit listed in Current Library Versions there are no modifications to the Xilium.CefGlue source required by the LVCef project so a direct replacement with the repository revision is possible. If future modifications are required to Xilium.CefGlue to support LVCef they will be documented in this Readme. 

> /dotnet

The source files for the LVCef .NET code is located in /dotnet and is divided into the following projects:

1. CefGlue: A project reference added from the /xilium.cefglue directory

2. LVCef: The bulk of the implementation broken into the following major components:
    * A Windows Form Control called LVCefControl for use in Windows Forms applications / LabVIEW Front Panels.
    * Handlers (App and Client) used for implementing the vast majority of the different behaviors and functionality in the LVCef Control, i.e. JavaScript communication, Request Handling, URL Navigation

3. LVCef.RenderApp: A lightweight dedicated render process implementation meant to execute independently from the LabVIEW execution context. CEF will spin-up multiple instances of the Render process as needed. For more information see the Required Reading section above.

4. LVCef.TestControlAppSimple: A sample application written in C# that utilizes the LVCef control. When running the example from within Visual Studio make sure to follow the steps in the Important Build Notes section.

   If running the sample outside of Visual Studio after it has been built a helper batch file is provided to configure the working directory and run the app:

        /dotnet/LVCef.TestControlAppSimple/runwithpaths.bat

    Modifications to the batch file may be required to support Release vs Debug configuration, etc.

> /html

A directory containing example HTML/CSS/JS applications for use with the LVCef.TestControlAppSimple as well as sample VIs in the /lv directory. The directory also contains the following batch file to start a simple web server using python to serve files for use by LVCef:

        /html/runwebserver.bat

Modifications to the batch file may be required to support different python installation locations or to utilize a different web server, etc.

> /logs

The logs directory is empty by default (containing just a place holder file) but it is required to be in available in order to have the different processes correctly log information during execution

> /lv

The lv directory contains the project files and LabVIEW source code to wrap the LVCef .NET library and the LVCefControl component. The LabVIEW source is still very much a work in progress until the LVCef .NET API has stabilized.

This directory contains a batch file that will launch LabVIEW using the correct working directory so the libcef.dll will be properly identified:

        /lv/runwithpaths.bat

Modifications to the batch file may be required to support different versions of LabVIEW, etc.

Important Build Notes
---------------------

### Configuring Visual Studio Working Directory ###

The following two projects should be modified so that  properties > Debug > Working directory points to the /cef/ directory:

        /dotnet/LVCef.RenderApp/LVCef.RenderApp.csproj

and 

        /dotnet/LVCef.TestControlAppSimple/TestControlAppSimple.csproj

This has to be performed manually as [relative paths cannot be used in C# projects] (http://connect.microsoft.com/VisualStudio/feedback/details/618256/relative-path-or-macros-not-allowed-in-c-project-debug-working-directory) for specifying the Working Directory. 

### Mass Compile VIs Before First Run ###

When the project is first synced the paths to the .NET assemblies referenced by the VIs will need to be updated. In the lvcef.lvproj project window right click on the project name and choose Mass Compile. If a VI is opened or run before the Mass Compile is performed you may need to manually update any opened VIs (i.e. reselect constructors for the constructor node, reselect invoke node functions, etc). This step will be mitigated by the process discussed in the Deployed Project Layout section for installed systems.

### Debugging LVCef .NET Assemblies ###

The LVCef .NET Assemblies have debugging logging added for most functions and delegates that are executed. A convenient way to see the logging statements is to use the [Debug View] (http://technet.microsoft.com/en-us/sysinternals/bb896647.aspx) tool from Microsoft SysInternals. The debug logging statements are only configured to be available when LVCef is built in the Debug configuration.

Deployed Project Layout
-----------------------
Developers wishing to inspect the source, modify, and build LVCev should refer to the Source Project Layout section. Developers wishing to use LVCef in a project should continue in this section.

The details for how LVCef will be installed in <LabVIEW>/vi.lib have yet to be determined.

FAQ
---
* How do I decrease the size of the libcef dll or remove unwanted features?

  **To remove unwanted features you have to create a [custom build of CEF] (https://code.google.com/p/chromiumembedded/wiki/BranchesAndBuilding).**
