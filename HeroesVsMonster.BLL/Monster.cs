using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Monster : Character
    {
        protected int _exp_bonus;
        public int ExpBonus
        {
            get { return _exp_bonus; }
        }
        public Monster(string name, int stamina, int strenght, int life, int speed) : base(name, stamina, strenght, life, speed)
        {
        }
    }
}
