The source repository for Coded Humanoid. An open source online programmer game.

## Overview

Currently the code is a bit of a mess of used and unused code as the idea for this game has changed including the structure
of the server network.

The basic components are:
	- Client (The actual game)
	- MainServerV2 which keeps track of all online players
	- ArenaHost which the main server uses to launch new matches for all online players
	- SingleArena which represents a single match instance
	
The server types are in their their own directories in the root directory. Eg: 
 /MainServerV2
 /ArenaHost
 /SingleArena
 
The client assets are all in the Assets directory including both scripts and graphics.
The game engine (atleast for now) is Unity and the scripting language is therefore C#.

## Contributing

Just simply fork and make your own pull request. Bugs, new assets and completly new ideas or extensions are always welcome.
If you want a response to an idea of your own before you start working on it create an enhancement request and I will respond
as fast as I can.