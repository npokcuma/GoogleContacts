using ipogonyshevNetTest.Services;
using ipogonyshevNetTest.ViewModel;

namespace ipogonyshevNetTest.View
{
	public partial class MainWindow
	{
		public MainWindow()
		{
			InitializeComponent();

			IContactService contactService = new MockContactsService();
			DataContext = new MainWindowViewModel(contactService);
		}
	}
}
