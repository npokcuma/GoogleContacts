using System.Windows;

namespace ipogonyshevNetTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
            //applicationViewModel
        }
    }

}