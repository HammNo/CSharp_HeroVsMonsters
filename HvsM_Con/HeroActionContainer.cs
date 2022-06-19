using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HvsM_Con
{
    class HeroActionContainer : Container
    {
        private bool _combat, _in_bag;
        public bool Combat
        {
            get { return _combat; }
            set { _combat = value; }
        }
        public bool InBag
        {
            get { return _in_bag; }
            set { _in_bag = value; }
        }
        public HeroActionContainer(int[] first_point, int height, int width) : base(first_point, height, width)
        {
            Combat = false;
            InBag = false;
            DrawOutlines();
        }
        public override void DrawOutlines()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(FirstPoint[0], FirstPoint[1]);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("┌");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┐");
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(FirstPoint[0], FirstPoint[1] + i + 1);
                Console.Write("│");
                Console.SetCursorPosition(FirstPoint[0] + Width - 1, FirstPoint[1] + i + 1);
                Console.Write("│");
            }
            Console.SetCursorPosition(FirstPoint[0], FirstPoint[1] + Height + 1);
            Console.Write("└");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("─");
            }
            Console.Write("┘");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void PrintContent(List<string> toPrint)
        {
            Clear();
            Console.CursorVisible = false;
            int index = 0;
            foreach (string str in toPrint)
            {
                string linePrint = str;
                Console.SetCursorPosition(FirstPoint[0] + 1, FirstPoint[1] + 1 + index);
                Console.ForegroundColor = ConsoleColor.White;
                if (Combat)
                {
                    if (str.Length > 2 && str[0] == char.Parse("H"))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        linePrint = "  " + linePrint;
                    }
                    else if (str.Length > 2 && str[0] == char.Parse("M"))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        linePrint = "  " + linePrint;
                    }
                }
                if (Combat && !InBag && linePrint[1].ToString() != ">") ProgressiveLine(((Combat) ? "\t" : "") + linePrint);
                else Console.Write(" "+ linePrint);
                index ++;
            }
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
