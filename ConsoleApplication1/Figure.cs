using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Figure
    {
        public static bool[,] fields = new bool[9, 9];
        public static List<Figure> listOfFigures;

        private byte row;
        public byte Row
        {
            get { return row; }
            set { row = value; }
        }
        private byte column;
        public byte Column
        {
            get { return column; }
            set { column = value; }
        }
        private bool isBlack;
        public bool IsBlack
        {
            get { return isBlack; }
            set { isBlack = value; }
        }
        private bool isPromoted;
        public bool IsPromoted
        {
            get { return isPromoted; }
            set { isPromoted = value; }
        }

        public Figure(byte Row, byte Column, bool IsBlack, bool IsPromoted)
        {
            if (IsSomethingThere(Row, Column) == false)
            {
                this.Row = Row;
                this.Column = Column;
                this.IsBlack = IsBlack;
                this.IsPromoted = IsPromoted;
            }
            else
            {
                this.Row = 254;
                this.Column = 254;
                Console.WriteLine("Someone fucked up, there is something on that field");

            }
        }

        public virtual void WhereToMove(byte Row, byte Column)
        {
            throw new Exception("You started virtual method, something is wrong");
        }

        private static void clearMoveBoard()
        {
            clearMoveBoard(fields);
        }
        private static void clearMoveBoard(bool[,] SoonEmpty)
        {
            for (int i = 0; i <= 8; i++)
                for (int j = 0; j <= 8; j++)
                    SoonEmpty[i, j] = false;
        }
        private static void Die(byte Row, byte Column)
        {
            Figure A = null;
            foreach (var Fig in listOfFigures)
            {
                if (Row == Fig.Row && Column == Fig.Column)
                    A = Fig;
            }
            A.Row = byte.MaxValue;
            A.Column = byte.MaxValue;
            A.IsPromoted = false;
            A.IsBlack = A.IsBlack ^ true;
        }
        private static bool IsSomethingThere(byte Row, byte Column)
        {
            foreach (Figure FigTest in listOfFigures)
            {
                if (FigTest.Row == Row && FigTest.Column == Column)
                    return true;
            }
            return false;
        }
        private void WhereToRessurect()
        {
            clearMoveBoard();
            bool[,] copy = new bool[9, 9];
            bool[,] copy2 = new bool[9, 9];
            clearMoveBoard(copy);
            clearMoveBoard(copy2);
            for (byte i = 0; i <= 8; i++)
                for (byte j = 0; j <= 8; j++)
                    if (IsSomethingThere(i, j) == false)
                        copy[i, j] = true;
            for (byte i = 0; i <= 8; i++)
                for (byte j = 0; j <= 8; j++)
                    if (copy[i, j] == true)
                    {
                        WhereToMove(i, j);
                        if (IsAnyMovePossible(i, j) == true)
                        {
                            copy2[i, j] = true;
                        }
                        clearMoveBoard();//move board: empty, copy: empty fields, copy2: empty fields && with possible moves
                    }
            clearMoveBoard(copy);
            if (IsPawn() == true)
            {
                byte r;
                byte c;
                FindEnemyKing(out r, out c);
                if (isBlack == true)
                {
                    if ( (r - 1)  <= 8)  copy2[r + 1, c] = false;
                }
                if (IsBlack == false)
                {
                    if( ( r - 1 ) >= 0) copy2[r - 1, c] = false;
                }
                if (isBlack == true)
                {
                    foreach (Figure Figtest in listOfFigures)
                    {
                        if (Figtest.isBlack == true && Figtest.IsPawn() == true && Figtest.IsPromoted == false)
                            for (byte i = 0; i <= 8; i++)
                                if (Figtest.column <= 8) copy2[i, Figtest.Column] = false;
                    }
                }
                if (isBlack == false)
                {
                    foreach (Figure Figtest in listOfFigures)
                    {
                        if (Figtest.isBlack == false && Figtest.IsPawn() == true && Figtest.IsPromoted == false)
                            for (byte i = 0; i <= 8; i++)
                                if (Figtest.column <= 8) copy2[i, Figtest.Column] = false;
                    }
                }
            }
            for (byte i = 0; i <= 8; i++)
            {
                for (byte j = 0; j <= 8; j++)
                {
                    fields[i, j] = copy2[i, j];
                }
            }
        }
        private bool IsKing()
        {
            if (this.GetType() == typeof(King))
                return true;
            else
                return false;
        }
        private bool IsGold()
        {
            if (this.GetType() == typeof(Gold))
                return true;
            else
                return false;
        }
        private bool IsPawn()
        {
            if (this.GetType() == typeof(Pawn))
                return true;
            else
                return false;
        }

        public static Figure FindFigure(byte Row, byte Column)
        {
            foreach (Figure Figtest in listOfFigures)
            {
                if (Figtest.Row == Row && Figtest.Column == Column)
                {
                    Figure A = Figtest;
                    return A;
                }
            }
            Console.WriteLine("There is nothing there");
            return null;
        }
        public static void printAllowedFieldsTab()
        {
            for (byte r = 0; r < 9; r++)
            {
                for (byte c = 0; c < 9; c++)
                {
                    System.Console.Write(Figure.fields[r, c] + "\t");
                }
                System.Console.WriteLine();
            }
            clearMoveBoard();
        }
        public static bool IsThatMovePossible(byte Row, byte Column)
        {
            bool possible = fields[Row, Column];
            clearMoveBoard();
            return possible;
        }
        public bool Ressurect(byte Row, byte Column)
        {
            this.WhereToRessurect();
            if (Row < 9 && Row >= 0 && Column < 9 && Column >= 0)
            {
                if (fields[Row, Column] == true)
                {
                    this.Row = Row;
                    this.Column = Column;
                    return true;
                }
                else return false;
            }
            else return false;
        }
        public bool StayInPromotionZone()
        {
            if (Column > 8)
            {
                Console.WriteLine("Someone fucked up, why we checking dead figures?!");
                return false;
            }
            if (((IsBlack == true && Row > 5) || (IsBlack == false && Row < 3)) && IsGold() == false && IsKing() == false)
                return true;
            else
                return false;
        }
        public bool IsAttacked()
        {
            foreach (Figure Figtest in listOfFigures)
            {
                clearMoveBoard();
                Figtest.WhereToMove();
                if (fields[this.Row, this.Column] == true)
                    clearMoveBoard();
                return true;
            }
            clearMoveBoard();
            return false;
        }
        public bool IsLegalMovePossible()
        {
            string type = this.GetType().ToString();
            type = type.Substring(type.IndexOf(".") + 1);
            if (type == "Knight")
            {
                if ((isBlack && (Row == 8 || Row == 7)) || !isBlack && (Row == 0 || Row == 1))
                    return false;
            }
            else if (type == "Pawn" || type == "Lance")
            {
                if ((isBlack && Row == 8 || !isBlack && (Row == 0)))
                    return false;
            }
            return true;
        }
        public bool IsAnyMovePossible()
        {
            WhereToMove();
            for (byte i = 0; i <= 8; i++)
                for (byte j = 0; j <= 8; j++)
                    if (fields[i, j] == true)
                        return true;
            return false;
        }

        public bool IsAnyMovePossible(byte r, byte c)
        {
            WhereToMove(r, c);
            for (byte i = 0; i <= 8; i++)
                for (byte j = 0; j <= 8; j++)
                    if (fields[i, j] == true)
                        return true;
            return false;
        }
        public void FindEnemyKing(out byte r, out byte c)
        {
            r = 254;
            c = 254;
            clearMoveBoard();
            foreach (Figure Figtest in listOfFigures)
            {
                if (Figtest.IsKing() == true && Figtest.IsBlack != this.IsBlack)
                {
                    r = Figtest.Row;
                    c = Figtest.Column;
                    break;
                }
            }
        }

        public void WhereToMove()
        {
            WhereToMove(this.Row, this.Column);
        }
        public void Promote()
        {
            if (IsPromoted == false)
                IsPromoted = true;
            else
                throw new Exception("It is already promoted");
        }
        public void Move(byte Row, byte Column)
        {
            if (IsSomethingThere(Row, Column) == false)
            {
                this.Row = Row;
                this.Column = Column;
            }
            else
            {
                Die(Row, Column);
                this.Row = Row;
                this.Column = Column;
            }
        }
    }
}
