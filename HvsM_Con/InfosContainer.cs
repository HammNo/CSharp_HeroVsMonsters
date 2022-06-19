using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HvsM_Con
{
    class InfosContainer : Container
    {
        private int _current_line;
        private int _current_line_removable;
        public int CurrentLine
        {
            get { return _current_line; }
            private set { _current_line = value; }
        }
        public int CurrentLineRemovable
        {
            get { return _current_line_removable; }
            private set { _current_line_removable = value; }
        }

        public InfosContainer(int[] first_point, int height, int width) : base(first_point, height, width)
        {
            DrawOutlines();
            CurrentLine = 0;
            CurrentLineRemovable = 0;
        }
        public override void DrawOutlines()
        {
            Console.CursorVisible = false;
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(FirstPoint[0], FirstPoint[1] + i);
                Console.Write("│");
            }
        }
        public  void PrintContent(string toPrint, bool sprite)
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(FirstPoint[0] +1, FirstPoint[1] + CurrentLine);
            if(!sprite) ProgressiveLine(" " + toPrint);
            else Console.Write(" " + toPrint);
            CurrentLine ++;
        }
        public void PrintTempContent(string toPrint)
        {
            Console.CursorVisible = false;
            if(CurrentLineRemovable > 2)
            {
                for(int i = 0; i < 3; i++)
                {
                    Console.SetCursorPosition(FirstPoint[0] + 1, FirstPoint[1] + CurrentLine + i);
                    Console.Write("                                                             ");
                }
                CurrentLineRemovable = 0;
            }
            Console.SetCursorPosition(FirstPoint[0] + 1, FirstPoint[1] + CurrentLine + CurrentLineRemovable);
            ProgressiveLine(" " + toPrint);
            if (CurrentLineRemovable == 2) Thread.Sleep(1500);
            CurrentLineRemovable++;
        }
        public override void Clear()
        {
            base.Clear();
            CurrentLine = 0;
        }
    }
}


