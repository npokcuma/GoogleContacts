using System;
using System.Windows;
using System.Windows.Data;

namespace ipogonyshevNetTest.Converters
{
	/// <summary>
	/// Converter for working with object visibility in View.
	/// </summary>
	public class NullToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null)
			{
				return Visibility.Hidden;
			}

			return Visibility.Visible;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
