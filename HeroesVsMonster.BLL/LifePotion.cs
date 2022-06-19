using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class LifePotion : Consumable
    {
        public LifePotion(Character owner)
        {
            Owner = owner;
            _use = () =>
            {
                if (owner != null)
                {
                    owner.RestoreLife(10);
                }
            };
        }
    }
}
