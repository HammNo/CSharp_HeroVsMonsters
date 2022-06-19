using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class Board
    {
        #region fields
        private static Board _board;
        private Hero _hero;
        private Grid _map;
        private int _game_level;
        #endregion
        #region properties
        public Hero Hero
        {
            get { return _hero; }
            set { _hero = value; }
        }
        public Grid Map
        {
            get { return _map; }
            private set { _map = value; }
        }
        public int GameLevel
        {
            get { return _game_level; }
            set { _game_level = value; }
        }
        #endregion
        #region singleton
        public static Board Instance
        {
            get
            {
                if (_board is null)
                {
                    _board = new Board();
                }
                return _board;
            }
        }
        #endregion
        #region constructor
        public Board()
        {
            Hero = null;
            Map = new Grid();
            GameLevel = 1;
            CreateMap();
        }
        #endregion        
        public void Reset()
        {
            _board = new Board();
        }
        public Character CreateCharacter<T>(string name)
            where T : Character
        {
            int stamina = 0, strenght = 0, life = 0, speed = 0;
            List<int> values = GetThreeRolls();
            foreach (int val in values) stamina += val;
            values = GetThreeRolls();
            foreach (int val in values) strenght += val;
            values = GetThreeRolls();
            foreach (int val in values) speed += val;
            life = stamina;
            life += Modificator(stamina);
            object[] param = new object[]
                {
                    name,
                    stamina,
                    strenght,
                    life,
                    speed
                };
            Character charac = (T)Activator.CreateInstance(typeof(T), param);
            if(charac is Monster)
            {
                charac.Gold = Dice.Roll(1, 6);
                charac.Leather = Dice.Roll(1, 4);
            }
            else
            {
                Map.Boxes_tab[Map.Current_box[0], Map.Current_box[1]].Charac = Hero;
                Map.Boxes_tab[Map.Current_box[0], Map.Current_box[1]].State = Enums.BoxState.Hero;
            }
            return charac;
        }
        private Monster CreaterRandomEnemy()
        {
            Monster monst = null;
            int rand = (new Random()).Next(0, 3);
            int rand2 = (new Random()).Next(0, Enum.GetValues(typeof(Enums.Names)).Length);
            string name = ((Enums.Names)rand2).ToString();
            switch (rand)
            {
                case 0:
                    monst = (Monster)CreateCharacter<Orc>(name);
                    break;
                case 1:
                    monst = (Monster)CreateCharacter<Wolf>(name);
                    break;
                case 2:
                    monst = (Monster)CreateCharacter<Dragonnet>(name);
                    break;
            }
            for(int i = 1; i < GameLevel; i++)
            {
                monst.LevelUp();
            }
            return monst;
        }
        private List<int> GetThreeRolls()
        {
            List<int> values = new List<int>();
            int min_index = 0;
            int min_val = int.MaxValue;
            for (int i = 0; i < 4; i++)
            {
                values.Add(Dice.Roll(1, 6));
                if (values[i] < min_val)
                {
                    min_index = i;
                    min_val = values[i];
                }
            }
            values.Remove(values[min_index]);
            return values;
        }
        private int Modificator(int base_stat)
        {
            int result_stat = 0;
            if (base_stat < 5) result_stat += -1;
            else if (base_stat >= 10 && base_stat < 15) result_stat += 1;
            else if (base_stat >= 15) result_stat += 2;
            return result_stat;
        }
        private void CreateMap()
        {
            int rows = 0, cols = 0, monsters_count = 0;
            int[] hero_position = new int[2];
            List<List<int>> canevas = new List<List<int>>();
            string path = @"Maps\map" + GameLevel + ".txt";
            StreamReader sr = null;
            if (File.Exists(path))
            {
                sr = File.OpenText(path);
                string line = sr.ReadLine();
                while(line is not null)
                {
                    string[] line_sep = line.Split(" ");
                    rows++;
                    List<int> map_line = new List<int>();
                    cols = 0;
                    foreach(string str in line_sep)
                    {
                        cols++;
                        int str_conv = 0;
                        int.TryParse(str, out str_conv);
                        if ((Enums.BoxState)str_conv == Enums.BoxState.Monster) monsters_count++;
                        else if ((Enums.BoxState)str_conv == Enums.BoxState.Hero)
                        {
                            hero_position[0] = rows - 1;
                            hero_position[1] = cols - 1;
                        }
                        map_line.Add(str_conv);
                    }
                    canevas.Add(map_line);
                    line = sr.ReadLine();
                }
                sr.Dispose();
                sr.Close();
            }
            List<Monster> monster_list = new List<Monster>();
            for(int i = 0; i < monsters_count; i++)
            {
                Monster monst = CreaterRandomEnemy();
                monster_list.Add(monst);
            }
            Map.NewBoxTab(rows, cols);
            Map.FillGrid(canevas, monster_list);
            Map.Current_box[0] = hero_position[0];
            Map.Current_box[1] = hero_position[1];
        }
        public List<string> GetCharacInfos(Character charac)
        {
            List<string> result = new List<string>();
            result.Add($"{charac.Name} ({charac.GetType().Name})");
            result.Add($" - {charac.Current_life} pv{((charac.Current_life > 1) ? "s" : "")}");
            result.Add($" - {charac.Stamina} (E)");
            result.Add($" - {charac.Strenght} (F)");
            result.Add($" - {charac.Speed} (V)");
            result.Add($" - $ Or : {charac.Gold} Cuir : {charac.Leather} $");
            return result;
        }
        public string Confrontation(Character charac1, Character charac2)
        {
            if (!charac1.IsAlive || !charac2.IsAlive) throw new ArgumentException();
            string log = "";
            int damages = Dice.Roll(1, 4);
            damages += Modificator(charac1.Strenght);
            try
            {
                charac1.Hit(damages, charac2);
                log += $"{((charac1 is Hero)? "H" : "M")} : {charac1.Name} frappe {charac2.Name} ({damages})";
            }
            catch (NullReferenceException)
            {
            }
            return log;
        }
        public bool HeroWin(Monster enemy)
        {
            if(Hero.Level < 2) Hero.RestoreAllLife();
            Hero.Gold += enemy.Gold;
            Hero.Leather += enemy.Leather;
            return HeroGainExp(enemy.ExpBonus);
        }
        public bool BattleTurn(Monster enemy, Enums.BattleChoice choice, List<string> toPrint)
        {
            if(choice == Enums.BattleChoice.Bag)
            {
                toPrint.Add(Confrontation(enemy, Hero));
                return false;
            }
            if (Hero.Speed > enemy.Speed)
            {
                if (choice == Enums.BattleChoice.Escape) return true;
                try
                {
                    toPrint.Add(Confrontation(Hero, enemy));
                    toPrint.Add(Confrontation(enemy, Hero));
                }
                catch (ArgumentException) { }
            }
            else if (Hero.Speed == enemy.Speed)
            {
                int rand = Dice.Roll(0, 1);
                if (rand == 0)
                {
                    if (choice == Enums.BattleChoice.Escape) return true;
                    try
                    {
                        toPrint.Add(Confrontation(Hero, enemy));
                        toPrint.Add(Confrontation(enemy, Hero));
                    }
                    catch (ArgumentException) { }
                }
                else
                {
                    try
                    {
                        toPrint.Add(Confrontation(enemy, Hero));
                        if (choice == Enums.BattleChoice.Attack) toPrint.Add(Confrontation(Hero, enemy));
                    }
                    catch (ArgumentException) { }
                }
            }
            else
            {
                try
                {
                    toPrint.Add(Confrontation(enemy, Hero));
                    if (choice == Enums.BattleChoice.Attack) toPrint.Add(Confrontation(Hero, enemy));
                }
                catch (ArgumentException) { }
            }
            return false;
        }
        public void NextGameLevel()
        {
            _game_level ++ ;
            Map = new Grid();
            CreateMap();
        }
        public bool UseItem<T>()
            where T : Consumable
        {
            if (Hero.Bag.OfType<T>().Any())
            {
                Consumable item = (Consumable)Hero.Bag.Find(x => x.GetType() == typeof(T));
                item.Use();
                Hero.RemoveItem(item);
                return true;
            }
            return false;
        }
        public bool ToggleItemEquip<T>()
            where T : Equipment
        {
            if (Hero.Bag.OfType<T>().Any())
            {
                Equipment item = (Equipment)Hero.Bag.Find(x => x.GetType() == typeof(T));
                if (!item.Equiped)
                {
                    Hero.EquipItem(item);
                    return true;
                }
                else Hero.UnequipItem(item);
            }
            return false;
        }
        public bool HeroGainExp(int exp)
        {
            Hero.GainExp(exp);
            int level_fact = Hero.Experience / 20;
            if (level_fact != 0)
            {
                int nbrLvl_up = level_fact / Hero.Level;
                for (int i = 0; i < nbrLvl_up; i++) Hero.LevelUp();
                if(nbrLvl_up > 0) return true;
            }
            return false;
        }
    }
}
