using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class King : Figure
    {
        public const bool eligibleForPromote = false;
        public King(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
        }

        private bool sthIsStayHere;
        private void checkSingleField(byte NextRow, byte nextColumn, byte x, byte y, List<Figure> FigList, bool[,] tab)
        {
            byte otherFigureIndex = 0;
            while (sthIsStayHere == false && otherFigureIndex < 40)
            {
                if (FigList[otherFigureIndex].Row == NextRow && FigList[otherFigureIndex].Column == nextColumn)
                {
                    sthIsStayHere = true;
                }
                else otherFigureIndex++;
            }
            if (sthIsStayHere == true)
            {
                if ((FigList[otherFigureIndex].IsBlack == this.IsBlack))
                {
                    tab[NextRow, nextColumn] = false;
                }
                else if ((FigList[otherFigureIndex].IsBlack != this.IsBlack))
                {
                    tab[NextRow, nextColumn] = true;
                }
            }
            else tab[NextRow, nextColumn] = true;
            sthIsStayHere = false;
        }

        public override void WhereToMove(byte objectRow, byte objectColumn)
        {
            bool[,] tab;
            tab = new bool[9, 9];

            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    tab[r, c] = false;
                }
            }

            sthIsStayHere = false;
            byte checkedRow = objectRow, checkedColumn = objectColumn;
            checkedRow--;
            checkedColumn--;
            for (int i=0; i<3 ;i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (((byte)(checkedRow + i) >= 0 && (byte)(checkedRow + i) < 9) && ((byte)(checkedColumn + j) >= 0 && (byte)(checkedColumn + j) < 9)) checkSingleField((byte)(checkedRow + i), (byte)(checkedColumn + j), objectRow, objectColumn, Figure.listOfFigures, tab);
                    sthIsStayHere = false;
                }
            }
            Figure.fields = tab;
        }
    }
}
