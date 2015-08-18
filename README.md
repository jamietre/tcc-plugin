##TccPlugin

C# framework for creating plugins for the [TCC](https://jpsoft.com/tccle-cmd-replacement.html) command line replacement for Windows.

##Status

* **8/18/2015**: It's brand new, but it seems to work.

##How to use

Compile it, and copy the resulting DLL into a `plugins` folder where TCC is compiled. Or, you can load a plugin interactively:

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
        sb.Append(reverse);
        return 0;
    }

TCC:

    > echo %@rev["abcde"]
    "edcba"

You can define new functions, including ones that override existing ones:

 
    public static uint CD([MarshalAs(UnmanagedType.LPTStr)] StringBuilder sb)
    {
        //special return values - says that I didn't do anything, defer to native CD
        //.. but I could change the contents of command line first..
        return 0xFEDCBA98;
    }


You can invoke functions directly by the exported methods on `TakeCmd.dll` -- this needs a lot of building out

You can also capture every keystroke and rewrite the command line. This seems to open a lot of possibilities, e.g. it would be nice to automatically translate `~` to your home directory. While there appears to exist no API for directly parsing input, we can try rewriting it as they type or hit Enter. (Haven't tried yet)

##API

This library includes the scaffolding for a .NET wrapper of the TakeCmd.dll API. I haven't implemented much yet.

##Debugging

After loading a plugin just attach the debugger to `tcc` process.

##Things to do

* Can't figure out how to use PRE_EXEC, POST_EXEC -- seems to crash 
* Can't figure out how to just invoke a command in the processor from the plugin
* Want to abstract the attributes needed to expose methods; however my attempt to override `DllExport` didn't seem to work so maybe the library doesn't test for derived types.
* Automatically generate `PluginInfo.pszFunctions` from code
* Abstract basic framework (e.g. all required methods & keys)
* Add a basic tool for loading config data from JSON at run time to simplify creating customizable plugins
* Try to figure out what key codes mean
* Try to figure out why we must space-pad the command line when replacing it with something shorter
* Test 32 bit dll (only tried 64 so far)

##Warning

I am know almost nothing about working with unmanaged memory from C#. Everything you see here I learned in one day. This may cause memory leaks or any number of other problems. Use at your own risk. If I am doing something wrong, please let me know the right way.
 

##Why

There is a distinct lack of a basic usable shell for Windows. Mintty under Cygwin can be set up well (e.g. Babun & zsh) but Cygwin doesn't work well for some things - particularly Node.

TCC happens to have good basic human interaction in the form of an editable command line that works with familiar Windows key bindings, which all the other shells lack. It integrates well with Conemu. And it's extensible with a plugin API. This means I can bend it to my will. And TCC/LE is free. This seems like the perfect platform for creating a good Windows command line environment.

However, the plugin API isn't particularly well documented, and JPSoft provide only a C++ example. Having no interest in writing all my manhandling code in C++, I put this together. I hope this project will facilitate customizing the shell more easily.

Ultimately I'd like to build something on top of the TCC shell that uses Git for Windows and/or Gow to provide as good a unix-like experience on windows as possible. Be nice to be able to defer to `bash` automatically for shell scripts.

Things to figure out... using the Git tools like `ls` considers your home directory to be `/`. Need some parity when working with unix-like paths and the dos tools in TCC. Perhaps just always defer to the git version, e.g. `alias dir=ls` and so on? What about `cd`? Do I need to rewrite these commands as plugins and map to windows folder..
