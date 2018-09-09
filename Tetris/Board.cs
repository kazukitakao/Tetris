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
        private Tetramino _currentTetrimino;
        private Label[,] BlockControls;

        static private Brush NoBrush = Brushes.Transparent;
        static private Brush SilverBrush = Brushes.Gray;

        /// <summary>
        /// 現在のテトリミノの位置
        /// </summary>
        public Point CurrentPoint {
            get{ return _currentTetrimino.GetCurrentPosition();}
        }

        /// <summary>
        /// コンスタラクタ
        /// </summary>
        /// <param name="TetrisGrid"></param>
        public Board(Grid TetrisGrid)
        {
            Rows = TetrisGrid.RowDefinitions.Count;
            Cols = TetrisGrid.ColumnDefinitions.Count;
            Score = 0;
            LinesFilled = 0;

            // TODO:できたらネストを浅くする。foreachで書き直す
            BlockControls = new Label[Cols, Rows];
            for (int ColCount = 0; ColCount < Cols; ColCount++)
            {
                for (int RowCount = 0; RowCount < Rows; RowCount++)
                {
                    BlockControls[ColCount, RowCount] = new Label
                    {
                        Background = NoBrush,
                        BorderBrush = SilverBrush,
                        BorderThickness = new Thickness(1, 1, 1, 1)
                    };

                    Grid.SetRow(BlockControls[ColCount, RowCount], RowCount);
                    Grid.SetColumn(BlockControls[ColCount, RowCount], ColCount);
                    TetrisGrid.Children.Add(BlockControls[ColCount, RowCount]);
                }
            }

            _currentTetrimino = new Tetramino();
            CurrentTetriminoDraw();
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

        /// <summary>
        /// テトリミノの描写
        /// </summary>
        private void CurrentTetriminoDraw()
        {
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] Shapes = _currentTetrimino.GetCurrentShape();
            Brush Color = _currentTetrimino.GetCurrentColor();

            foreach (Point Shape in Shapes)
            {
                BlockControls[(int)(Shape.X + position.X) + ((Cols / 2) - 1),
                    (int)(Shape.Y + position.Y + 2)].Background = Color;
            }
        }

        /// <summary>
        /// テトリミノの削除処理
        /// </summary>
        private void CurrentTetriminoErase()
        {
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] Shape = _currentTetrimino.GetCurrentShape();
            Brush Color = _currentTetrimino.GetCurrentColor();

            foreach (Point s in Shape)
            {
                BlockControls[(int)(s.X + position.X) + (Cols / 2) - 1,
                    (int)(s.Y + position.Y + 2)].Background = NoBrush;
            }
        }

        /// <summary>
        /// テトリミノが埋まっているかどうかの判定
        /// </summary>
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

        /// <summary>
        /// 揃ったテトリミノを削除する
        /// </summary>
        /// <param name="row"></param>
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


        // 判定用の関数を作る必要があるか
        // 左右の動きによって判定ポイントが違う

        // TODO:処理が冗長なのでコンパクトにできないか検討
        // TODO:Move系全般に枠外に出ると範囲外でエラー
        /// <summary>
        /// テトリミノを右に移動させたときの処理
        /// </summary>
        public void CurrentTetriminoMoveRight()
        {
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] Shapes = _currentTetrimino.GetCurrentShape();
            bool move = true;
            CurrentTetriminoErase();
            foreach (Point Shape in Shapes)
            {
                // 移動場所が盤面の範囲を超えたら即false
                // 値を判定しないと配列要素のインデックスを超えるためエラーになる
                // xの横の範囲が超えたらそのまま描写する
                
                // テトリミノの形と形成している座標と現在の座標
                if ((int)(Shape.X + position.X) + (((Cols / 2) - 1)) >= Cols - 1)
                {
                    move = false;
                }
                else  if (BlockControls[((int)(Shape.X + position.X) + ((Cols / 2) - 1) - 1),
                    (int)(Shape.Y + position.Y) + 2].Background != NoBrush)
                {
                    move = false;
                }
            }

            if (move)
            {
                _currentTetrimino.MoveRight();
                CurrentTetriminoDraw();
            }
            else
            {
                CurrentTetriminoDraw();
            }
        }

        /// <summary>
        /// テトリミノを左に移動させてたときの処理
        /// </summary>
        public void CurrentTetriminoMoveLeft()
        {
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] Shape = _currentTetrimino.GetCurrentShape();
            bool move = true;
            CurrentTetriminoErase();
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
                _currentTetrimino.MoveLeft();
                CurrentTetriminoDraw();
            }
            else
            {
                CurrentTetriminoDraw();
            }
        }

        /// <summary>
        /// テトリミノが落ちたときの処理
        /// </summary>
        public void CurrentTetriminoMoveDown()
        {
            // テトリミノの位置
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] Shape = _currentTetrimino.GetCurrentShape();
            bool move = true;
            CurrentTetriminoErase();
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
                _currentTetrimino.MoveDown();
                CurrentTetriminoDraw();
            }
            else
            {
                CurrentTetriminoDraw();
                CheckRows();
                _currentTetrimino = new Tetramino();
            }
        }

        /// <summary>
        /// テトリミノを回転させた時の処理
        /// </summary>
        public void CurrentTetriminoMoveRotate()
        {
            Point position = _currentTetrimino.GetCurrentPosition();
            Point[] s = new Point[4];
            Point[] Shape = _currentTetrimino.GetCurrentShape();
            bool move = true;
            Shape.CopyTo(s, 0);
            CurrentTetriminoErase();
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
                _currentTetrimino.MovRotate();
                CurrentTetriminoDraw();
            }
            else
            {
                CurrentTetriminoDraw();
            }
        }

    }
}
