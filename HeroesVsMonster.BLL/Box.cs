using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class Box
    {
        #region fields
        private Character _charac;
        private bool _visible;
        private int _x, _y;
        private Enums.BoxState _state;
        #endregion
        #region properties
        public Character Charac
        {
            get { return _charac; }
            set { _charac = value; }
        }
        public bool Visible
        {
            get { return _visible; }
            private set { _visible = value; }
        }
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }
        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public Enums.BoxState State
        {
            get { return _state; }
            set { _state = value; }
        }
        #endregion
        #region constructors
        public Box()
        {
            Charac = null;
            Visible = false;
            State = Enums.BoxState.Empty;
        }
        #endregion
        public void SetVisibility(bool visible)
        {
            Visible = visible;
        }
    }
}
