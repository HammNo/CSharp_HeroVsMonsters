using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Consumable : Item
    {
        protected Action _use;
        public Action Use
        {
            get { return _use; }
        }
    }
}
