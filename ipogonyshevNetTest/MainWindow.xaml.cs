using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace ipogonyshevNetTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ApplicationViewModel();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DeleteContact.FontWeight = FontWeights.Bold;
            DeleteContact.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void Click_DeleteContact(object sender, RoutedEventArgs e)
        {
            
        }
    }

}