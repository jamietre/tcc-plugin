##TildePlugin

####What is this

A TCC plugin to add support for using the ~ to represent the home directory.

####What is it really

A plugin to do a whole bunch of stuff. 

###Features


####Tilde support

Allows use of ~ to represent home directory in CD, DIR

TODO:

* Add tab expansion. It would be easy to have it replace ~ with the home directory interactively but this is not so nice. Rather we should use the TabExpansion API and capture tab key.
*  Add support for other commands.

####SPAWN command

`SPAWN {command}` will start the named process in a new shell headless shell and return control, and return the PID of the new shell. This uses the command `START /B /C /PGM {command}` to launch the program, which seems to both return control and prevent UI interaction. 

`SPAWN` typically creates two processes; a new TCC process, and the actual application that runs in the shell. `SPAWN` provides the following additional helpers:

`SPAWNED` lists all processes, and child processes, that were started from the current shell and remain running.

`KILL` kills a process and its children. (The normal `TASKKILL` command will generally not kill child processes; so killing the direct PID created with SPAWN would leave the actual process running.)

The internal variable `_STARTPID` is the last spawned process
The internal variable `_STARTPIDS` is all spawned processes (TODO: should match SPAWNED list)

TODO:

* Add some error handling to KILL
* Add some features to SPAWNED to make output consistent with options for TASKLIST. Allow typing SPAWN with no args instead of SPAWNED.
* Is there any way to allow a console to re-attached to a running process??
* Right now just captures the PID at launch time; we really should just find a way to detect our own PID and identify children for SPAWNED
* Add a command to kill the previously spawned process by default

#####EDIT command

Right now `EDIT {filename}` just launches hardcoded Sublime Text 3.

TODO:

* Add a JSON config file to set up file path mappings to editors
* Add other aliases


#####EXPLORE

Very similar to typing `EXPLORER` except when invoked with no parameters, will open EXPLORER in the current directory