using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public struct Enums
    {
        public enum MenuOptions
        {
            Humain = 1,
            Nain = 2,
            Quitter = 3
        }
        public enum BoxState
        {
            Empty,
            Forest,
            Rock,
            Hero,
            Monster,
            Water,
            End
        }
        public enum BattleState
        {
            Won,
            Lost,
            Escaped
        }
        public enum BattleChoice
        {
            Attack,
            Bag,
            Escape
        }
        public enum ItemsTypes
        {
            Undefined,
            LifePotion,
            OldSword,
            OldShield
        }
        public enum HeroActionMenu
        {
            Main,
            Caracs,
            Bag,
            Options,
            Music,
            Exit
        }
        public enum Names
        {
            Thony,
            Adam,
            Hugo,
            Harry,
            Gauth,
            Nemo,
            Dany,
            Dylan,
            Gael,
            Jean,
            Paul,
            Thom,
            Marc,
            Math,
            Josh,
            Kent,
            Leo,
            Sami,
            Sarah,
            Luna,
            Arya,
            Jessi,
            Jenny,
            Julie,
            Sonia,
            Sora,
            Kim,
            Sophie,
            Kayla,
            Lise,
            Nath,
            Eve,
            Ines,
            Ivy,
            Alix,
            Yoko
        }
    }
}
