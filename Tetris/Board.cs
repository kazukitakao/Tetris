using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            BlockControls = new Label[Cols, Rows];
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
                    Grid.SetRow(BlockControls[i, j], j);
                    Grid.SetColumn(BlockControls[i, j], i);
                    TetrisGrid.Children.Add(BlockControls[i, j]);
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
            for (int i = Rows - 1; i > 0; i--)
            {
                full = true;
                for (int j = 0; j < Cols; j++)
                {
                    if (BlockControls[j, i].Background == NoBrush)
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
            for (int i = row; i > 2; i--)
            {
                for (int j = 0; j < Cols; j++)
                {
                    BlockControls[j, i].Background = BlockControls[j, i - 1].Background;
                }
            }
        }

        // TODO:処理が冗長なのでコンパクトにできないか検討
        // TODO:Move系全般に枠外に出ると範囲外でエラー
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

        /// <summary>
        /// テトリミノが落ちたときの処理
        /// </summary>
        public void CurrentTetriminoMoveDown()
        {
            // テトリミノの位置
            Point position = currentTetrimino.getCurrentPosition();
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            currentTetriminoErase();
            foreach (Point s in Shape)
            {
                if ((int)(s.Y + position.Y + 2 + 1) >= Rows)
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

        /// <summary>
        /// テトリミノを回転させた時bの処理
        /// </summary>
        public void CurrentTetriminoMoveRotate()
        {
            Point position = currentTetrimino.getCurrentPosition();
            Point[] s = new Point[4];
            Point[] Shape = currentTetrimino.GetCurrentShape();
            bool move = true;
            Shape.CopyTo(s, 0);
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
}
