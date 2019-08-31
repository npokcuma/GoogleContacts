using ipogonyshevNetTest.Services;
using ipogonyshevNetTest.ViewModel;

namespace ipogonyshevNetTest.View
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			IContactService contactService = new GoogleContactsService();
			DataContext = new MainWindowViewModel(contactService);
		}
	}
}
