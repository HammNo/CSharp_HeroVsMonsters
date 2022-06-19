using HeroesVsMonster.BLL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace HvsM_Con
{
    class Program
    {
        private static void Start()
        {
            Board.Instance.Reset();
            UI_NotePad.Instance.Reset();
            UI_NotePad.Instance.OverworldMusicToggle(true);
            Enums.MenuOptions choice = MainMenu(out string name);
            switch (choice)
            {
                case Enums.MenuOptions.Humain:
                    Board.Instance.Hero = (Hero)Board.Instance.CreateCharacter<Human>(name);
                    Board.Instance.Hero.AddItem(new LifePotion(Board.Instance.Hero));
                    Board.Instance.Hero.AddItem(new LifePotion(Board.Instance.Hero));
                    Board.Instance.Hero.AddItem(new OldSword());
                    Board.Instance.Hero.AddItem(new OldShield());
                    Game();
                    break;
                case Enums.MenuOptions.Nain:
                    Board.Instance.Hero = (Hero)Board.Instance.CreateCharacter<Dwarf>(name);
                    Board.Instance.Hero.AddItem(new LifePotion(Board.Instance.Hero));
                    Game();
                    break;
                case Enums.MenuOptions.Quitter:
                    break;
            }
        }
        private static Enums.MenuOptions MainMenu(out string name)
        {
            char key;
            int key_sh = 0;
            int[] position = new int[2];
            Console.Clear();
            UI_NotePad.Instance.PrintTitle();
            Console.Write("\t\t\t\t\t       1. Jouer un Humain\n\n");
            Console.Write("\t\t\t\t\t        2. Jouer un Nain\n\n");
            Console.Write("\t\t\t\t\t           3. Quitter\n\n\n");
            position[0] = Console.GetCursorPosition().ToTuple().Item1;
            position[1] = Console.GetCursorPosition().ToTuple().Item2;
            Console.SetCursorPosition(position[0], position[1] + 3);
            UI_NotePad.Instance.PrintSprite(null);
            Console.CursorVisible = false;
            do
            {
                key = Console.ReadKey(true).KeyChar;
                int.TryParse(key.ToString(), out key_sh);
            } while (!Enum.IsDefined(typeof(Enums.MenuOptions), key_sh));
            Console.SetCursorPosition(position[0], position[1]);
            Enums.MenuOptions choice = (Enums.MenuOptions)key_sh;
            name = "";
            if (choice != Enums.MenuOptions.Quitter) name = AskName();
            return choice;
        }
        private static string AskName()
        {
            string name = "";
            Console.CursorVisible = true;
            int[] position = new int[2];
            Console.Write("\t\t\t\t\t          ");
            position[0] = Console.GetCursorPosition().ToTuple().Item1;
            position[1] = Console.GetCursorPosition().ToTuple().Item2;
            do
            {
                Console.SetCursorPosition(position[0], position[1]);
                Console.Write("Nom :                             ");
                Console.SetCursorPosition(position[0] + 6, position[1]);
                name = Console.ReadLine();
            } while (name.Length < 3);
            return name;
        }
        private static void Game()
        {
            UI_NotePad.Instance.MainWindowRefresh(true);
            bool game_state = Controle();
            if (game_state)
            {
                if(Board.Instance.GameLevel < 3)
                {
                    UI_NotePad.Instance.InfoCont.PrintContent("Bravo, vous avez atteint la partie suivante!", false);
                    Console.ReadLine();
                    Board.Instance.NextGameLevel();
                    Game();
                }
                else
                {
                    UI_NotePad.Instance.InfoCont.PrintContent("Bravo, vous avez gagné!!!", false);
                    Console.ReadLine();
                }
            }
            //else Console.WriteLine("Perdu!");
            Start();
        }
        private static bool Controle()
        {
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (true)
            {
                Console.CursorVisible = false;
                int[] position_move = new int[2];
                position_move[0] = 0;
                position_move[1] = 0;
                switch (info.Key)
                {
                    case ConsoleKey.DownArrow:
                        position_move[0] = 1;
                        break;
                    case ConsoleKey.UpArrow:
                        position_move[0] = -1;
                        break;
                    case ConsoleKey.LeftArrow:
                        position_move[1] = -1;
                        break;
                    case ConsoleKey.RightArrow:
                        position_move[1] = 1;
                        break;
                    case ConsoleKey.C:
                        HAMenuCaracs();
                        UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Main);
                        break;
                    case ConsoleKey.O:
                        bool cont = HAMenuOptions();
                        if (!cont) return false;
                        UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Main);
                        break;
                    case ConsoleKey.S:
                        HAMenuBag();
                        UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Main);
                        break;
                    default:
                        break;
                }
                Enums.BoxState nextBox_state = Board.Instance.Map.Move(position_move[0], position_move[1]);
                MoveRefresh(position_move);
                if (nextBox_state == Enums.BoxState.Monster)
                {
                    bool hasWon = MeetAMonster(position_move);
                    if (!hasWon) return false;
                }
                if (Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0], Board.Instance.Map.Current_box[1]].State == Enums.BoxState.End)
                {
                    return true;
                }
                info = Console.ReadKey(true);
            }
        }
        private static bool HAMenuOptions()
        {
            UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Options);
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (true)
            {
                switch (info.Key)
                {
                    case ConsoleKey.M:
                        UI_NotePad.Instance.MusicToggle();
                        return true;
                        break;
                    case ConsoleKey.Q:
                        return false;
                        break;
                    case ConsoleKey.R:
                        return true;
                        break;
                }
                info = Console.ReadKey(true);
            }
        }
        private static void HAMenuCaracs()
        {
            UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Caracs);
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (true)
            {
                switch (info.Key)
                {
                    case ConsoleKey.R:
                        return;
                        break;
                }
                info = Console.ReadKey(true);
            }
        }
        private static string HAMenuBag()
        {
            UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Bag);
            ConsoleKeyInfo info = Console.ReadKey(true);
            while (true)
            {
                int key_convert = 0;
                string key_string = info.Key.ToString();
                string key_last = key_string[key_string.Length - 1].ToString();
                int.TryParse(key_last, out key_convert);
                if (Enum.IsDefined(typeof(Enums.ItemsTypes), key_convert) && key_convert!= 0)
                {
                    string result = BagSelectItem((Enums.ItemsTypes)key_convert);
                    if (result != "" && UI_NotePad.Instance.HeroActionCont.Combat) return result;
                    else
                    {
                        UI_NotePad.Instance.PrintMapHeroAction(Enums.HeroActionMenu.Bag);
                        UI_NotePad.Instance.InfoCont.PrintTempContent(result);
                    }
                }
                else if (info.Key == ConsoleKey.R) return "";
                info = Console.ReadKey(true);
            }
        }
        private static string BagSelectItem(Enums.ItemsTypes type)
        {
            string effect = "";
            bool equiped = false;
            switch (type)
            {
                case Enums.ItemsTypes.LifePotion:
                    Board.Instance.UseItem<LifePotion>();
                    effect = "H : récupère quelques pvs";
                    UI_NotePad.Instance.HeroBarCont.PrintContent();
                    break;
                case Enums.ItemsTypes.OldSword:
                    equiped = Board.Instance.ToggleItemEquip<OldSword>();
                    if(equiped) effect = $"{Board.Instance.Hero.Name} s'équipe d'une vieille épée et gagne 3 de force.";
                    else effect = $"{Board.Instance.Hero.Name} se déséquipe de la vieille épée et perd 3 de force.";
                    break;
                case Enums.ItemsTypes.OldShield:
                    equiped = Board.Instance.ToggleItemEquip<OldShield>();
                    if (equiped) effect = $"{Board.Instance.Hero.Name} s'équipe d'un vieux bouclier et gagne 3 pvs.";
                    else effect = $"{Board.Instance.Hero.Name} se déséquipe du vieux bouclier et perd 3 pvs.";
                    UI_NotePad.Instance.HeroBarCont.PrintContent();
                    break;
            }
            return effect;
        }
        private static void MoveRefresh(int[] position_move)
        {
            if (position_move[0] != 0 || position_move[1] != 0)
            {
                //Refresh old position
                int oldCursorPosition_X = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0] - position_move[0], Board.Instance.Map.Current_box[1] - position_move[1]].X;
                int oldCursorPosition_Y = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0] - position_move[0], Board.Instance.Map.Current_box[1] - position_move[1]].Y;
                Console.SetCursorPosition(oldCursorPosition_X, oldCursorPosition_Y);
                UI_NotePad.Instance.PrintSquare(Board.Instance.Map.Current_box[0] - position_move[0], Board.Instance.Map.Current_box[1] - position_move[1]);
                //Refresh new position
                Console.SetCursorPosition(Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0], Board.Instance.Map.Current_box[1]].X, Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0], Board.Instance.Map.Current_box[1]].Y);
                UI_NotePad.Instance.PrintSquare(Board.Instance.Map.Current_box[0], Board.Instance.Map.Current_box[1]);
            }
        }
        private static bool MeetAMonster(int[] position_move)
        {
            UI_NotePad.Instance.CombatMusicToggle(true);
            Box next_box = Board.Instance.Map.Boxes_tab[Board.Instance.Map.Current_box[0] + position_move[0], Board.Instance.Map.Current_box[1] + position_move[1]];
            next_box.SetVisibility(true);
            UI_NotePad.Instance.BattleWindowInit((Monster)next_box.Charac);
            Enums.BattleState battle_state = Battle((Monster)next_box.Charac);
            UI_NotePad.Instance.OverworldMusicToggle(true);
            if (battle_state == Enums.BattleState.Lost) return false;
            else if (battle_state == Enums.BattleState.Won)
            {
                next_box.State = Enums.BoxState.Empty;
                next_box.Charac = null;
            }
            UI_NotePad.Instance.MainWindowRefresh(false);
            return true;
        }
        private static Enums.BattleState Battle(Monster enemy)
        {
            Hero hero = Board.Instance.Hero;
            bool escape = LaunchBattle(enemy);
            if (escape)
            {
                UI_NotePad.Instance.InfoCont.PrintTempContent($"{hero.Name} a réussi à fuir!");
                Console.ReadLine();
                return Enums.BattleState.Escaped;
            }
            else
            {
                if (hero.IsAlive)
                {
                    BattleWin(enemy);
                    Console.ReadLine();
                    return Enums.BattleState.Won;
                }
                else
                {
                    UI_NotePad.Instance.InfoCont.PrintTempContent($"{hero.Name} a perdu...");
                    Console.ReadLine();
                    return Enums.BattleState.Lost;
                }
            }
        }
        private static bool LaunchBattle(Monster enemy)
        {
            Hero hero = Board.Instance.Hero;
            bool escape = false;
            while (hero.IsAlive && enemy.IsAlive && !escape)
            {
                List<string> toPrint = new List<string>();
                toPrint.Add(">>>  [a] Attaquer [s] sac [f] Fuir <<<");
                ConsoleKeyInfo info = Console.ReadKey(true);
                bool action = false;
                switch (info.Key)
                {
                    case ConsoleKey.A:
                        escape = Board.Instance.BattleTurn(enemy, Enums.BattleChoice.Attack, toPrint);
                        action = true;
                        break;
                    case ConsoleKey.S:
                        string action_report = HAMenuBag();
                        if (action_report != "")
                        {
                            toPrint.Add(action_report);
                            Board.Instance.BattleTurn(enemy, Enums.BattleChoice.Bag, toPrint);
                        }
                        action = true;
                        break;
                    case ConsoleKey.F:
                        escape = Board.Instance.BattleTurn(enemy, Enums.BattleChoice.Escape, toPrint);
                        if(!escape) UI_NotePad.Instance.InfoCont.PrintTempContent("Echec de la fuite...");
                        action = true;
                        break;
                }
                if (action)
                {
                    UI_NotePad.Instance.HeroBarCont.PrintContent();
                    UI_NotePad.Instance.EnemyBarCont.PrintContent();
                    UI_NotePad.Instance.HeroActionCont.PrintContent(toPrint);
                }
            }
            return escape;
        }
        private static void BattleWin(Monster enemy)
        {
            Hero hero = Board.Instance.Hero;
            UI_NotePad.Instance.InfoCont.PrintTempContent($"{hero.Name} a gagné!");
            if(hero.Level < 2) UI_NotePad.Instance.InfoCont.PrintTempContent("Il retrouve ses points de vie et ramasse le cuir et l'or.");
            bool lvl_up = Board.Instance.HeroWin(enemy);
            UI_NotePad.Instance.InfoCont.PrintTempContent($"{hero.Name} gagne {enemy.ExpBonus} point d'expérience.");
            if (lvl_up) UI_NotePad.Instance.InfoCont.PrintTempContent($"{hero.Name} monte au niveau {hero.Level}!");
        }
        static void Main(string[] args)
        {
            Start();
        }
    }
}
