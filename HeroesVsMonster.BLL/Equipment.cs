using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Equipment : Item
    {
        protected int _life, _stamina, _strenght, _speed;
        protected bool _equiped;
        public int Life
        {
            get { return _life; }
        }
        public int Stamina
        {
            get { return _stamina; }
        }
        public int Strenght
        {
            get { return _strenght; }
        }
        public int Speed
        {
            get { return _speed; }
        }
        public bool Equiped
        {
            get { return _equiped; }
            set { _equiped = value; }
        }
        public Equipment()
        {
            _life = 0;
            _stamina = 0;
            _strenght = 0;
            _speed = 0;
        }
    }
}
