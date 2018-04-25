Automates the release process all the way.
Under construction.

## The three intended phases:

- pre
	Supposed to increment the version numbers and assign them to both the client and the server.
- build
	Actually builds the client + servers.
- post
	Copies extra client files to the production path.

After this the installation and update tool should take over.
NOTE: releases are supposed to be released to the public while the server is down and restarted.
The only implentation is for windows currently.
The functionality below has to be taken into account when implementing a new target.

## Implementation spreadsheet:
- Increment manifest number: Done
- Copy zerobrane files to target dir: Done
- Cloud build
- Local build
- Write manifest info to the client and server builds: Done

## Requirements
- dmd >= 2.078

## Current available usage
>> dmd -debug -g -run ReleaseProcessor.d --stage=? --build_mode=?
Where -stage can be PreProcess or PostProcess.
Where --build_mode can be Debug or Release. This affects if either debug_bin or release_bin is written to respectively.

If you want to change the semver number you do that by changing the manifest.txt file in this directory.
NOTE: Do not touch the build number as that is what is actually used to distinct between different builds
and therefore affects updates. The semver string is only for humans.

## Command line bats

This is windows specific and really intended for my use only but is here for the curious one. 
But with these bat files you can increase the development process when debugging network code by
building remotely.
They are also used to make the actual releases of this software.

### Server debugging
Then the handle_remote_build_debug is used. It is meant to be run after you have started a new remote build.
So usually after a git push command. This command will then wait for the download to complete and then download the zip
and do the postprocessing steps on it. When that is complete this server is complete.


### Making release builds
Before you push the final changes you have to run the preprocess bat file. This will update and sync the client and server
with the correct version information. Then you have to start the build on the unity cloud build website.
And finally you can run the handle_remote_build_release.

### Adding new platforms
Is done by adding new CloudBuildDownloader commands. That is the only one command which is platform specific.
The rest is ony build mode specific.