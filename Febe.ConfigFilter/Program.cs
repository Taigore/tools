// See https://aka.ms/new-console-template for more information
using Febe.ConfigFilter;

var action = args.FirstOrDefault();
if (string.IsNullOrEmpty(action))
{
    Console.WriteLine("Call the program with argument clean or smudge");
    return -1;
}

var invalidArgsFound = false;
var configSource = (string?)null;
for (int i = 1; i < args.Length; ++i)
{
    switch (args[i])
    {
        case "--config":
            configSource = args[++i];
            continue;
        default:
            Console.WriteLine("Unrecognized argument: {0}", args[i]);
            invalidArgsFound = true;
            continue;
    }
}
if (invalidArgsFound)
{
    return -1;
}

var filter = new Filter(
    Console.OpenStandardInput(),
    Console.OpenStandardOutput()
);
if (action == "clean")
{
    filter.Clean();
    return 0;
}
else if (action == "smudge")
{
    if(string.IsNullOrEmpty(configSource))
    {
        Console.WriteLine("Define a source for the values by passing the argument --config \"path\"");
        return -1;
    }

    var configLoader = new ConfigLoader(configSource);
    filter.Smudge(configLoader);
    return 0;
}
else
{
    Console.WriteLine("Call the program with argument clean or smudge");
    return -1;
}
