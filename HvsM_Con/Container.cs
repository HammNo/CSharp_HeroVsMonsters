using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HvsM_Con
{
    abstract class Container
    {
        #region fields
        private int[] _first_point;
        private int _height;
        private int _width;
        #endregion
        #region properties
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }
        public int[] FirstPoint
        {
            get { return _first_point; }
            private set { _first_point = value; }
        }
        #endregion
        #region constructor
        public Container(int[] first_point, int height, int width)
        {
            FirstPoint = first_point;
            Height = height;
            Width = width;
        } 
        #endregion
        public abstract void DrawOutlines();
        public virtual void Clear()
        {
            Console.CursorVisible = false;
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(FirstPoint[0] + 1, FirstPoint[1] + i + 1);
                for (int j = 0; j < Width - 2; j++)
                {
                    Console.Write(" ");
                }
            }
        }
        protected void ProgressiveLine(string line)
        {
            Console.CursorVisible = false;
            for (int i = 0; i < line.Length; i++)
            {
                Console.Write(line[i]);
                if (line[i] != Char.Parse(" ")) Thread.Sleep(10);
            }
        }
    }
}
