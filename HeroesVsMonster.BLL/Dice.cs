using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public static class Dice
    {
        public static int Roll(int min, int max)
        {
            return (new Random()).Next(min, max + 1);
        }
    }
}
