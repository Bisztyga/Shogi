using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Pawn : Figure
    {
        public const bool eligibleForPromote = true;
        public Pawn(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
        }

        private void checkSingleField(byte NextRow, byte nextColumn, bool sthIsStayHere, int otherFigureIndex, byte x, byte y, List<Figure> FigList, bool[,] tab)   //without block
        {
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
        }

        public override void WhereToMove(byte objectRow, byte objectColumn)
        {
            bool[,] tab;

            if (!this.IsPromoted)
            {
                tab = new bool[9, 9];
                bool sthIsStayHere = false;

                for (byte r = 0; r < 9; r++)
                {
                    for (byte c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }

                byte otherFigureIndex = 0;

                sthIsStayHere = false;
                byte nextRow = objectRow, nextColumn = objectColumn;
                if(this.IsBlack) checkSingleField((byte)(nextRow+1), nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
                else checkSingleField((byte)(nextRow - 1), nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
            }
            else
            {
                tab = new bool[9, 9];
                bool sthIsStayHere = false;

                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }

                byte otherFigureIndex = 0;

                sthIsStayHere = false;
                byte nextRow = objectRow, nextColumn = objectColumn;

                if (this.IsBlack)
                {
                    nextRow--;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextRow++;
                    nextColumn--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                        }
                        nextRow++;
                    }
                }
                else
                {
                    nextRow++;
                    nextColumn--;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                        }
                        nextRow--;
                    }
                    nextRow = objectRow;
                    nextRow--;
                    nextColumn = objectColumn;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, sthIsStayHere, otherFigureIndex, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }

            /*for (byte r = 0; r < 8; r++)
            {
                for (byte c = 0; c < 8; c++)
                {
                    System.Console.Write(tab[r, c]);
                }
                System.Console.WriteLine();
            }*/

            Figure.fields = tab;
        }
    }
}
