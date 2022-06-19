using System;
using System.Collections.Generic;
using System.IO;
using WMPLib;
using HeroesVsMonster.BLL;
using System.Threading;

namespace HvsM_Con
{
    class UI_NotePad
    {
        #region fields
        private static UI_NotePad _ui_Notepad;
        private WindowsMediaPlayer[] _player;
        private int[] _infosCont_offset;
        private bool _music;
        private InfosContainer _info_cont;
        private HeroActionContainer _heroAction_cont;
        private CharacBarContainer _heroBar_cont;
        private CharacBarContainer _enemyBar_cont;
        public CharacBarContainer EnemyBarCont
        {
            get { return _enemyBar_cont; }
            set { _enemyBar_cont = value; }
        }
        public CharacBarContainer HeroBarCont
        {
            get { return _heroBar_cont; }
            set { _heroBar_cont = value; }
        }
        public HeroActionContainer HeroActionCont
        {
            get { return _heroAction_cont; }
            set { _heroAction_cont = value; }
        }
        public InfosContainer InfoCont
        {
            get { return _info_cont; }
            set { _info_cont = value; }
        }
        #endregion
        #region properties
        public WindowsMediaPlayer[] Player
        {
            get { return _player; }
            private set { _player = value; }
        }
        public int[] InfosContOffset
        {
            get { return _infosCont_offset; }
            private set { _infosCont_offset = value; }
        }
        #endregion
        #region singleton
        public static UI_NotePad Instance
        {
            get
            {
                if (_ui_Notepad is null)
                {
                    _ui_Notepad = new UI_NotePad();
                }
                return _ui_Notepad;
            }
        }
        #endregion
        #region constructor
        public UI_NotePad()
        {
            //Player[0] = Overworld music, Player[1] = Combat music
            Player = new WindowsMediaPlayer[2];
            InfosContOffset = new int[2];
            _music = true;
        }
        #endregion
        #region Music control methods
        public void OverworldMusicToggle(bool play)
        {
            if (!_music) return;
            ShutAllOtherMusic(Player[0]);
            if (Player[0] is null)
            {
                Player[0] = new WindowsMediaPlayer();
                Player[0].URL = @"Medias\Strange World (VIII).mp3";
                Player[0].controls.play();
            }
            else
            {
                if (play)
                {
                    if ((int)Player[0].playState != 3) Player[0].controls.play();
                }
                else Player[0].controls.stop();
            }
        }
        public void CombatMusicToggle(bool play)
        {
            if (!_music) return;
            ShutAllOtherMusic(Player[1]);
            if (Player[1] is null)
            {
                Player[1] = new WindowsMediaPlayer();
                Player[1].URL = @"Medias\HoMM Combat.mp3";
                Player[1].controls.play();
            }
            else
            {
                if (play)
                {
                    if ((int)Player[1].playState != 3) Player[1].controls.play();
                }
                else Player[1].controls.stop();
            }
        }
        private void ShutAllOtherMusic(WindowsMediaPlayer keepUp)
        {
            foreach (WindowsMediaPlayer player in Player)
            {
                if (player is not null && player != keepUp && (int)player.playState == 3) player.controls.stop();
            }
        }
        public void MusicToggle()
        {
            _music = !_music;
            if (!_music) ShutAllOtherMusic(null);
            else OverworldMusicToggle(true);
        }
        #endregion
        #region Interface print methods
        public void MainWindowRefresh(bool nextGLevel)
        {
            Console.Clear();
            PrintTitle();
            PrintMap();
            if(InfoCont is null ||nextGLevel)
            {
                int[] infosCont_offset = new int[2];
                infosCont_offset[0] = Board.Instance.Map.Boxes_tab[0, Board.Instance.Map.Cols_nbr - 1].X + 5;
                infosCont_offset[1] = Board.Instance.Map.Boxes_tab[0, 0].Y;
                int info_height = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Rows_nbr - 1, 0].Y + 6 - Board.Instance.Map.Boxes_tab[0, 0].Y;
                InfoCont = new InfosContainer(infosCont_offset, info_height, 50);
                int[] characCont_offset = new int[2];
                characCont_offset[0] = Board.Instance.Map.Boxes_tab[0, 0].X;
                characCont_offset[1] = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Rows_nbr - 1, 0].Y + 2;
                HeroActionCont = new HeroActionContainer(characCont_offset, 3, 45);
                int[] heroBar_offset = new int[2];
                heroBar_offset[0] = Board.Instance.Map.Boxes_tab[0, 0].X;
                heroBar_offset[1] = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Rows_nbr - 1, 0].Y + 7;
                HeroBarCont = new CharacBarContainer(heroBar_offset, 1, 45, Board.Instance.Hero);
                int[] enemyBar_offset = new int[2];
                enemyBar_offset[0] = Board.Instance.Map.Boxes_tab[0, Board.Instance.Map.Cols_nbr - 1].X + 5;
                enemyBar_offset[1] = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Rows_nbr - 1, 0].Y + 7;
                EnemyBarCont = new CharacBarContainer(enemyBar_offset, 1, 45, null);
            }
            else
            {
                InfoCont.DrawOutlines();
                InfoCont.Clear();
                HeroActionCont.DrawOutlines();
                HeroActionCont.Combat = false;
                HeroBarCont.DrawOutlines();
                HeroBarCont.PrintContent();
                EnemyBarCont.Charac = null;
            }
            PrintMapHeroAction(Enums.HeroActionMenu.Main);
            PrintOverworldInfos();
            Console.CursorVisible = false;
        }
        public void BattleWindowInit(Monster enemy)
        {
            Console.Clear();
            PrintTitle();
            PrintMap();
            InfoCont.DrawOutlines();
            InfoCont.Clear();
            HeroActionCont.DrawOutlines();
            HeroActionCont.Combat = true;
            HeroBarCont.DrawOutlines();
            HeroBarCont.PrintContent();
            EnemyBarCont.Charac = enemy;
            EnemyBarCont.DrawOutlines();
            EnemyBarCont.PrintContent();
            PrintCombatInfos(enemy);
        }
        public void PrintSquare(int row, int col)
        {
            Console.CursorVisible = false;
            switch (Board.Instance.Map.Boxes_tab[row, col].State)
            {
                case Enums.BoxState.Empty:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("   ");
                    break;
                case Enums.BoxState.Forest:
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    Console.Write("   ");
                    break;
                case Enums.BoxState.Rock:
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.Write("   ");
                    break;
                case Enums.BoxState.Hero:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write(" ¥ ");
                    break;
                case Enums.BoxState.Monster:
                    if (Board.Instance.Map.Boxes_tab[row, col].Visible)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" M ");
                    }
                    else Console.Write("   ");
                    break;
                case Enums.BoxState.Water:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.Write("   ");
                    break;
                case Enums.BoxState.End:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.Write("   ");
                    break;
            }
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void PrintLine(short mode)
        {
            Console.CursorVisible = false;
            if (mode == 0) Console.Write("\t┌");
            else if (mode == 1) Console.Write("\t└");
            if (mode == 0 || mode == 1)
            {
                for (short i = 0; i < Board.Instance.Map.Cols_nbr; i++)
                {
                    Console.Write("───");
                }
            }
            if (mode == 0) Console.Write("┐");
            else if (mode == 1) Console.Write("┘");
        }
        public void PrintMap()
        {
            Console.CursorVisible = false;
            PrintLine(0);
            for (short i = 0; i < Board.Instance.Map.Rows_nbr; i++)
            {
                Console.WriteLine();
                Console.Write("\t│");
                for (short j = 0; j < Board.Instance.Map.Cols_nbr; j++)
                {
                    (int, int) position = Console.GetCursorPosition();
                    Board.Instance.Map.Boxes_tab[i, j].X = position.Item1;
                    Board.Instance.Map.Boxes_tab[i, j].Y = position.Item2;
                    PrintSquare(i, j);
                }
                Console.Write("│");
            }
            Console.WriteLine();
            PrintLine(1);
        }
        public void PrintTitle()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n\n");
            Console.Write("\t\t\t\t╦ ╦┌─┐┬─┐┌─┐┌─┐┌─┐  ╦  ╦┌─┐  ╔╦╗┌─┐┌┐┌┌─┐┌┬┐┌─┐┬─┐┌─┐\n" +
                          "\t\t\t\t╠═╣├┤ ├┬┘│ │├┤ └─┐  ╚╗╔╝└─┐  ║║║│ ││││└─┐ │ ├┤ ├┬┘└─┐\n" +
                          "\t\t\t\t╩ ╩└─┘┴└─└─┘└─┘└─┘   ╚╝ └─┘  ╩ ╩└─┘┘└┘└─┘ ┴ └─┘┴└─└─┘\n");
            Console.WriteLine("\n\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
        public void PrintMapHeroAction(Enums.HeroActionMenu option)
        {
            List<string> toPrint = new List<string>();
            switch (option)
            {
                case Enums.HeroActionMenu.Main:
                    toPrint.Add(" [c] Caractéristiques");
                    toPrint.Add(" [s] Sac");
                    toPrint.Add(" [o] Options");
                    break;
                case Enums.HeroActionMenu.Options:
                    toPrint.Add(" [m] Musique on/off");
                    toPrint.Add(" [q] Quitter le jeu");
                    toPrint.Add(" [r] Retour");
                    break;
                case Enums.HeroActionMenu.Caracs:
                    toPrint.Add($"\tPvs max. {Board.Instance.Hero.Life} /\\ Exp. {Board.Instance.Hero.Experience}");
                    toPrint.Add($"\tEndu. {Board.Instance.Hero.Stamina} /\\ " +
                                $"Force {Board.Instance.Hero.Strenght} /\\ " +
                                $"Vit. {Board.Instance.Hero.Speed}");
                    toPrint.Add(" [r] Retour");
                    break;
                case Enums.HeroActionMenu.Bag:
                    HeroActionCont.InBag = true;
                    toPrint = PrintBag();
                    break;
            }
            HeroActionCont.PrintContent(toPrint);
            HeroActionCont.InBag = false;
        }
        public void PrintSprite(Character character)
        {
            List<string> sprite = CreateSprite(character);
            if(character != null)
            {
                foreach (string line in sprite)
                {
                    InfoCont.PrintContent(line, true);
                    Thread.Sleep(50);
                }
                InfoCont.PrintContent(" ", false);
            }
            else
            {
                int[] position = new int[2];
                position[0] = Console.GetCursorPosition().ToTuple().Item1;
                position[1] = Console.GetCursorPosition().ToTuple().Item2;
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                foreach (string line in sprite)
                {
                    Console.Write("\t\t" + line);
                    Console.SetCursorPosition(position[0], ++position[1]);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
        private void PrintOverworldInfos()
        {
            InfoCont.PrintContent("Bienvenue dans la forêt de Sheerwood...", false);
            InfoCont.PrintContent(" ", false);
            PrintSprite(Board.Instance.Hero);
            Console.CursorVisible = false;
        }
        private void PrintCombatInfos(Monster enemy)
        {
            List<string> toPrint = new List<string>();
            toPrint.Add(">>>  [a] Attaquer [s] sac [f] Fuir <<<");
            HeroActionCont.PrintContent(toPrint);
            InfoCont.PrintContent($"Un {enemy.GetType().Name} sauvage apparait!", false);
            InfoCont.PrintContent(" ", false);
            PrintSprite(enemy);
            Console.CursorVisible = false;
        }
        private List<string> PrintBag()
        {
            List<string> toPrint = new List<string>();
            string line = " ";
            string[,] work_tab = new string[Enum.GetValues(typeof(Enums.ItemsTypes)).Length, 2];
            foreach (Item item in Board.Instance.Hero.Bag)
            {
                Enums.ItemsTypes item_type = 0;
                foreach(Enums.ItemsTypes type in Enum.GetValues(typeof(Enums.ItemsTypes)))
                {
                    if(type.ToString() == item.GetType().Name) item_type = type;
                }
                if (work_tab[(int)item_type, 1] is null) work_tab[(int)item_type, 1] = "1";
                else work_tab[(int)item_type, 1] = (int.Parse(work_tab[(int)item_type, 1]) + 1).ToString();
                if (!HeroActionCont.Combat)
                {
                    if (item is not Equipment) work_tab[(int)item_type, 0] = $"[{(int)item_type}] {item_type} ({work_tab[(int)item_type, 1]})";
                    else
                    {
                        Equipment equip = (Equipment)item;
                        work_tab[(int)item_type, 0] = $"[{(int)item_type}] {item_type} ({((equip.Equiped)? "V" : "X")})";
                    }
                }
                else
                {
                    if (item is not Equipment) work_tab[(int)item_type, 0] = $"[{(int)item_type}] {item_type} ({work_tab[(int)item_type, 1]})";
                }
            }
            int items_count = 0;
            for(int i = 0; i < Enum.GetValues(typeof(Enums.ItemsTypes)).Length; i++)
            {
                if(work_tab[i, 0] is not null)
                {
                    line += work_tab[i, 0] + " ";
                    items_count++;
                    if (items_count % 2 == 0)
                    {
                        toPrint.Add(line);
                        line = " ";
                    }
                }
            }
            if (items_count % 2 != 0) toPrint.Add(line);
            if (items_count < 3) toPrint.Add(" ");
            toPrint.Add(" [r] Retour" + $"\t\t{Board.Instance.Hero.Gold} Or $$ { Board.Instance.Hero.Leather} Cuir");
            return toPrint;
        }
        #endregion
        private List<string> CreateSprite(Character character)
        {
            List<string> canevas = new List<string>();
            string path = "";
            if (character != null) path = @$"Sprites\{character.GetType().Name}.txt";
            else path = @$"Sprites\MenuSprite.txt";
            if (File.Exists(path))
            {
                StreamReader sr = File.OpenText(path);
                string line = sr.ReadLine();
                while (line is not null)
                {
                    canevas.Add(line);
                    line = sr.ReadLine();
                }
                sr.Dispose();
                sr.Close();
            }
            return canevas;
        }
        public void Reset()
        {
            ShutAllOtherMusic(null);
            _ui_Notepad = new UI_NotePad();
        }
    }
}
