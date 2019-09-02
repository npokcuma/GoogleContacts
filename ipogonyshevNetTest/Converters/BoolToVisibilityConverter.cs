using System;
using System.Windows;
using System.Windows.Data;

namespace ipogonyshevNetTest.Converters
{
	/// <summary>
	/// The converter adjusts the visibility of the Login / Logout button
	/// </summary>
	public class BoolToVisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			bool isInverted = (string) parameter == "not";

			bool result = (bool) value;

			if (isInverted)
			{
				return result ? Visibility.Collapsed : Visibility.Visible;
			}
			else
			{
				return result ? Visibility.Visible : Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
