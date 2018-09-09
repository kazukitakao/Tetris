using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Tetris
{
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
            currentShape = SetRandomShape();
        }

        // TODO:プロパティに置き換え
        public Brush GetCurrentColor()
        {
            return currentColor;
        }

        public Point GetCurrentPosition()
        {
            return currentPosition;
        }

        public Point[] GetCurrentShape()
        {
            return currentShape;
        }

        public void MoveLeft()
        {
            currentPosition.X -= 1;
        }

        public void MoveRight()
        {
            currentPosition.X += 1;
        }

        public void MoveDown()
        {
            currentPosition.Y += 1;
        }

        public void MovRotate()
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

        private Point[] SetRandomShape()
        {
            Random rand = new Random();
            switch (rand.Next() % 7)
            {
                case 0: // I
                    rotate = true;
                    currentColor = Brushes.LightBlue;
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
                        new Point(1,1),
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
                    currentColor = Brushes.YellowGreen;
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
                        new Point(0,1)
                    };
                case 6:// Z
                    rotate = true;
                    currentColor = Brushes.Red;
                    return new Point[]
                    {
                        new Point(0,0),
                        new Point(-1,0),
                        new Point(0,1),
                        new Point(1,1)
                    };
                default:
                    return null;
            }
        }

    }

}
