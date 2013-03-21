using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace StockGames.Resources
{
    /// <summary>   Negative to visibility converter. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class NegativeToVisibilityConverter : IValueConverter
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
            Debug.Assert(value is decimal);
            decimal from = (decimal)value;

            if (from < 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        /// <summary>
        /// Not Implemented.  Modifies the target data before passing it to the source object.  This method is called only
        /// in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// 
        ///
        /// <exception cref="NotImplementedException">  Thrown when the requested operation is
        ///                                             unimplemented. </exception>
        ///
        /// <param name="value">        The value. </param>
        /// <param name="targetType">   Type of the target. </param>
        /// <param name="parameter">    The parameter. </param>
        /// <param name="culture">      The culture. </param>
        ///
        /// <returns>   The value to be passed to the source object. </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
