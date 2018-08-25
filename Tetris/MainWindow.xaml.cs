using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                        new Point(=1,0),
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
                    rotate = true;
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
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
