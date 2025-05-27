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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tank_naplo
{
    public class Tankolas
    {
        public string Date { get; set; }
        public string Rendszam { get; set; }
        public int Km { get; set; }
        public double Liter { get; set; }
        public double Avg { get; set; }

        public Tankolas(string d, string r, int k, double l)
        {
            Date = d;
            Rendszam = r;
            Km = k;
            Liter = l;
            Avg = (double)(k) / l;
        }

    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Tankolas> tankolasok;
        public MainWindow()
        {
            InitializeComponent();

            tankolasok = new();

            this.submit.Click += (s, e) =>
            {
                try { 
                    if(!inp_t(inp_d.Text) && !inp_t(inp_r.Text) && !inp_t(inp_k.Text) && !inp_t(inp_l.Text))
                    {
                        alert("Bad input text");
                    } else
                    {
                        store_data();
                        display_data();
                    }
                }
                catch
                {
                    alert("Nem működik a hozzáadás");
                }
            };


            this.close.Click += (s, e) =>
            {
                    var result = MessageBox.Show("Biztos kilépsz?", "Kilépés", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        Close();
                    }
            };

            this.load.Click += (s, e) =>
            {
                try
                {
                    load_data();
                }
                catch {
                    alert("Not working Loading");
                }
            };
            this.export.Click += (s, e) =>
            {
                try
                {
                    export_data();
                }
                catch
                {
                    alert("Not working Exporting");
                }
            };
        }

        public void store_data()
        {
            string d = inp_d.Text;
            string r = inp_r.Text;
            string k = inp_k.Text;
            string l = inp_l.Text;

            if (!int.TryParse(k, out int k_int))
            {
                alert("Km szám formátumba kell hogy legyen");
                return;
            }
            if (!double.TryParse(k, out double l_doub))
            {
                alert("Tankolt liter szám formátumba kell hogy legyen\n(ponttal elválasztva a tizedes vessző helyén)");
                return;
            }

            if (!(d.Length==11) || !(d[4] == '.') || !(d[10] == '.') || !(d[7] == '.'))
            {
                alert("Km szám formátumba kell hogy legyen ponttal elválasztva\nPélda : 2024.10.10.");
                return;
            }

            var Data = new Tankolas(d,r,k_int, l_doub);
            tankolasok.Add(Data);

            clean_input();
        }

        public void display_data()
        {
            datagrid.ItemsSource = tankolasok;
            datagrid.UpdateLayout();
        }


        public bool inp_t(string input)
        {
            if (input.Trim().Length != 0){return true;}
            else{return false;}
        }


        public void clean_input()
        {
            inp_d.Text = string.Empty;
            inp_r.Text = string.Empty;
            inp_k.Text = string.Empty;
            inp_l.Text = string.Empty;
        }


        public void alert(string error)
        {
            MessageBox.Show($"Error: {error}","Error");
        }

        public void load_data()
        {
            alert("Not yet implemented");
        }
        public void export_data()
        {
            alert("Not yet implemented");
        }
    }
}