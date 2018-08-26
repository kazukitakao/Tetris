using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Tetris
{

    public class Board
    {
        private int Rows;
        private int Cols;
        private int Score;
        private int LinesFilled;
        private Tetramino currentTetrimino;
        private Label[,] BlockControls;

        static private Brush NoBrush = Brushes.Transparent;
        static private Brush SilverBrush = Brushes.Gray;

        public Board(Grid TetrisGrid)
        {
            Rows = TetrisGrid.RowDefinitions.Count;
            Cols = TetrisGrid.ColumnDefinitions.Count;
            Score = 0;
            LinesFilled = 0;

            // TODO:できたらネストを浅くする。foreachで書き直す
            BlockControls = new Label[Cols,Rows];
            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    BlockControls[i, j] = new Label
                    {
                        Background = NoBrush,
                        BorderBrush = SilverBrush,
                        BorderThickness = new Thickness(1, 1, 1, 1)
                    };
                    Grid.SetRow(BlockControls[i,j],j);
                    Grid.SetColumn(BlockControls[i, j], i);
                    TetrisGrid.Children.Add(BlockControls[i,j]);
                }
            }
            currentTetrimino = new Tetramino();
            currentTetriminoDraw();
        }

        // TODO:ここもプロパティに
        public int GetScore()
        {
            return Score;
        }

        public int Getlines()
        {
            return LinesFilled;
        }

        private void currentTetriminoDraw()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            Brush Color = currentTetrimino.GetCurrentColor();

            foreach (Point s in Shape)
            {
                BlockControls[(int)(s.X + position.X) + ((Cols / 2) - 1), 
                    (int)(s.Y + position.Y + 2)].Background = Color;
            }
        }

        private void currentTetriminoErase()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            Brush Color = currentTetrimino.GetCurrentColor();

            foreach (Point s in Shape)
            {
                BlockControls[(int)(s.X + position.X) + (Cols / 2) - 1, 
                    (int)(s.Y + position.Y + 2)].Background = NoBrush;
            }
        }

        private void CheckRows()
        {
            bool full;
            for (int i = Rows - 1; i < 0; i++)
            {
                full = true;
                for (int j = 0; j < Cols; i++)
                {
                    if (BlockControls[j,i].Background == NoBrush)
                    {
                        full = false;
                    }
                }
                if (full)
                {
                    RemoveRow(i);
                    Score += 100;
                    LinesFilled += 1;
                }
            }
            
        }

        private void RemoveRow(int row)
        {
            for (int i = row; i < 2; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    BlockControls[j, i].Background = BlockControls[j,i - 1].Background;
                }
            }
        }

        // TODO:処理が冗長なのでコンパクトにできないか検討
        public void CurrentTetriminoMoveRight()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            currentTetriminoErase();
            foreach (Point s in Shape)
            {
                if ((int)(s.X + position.X) + (((Cols / 2) - 1) - 1) >= Cols)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + position.X) + ((Cols / 2) - 1) + 1),
                    (int)(s.Y + position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currentTetrimino.moveRight();
                currentTetriminoDraw();
            }
            else
            {
                currentTetriminoDraw();
            }
        }
        public void CurrentTetriminoMoveLeft()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            currentTetriminoErase();
            foreach (Point s in Shape)
            {
                if ((int)(s.X + position.X) + (((Cols / 2) - 1) - 1) < 0)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + position.X) + ((Cols / 2) - 1) - 1),
                    (int)(s.Y + position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currentTetrimino.moveLeft();
                currentTetriminoDraw();
            }
            else
            {
                currentTetriminoDraw();
            }
        }
        public void CurrentTetriminoMoveDown()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            foreach (Point s in Shape)
            {
                if ((int)(s.Y + position.Y +2 + 1) >= Rows)
                {
                    move = false;
                }
                else if (BlockControls[((int)(s.X + position.X) + ((Cols / 2) - 1)),
                    (int)(s.Y + position.Y) + 2 + 1].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currentTetrimino.moveDown();
                currentTetriminoDraw();
            }
            else
            {
                currentTetriminoDraw();
                CheckRows();
                currentTetrimino = new Tetramino();
            }
        }
        public void CurrentTetriminoMoveRotate()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] s = new Point[4];
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            Shape.CopyTo(s,0);
            currentTetriminoErase();
            for (int i = 0; i < s.Length; i++)
            {
                double x = s[i].X;
                s[i].X = s[i].Y;
                s[i].Y = x;
                if (((int)((s[i].Y + position.Y) + 2)) >= Rows)
                {
                    move = false;
                }
                else if (((int)((s[i].X + position.X) + (Cols / 2) - 1)) < 0)
                {

                }
                else if (((int)((s[i].X + position.X) + (Cols / 2) - 1)) >= Rows)
                {

                }
                else if (BlockControls[(int)(s[i].X + position.X) + ((Cols / 2) - 1),
                    (int)(s[i].Y + position.Y + 2)].Background != NoBrush)
                {
                    move = false;
                }
            }
            if (move)
            {
                currentTetrimino.movRotate();
                currentTetriminoDraw();
            }
            else
            {
                currentTetriminoDraw();
            }
        }

    }
    public class Tetramino
    {
        private Point currentPosition;
        private Point[] currentShape;
        private Brush currentColor;
        private Boolean rotate;

        public Tetramino()
        {
            currentPosition = new Point(0, 0);
            currentColor = Brushes.Transparent;
            currentShape = setRandomShape();
        }

        // TODO:プロパティに置き換え
        public Brush GetCurrentColor()
        {
            return currentColor;
        }

        public Point getCurrentPosition()
        {
            return currentPosition;
        }

        public Point[] GetCurrentShape()
        {
            return currentShape;
        }

        public void moveLeft()
        {
            currentPosition.X -= 1;
        }

        public void moveRight()
        {
            currentPosition.X += 1;
        }

        public void moveDown()
        {
            currentPosition.Y += 1;
        }

        public void movRotate()
        {
            if (rotate)
            {
                for (int i = 0; i < currentShape.Length; i++)
                {
                    double x = currentShape[i].X;
                    currentShape[i].X = currentShape[i].Y * -1;
                    currentShape[i].Y = x;
                }
            }
        }

        private Point[] setRandomShape()
        {
            Random rand = new Random();
            switch (rand.Next() % 7)
            {
                case 0: // I
                    rotate = true;
                    currentColor = Brushes.Cyan;
                    return new Point[] 
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(2,0)
                    };
                case 1:// J
                    rotate = true;
                    currentColor = Brushes.Blue;
                    return new Point[]
                    {
                        new Point(1,-1),
                        new Point(1,0),
                        new Point(0,0),
                        new Point(0,0)
                    };
                case 2:// L
                    rotate = true;
                    currentColor = Brushes.Orange;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(1,0),
                        new Point(1,-1)
                    };
                case 3:// O
                    rotate = false;
                    currentColor = Brushes.Yellow;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(0,1),
                        new Point(1,0),
                        new Point(1,1)
                    };
                case 4:// S
                    rotate = true;
                    currentColor = Brushes.Green;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,0)
                    };
                case 5:// T
                    rotate = true;
                    currentColor = Brushes.Purple;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,-1),
                        new Point(1,1)
                    };
                case 6:// Z
                    rotate = true;
                    currentColor = Brushes.Red;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(0,1),
                        new Point(1,1)
                    };
                default:
                    return null;
            }
        }

    }


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
