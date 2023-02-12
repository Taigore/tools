// See https://aka.ms/new-console-template for more information
using Febe.ConfigFilter;

var action = args.FirstOrDefault();
if(string.IsNullOrEmpty(action))
{
    Console.WriteLine("Call the program with argument clean or smudge");
    return -1;
}

var filter = new Filter(
    Console.OpenStandardInput(),
    Console.OpenStandardOutput()
);
if(action == "clean")
{
    filter.Clean();
    return 0;
}
else if(action == "smudge")
{
    filter.Smudge();
    return 0;
}
else
{
    Console.WriteLine("Call the program with argument clean or smudge");
    return -1;
}
