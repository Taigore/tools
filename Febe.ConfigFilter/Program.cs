// See https://aka.ms/new-console-template for more information
using Febe.ConfigFilter;
using Febe.ConfigFilter.Cli;

var programArgs = ProgramArgs.Parse(args);
if (programArgs == null)
{
    return -1;
}

var filter = new Filter(
    Console.OpenStandardInput(),
    Console.OpenStandardOutput()
);

if (programArgs.Action == "clean")
{
    filter.Clean();
    return 0;
}

if (programArgs.Action == "smudge")
{
    var configPath = programArgs.ConfigPath;
    if (string.IsNullOrEmpty(configPath))
    {
        Console.WriteLine("Define a source for the values by passing the argument --config \"path\"");
        return -1;
    }

    var configLoader = new ConfigLoader(configPath);
    filter.Smudge(configLoader);
    return 0;
}

return 0;
