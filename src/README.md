####TccPlugin Source Notes

The architecture is a little tricky because of several competing concerns

1) We use [RGiesecke.DllExport.Metadata](https://www.nuget.org/packages/UnmanagedExports) to export methods from managed code to the native API. This does **not** work with "Any Cpu" targets. You will need to build separately for 32 and 64 bit applications. 

2) We use [Costura.Fody](https://www.nuget.org/packages/Costura.Fody/) to put the dependencies into a single DLL. This packages dependencies as a resource and loads them from memory. ILMerge is not possible after `DllExport` because it does not work with unmanaged code.

Both of these packages, as well as `TccPlugin`,  should be dependencies of the actual plugin project.  

Once this is stable `TccPlugin` can just be a Nuget package. But you will still need to compile the basic exported API into the custom part of your plugin, so we will provide this template as content in the Nuget package. It's not possible to expose the API of the plugin from more than one DLL, so in order to encapsulate as much of the plumbing as possible in the the common `TccPlugin`, you will just include each API method and call back to `TccPlugin.TccEventManager`. This will in turn exposes events in managed code that you can use. This allows (at least) the API code to be a template that won't need to change, even if it still needs to be compiled with your plugin.

If changing between x86 and x64, you will need to change **Test -> Test Settings -> Default Processor Architecture** as well.

####Possible ways to simplify this

Right now you need to add a method for each TCC command you override. We could just put the put the entire API of commands in the template; this seems a bit overkill, but would enable some nice things like abstracting an event for rewriting paths for any command.

Possibly we could do this with the PRE_EXEC method? But so far have not been able to figure out the API for it, as everything I do causes a crash. 