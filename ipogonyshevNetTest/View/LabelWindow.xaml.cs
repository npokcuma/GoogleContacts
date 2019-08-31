using System.Windows;
using ipogonyshevNetTest.ViewModel;

namespace ipogonyshevNetTest.View
{
	public partial class LabelWindow
	{
		public LabelWindow(LabelWindowViewModel viewModel)
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
