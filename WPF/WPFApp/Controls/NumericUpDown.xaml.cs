using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFApp.Controls
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        public int Count { get; set; }
        public NumericUpDown()
        {
            InitializeComponent();
            this.DataContext = this;
            this.PropertyChanged += NumericUpDown_PropertyChanged;
        }

        private void NumericUpDown_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Count)) 
                this.valueText.Text = Count.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            Count++;
            this.PropertyChanged(sender, new PropertyChangedEventArgs("Count"));
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            Count--;
            this.PropertyChanged(sender, new PropertyChangedEventArgs("Count"));
        }

    }
}
