using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace PingPong
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Sets the speed of the racket
        private int racketSpeed = 8;

        //Starting speed for the ball
        private double speedX = 2;
        private double speedY = 2;

        //Chooses the direction for the racket
        private bool goLeft;
        private bool goRight;

        //Cooldown to prevent re-collision (ball getting stuck)
        bool cooldownRacket;
        bool cooldownLeft;
        bool cooldownRight;
        bool cooldownTop;
        bool cooldownBottom;

        //Point system
        int points;


        //Creates the timer
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();

            //Spawns the ball at a random location
            Random rnd = new Random();
            Canvas.SetTop(Ball, rnd.Next(250));
            Canvas.SetLeft(Ball, rnd.Next(750));

            //Runs timer_tick method for every tick
            dispatcherTimer.Tick += Timer_Tick;

            //Sets timer interval to 1 millisecond
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1);

            //Starts the time
            dispatcherTimer.Start();

            //Placing the racket at the middle
            Loaded += delegate
            {
                Canvas.SetTop(Racket, MyCanvas.ActualHeight - (MyCanvas.ActualHeight / 10));
                Canvas.SetLeft(Racket, MyCanvas.ActualWidth - (MyCanvas.ActualWidth / 2 + (Racket.Width / 2)));
            };
        }

        private void Canvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = true; // Left key is pressed go Left will be true
            }
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
            else if (e.Key == Key.Right)
            {
                goRight = true; // right key is pressed go right will be true
            }
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                goLeft = false; // Left key is released go Left will be false
            }
            else if (e.Key == Key.Right)
            {
                goRight = false; // right key is released go right will be false
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Object ball = new Object(Ball);
            Object racket = new Object(Racket);
            Object border = new Object(Border);


            //Ball movement
            Canvas.SetTop(Ball, ball.Top += speedY);
            Canvas.SetLeft(Ball, ball.Left += speedX);


            //Controls the racket
            if (goLeft && racket.Left > 0)
            {
                Canvas.SetLeft(Racket, racket.Left - racketSpeed);
            }

            if (goRight && racket.Left + (Racket.Width * 1.1) < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(Racket, racket.Left + racketSpeed);
            }


            //Ball & racket collision
            if (ball.Bottom >= racket.Top && ball.Bottom <= racket.Bottom && ball.Left >= racket.Left && ball.Right <= racket.Right && !cooldownRacket)
            {
                cooldownRacket = true;
                cooldownLeft = false;
                cooldownRight = false;
                cooldownTop = false;
                cooldownBottom = false;
                speedX *= 1.05;
                speedY *= 1.05;
                speedY= -speedY;
                points++;
            }

            //Ball & border collision
            if (ball.Left <= border.Left && !cooldownLeft)
            {
                cooldownLeft= true;
                cooldownRight= false;
                speedX = -speedX;
            }

            if (ball.Right >= Border.ActualWidth && !cooldownRight)
            {
                cooldownRight= true;
                cooldownLeft = false;
                speedX = - speedX;
            }

            if (ball.Top <= 0.0 && !cooldownTop)
            {
                cooldownTop= true;
                cooldownRacket = false;
                speedY = - speedY;
            }

            if (ball.Bottom >= border.Bottom - 20 && !cooldownBottom)
            {
                cooldownBottom= true;
                dispatcherTimer.Stop();
                infoText.Text = "Game Over \nPress ESC to exit";
                
            }
            pointText.Text= $"Points: {points}";
        }
    }
}