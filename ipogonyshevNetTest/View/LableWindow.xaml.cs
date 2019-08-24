using System.Windows;
using ipogonyshevNetTest.ViewModel;

namespace ipogonyshevNetTest.View
{
	public partial class LableWindow
	{
		public LableWindow(LableViewModel viewModel)
		{
			InitializeComponent();
			DataContext = viewModel;
		}


		private void Save_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}

		private void Cancel_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}
	}
}
