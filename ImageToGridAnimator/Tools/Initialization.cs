using System;
using System.IO;

class Initalization
{
    public static void Start()
    {
        ConsoleInit();

        PathDirsInit();

        SettingsManager.InitializeSettings();
    }

    private static void ConsoleInit()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WindowWidth = 200;

        Publics.ConstantData.AplicationCallout = "ITGA";
    }
    private static void PathDirsInit()
    {
        string currentPath = Environment.CurrentDirectory;

        //dev
        //currentPath = @"D:\Development\RedsSoft\ImageToGridAnimator";
        if (!Directory.Exists(currentPath))
            Directory.CreateDirectory(currentPath);
        //dev 

        Publics.PathDirs.SetFromExecutionPath(currentPath, Publics.PathNames);
        Dir.CreateAllDirsInObject(Publics.PathDirs);
    }
}
