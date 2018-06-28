using System;
using Windows.UI.Xaml.Data;

namespace UnoReactiveUIBindingIssuesRepro.Shared
{
    public class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => !(bool) value;

        public object ConvertBack(object value, Type targetType, object parameter, string language) => !(bool)value;
    }
}
