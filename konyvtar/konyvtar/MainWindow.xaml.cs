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
using System.IO;
using System;
using System.Globalization;

namespace konyvtar
{

    public static class StringExtensions
    {
        public static string ToBasicForm(this string input)
        {
            // Normalize the string to decompose characters with diacritics
            string normalized = input.Normalize(NormalizationForm.FormD);

            // Build a new string excluding diacritics
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                // Only include characters with no diacritic marks
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
        public static string CapitalizeFront(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input; // or return string.Empty; depending on your needs
            }

            // Make the first character uppercase, rest lowercase
            return char.ToUpper(input[0]) + (input.Length > 1 ? input.Substring(1).ToLower() : "");
        }
    }

    public enum Allapot
    {
        Olvasott,
        Olvasatlan,
    }
    public class Konyv
    {
        public string Szerzo { get; set; }
        public string Cim { get; set; }
        public int Kiadas_Eve { get; set; }
        public string Isbn { get; set; }
        public Allapot Allapot { get; set; }

        public Konyv(string sor) 
        {
            (string a_szerzo, string a_cim, int a_ev, string a_isbn, Allapot a_allapot) = this.parse_from_line(sor);

            this.Szerzo = a_szerzo;
            this.Cim = a_cim;
            this.Kiadas_Eve = a_ev;
            this.Isbn = a_isbn;
            this.Allapot = a_allapot;

        }


        public (string a_szerzo,string a_cim, int a_ev, string a_isbn, Allapot a_allapot) parse_from_line(string line)
        {
            string[] arra = line.Split(",");
            if (arra.Length!=5)
            {
                throw new ArgumentException($"Invalid input line: {line}");
            }
            string a_szerzo = arra[0];
            string a_cim = arra[1];
            int a_ev = int.Parse(arra[2]);
            string a_isbn = arra[3];

            Allapot a_allapot;
            if (!Enum.TryParse(arra[4], out Allapot state))
            {
                throw new ArgumentException($"Invalid állapot: {arra[4]}");
            }
            else
            {
                a_allapot = state;
            }


            return (a_szerzo, a_cim, a_ev, a_isbn, a_allapot);
        }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> data;
        public string[] headers;
        public List<Konyv> konyvtar;

        public MainWindow()
        {
            InitializeComponent();

            data = new List<string>();
            konyvtar = new List<Konyv>();

            load_button.Click += (s,e) =>
            {
                try
                {
                    read_lines();
                    parse_lines();
                    clear_lines();
                    display_lines();
                    MessageBox.Show("the loading worked");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("not working : "+ex.Message);
                }
            };

            close_button.Click += (s, e) =>
            {
                close_ask();
            };
            clear_button.Click += (s, e) =>
            {
                clear_lines();
            };
        }



        public void read_lines()
        {
            if (!File.Exists("konyvtar.txt"))
                throw new FileNotFoundException("The file 'konyvtar.txt' was not found.");

            string[] file_lines = File.ReadAllLines("konyvtar.txt");

            // Skip header line
            data = file_lines.Skip(1).ToList();
            headers = file_lines[0].ToBasicForm().Replace(' ','_').Split(',');
        }
        public void parse_lines()
        {
            foreach (var line in data)
            {
                konyvtar.Add(
                    new Konyv(line)
                );
            }
        }

        public void display_lines()
        {

            // Initialize columns
            if (myDataGrid.Columns.Count==0)
            {
                foreach (var header in headers)
                {

                    myDataGrid.Columns.Add(new DataGridTextColumn
                    {
                        Header = header.Replace('_', ' '),

                        Binding = new System.Windows.Data.Binding(header == "ISBN" ? header.CapitalizeFront() : header)
                    });
                }
            }


            // Set ItemsSource
            myDataGrid.ItemsSource = konyvtar;
        }

        public void clear_lines()
        {
            myDataGrid.ItemsSource = null;
            myDataGrid.Items.Clear();
            myDataGrid.ItemsSource = null;
        }

        public void close_ask()
        {
            var result = MessageBox.Show("you sure you wanna leave?","leave",MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}