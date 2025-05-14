using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Drawing.Color;

namespace TP.ConcurrentProgramming.PresentationView;

public class ColorConverter : IValueConverter{
    public static readonly ColorConverter Instance = new();
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
        if (value == null) return null;
        int val = System.Convert.ToInt32(value);
        switch (val) {
            case 1:
                return new SolidColorBrush(Colors.Blue);
            case 2:
                return new SolidColorBrush(Colors.Red);
        }
        
        return new SolidColorBrush(Colors.Gray);
    }
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
        throw new NotImplementedException();
    }
}
