using System.IO;
using System.Reflection;

public class Dir
{
    public static void CreateAllDirsInObject<T>(T inputObjectOfClass)
    {
        foreach (PropertyInfo propertyInfo in inputObjectOfClass.GetType().GetProperties())
        {
            var value = propertyInfo.GetValue(inputObjectOfClass);
            string name = propertyInfo.Name;

            if (value is string)
            {
                string path = (string)value;

                if (!Directory.Exists(path) && !path.Contains("."))
                    Directory.CreateDirectory(path);
            }
        }
    }
}