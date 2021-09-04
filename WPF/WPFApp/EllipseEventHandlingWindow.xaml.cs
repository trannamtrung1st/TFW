using System.Windows;
using System.Windows.Input;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for EllipseEventHandlingWindow.xaml
    /// </summary>
    public partial class EllipseEventHandlingWindow : Window
    {
        public EllipseEventHandlingWindow()
        {
            InitializeComponent();
        }

        void clickableEllipse_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Display a message
            MessageBox.Show("You clicked the ellipse!");
        }
    }
}
