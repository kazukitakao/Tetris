using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer Timer;
        Board myBoard;

        public MainWindow()
        {
            InitializeComponent();
        }

        void MainWindowInitilized(object sender,EventArgs e)
        {
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(GameTick);
            Timer.Interval = new TimeSpan(0,0,0,0,400);
            GameStart();
        }

        private void GameStart()
        {
            MainGrid.Children.Clear();
            myBoard = new Board(MainGrid);
            Timer.Start();
        }

        void GameTick(object sender,EventArgs e)
        {
            Score.Content = myBoard.GetScore().ToString("0000000000");
            Lines.Content = myBoard.Getlines().ToString("0000000000");
            myBoard.CurrentTetriminoMoveDown();
        }

        private void GamePause()
        {
            if (Timer.IsEnabled)
            {
                Timer.Stop();
            }
            else
            {
                Timer.Start();
            }
        }

        private void HandleKeyDown(object sender,KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    if (Timer.IsEnabled)
                    {
                        myBoard.CurrentTetriminoMoveLeft();
                    }
                    break;
                case Key.Right:
                    if (Timer.IsEnabled)
                    {
                        myBoard.CurrentTetriminoMoveRight();
                    }
                    break;
                case Key.Down:
                    if (Timer.IsEnabled)
                    {
                        myBoard.CurrentTetriminoMoveDown();
                    }
                    break;
                case Key.Up:
                    if (Timer.IsEnabled)
                    {
                        myBoard.CurrentTetriminoMoveRotate();
                    }
                    break;
                case Key.F2:
                    GameStart();
                    break;
                case Key.F3:
                    GamePause();
                    break;
                default:
                    break;
            }
        }
    }
}
