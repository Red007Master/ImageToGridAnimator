using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Reflection;

internal class ImageProcessingSettings
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

    public int ActualTargetSquareSideLenght { get; set; }

    public int ActualAnimationStartPixelDistance { get; set; }
    public int ActualAnimationEndPixelDistance { get; set; }

    internal void SetFromSettings()
    {
        foreach (PropertyInfo pi in this.GetType().GetProperties())
        {
            string name = pi.Name;

            if (!name.ToLowerInvariant().Contains("actual"))
            {
                PropertyInfo propertyThis = typeof(ImageProcessingSettings).GetProperty(name);
                PropertyInfo propertySettings = typeof(SettingsManager.SettingsList).GetProperty(name);

                object settingValue = propertySettings.GetValue(SettingsManager.Settings, null);

                propertyThis.SetValue(this, settingValue);
            }
        }

        if (!File.Exists(ImageToProcessPath))
        {
            while (true)
            {
                Console.Write($"File = [{ImageToProcessPath}] don't exist.\nInput correct image path:");
                ImageToProcessPath = Console.ReadLine();

                if (File.Exists(ImageToProcessPath))
                {
                    break;
                }
            }
        }
    }

    internal void PrecalculateActualValues(string inputImagePath)
    {
        int Width = 0;
        int Height = 0;

        if (!UseHardcodetAnimationValues || !UseTargetSquareSideLenght)
        {
            Bitmap bmp = new Bitmap(inputImagePath);

            Width = bmp.Width;
            Height = bmp.Height;
        }

        if (UseTargetSquareSideLenght)
        {
            ActualTargetSquareSideLenght = TargetSquareSideLenght;
        }
        else
        {
            ActualTargetSquareSideLenght = Width / TargetSquareWidthCount;
        }

        if (UseHardcodetAnimationValues)
        {
            ActualAnimationStartPixelDistance = AnimationStartPixelDistance;
            ActualAnimationEndPixelDistance = AnimationEndPixelDistance;
        }
        else
        {
            int onePercent = ActualTargetSquareSideLenght / 100;

            ActualAnimationStartPixelDistance = onePercent * AnimationStartPixelDistanceInSquareSideLenghtPercent;
            ActualAnimationEndPixelDistance = onePercent * AnimationEndPixelDistanceInSquareSideLenghtPercent;
        }
    }

    internal void SetFromConsoleDialog()
    {
        foreach (PropertyInfo pi in this.GetType().GetProperties())
        {
            string name = pi.Name;

            if (!name.ToLowerInvariant().Contains("actual"))
            {
                PropertyInfo property = typeof(ImageProcessingSettings).GetProperty(name);
                Type T = property.PropertyType;

                Console.Write($"Input value for <{T}>[{name}]:");
                string userInput = Console.ReadLine();

                property.SetValue(this, TConverter.ChangeType(T, userInput));
            }
        }
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
}