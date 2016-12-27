using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Board : BoardConsole
    {
        public static bool actionAborted = false;

        public static void ControlTheGame()
        {
            GameManager.LoadTable();
            byte decisionMaker;
            bool end = false;
            while (end == false)
            {
                WriteWhichPlayerShouldMove();
                Console.ReadKey();
                do
                {
                    decisionMaker = Menu();
                    actionAborted = false;
                    switch (decisionMaker)
                    {
                        case 0:
                            MovingActions.MakeAMove();
                            break;
                        case 1:
                            Ressurection.Resurrect();
                            break;
                        case 2:
                            if (GameManager.MoveCounter != 0) GameManager.undoMove();
                            else actionAborted = true;
                            CantUndoText();
                            break;
                    }
                }
                while (actionAborted == true);
            }
        } 
        private static byte Menu()
        {
            MenuText();
            return ChooseNumber();
        }
        public static void getTargetCoordinates(out byte Row, out byte Column)
        {
            getTargetCoordinatesText();
            ChooseRowText();
            Row = getcoordinate();
            ChooseColumnText();
            Column = getcoordinate();
        }
        public static void NextTurn()
        {
            GameManager.MoveCounter++;
            GameManager.BlackToMove = GameManager.BlackToMove ^ true;
            GameManager.SaveTable();
        }
    }
    class BoardConsole
    {
        protected static void WriteWhichPlayerShouldMove()
        {
            clearConsole();
            if (GameManager.BlackToMove == true)
                Console.WriteLine("Turn of blue player");
            else
                Console.WriteLine("Turn of red player");
    }
        protected static void ChooseRowText()
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("Press row index: ");
            Console.WriteLine();
        }
        protected static void ChooseColumnText()
        {

            Console.WriteLine();
            Console.WriteLine("Press Column index: ");
            Console.WriteLine();
        }
        protected static byte getcoordinate()
    {
        ConsoleKeyInfo key;
        while (true)
        {
            key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '0':
                    return 0;
                case '1':
                    return 1;
                case '2':
                    return 2;
                case '3':
                    return 3;
                case '4':
                    return 4;
                case '5':
                    return 5;
                case '6':
                    return 6;
                case '7':
                    return 7;
                case '8':
                    return 8;
                case '9':
                    return 9;
                default:
                    Console.WriteLine("You pressed wrong key! Press number from 0 to 8");
                    break;
            }
        }
    }
        protected static bool yesNo()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '0':
                        return false;
                    case '1':
                        return true;
                    default:
                        Console.WriteLine("Choose N or Y!");
                        break;
                }
            }
        }
        protected static void clearConsole(bool hint=false)
        {
            Console.Clear();
            char[,] board;
            board = new char[9, 9];
            char figSymbol = 'x';
            for (int r = 0; r < 9; r++)
            {
                for (int c = 0; c < 9; c++)
                {
                    board[r, c] = figSymbol;
                }
            }
            foreach (Figure Fig in Figure.listOfFigures)
            {
                if (Fig.Row < 9 && Fig.Column < 9)
                {
                    takeInfoToPrint(Fig, board);//Writing everything in table, capital letters for black
                }
            }
            System.Console.Write(" \t");
            for (int c = 0; c < 9; c++)
            {
                System.Console.Write(c);
                System.Console.Write("\t");
            }
            System.Console.WriteLine("");
            for (int r = 0; r < 9; r++) //Printing table, lowercase letter goes red
            {
                System.Console.Write(r);
                System.Console.Write("\t");
                for (int c = 0; c < 9; c++)
                {
                    if (hint==true&&Figure.fields[r,c]==true)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    } 
                    if (char.IsLower(board[r, c]) && board[r, c] != 'x')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        board[r, c] = char.ToUpper(board[r, c]);
                    }
                    else if (char.IsUpper(board[r, c]))
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write(board[r, c] + "\t");
                    Console.ResetColor();
                }
                Console.WriteLine();
                Console.WriteLine();
            }//<--Print unrotate
        }
        protected static void Print(byte row, byte column, bool black, char name, char[,] board)
        {
            if (black == false)
            {
                name = char.ToLower(name);
            }
            board[row, column] = name;
            Console.ResetColor();
        }
        protected static void takeInfoToPrint(Figure A, char[,] board)
        {
            string typeOfA = A.GetType().ToString();
            switch (typeOfA.Substring(typeOfA.IndexOf(".") + 1))
            {
                case "Bishop":
                    Print(A.Row, A.Column, A.IsBlack, 'B', board);
                    break;
                case "Gold":
                    Print(A.Row, A.Column, A.IsBlack, 'G', board);
                    break;
                case "King":
                    Print(A.Row, A.Column, A.IsBlack, 'K', board);
                    break;
                case "Knight":
                    Print(A.Row, A.Column, A.IsBlack, 'N', board);
                    break;
                case "Lance":
                    Print(A.Row, A.Column, A.IsBlack, 'L', board);
                    break;
                case "Pawn":
                    Print(A.Row, A.Column, A.IsBlack, 'P', board);
                    break;
                case "Rook":
                    Print(A.Row, A.Column, A.IsBlack, 'R', board);
                    break;
                case "Silver":
                    Print(A.Row, A.Column, A.IsBlack, 'S', board);
                    break;
            }
        }
        protected static void CantUndoText()
        {
            clearConsole();
            Console.WriteLine("U cant undo before first move!");
        }
        protected static void MenuText()
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("Press number to choose what do you want to do:");
            Console.WriteLine();
            Console.WriteLine("0 - Move my figure");
            Console.WriteLine("1 - Ressurect one of my Figures");
            Console.WriteLine("2 - Undo move of the enemy");
            Console.WriteLine();
        }
        protected static byte ChooseNumber()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '0':
                        return 0;
                    case '1':
                        return 1;
                    case '2':
                        return 2;
                    default:
                        Console.WriteLine("Choose number 0, 1 or 2 - there is no more options for now");
                        break;
                }
            }
        }
        protected static void getTargetCoordinatesText()
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("Now u need to choose the place, where u want to put your figure");
            Console.ReadKey();
        }
    }
}

