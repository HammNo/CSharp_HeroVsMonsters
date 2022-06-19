using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Item
    {
        protected Character _owner;
        public Character Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
    }
}
