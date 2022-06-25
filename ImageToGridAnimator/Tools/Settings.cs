using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

class SettingsManager
{
    static public string FilePath { get; set; }
    static public int LoopCounter = 0;

    static public SettingsList Settings { get; set; }
    static private IDictionary<string, ISetting> SettingsDictionary = new Dictionary<string, ISetting>();

    public static void InitializeSettings()
    {
        Settings = new SettingsList();

        using (TimeLogger tl = new TimeLogger("GetDefaultSettingsList"))
        {
            GetDefaultSettingsList();
        }

        using (TimeLogger tl = new TimeLogger("ReadSettings"))
        {
            ReadSettings();
        }

        using (TimeLogger tl = new TimeLogger("ApplySettingsList"))
        {
            FromDictionaryToClass();
        }
    }

    static public void ReadSettings()
    {
        if (File.Exists(SettingsManager.FilePath))
        {
            Logger.DebugLog($"ReadSettings: SettingsFile detected, path=[{FilePath}]");

            string[] fileContentArray = File.ReadAllLines(SettingsManager.FilePath);
            string[] cuted1, cuted2;

            for (int i = 0; i < fileContentArray.Length; i++)
            {
                if (fileContentArray[i].Contains("|"))
                {
                    fileContentArray[i] = fileContentArray[i].Replace("[", "");
                    fileContentArray[i] = fileContentArray[i].Replace("]", "");
                    cuted1 = fileContentArray[i].Split('|');
                    cuted2 = fileContentArray[i + 1].Split('=');

                    Logger.DebugLog($"ReadSettings: Try apply [{cuted2[1]}] to [{cuted1[0]}]");
                    try
                    {
                        SettingsDictionary[cuted1[0]].SetFromString(cuted2[1]);
                        Logger.DebugLog($"ReadSettings: [{cuted2[1]}] applyed with walue = [{cuted1[0]}]");
                    }
                    catch (System.Exception ex)
                    { Logger.DebugLog($"ReadSettings: [{cuted2[1]}] don't applyed with walue = [{cuted1[0]}] ex=[{ex}]"); }
                }
            }
        }
        else
        {
            if (LoopCounter > 5)
            {
                Logger.DebugLog("RecursiveLoop");
                Console.ReadLine();
            }

            LoopCounter++;

            Logger.DebugLog($"ReadSettings: Error SettingsFile don't detected, path=[{FilePath}], applying DefaultSettings, WriteSettings: Try");

            using (TimeLogger tl = new TimeLogger("ApplySettingsList"))
            {
                FromDictionaryToClass();
            }

            using (TimeLogger tl = new TimeLogger("WriteSettings"))
            {
                WriteSettings();
            }

            Logger.DebugLog($"ReadSettings: Error SettingsFile don't detected, path=[{FilePath}], WriteSettings: Success, Initiating Recursive Call of ReadSettings");

            ReadSettings();
        }
    }
    static public void WriteSettings()
    {
        FromClassToDictionary();

        try
        {
            File.Delete(FilePath);
        }
        catch (Exception)
        { } //TODO

        using (StreamWriter sw = new StreamWriter(FilePath, true, System.Text.Encoding.Default))
        {
            for (int i = 0; i < SettingsDictionary.Count; i++)
            {
                var kayValueBuffer = SettingsDictionary.ElementAt(i);
                SettingStr settingStrBuffer = kayValueBuffer.Value.GetSettingStr();

                sw.WriteLine($"[{settingStrBuffer.Name}|{settingStrBuffer.Description}]");
                sw.WriteLine($"{settingStrBuffer.Name}={settingStrBuffer.Value}\n");
            }
        }
    }

    static private void GetDefaultSettingsList()
    {
        AddSetting<bool>(true, "UseTargetSquareSideLenght", "Core");
        AddSetting<int>(50, "TargetSquareSideLenght", "In pixels");
        AddSetting<int>(20, "TargetSquareWidthCount", "");

        AddSetting<bool>(true, "UseHardcodetAnimationValues", "Core");
        AddSetting<int>(0, "AnimationStartPixelDistance", "Start value of distance between squares");
        AddSetting<int>(100, "AnimationEndPixelDistance", "End value of distance between squares");
        AddSetting<int>(0, "AnimationStartPixelDistanceInSquareSideLenghtPercent", "Start value of distance between squares in Percent");
        AddSetting<int>(50, "AnimationEndPixelDistanceInSquareSideLenghtPercent", "End value of distance between squares in Percent");

        AddSetting<string>("Empty", "ImageToProcessPath", "NC");
    }
    static private void AddSetting<Tinput>(Tinput inputValue, string inputName, string inputDescription)
    {
        Setting<Tinput> buffer = new Setting<Tinput>(inputValue, inputName, inputDescription);
        SettingsDictionary.Add(buffer.Name, buffer);
    }

    static private void FromClassToDictionary()
    {
        foreach (PropertyInfo propertyInfo in Settings.GetType().GetProperties())
        {
            var value = propertyInfo.GetValue(Settings);
            string name = propertyInfo.Name;

            SettingsDictionary[name].SetFromObject(value);
        }
    }
    static private void FromDictionaryToClass()
    {
        foreach (KeyValuePair<string, ISetting> item in SettingsDictionary)
        {
            ISetting current = item.Value;

            var property = typeof(SettingsList).GetProperty(current.GetName());
            property.SetValue(Settings, current.GetValue(), null);
        }
    }

    static public void ConsoleOutputSettings()
    {
        foreach (var setting in SettingsDictionary)
        {
            ISetting settingBuffer = setting.Value;
            SettingStr settingStr = settingBuffer.GetSettingStr();

            Console.WriteLine($"Name=[{settingStr.Name}][{settingStr.Value}]");
        }
        Console.WriteLine("\n");
    }

    public class SettingsList
    {
        public string ImageToProcessPath { get; set; }
        public bool UseTargetSquareSideLenght { get; internal set; }
        public int TargetSquareSideLenght { get; internal set; }
        public int TargetSquareWidthCount { get; internal set; }

        public bool UseHardcodetAnimationValues { get; internal set; }
        public int AnimationStartPixelDistance { get; internal set; }
        public int AnimationEndPixelDistance { get; internal set; }
        public int AnimationStartPixelDistanceInSquareSideLenghtPercent { get; internal set; }
        public int AnimationEndPixelDistanceInSquareSideLenghtPercent { get; internal set; }
    }
}

public interface ISetting
{
    SettingStr GetSettingStr();
    void SetFromString(string inputStrValue);
    void SetFromObject(object inputObject);
    string GetName();
    object GetValue();
}
class Setting<TValue> : ISetting
{
    public TValue Value { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public void SetFromString(string inputStrValue)
    {
        Value = TConverter.ChangeType<TValue>(inputStrValue);
    }
    public void SetFromObject(object inputObject)
    {
        Value = (TValue)inputObject;
    }

    public object GetValue()
    {
        return Value;
    }
    public string GetName()
    {
        return Name;
    }

    public Setting(TValue value, string name, string description)
    {
        Value = value;
        Name = name;
        Description = description;
    }
    public SettingStr GetSettingStr()
    {
        SettingStr result = new SettingStr();

        result.Value = Value.ToString();
        result.Name = this.Name;
        result.Description = this.Description;

        return result;
    }
}

public class SettingStr
{
    public string Value { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}
public static class TConverter
{
    public static T ChangeType<T>(object value)
    {
        return (T)ChangeType(typeof(T), value);
    }

    public static object ChangeType(Type t, object value)
    {
        TypeConverter tc = TypeDescriptor.GetConverter(t);
        return tc.ConvertFrom(value);
    }

    public static void RegisterTypeConverter<T, TC>() where TC : TypeConverter
    {
        TypeDescriptor.AddAttributes(typeof(T), new TypeConverterAttribute(typeof(TC)));
    }
}