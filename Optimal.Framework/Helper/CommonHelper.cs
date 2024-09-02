using System.ComponentModel;

namespace Optimal.Framework.Helper;

public class CommonHelper
{
    public static long ConvertToUnixTimestamp(DateTime value)
    {
        return (long)
            (value - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
    }

    public static object To(object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    public static object To(object value, Type destinationType, CultureInfo culture)
    {
        if (value == null)
        {
            return null;
        }

        Type type = value.GetType();
        TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
        if (converter.CanConvertFrom(value.GetType()))
        {
            return converter.ConvertFrom(null, culture, value);
        }

        TypeConverter converter2 = TypeDescriptor.GetConverter(type);
        if (converter2.CanConvertTo(destinationType))
        {
            return converter2.ConvertTo(null, culture, value, destinationType);
        }

        if (destinationType.IsEnum && value is int v)
        {
            return Enum.ToObject(destinationType, v);
        }

        if (!destinationType.IsInstanceOfType(value))
        {
            return Convert.ChangeType(value, destinationType, culture);
        }

        return value;
    }

    public static T To<T>(object value)
    {
        return (T)To(value, typeof(T));
    }

    public static string ConvertEnum(string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return string.Empty;
        }

        string text = string.Empty;
        for (int i = 0; i < str.Length; i++)
        {
            char c = str[i];
            text = !(c.ToString() != c.ToString().ToLower()) ? text + c : text + " " + c;
        }

        return text.TrimStart();
    }
}
