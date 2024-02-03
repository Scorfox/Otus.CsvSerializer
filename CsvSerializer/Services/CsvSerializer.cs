using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CsvSerializer.Services;

public class CsvSerializer : ICsvSerializer
{
    private static string Separator { get; set; } = ";";
    private static string NullValueString { get; set; } = "null";

    public string Serialize<T>(T obj)
    {
        var sb = new StringBuilder();
        var values = new List<string>();
        var properties = GetProperties<T>();

        sb.AppendLine(GetHeader(properties));

        foreach (var p in properties)
        {
            var raw = p.GetValue(obj);
            var value = raw == null ? NullValueString : raw.ToString();
            values.Add(value!);
        }

        sb.AppendLine(string.Join(Separator, values));

        return sb.ToString();
    }
    
    public T Deserialize<T>(string input)
    {
        var type = typeof(T);
        var obj = (T)Activator.CreateInstance(type)!;
        var properties = GetProperties<T>();
        
        var rows = input.Split(Environment.NewLine).Where(e => !string.IsNullOrWhiteSpace(e)).ToList();
        var propertyNames = rows[0].Split(Separator).ToList();
        var values = rows[1].Split(Separator).ToList();

        for (var columnNumber = 0; columnNumber < propertyNames.Count; columnNumber++)
        {
            var propertyName = propertyNames[columnNumber];
            var value = values[columnNumber];

            var property = properties.First(e => e.Name == propertyName);
            var converter = TypeDescriptor.GetConverter(property.PropertyType);
            var convertedValue = converter.ConvertFrom(value);
            
            property.SetValue(obj, convertedValue);
        }

        return obj;
    }
    
    private static string GetHeader(PropertyInfo[] propertyInfos)
    {
        var columns = propertyInfos.Select(e => e.Name).ToList();
        var header = string.Join(Separator, columns);
        return header;
    }

    private static PropertyInfo[] GetProperties<T>()
    {
        var type = typeof(T); 
        
        return type.GetProperties(BindingFlags.Public | 
                                                        BindingFlags.NonPublic | 
                                                        BindingFlags.Instance | 
                                                        BindingFlags.Static);
    }
}