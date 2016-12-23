using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Knight : Figure
    {
        public const bool eligibleForPromote = true;
        private int directionOfMove;    //it is responsible for direction of move. If figure move down it muliply row move by -1
        public Knight(byte Row, byte Column, bool IsBlack, bool IsPromoted)
            : base(Row, Column, IsBlack, IsPromoted)
        {
            if (IsBlack) directionOfMove = 1;
            else directionOfMove = -1;
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
            if (!this.IsPromoted)
            {
                tab = new bool[9, 9];
                for (int r = 0; r < 9; r++)
                {
                    for (int c = 0; c < 9; c++)
                    {
                        tab[r, c] = false;
                    }
                }

                sthIsStayHere = false;
                byte nextColumn = (byte)(objectColumn + 1);
                byte nextRow = (byte)(objectRow + (2*directionOfMove));
                if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
                nextColumn = (byte)(nextColumn - 2);
                if ((nextRow >= 0 && nextRow < 9) && (nextColumn >= 0 && nextColumn < 9)) checkSingleField(nextRow, nextColumn, objectRow, objectColumn, Figure.listOfFigures, tab);
            }
            else
            {
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
                    nextColumn--;
                    for (int i = 1; i < 3; i++) //figure rowe and next row
                    {
                        for (int j = 0; j < 3; j++) //all 3 columns
                        {
                            if (((byte)(nextRow - i) >= 0 && (byte)(nextRow - i) < 9) && ((byte)(nextColumn - j) >= 0 && (byte)(nextColumn - j) < 9)) checkSingleField((byte)(nextRow - i), (byte)(nextColumn - j), objectRow, objectColumn, Figure.listOfFigures, tab);
                            sthIsStayHere = false;
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
