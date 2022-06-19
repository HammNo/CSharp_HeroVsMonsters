using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class Orc : Monster
    {
        public Orc(string name, int stamina, int strenght, int life, int speed) : base(name, stamina, strenght, life, speed)
        {
            _exp_bonus = 10;
        }
    }
}
