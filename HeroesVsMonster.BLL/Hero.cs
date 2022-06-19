using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Hero : Character
    {
        private int _experience;
        public int Experience
        {
            get { return _experience; }
            private set { _experience = value; }
        }
        public Hero(string name, int stamina, int strenght, int life, int speed) : base(name, stamina, strenght, life, speed)
        {
            Experience = 0;
        }
        public void RestoreAllLife()
        {
            _current_life = _life;
        }
        public void GainExp(int exp)
        {
            Experience += exp;
        }
        public void EquipItem(Equipment equip)
        {
            if (equip != null && !equip.Equiped)
            {
                _life += equip.Life;
                _current_life += equip.Life;
                _stamina += equip.Stamina;
                _strenght += equip.Strenght;
                _speed += equip.Speed;
                equip.Equiped = true;
            }
        }
        public void UnequipItem(Equipment equip)
        {
            if (equip != null && equip.Equiped)
            {
                _life -= equip.Life;
                _current_life -= equip.Life;
                _stamina -= equip.Stamina;
                _strenght -= equip.Strenght;
                _speed -= equip.Speed;
                equip.Equiped = false;
            }
        }
    }
}
