using HeroesVsMonster.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HvsM_Con
{
    class CharacBarContainer : Container
    {
        private Character _charac;
        public Character Charac
        {
            get { return _charac; }
            set { _charac = value; }
        }
        public CharacBarContainer(int[] first_point, int height, int width, Character charac) : base(first_point, height, width)
        {
            Charac = charac;
            if (Charac is not null)
            {
                DrawOutlines();
                PrintContent();
            }
        }
        public override void DrawOutlines()
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(FirstPoint[0], FirstPoint[1]);
            Console.ForegroundColor = (Charac is Hero) ? ConsoleColor.Yellow : ConsoleColor.Red;
            Console.Write("╔");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╗");
            for (int i = 0; i < Height; i++)
            {
                Console.SetCursorPosition(FirstPoint[0], FirstPoint[1] + i + 1);
                Console.Write("║");
                Console.SetCursorPosition(FirstPoint[0] + Width - 1, FirstPoint[1] + i + 1);
                Console.Write("║");
            }
            Console.SetCursorPosition(FirstPoint[0], FirstPoint[1] + Height + 1);
            Console.Write("╚");
            for (int i = 0; i < Width - 2; i++)
            {
                Console.Write("═");
            }
            Console.Write("╝");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void PrintContent()
        {
            Clear();
            Console.CursorVisible = false;
            Console.SetCursorPosition(FirstPoint[0] + 1, FirstPoint[1] + 1);
            Console.Write($" {Charac.Name} {Charac.Level}");
            Console.SetCursorPosition(FirstPoint[0] + 15, FirstPoint[1] + 1);
            Console.Write($"{Charac.Current_life} / {Charac.Life}");
            float calc = (float)Charac.Current_life / (float) Charac.Life * (float) 20;
            ConsoleColor color;
            if (calc < 6.66) color = ConsoleColor.Red;
            else if (calc > 13.33) color = ConsoleColor.Green;
            else color = ConsoleColor.Magenta;
            Console.SetCursorPosition(FirstPoint[0] + 24, FirstPoint[1] + 1);
            Console.BackgroundColor = color;
            for (int i = 0; i < calc; i++)
            {
                Console.Write("/");
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
