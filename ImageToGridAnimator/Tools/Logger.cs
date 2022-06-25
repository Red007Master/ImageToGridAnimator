using System;
using System.Diagnostics;
using System.IO;

class Logger
{
    static public string FilePath { get; set; }

    static public void DebugLog(string inputLogMassage)
    {
        string result = $"[{DateTime.Now}] [{Publics.ConstantData.AplicationCallout}] {inputLogMassage}.";

        Console.WriteLine(result);
        using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
        {
            sw.WriteLine(result);
        }
    }
    static public void DebugLog(string inputLogMassage, int gapSize)
    {
        using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
        {
            string result = $"[{DateTime.Now}] [{Publics.ConstantData.AplicationCallout}] {inputLogMassage}.";
            string resultGap = "";

            for (int i = 0; i <= gapSize - 3; i++)
                resultGap += "\n";

            if (gapSize >= 2)
                sw.WriteLine(resultGap);

            Console.WriteLine(result);
            sw.WriteLine(result);
        }
    }
}

class TimeLogger : IDisposable
{
    private Stopwatch Stopwatch = new Stopwatch();
    private string Sender;

    public TimeLogger(string sender)
    {
        Stopwatch.Start();
        Sender = sender;
        Logger.DebugLog($"[{Sender}]-Try");
    }

    public void Dispose()
    {
        Stopwatch.Start();
        Logger.DebugLog($"[{Sender}] completed in [{Stopwatch.ElapsedMilliseconds}]ms");
    }
}
