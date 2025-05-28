using System;
using System.Globalization;
using System.Windows.Data;

namespace FinanceApp.UI.Converters
{
    public class CategoryToImagePathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string category)
            {
                string normalizedCategory = category.ToLower().Replace(" ", "");

                string fileName = normalizedCategory switch
                {
                    "food" => "food.png",
                    "transport" => "transport.png",
                    "utilities" => "utilities.png",
                    "entertainment" => "entertainment.png",
                    "purchases" => "purchases.png",
                    "eatingout" => "eatingout.png",
                    "services" => "services.png",
                    _ => "food.png"
                };

                return $"pack://application:,,,/Assets/Icons/{fileName}";
            }

            return "pack://application:,,,/Assets/Icons/food.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}