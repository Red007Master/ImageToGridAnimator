using System;

class Publics
{
    public static string[] InputArgs { get; set; }

    public static PathNames PathNames { get; set; } = new PathNames();
    public static PathDirs PathDirs { get; set; } = new PathDirs();

    public static string ImageToProcess { get; set; }

    public class ConstantData
    {
        public static string VersionName { get; set; }
        public static int VersionId { get; set; }
        public static string AplicationCallout { get; set; }
    }
}


public class PathCoreClass
{
    public string Core { get; set; }

    public string Settings { get; set; }
    public string Debug { get; set; }

    public string Output { get; set; }
}

public class PathNames : PathCoreClass
{
    public PathNames()
    {
        Core = "ImageToGridAnimator";

        Settings = "Settings.txt";
        Debug = "DebugLog.txt";

        Output = "Output";
    }
}

public class PathDirs : PathCoreClass
{
    public new string Settings { get { return SettingsManager.FilePath; } set { SettingsManager.FilePath = value; } }
    public new string Debug { get { return Logger.FilePath; } set { Logger.FilePath = value; } }

    public void SetFromExecutionPath(string inputExecutionPath, PathNames inputPathNames)
    {
        this.Core = GetCorePath(inputExecutionPath, inputPathNames.Core);

        this.Debug = this.Core + @"\" + inputPathNames.Debug;
        this.Settings = this.Core + @"\" + inputPathNames.Settings;

        this.Output = this.Core + @"\" + inputPathNames.Output;
    }

    private static string GetCorePath(string inputCurrentPath, string inputCorePathName)
    {
        string corePathBuffer = "";
        string[] currentPathAsArray;
        bool corePathIsDetected = false;

        currentPathAsArray = inputCurrentPath.Split(Convert.ToChar(@"\"));

        for (int i = 0; i < currentPathAsArray.Length; i++)
        {
            corePathBuffer += currentPathAsArray[i] + @"\";

            if (currentPathAsArray[i] == inputCorePathName)
            {
                corePathIsDetected = true;
                break;
            }
        }

        corePathBuffer = corePathBuffer.Remove(corePathBuffer.Length - 1);

        if (!corePathIsDetected)
        {
            //Error implementation
            return null;
        }
        else
        {
            return corePathBuffer;
        }
    }
}