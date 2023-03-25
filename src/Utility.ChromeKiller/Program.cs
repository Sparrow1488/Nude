using System.Diagnostics;

var all = Process.GetProcesses();
var processes = all.Where(x => x.ProcessName.Contains("chrome"));

foreach (var process in processes)
{
    try
    {
        Console.WriteLine($"Killing process {process.Id}");
        process.Kill();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    
    Console.WriteLine("Success");
}

Console.WriteLine("Finish");