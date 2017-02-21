using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    class Lance : Figure
    {
        

        public const bool eligibleForPromote = true;
        public Lance(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
        }

        private bool block; //it lock line when sth is on piece way
        private bool sthIsStayHere = false;
        private void checkSingleField(byte NextRow, byte nextColumn, byte x, byte y, List<Figure> FigList, bool[,] tab)
        {
            int otherFigureIndex = 0;
            sthIsStayHere = false;
            if (IsPromoted)
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
                else
                    tab[NextRow, nextColumn] = true;
                sthIsStayHere = false;
                return;
            }
            while (sthIsStayHere == false && otherFigureIndex < 40)
            {
                if (FigList[otherFigureIndex].Row == NextRow && FigList[otherFigureIndex].Column == nextColumn)
                {
                    sthIsStayHere = true;
                }
                else otherFigureIndex++;
                if (otherFigureIndex == 40)
                    sthIsStayHere = false;
            }
            if (block == true)
                tab[NextRow, nextColumn] = false;
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
            block = false;
            tab = new bool[9, 9];

            for (byte r = 0; r < 9; r++)
            {
                for (byte c = 0; c < 9; c++)
                {
                    tab[r, c] = false;
                }
            }
            sthIsStayHere = false;
            byte nextRow = objectRow, nextColumn = objectColumn;


            if (!this.IsPromoted)
            {
                if (this.IsBlack)
                {
                    for (; (nextRow < 9) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0); nextRow++)
                    {
                        {
                            checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                        }
                    }
                }
                else
                {
                    block = false;
                    for (; (nextRow < 9) && (nextRow >= 0) && (nextColumn < 9) && (nextColumn >= 0); nextRow--)
                    {
                        {
                            checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                        }
                    } 
                }
            }


            else
            {

                if (this.IsBlack)
                {
                    nextRow--;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9))
                        checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn--;
                    nextRow++;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {   for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9) && (nextColumn != objectColumn || nextRow != objectRow))
                                checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                        }
                        nextRow++;
                        nextColumn = objectColumn;
                        nextColumn--;
                    }
                }
                else
                {
                    nextRow++;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9))
                        checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextColumn--;
                    nextRow--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9) && (nextColumn != objectColumn || nextRow != objectRow))
                                checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            nextColumn++;
                        }
                        nextRow--;
                        nextColumn = objectColumn;
                        nextColumn--;
                    }
                }
            }

            /*for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    System.Console.Write(tab[r, c]);
                }
                System.Console.WriteLine(" ");
            }*/

            Figure.fields = tab;
        }
    }
}
