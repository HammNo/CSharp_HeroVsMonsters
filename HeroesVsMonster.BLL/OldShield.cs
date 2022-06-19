using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class OldShield : Armor
    {
        public OldShield() : base()
        {
            _life += 3;
        }
    }
}
