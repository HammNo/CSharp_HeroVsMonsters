using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesVsMonster.BLL
{
    public class Grid
    {
        #region fields
        private Box[,] _boxes_tab;
        private int _rows_nbr, _cols_nbr;
        private int[] _current_box;
        #endregion
        #region properties
        public Box[,] Boxes_tab
        {
            get { return _boxes_tab; }
            set { _boxes_tab = value; }
        }
        public int Rows_nbr
        {
            get { return _rows_nbr; }
            private set { _rows_nbr = value; }
        }
        public int Cols_nbr
        {
            get { return _cols_nbr; }
            private set { _cols_nbr = value; }
        }
        public int[] Current_box
        {
            get { return _current_box; }
            private set { _current_box = value; }
        }
        #endregion
        #region constructor
        public Grid(int rows, int cols)
        {
            NewBoxTab(rows, cols);
            Current_box = new int[2];
            Current_box[0] = 0;
            Current_box[1] = 0;
        }
        public Grid()
        {
            Current_box = new int[2];
            Current_box[0] = 0;
            Current_box[1] = 0;
        }
        #endregion
        public void NewBoxTab(int rows, int cols)
        {
            Boxes_tab = new Box[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Boxes_tab[i, j] = new Box();
                }
            }
            Rows_nbr = rows;
            Cols_nbr = cols;
        }
        public void FillGrid(List<List<int>> canevas, List<Monster> monster_list)
        {
            for (int i = 0; i < canevas.Count; i++)
            {
                List<int> line = canevas[i];
                for (int j = 0; j < line.Count; j++)
                {
                    Boxes_tab[i, j].State = (Enums.BoxState)line[j];
                    if (Boxes_tab[i, j].State == Enums.BoxState.Monster)
                    {
                        Boxes_tab[i, j].Charac = monster_list[0];
                        monster_list.Remove(monster_list[0]);
                    }
                }
            }
        }
        public Enums.BoxState Move(int row, int col)
        {
            bool isHero = false, hasChanged = false;
            if (Boxes_tab[Current_box[0], Current_box[1]].State == Enums.BoxState.Hero) isHero = true;
            if (row != 0)
            {
                if (Current_box[0] + row >= 0 && Current_box[0] + row < Rows_nbr)
                {
                    if (Boxes_tab[Current_box[0] + row, Current_box[1]].State != Enums.BoxState.Empty && Boxes_tab[Current_box[0] + row, Current_box[1]].State != Enums.BoxState.End)
                    {
                        return Boxes_tab[Current_box[0] + row, Current_box[1]].State;
                    }
                    Current_box[0] += row;
                    hasChanged = true;
                }
            }
            else if (col != 0)
            {
                if (Current_box[1] + col >= 0 && Current_box[1] + col < Cols_nbr)
                {
                    if (Boxes_tab[Current_box[0], Current_box[1] + col].State != Enums.BoxState.Empty && Boxes_tab[Current_box[0], Current_box[1] + col].State != Enums.BoxState.End)
                    {
                        return Boxes_tab[Current_box[0], Current_box[1] + col].State;
                    }
                    Current_box[1] += col;
                    hasChanged = true;
                }
            }
            if (isHero && hasChanged)
            {
                Boxes_tab[Current_box[0], Current_box[1]].Charac = Boxes_tab[Current_box[0] - row, Current_box[1] - col].Charac;
                if(Boxes_tab[Current_box[0], Current_box[1]].State != Enums.BoxState.End) Boxes_tab[Current_box[0], Current_box[1]].State = Enums.BoxState.Hero;
                Boxes_tab[Current_box[0] - row, Current_box[1] - col].Charac = null;
                Boxes_tab[Current_box[0] - row, Current_box[1] - col].State = Enums.BoxState.Empty;
            }
            return Enums.BoxState.Empty;
        }
    }
}
