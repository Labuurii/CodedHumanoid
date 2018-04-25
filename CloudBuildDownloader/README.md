Tool for downloading the next sucessful unity cloudbuild.

## Setup
For this program to work a Unity Online Services access token is required.
That token has to be placed in a file named 'unity_access_token.txt'.
Then the program can be launched.

## Usage
From any directory run the debug build.
This will cause the program to wait for a new successful unity cloud build.
When the build is complete the build will be downloaded and unzipped. 
Eg if launched from this directory:
>> ./CloudBuildDownloader/bin/Debug/CloudBuildDownloader.exe --build_mode=Debug --build_name="win64-debug" --platform=Windows

## Command line arguments

* --build_mode
	* options: Debug, Release
	* Downloads to the debug_bin or release_bin directory respectively
* --build_name
	* string
	* The identifier unity uses for the build type
* --platform
	* options: Windows
	* The platform for which the build was made.
	* Affects which sub directory the build is put in.