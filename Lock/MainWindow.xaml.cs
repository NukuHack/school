using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Random rnd = new Random();
        private int Lock;
        private int tries = 6;
        private bool won = false;

        public MainWindow()
        {
            InitializeComponent();
            Lock = randomStart();
            help.Items.Add("-   :   bad");
            help.Items.Add("o   :   nearly");
            help.Items.Add("x   :   good");
        }

        private int randomStart() => rnd.Next(0, 10000);

        private void WrapPanel_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Find the element that was clicked
            DependencyObject hitElement = e.OriginalSource as DependencyObject;

            while (hitElement != null && !(hitElement is Button))
            {
                hitElement = VisualTreeHelper.GetParent(hitElement);
            }

            if (hitElement is Button button)
            {
                var data = button.Content.ToString();
                switch (data)
                {
                    case var _ when data.Length > 1 && data.IndexOf('r') == -1:
                        delNum();
                        break;
                    case var _ when data.Length > 1 && data.IndexOf('r') != -1:
                        enterData();
                        break;
                    default:
                        inputNum(data);
                        break;
                }
            }
        }
        private void inputNum(string I)
        {
            if (nums.Text.Length>3)
            {
                MessageBox.Show("Num max 4", "not nice");
                return;
            }
            nums.Text += I;
        }
        private void delNum()
        {
            if (nums.Text.Length < 1)
            {
                MessageBox.Show("Nothing to del", "not nice");
                return;
            }
            nums.Text = nums.Text.Substring(0,nums.Text.Length-1);
        }
        private void enterData()
        {
            if (won)
            {
                MessageBox.Show($"You have already won... \nthis is the winning number:\n{tostring(Lock)}");
                return;
            }
            string data = nums.Text;
            if (data.Length < 4)
            {
                MessageBox.Show("Num needs 4", "not nice");
                return;
            }
            char[] lockChars = tostring(Lock).ToCharArray();
            string[] feedback = new string[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                if (i < lockChars.Length && data[i] == lockChars[i])
                    feedback[i] = "x"; // Correct position
                else if (lockChars.Contains(data[i]))
                    feedback[i] = "o"; // Correct digit but wrong position
                else
                    feedback[i] = "-"; // Incorrect digit
            }
            answer.Items.Add($"{data} : {string.Concat(feedback)}");
            if (tostring(Lock) == data)
            {
                MessageBox.Show("AHHH nice", "won");
                won = true;
            }
            else
            {
                // Clear the input field
                nums.Text = string.Empty;

                if (tries > 1)
                    tries--;
                else
                {
                    MessageBox.Show("You tried it too many times\nyou have to start again");
                    MessageBox.Show($"Corrent thing would be :\n{tostring(Lock)}");
                    return;
                }
            }
        }

        private void reset(object sender, RoutedEventArgs e)
        {
            Lock = randomStart();
            nums.Text = string.Empty;
            answer.Items.Clear();
            tries = 6;
            won = false;
        }
        private string tostring(int x)
        {
            // custom function to keep zeros inside
            return x.ToString("D4");
        }
    }
}