using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
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

        public override void WhereToMove(byte objectRow, byte objectColumn) //it send allowed bool table fore pice, true means that you can move pice to this place
        {
            bool[,] tab;
            if (!this.IsPromoted)
            {
                //table create
                tab = new bool[9, 9];
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }
                //
                sthIsStayHere = false;
                byte nextRow = objectRow, nextColumn = objectColumn; 
                nextRow++;
                nextColumn--;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                        nextColumn++;
                        sthIsStayHere = false;
                        if (i == 2 && j == 1) nextColumn++;
                    }
                    nextRow--;
                    if (i == 1) nextRow++;
                }
            }
            else
            {
                tab = new bool[9, 9];
                sthIsStayHere = false;

                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }

                sthIsStayHere = false;
                byte nextRow = objectRow, nextColumn = objectColumn;

                if (this.IsBlack)
                {
                    nextRow--;
                    if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                    nextRow++;
                    nextColumn--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                            sthIsStayHere = false;
                        }
                    }
                }
                else
                {
                    nextRow++;
                    nextColumn--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                        }
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
