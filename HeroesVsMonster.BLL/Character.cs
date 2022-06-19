using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public abstract class Character
    {
        #region fields
        protected int _stamina, _strenght, _speed, _life, _current_life, _gold, _leather;
        private string _name;
        private List<Item> _bag;
        protected int _level;
        #endregion
        #region properties
        public int Stamina
        {
            get { return _stamina; }
            private set { _stamina = value; }
        }
        public int Strenght
        {
            get { return _strenght; }
            private set { _strenght = value; }
        }
        public int Gold
        {
            get { return _gold; }
            set { _gold = value; }
        }
        public int Leather
        {
            get { return _leather; }
            set { _leather = value; }
        }
        public int Level
        {
            get { return _level; }
        }
        public int Life
        {
            get { return _life; }
        }
        public int Current_life
        {
            get { return _current_life; }
            private set { _current_life = value; }
        }
        public List<Item> Bag
        {
            get { return _bag; }
            private set { _bag = value; }
        }
        public int Speed
        {
            get { return _speed; }
        }
        public string Name
        {
            get { return _name; }
            private set { _name = value; }
        }
        public bool IsAlive
        {
            get { return Current_life > 0; }
        }
        #endregion
        #region constructors
        public Character(string name, int stamina, int strenght, int life, int speed)
        {
            _name = name;
            _stamina = stamina;
            _strenght = strenght;
            _life = life;
            _current_life = life;
            _speed = speed;
            Gold = 0;
            Leather = 0;
            Bag = new List<Item>();
            _level = 1;
        }
        #endregion
        public void GetHurted(int damages)
        {
            _current_life -= damages;
            if (_current_life < 0) _current_life = 0;
        }
        public virtual void Hit(int damages, Character enemy)
        {
            if (enemy is null) throw new NullReferenceException();
            else enemy.GetHurted(damages);
        }
        public void RestoreLife(int toRestore)
        {
            if (Current_life + toRestore <= Life) Current_life += toRestore;
            else Current_life = Life;
        }
        public void AddItem(Item toAdd)
        {
            if (toAdd != null)
            {
                Bag.Add(toAdd);
            }
        }
        public void RemoveItem(Item toRemove)
        {
            if (toRemove != null) Bag.Remove(Bag.Find(x => x == toRemove));
        }
        public void LevelUp()
        {
            _level++;
            _life = (int)(Life * 1.3);
            _stamina = (int)(_stamina * 1.3);
            _strenght = (int)(_strenght * 1.3);
            _speed = (int)(_speed * 1.3);
            _current_life = _life;
        }
    }
}
