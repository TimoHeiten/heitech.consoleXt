# heitech.consoleXt
Console Host for running your custom scripts with Autocomplete, isolated execution in a Loop, Argument Parsing and Depdency Injection for each Script

<b>Setup in seconds, and concentrate on your scripts instead!</b>

## Why?
The basic idea came up, when I used c# for scriptings in different software projects for different customers.
Recurring Tasks for scripting/automation where:
- Starting a process (like FE application, docker container or databases)
- update db
- use ef core scripts
- test some libraries functionality in an isolated fashion
- having a collection of links and authentication mechanisms in one place
- and many more

But instead of writing bash scripts etc. I tend to have this all in one place for any given project. So most of the time I wrote a simple python script or a .net console application. But lots of them where just things I did before, hence I wanted to have this solution.

There is lots of packages out there for any one of the features, but I wanted them all packaged together and fine tuned for my needs. 
So I build this :)

## What
The consoleXt library gives you a scripting environment that is running and takes on all the scripts you want to execute. 
You simply install the package, create a class that implements the <i>IScript</i> interface and call the static Start method on the Skript class.
```
// from the /tests/heitech.consoleXt.example.Program file:
private class DisplayScript : IScript
{
    // The Script will be registered with this name, and by typing it in the running Loop you can execute this script
    public string Name => "display";

    public IEnumerable<Parameter> AcceptedParameters => new Parameter[] { ("c", "content", true)};

    public async Task RunAsync(ParameterCollection collection, OutputHelperMap output)
    {
        var needle = AcceptedParameters.Single();
        var incoming = collection.Single(x => x.Equals(needle));
        await output[Outputs.Console].WriteAsync(incoming.Value);
    }
}
```
The outputhelper can be used to take on standard outlets like the Console (in this example). You can also supply those via the <i>Skript.Start</i> overloads.

The help and kill command are always available.

See below a picture for the output of the executed ``` heitech.consoleXt.example.Program ```
![sample](https://user-images.githubusercontent.com/20025919/155524015-aa626ed1-2236-4c87-a7be-d7fee319a5bb.png)


