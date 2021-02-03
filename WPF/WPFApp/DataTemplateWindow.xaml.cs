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
    /// Interaction logic for DataTemplateWindow.xaml
    /// </summary>
    public partial class DataTemplateWindow : Window
    {
        public DataTemplateWindow()
        {
            InitializeComponent();
            this.DataContext = new[]
            {
                new TaskObject
                {
                    Description = "Task 1 desc",
                    TaskName = "Task 1",
                    Priority = 1
                },
                new TaskObject
                {
                    Description = "Task 2 desc",
                    TaskName = "Task 2",
                    Priority = 2
                },
                new TaskObject
                {
                    Description = "Task 3 desc",
                    TaskName = "Task 3",
                    Priority = 3
                },
            };
        }
    }
}
