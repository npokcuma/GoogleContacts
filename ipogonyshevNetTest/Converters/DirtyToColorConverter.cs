using System;
using System.Windows.Data;
using System.Windows.Media;

namespace ipogonyshevNetTest.Converters
{
	/// <summary>
	/// Converter for coloring a new or changed contact green and vice versa when saving.
	/// </summary>
	public class DirtyToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var isDirty = (bool) value;
			
			var background = new SolidColorBrush(Colors.Transparent);
			
			if (isDirty)
			{
				background = new SolidColorBrush(Colors.LightGreen);
			}

			return background;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
