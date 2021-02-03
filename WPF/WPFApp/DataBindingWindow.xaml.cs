using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for DataBindingWindow.xaml
    /// </summary>
    public partial class DataBindingWindow : Window
    {
        // Create Person data source
        Person person = new Person();

        public DataBindingWindow()
        {
            InitializeComponent();

            // Make data source available for binding
            this.DataContext = person;

            this.Closing += DataBindingWindow_Closing;
        }

        private void DataBindingWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBox.Show($"Final person name: {person.Name}");
        }
    }
}
