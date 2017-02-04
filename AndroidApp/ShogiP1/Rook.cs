using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    class Rook : Figure
    {
        public const bool eligibleForPromote = true;
        public Rook(byte Row, byte Column, bool IsBlack, bool IsPromoted)
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

        public override void WhereToMove(byte objectRow, byte objectColumn) //it send allowed bool table fore pice 1 means that you can move pice to this place
        {
            System.Console.WriteLine("rook");

            bool[,] tab;
            tab = new bool[9, 9];
            block = false;

            for (byte r = 0; r <= 8; r++)
            {
                for (byte c = 0; c <= 8; c++)
                {
                    tab[r, c] = false;
                }
            }

            block = false;
            sthIsStayHere = false;
            byte nextRow = objectRow, nextColumn = objectColumn;
            for (; (nextRow <= 8) && (nextRow >= 0) && (nextColumn <= 8) && (nextColumn >= 0); nextColumn--)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            sthIsStayHere = false;
            for (; (nextRow <= 8) && (nextRow >= 0) && (nextColumn <= 8) && (nextColumn >= 0); nextRow--)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            sthIsStayHere = false;
            for (; (nextRow <= 8) && (nextRow >= 0) && (nextColumn <= 8) && (nextColumn >= 0); nextRow++)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }

            nextRow = objectRow;
            nextColumn = objectColumn;
            block = false;
            sthIsStayHere = false;
            for (; (nextRow <= 8) && (nextRow >= 0) && (nextColumn <= 8) && (nextColumn >= 0); nextColumn++)
            {
                {
                    checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }

            if (this.IsPromoted)    //promoted rook = rook + king
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
