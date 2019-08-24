namespace ipogonyshevNetTest
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
