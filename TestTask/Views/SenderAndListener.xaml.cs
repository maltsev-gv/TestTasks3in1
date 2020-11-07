using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using TestTask.Primitives;

namespace TestTask.Views
{
    /// <summary>
    /// Логика взаимодействия для LanguageSelector.xaml
    /// </summary>
    public partial class SenderAndListener : UserControl
    {
        public SenderAndListener()
        {
            InitializeComponent();
        }

        private void ScrollBar_OnScroll(object sender, ScrollEventArgs e)
        {
            var numbersTextBox = (NumbersTextBox) ((ScrollBar) sender).Tag;
            if (e.ScrollEventType == ScrollEventType.SmallIncrement)
            {
                numbersTextBox.Value--;
            }
            if (e.ScrollEventType == ScrollEventType.SmallDecrement)
            {
                numbersTextBox.Value++;
            }
        }
    }
}
