using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    class Bishop : Figure
    {
        public const bool eligibleForPromote = true;

        public Bishop(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
        }

        private bool block; //it lock line when sth is on piece way
        private bool sthIsStayHere = false;
        private void checkSingleField(byte NextRow, byte nextColumn, byte x, byte y, List<Figure> FigList, bool[,] tab)
        {
            int otherFigureIndex = 0;
            sthIsStayHere = false;
            while (sthIsStayHere == false && otherFigureIndex < 40)
            {
                if (FigList[otherFigureIndex].Row == NextRow && FigList[otherFigureIndex].Column == nextColumn)
                {
                    sthIsStayHere = true;
                }
                else otherFigureIndex++;
                if (otherFigureIndex == 40) sthIsStayHere = false;
            }
            if (block == true) tab[NextRow, nextColumn] = false;
            else
            {
                if (NextRow == x && nextColumn == y)
                {
                    tab[NextRow, nextColumn] = false;
                }
                else
                {
                    if (sthIsStayHere == true)
                    {
                        if ((FigList[otherFigureIndex].IsBlack == this.IsBlack))
                        {
                            tab[NextRow, nextColumn] = false;
                            block = true;
                        }
                        else if ((FigList[otherFigureIndex].IsBlack != this.IsBlack))
                        {
                            tab[NextRow, nextColumn] = true;
                            block = true;
                        }
                    }
                    else
                    {
                        tab[NextRow, nextColumn] = true;
                    }
                }
            }
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

            block = false;
            sthIsStayHere = false;
            byte nextRow = objectRow, nextColumn = objectColumn;
            for (; (nextRow < 9) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0);)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn--;
                    nextRow--;
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            
            for (; (nextRow < 8) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0);)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn++;
                    nextRow++;
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            sthIsStayHere = false;
            for (; (nextRow < 9) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0); nextRow--)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn++;
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            sthIsStayHere = false;
            for (; (nextRow < 9) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0); nextRow++)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn--;
                }
            }

            if (this.IsPromoted)    //promoted bishop = bishop + king
            {
                sthIsStayHere = false;
                nextRow = objectRow;
                nextColumn = objectColumn;
                nextRow++;
                nextColumn--;
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((nextRow >= 0 && nextRow <= 9) && (nextColumn >= 0 && nextColumn <= 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                        nextColumn++;
                    }
                    nextRow--;
                }
            }
            Figure.fields = tab;
        }
    }
}
