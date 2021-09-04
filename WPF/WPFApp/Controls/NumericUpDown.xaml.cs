using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

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
