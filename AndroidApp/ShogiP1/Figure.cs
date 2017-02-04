using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    
    class Figure
    {

        public static bool[,] TableToFrotend = new bool[9, 9];
        protected static bool[,] fields = new bool[9, 9];
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
            GameManager.LogMessageToFile("Figure method started");
            if (IsSomethingThere(Row, Column) == false)
            {
                this.Row = Row;
                this.Column = Column;
                this.IsBlack = IsBlack;
                this.IsPromoted = IsPromoted;
                GameManager.LogMessageToFile("Figure initialized");
            }
            else
            {
                GameManager.LogMessageToFile("WRONG - There is something, u cant initialize figure here");
            }
        }
        public virtual void WhereToMove(byte Row, byte Column)
        {
            GameManager.LogMessageToFile("WRONG - virtual method started");
        }
        private static void clearMoveBoard()
        {
            clearMoveBoard(fields);
        }
        public static void clearMoveBoard(bool[,] SoonEmpty)
        {
            GameManager.LogMessageToFile("Clearing table");
            for (int i = 0; i <= 8; i++)
                for (int j = 0; j <= 8; j++)
                    SoonEmpty[i, j] = false;
        }
        private static void Die(byte Row, byte Column)
        {
            GameManager.LogMessageToFile("Starting Die method");
            Figure A=null;
            foreach (var Fig in listOfFigures)
            {
                if (Row == Fig.Row && Column == Fig.Column)
                    A = Fig;
            }
            A.Row = byte.MaxValue;
            A.Column = byte.MaxValue;
            A.IsPromoted = false;
            A.IsBlack = A.IsBlack ^ true;
            GameManager.LogMessageToFile("Figure killed");
        }
        private static bool IsSomethingThere(byte Row, byte Column)
        {
            GameManager.LogMessageToFile("Checking for figures on particular place");
            foreach (Figure FigTest in listOfFigures)
            {
                if (FigTest.Row == Row && FigTest.Column == Column)
                    return true;
            }
            return false;
        }
        public void WhereToRessurect()
        {
            GameManager.LogMessageToFile("Checking for places to ressurect...");
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
                        if (IsLegalMovePossible(i, j) == true)
                        {
                            copy2[i, j] = true;
                        }
                        clearMoveBoard();//move board: empty, copy: empty fields, copy2: empty fields && with possible moves
                    }
            clearMoveBoard(copy);
            if (IsPawn() == true)
            {
                GameManager.LogMessageToFile("This is Pawn so extra conditions...");
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
            GameManager.LogMessageToFile("and done");
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
            GameManager.LogMessageToFile("Looking for figure");
            foreach (Figure Figtest in listOfFigures)
            {
                if (Figtest.Row == Row && Figtest.Column == Column)
                {
                    Figure A = Figtest;
                    return A;
                }
            }
            GameManager.LogMessageToFile("Found nothing");
            return null;
        }
        public static bool IsThatMovePossible(byte Row, byte Column)
        {
            bool possible = fields[Row, Column];
            clearMoveBoard();
            return possible;
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
        public bool IsLegalMovePossible(byte Row, byte Column)
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
        public void FindEnemyKing(out byte r, out byte c)
        {
            GameManager.LogMessageToFile("Finding king");
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
            GameManager.LogMessageToFile("Started the 'Promote' Function");
            if (IsPromoted == false)
            {
                IsPromoted = true;
                GameManager.LogMessageToFile("Promoted Figure {0}" + this.GetType());
            }
            else
                GameManager.LogMessageToFile("This {0} is already promoted!!" + this.GetType());
        }
        public bool Move(byte Row, byte Column)
        {
            GameManager.LogMessageToFile("Moving Figure");
            bool promoteflag;
            promoteflag =this.StayInPromotionZone();
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
            if (this.StayInPromotionZone() == true)
                promoteflag = true;
            GameManager.LogMessageToFile("Figure moved");
            if (this.IsLegalMovePossible(this.Row, this.Column))
            {
                GameManager.LogMessageToFile("Promotion forced");
                Promote();
                promoteflag = false;
            }
            return promoteflag;
        }
        public static void GiveFieldsToFrontend()
        {
            for (byte i = 0; i < 9; i++)
                for (byte j = 0; j < 9; j++)
                    TableToFrotend[i, j] = fields[i, j];
        }
    }
}
