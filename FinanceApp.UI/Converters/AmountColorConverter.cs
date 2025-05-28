using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FinanceApp.UI.Converters
{
    public class AmountColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AbstractTransaction transaction)
            {
                return transaction is ExpenseTransaction ? Brushes.Red : Brushes.LightGreen;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}