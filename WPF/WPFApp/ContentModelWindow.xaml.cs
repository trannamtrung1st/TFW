using System.Windows;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for ContentModelWindow.xaml
    /// </summary>
    public partial class ContentModelWindow : Window
    {
        public ContentModelWindow()
        {
            InitializeComponent();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            // Show message box when button is clicked
            MessageBox.Show("Hello, Windows Presentation Foundation!");
        }
    }
}
