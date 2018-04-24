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
>> dmd -debug -g -run ReleaseProcessor.d -stage=?
Where -stage can be PreProcess or PostProcess.

If you want to change the semver number you do that by changing the manifest.txt file in this directory.
NOTE: Do not touch the build number as that is what is actually used to distinct between different builds
and therefore affects updates. The semver string is only for humans.