using System.Windows;

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
