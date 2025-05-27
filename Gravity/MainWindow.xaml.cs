using System.Numerics;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Gravity
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _gameTimer;
        private bool _isGamePaused = false;
        private Entity _player;
        private Stopwatch _gameStopwatch = Stopwatch.StartNew();
        private float _previousTime = 0f;

        // Physics constants
        private const float GRAVITY = 9.8f;
        private const float JUMP_FORCE = -15f;
        private const float MOVEMENT_FORCE = 25f;
        private const float FRICTION = 0.9f;
        private const float AIR_RESISTANCE = 0.98f;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            CenterPlayerOnCanvas();
            this.KeyDown += HandleKeyInput;
            this.KeyUp += HandleKeyRelease;
        }

        private void InitializeGame()
        {
            var wid = (float)(canvas.ActualWidth / 2 - PlayerCube.Width / 2);
            var hei = (float)(canvas.ActualHeight / 2 - PlayerCube.Height / 2);
            var pos = new Vector2(wid, hei);
            _player = new Entity(pos)
            {
                MaxSpeed = 150f,
                Grounded = false
            };

            // ~60 FPS game loop
            _gameTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };
            _gameTimer.Tick += (s, e) => UpdateGame();
            _gameTimer.Start();
        }

        private void UpdateGame()
        {
            if (_isGamePaused) return;

            float currentTime = (float)_gameStopwatch.Elapsed.TotalSeconds;
            float deltaTime = currentTime - _previousTime;

            ProcessMovement();

            // Apply gravity if not grounded
            if (!_player.Grounded)
            {
                _player.ApplyForce(new Vector2(0, GRAVITY));
            }

            // Apply friction if grounded
            if (_player.Grounded)
            {
                _player.Velocity *= FRICTION;
            }
            else
            {
                _player.Velocity *= AIR_RESISTANCE;
            }

            _player.Update(deltaTime);
            CheckBoundaries();
            UpdatePlayerPosition();

            _previousTime = currentTime;
        }

        private void CheckBoundaries()
        {
            // Floor collision (check Y position + height against canvas height)
            if (_player.Position.Y + PlayerCube.Height >= canvas.ActualHeight)
            {
                _player.Position = new Vector2(_player.Position.X, (float)(canvas.ActualHeight - PlayerCube.Height));
                _player.Velocity = new Vector2(_player.Velocity.X, 0);
                _player.Grounded = true;
            }
            else
            {
                _player.Grounded = false;
            }

            // Ceiling collision (check Y position against 0)
            if (_player.Position.Y <= 0)
            {
                _player.Position = new Vector2(_player.Position.X, 0);
                _player.Velocity = new Vector2(_player.Velocity.X, 0);
            }

            // Left/Right walls
            if (_player.Position.X <= 0)
            {
                _player.Position = new Vector2(0, _player.Position.Y);
                _player.Velocity = new Vector2(0, _player.Velocity.Y);
            }
            else if (_player.Position.X + PlayerCube.Width >= canvas.ActualWidth)
            {
                _player.Position = new Vector2((float)(canvas.ActualWidth - PlayerCube.Width), _player.Position.Y);
                _player.Velocity = new Vector2(0, _player.Velocity.Y);
            }
        }

        private void UpdatePlayerPosition()
        {
            if (PlayerCube != null)
            {
                Canvas.SetLeft(PlayerCube, _player.Position.X);
                Canvas.SetTop(PlayerCube, _player.Position.Y);
            }
        }

        // Track which keys are currently pressed
        private HashSet<Key> _pressedKeys = new HashSet<Key>();

        private void HandleKeyInput(object sender, KeyEventArgs e)
        {
            if (_isGamePaused) return;

            if (!_pressedKeys.Contains(e.Key))
            {
                _pressedKeys.Add(e.Key);

                switch (e.Key)
                {
                    case Key.Space:
                        if (_player.Grounded)
                        {
                            _player.ApplyForce(new Vector2(0, JUMP_FORCE));
                            _player.Grounded = false;
                        }
                        break;
                    case Key.Escape:
                        TogglePause();
                        break;
                    case Key.LeftCtrl:
                        CenterPlayerOnCanvas();
                        break;
                }
            }
        }

        private void HandleKeyRelease(object sender, KeyEventArgs e)
        {
            _pressedKeys.Remove(e.Key);
        }

        private void TogglePause()
        {
            _isGamePaused = !_isGamePaused;

            if (_isGamePaused)
            {
                PauseLable.Visibility = Visibility.Visible;
                _gameTimer.Stop();
                _gameStopwatch.Stop();
            }
            else
            {
                PauseLable.Visibility = Visibility.Hidden;
                _gameStopwatch.Restart();
                _previousTime = 0f;
                _gameTimer.Start();
            }
        }

        // In ProcessMovement(), swap the Y and X axes for W/S and A/D:
        private void ProcessMovement()
        {
            if (_isGamePaused) return;

            Vector2 movementForce = Vector2.Zero;

            if (_pressedKeys.Contains(Key.W))
                movementForce.Y -= MOVEMENT_FORCE;  // Up
            if (_pressedKeys.Contains(Key.S))
                movementForce.Y += MOVEMENT_FORCE;  // Down
            if (_pressedKeys.Contains(Key.A))
                movementForce.X -= MOVEMENT_FORCE;  // Left
            if (_pressedKeys.Contains(Key.D))
                movementForce.X += MOVEMENT_FORCE;  // Right

            _player.ApplyForce(movementForce);
        }

        public void CenterPlayerOnCanvas()
        {
            if (PlayerCube != null)
            {
                var wid = (float)(canvas.ActualWidth / 2 - PlayerCube.Width / 2);
                var hei = (float)(canvas.ActualHeight / 2 - PlayerCube.Height / 2);
                _player.Position = new Vector2(wid, hei);
                _player.Velocity = Vector2.Zero;
                _player.Acceleration = Vector2.Zero;
                UpdatePlayerPosition();
            }
        }
    }
}