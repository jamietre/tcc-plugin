##TccPlugin

C# framework for creating plugins for the [TCC](https://jpsoft.com/tccle-cmd-replacement.html) command shell (CMD) replacement for Windows.

##Status

* **9/30/2015**: Some more features added, SPAWN changed to detach new process from shell. There are some issues still. Only works when compiled with Debug target right now.

* **9/9/2015**: Basic API implemented. Adding features to the increasingly badly named [TildePlugin](./TildePlugin.md)
* **8/18/2015**: It's brand new, but it seems to work.

##How to use

Compile it, and copy the resulting DLL into a `plugins` folder where TCC is located. Or, you can load a plugin interactively:

    > PLUGIN /L MyPlugin.dll

##What can TCC plugins do

You can create system-defined variables (mostly interesting because they are really functions):

    public static int _hello([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
    {
        sb.Append("This is an internal variable.");            
        return 0;
    }


TCC:

    > echo %_hello
    "This is an internal variable"

You can create functions that take arguments:

    public static int f_rev([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
    {
        var reversed = sb.ToString().Reverse().ToArray();
        sb.Clear();
        sb.Append(reversed);
        return 0;
    }

TCC:

    > echo %@rev["abcde"]
    "edcba"

You can define new functions, including ones that override existing ones. For example, this makes `~` work in your paths for CD as an alias for home:

 
	public unsafe static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
    {
    	string path = sb.ToString().Replace(" ~", "%HOMEDRIVE%%HOMEPATH%");
        var expanded = TakeCmdLib.ExpandVariables(path);

        TakeCmdLib.CDD(expanded);
        return 0;
    }


You can invoke functions directly by the exported methods on `TakeCmd.dll` -- this needs a lot of building out

You can also capture every keystroke and rewrite the command line. This seems to open a lot of possibilities, e.g. it would be nice to automatically translate `~` to your home directory. While there appears to exist no API for directly parsing input, we can try rewriting it as they type or hit Enter. (Haven't tried yet)

##API

This library includes the scaffolding for a .NET wrapper of the TakeCmd.dll API. I haven't implemented much yet other than `CD`.

##Debugging

After loading a plugin just attach the debugger to `tcc` process.

##To Do

###The Framework itself

* Make most of this a reusable library
* Can't figure out how to use PRE_EXEC, POST_EXEC -- seems to crash 
* Want to abstract the attributes needed to expose methods; however my attempt to override `DllExport` didn't seem to work so maybe the library doesn't test for derived types.
* Automatically generate `PluginInfo.pszFunctions` using reflection
* Add a basic tool for loading config data from JSON at run time to simplify creating customizable plugins
* Try to figure out what key codes mean
* Try to figure out why we must space-pad the command line when replacing it with something shorter
* Test 32 bit dll (only tried 64 so far)

##Some caveats

The interop with `TccLib.dll` and the plugin callbacks is not very well documented. While the SDK on JpSoft's web site has headers for the library, there are nonintuitive things. For example, trial and error leads us to ensure that whenever we pass a pointer to a string as input, make sure it contains a buffer large enough to accept significant changes, *even when the parameter appears to be for input only*. That is, many methods seem to write to input parameters even when the output is the return value. It's pretty common to deal with access violations when adding new WIN32 API method calls.

Another caveat: I have no idea what the upper bound for a command line buffer in TCC is; I am using 512 bytes but this is arbitrary.

##Warning

I am know almost nothing about working with unmanaged memory from C#. Everything you see here I learned in one day. This may cause memory leaks or any number of other problems. Use at your own risk. If I am doing something wrong, please let me know the right way.
 
##Why

There is a distinct lack of a basic usable shell for Windows. Mintty under Cygwin can be set up well (e.g. Babun & zsh) but Cygwin doesn't work well for some things - particularly Node. Cygwin is also kind of slow.

TCC happens to have good basic human interaction in the form of an editable command line that works with familiar Windows key bindings, which all the other shells lack. It integrates well with ConEmu. And it's extensible with a plugin API. This means I can bend it to my will. And TCC/LE is free. This seems like the perfect platform for creating a good Windows command line environment.

However, the plugin API isn't particularly well documented, and JPSoft provide only Delphi and C++ examples. Having no interest in writing all my manhandling code in C++, I put this together. I hope this project will facilitate customizing the shell more easily. It seems very powerful -- there doesn't seem much that can't be controlled via plugins.

Ultimately I'd like to build something on top of the TCC shell that uses Git for Windows and/or Gow to provide as good a unix-like experience on windows as possible. Be nice to be able to defer to `bash` automatically for shell scripts.

Things to figure out... using the Git tools like `ls` considers your home directory to be `/`. Need some parity when working with unix-like paths and the dos tools in TCC. Since we will be working interchangeably with DOS shell and the Git bash when using the Windows Git unix commands, how do we resolve this?

##Official Docs

[Plugin SDK](https://jpsoft.com/all-downloads/plugins-take-command.html) on JPSoft's web site. Other than this I couldn't find any other code examples online. It appears I am the first person to feel compelled to write plugins for TCC in C#. Why no love for the command line, Windows users?

In addition to the plugin API you also need to call back to the `TakeCmd` library to cause things to happen from a plugin. This is not formally documented; luckily most of the public API is documented in `TakeCmd.h` in the SDK.