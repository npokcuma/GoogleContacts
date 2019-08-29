using System.Windows;
using ipogonyshevNetTest.ViewModel;

namespace ipogonyshevNetTest.View
{
	public partial class LableWindow
	{
		public LableWindow(LableWindowViewModel viewModel)
		{
			InitializeComponent();
			Txbx1.Focus();
			DataContext = viewModel;
		}


		private void Cancel_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
		}
		
	}
}
