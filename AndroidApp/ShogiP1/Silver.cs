using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    class Silver : Figure
    {
        public const bool eligibleForPromote = true;
        public Silver(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
        }

        private bool sthIsStayHere;
        private void checkSingleField(byte NextRow, byte nextColumn, byte x, byte y, List<Figure> FigList, bool[,] tab)
        {
            byte otherFigureIndex = 0;
            while (sthIsStayHere == false && otherFigureIndex < 40) //Checking if any of Figures is on particular field
            {
                if (FigList[otherFigureIndex].Row == NextRow && FigList[otherFigureIndex].Column == nextColumn)
                {
                    sthIsStayHere = true;
                }
                else otherFigureIndex++;
            }
            if (sthIsStayHere == true) 
            {
                if ((FigList[otherFigureIndex].IsBlack == this.IsBlack))//if this is my figure
                {
                    tab[NextRow, nextColumn] = false;
                }
                else if ((FigList[otherFigureIndex].IsBlack != this.IsBlack))//it isnt my figure
                {
                    tab[NextRow, nextColumn] = true;
                }
            }
            else tab[NextRow, nextColumn] = true;
        }

        public override void WhereToMove(byte objectRow, byte objectColumn) //it send allowed bool table fore pice, true means that you can move piece to this place
        {
            bool[,] tab;
            //table create
            tab = new bool[9, 9];
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    tab[r, c] = false;
                }
            }
            sthIsStayHere = false;
            byte nextRow = objectRow, nextColumn = objectColumn;
            nextRow++; //Going down on table (on start)
            nextColumn--; //Going left on table
            if (!this.IsPromoted)
            {
                
                if (!IsBlack)
                {
                    for (int i = 0; i < 3;)//Moving up
                    {
                        nextColumn = objectColumn;
                        nextColumn--;
                        for (int j = 0; j < 3;)//Moving right (columns)
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9))//not going beside table
                                checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                            j++;
                            sthIsStayHere = false;
                            if (i == 0 && j == 1)
                            {
                                nextColumn++;
                                j++;
                            }

                        }
                        nextRow--;
                        i++;
                        if (i == 1)
                        {
                            nextRow--;
                            i++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 3;)//Moving up
                    {
                        nextColumn = objectColumn;
                        nextColumn--;
                        for (int j = 0; j < 3;)//Moving right (columns)
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9))//not going beside table
                                checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                            j++;
                            sthIsStayHere = false;
                            if (i == 2 && j == 1)
                            {
                                nextColumn++;
                                j++;
                            }

                        }
                        nextRow--;
                        i++;
                        if (i == 1)
                        {
                            nextRow--;
                            i++;
                        }
                    }
                }

            }
            else //like gold
            {
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }

                sthIsStayHere = false;
                nextRow = objectRow;
                nextColumn = objectColumn;

                if (this.IsBlack)
                {
                    nextRow--;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if (((byte)(nextRow + i) >= 0 && (byte)(nextRow + i) < 9) && ((byte)(nextColumn + j) >= 0 && (byte)(nextColumn + j) < 9)) checkSingleField((byte)(nextRow + i), (byte)(nextColumn + j), objectRow, objectColumn, Figure.listOfFigures, tab);
                            sthIsStayHere = false;
                        }
                    }
                }
                else
                {
                    nextRow--;
                    nextColumn--;
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                        }
                        nextColumn -= 3;
                        nextRow++;
                    }
                    nextRow = objectRow;
                    nextRow++;
                    nextColumn = objectColumn;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                }
            }
            Figure.fields = tab;
        }
    }
}
