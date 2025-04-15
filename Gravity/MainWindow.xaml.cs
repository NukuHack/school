using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gravity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public DispatcherTimer timer;
        private bool isGamePaused = false;
        public Vector movement;
        public MainWindow()
        {
            InitializeComponent();

            this.KeyDown += MainWindow_KeyDown;
            movement= new Vector(0,0);


            var timer = new DispatcherTimer // ~60 FPS
            { Interval = TimeSpan.FromMilliseconds(16) };

            timer.Tick += (s, e) => Timer_Tick();
            timer.Start();
        }

        public void Timer_Tick()
        {
            if (isGamePaused) return;
            update_gravity();
            update_movement();

        }

        public void update_gravity()
        {
            if (cube != null)
            {
                movement= new Vector(movement.X + 1,movement.Y);
            }
        }
        private void update_movement()
        {
            if (cube != null)
            {
                var to_topp = Canvas.GetTop(cube);
                var to_left = Canvas.GetLeft(cube);
                Canvas.SetTop(cube, to_topp + movement.X*0.7);
                Canvas.SetLeft(cube, to_left + movement.Y * 0.7);

                movement = new Vector(movement.X * 0.4, movement.Y * 0.4);
            }
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key;
            switch (key)
            {
                case Key.Escape:
                    break;
                case Key.Space:
                    jump();
                    break;
                case Key.W:
                    move(-5, 0);
                    break;
                case Key.A:
                    move(0, -5);
                    break;
                case Key.S:
                    move(+5, 0);
                    break;
                case Key.D:
                    move(0, +5);
                    break;
            }
        }

        private void move(int direction1, int direction2)
        {
            movement = new Vector(movement.X += direction1, movement.Y+= direction2);
        }

        private void jump()
        {
            CenterOnCanvas(cube);
        }

        public void CenterOnCanvas(TextBox stuff)
        {
            if (cube != null)
            {
                movement = new Vector(-10, movement.Y);
            }
        }
    }
}