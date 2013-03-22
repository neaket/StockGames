using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StockGames.Resources
{
    /// <summary>
    /// This Converter class is extracted from the Presentation component of the full .Net framework.
    /// http://msdn.microsoft.com/en-us/library/system.windows.controls.booleantovisibilityconverter.aspx
    /// </summary>
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="targetType">   Type of the target. </param>
        /// <param name="parameter">    The parameter. </param>
        /// <param name="culture">      The culture. </param>
        ///
        /// <returns>   The value to be passed to the target dependency property. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only
        /// in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="targetType">   Type of the target. </param>
        /// <param name="parameter">    The parameter. </param>
        /// <param name="culture">      The culture. </param>
        ///
        /// <returns>   The value to be passed to the source object. </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
    }
}
