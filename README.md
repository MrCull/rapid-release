# rapid-release
A multi-threaded utility to load POCOs (Plain Old CLR Objects) into SQL Server Database in a fast and efficient manner.

This application can be used to deploy a new set, or updated set, of POCOs into a Database as part of a software release. 

POCSs are loaded from individual Files in specified Directories.
These Files can additionally have Macros in them which this utility can translate into "Magic Numbers"

Also Stored Procedures can be called during the build.

See "DeployReleaseConfig.xml" for an example deploy script.

![screenshot](https://raw.githubusercontent.com/MrCull/rapid-release/master/cd.png)