using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Ressurection : RessurectionConsole
    {
        public static void Resurrect()
        {
            bool NeedToChooseFigure = true;
            Figure A = null;
            while (NeedToChooseFigure)
            {
                ShowFiguresToRessurect();
                string name = ChooseFigure();
                if (name=="Nvm")
                {
                    Board.actionAborted = true;
                    return;
                }
                A = TakeDeadFigureToHand(name);
                if (A == null)
                    ThereIsNoFigureText();
                else
                    NeedToChooseFigure = false;
            }
            byte row=10, column=10;
            A = getTargetLocation(out row, out column, A);
            if (A==null)
            {
                Board.actionAborted = true;
                return;
            }
            Board.NextTurn();
        }
        private static void ShowFiguresToRessurect()
        {
            byte KnightCounter = 0;
            byte RookCounter = 0;
            byte BishopCounter = 0;
            byte PawnCounter = 0;
            byte LanceCounter = 0;
            byte SilverCounter = 0;
            byte GoldCounter = 0;
            string type;
            List<Figure> templist = new List<Figure>();
            foreach (var Fig in Figure.listOfFigures)
                if (Fig.IsBlack == GameManager.BlackToMove)
                    if (Fig.Column == byte.MaxValue)
                        templist.Add(Fig);
            foreach (var Fig in templist)
            {
                type = Fig.GetType().ToString();
                type = type.Substring(type.IndexOf(".") + 1);
                switch (type)
                {
                    case "Knight":
                        KnightCounter++;
                        break;
                    case "Rook":
                        RookCounter++;
                        break;
                    case "Lance":
                        LanceCounter++;
                        break;
                    case "Silver":
                        SilverCounter++;
                        break;
                    case "Gold":
                        GoldCounter++;
                        break;
                    case "Pawn":
                        PawnCounter++;
                        break;
                    case "Bishop":
                        BishopCounter++;
                        break;
                }

            }
            ListOfDeadFigures(KnightCounter, RookCounter, BishopCounter, PawnCounter, LanceCounter, SilverCounter, GoldCounter);
        }
        private static string ChooseFigure()
        {
            ConsoleKeyInfo key;
            while (true)
            {
                key = Console.ReadKey();
                switch (key.KeyChar)
                {
                    case '0':
                        return "Knight";
                    case '1':
                        return "Rook";
                    case '2':
                        return "Bishop";
                    case '3':
                        return "Pawn";
                    case '4':
                        return "Lance";
                    case '5':
                        return "Silver";
                    case '6':
                        return "Gold";
                    case '7':
                        return "Nvm";
                    default:
                        ShowFiguresToRessurect();
                        break;
                }
            }
            
        }
        private static Figure TakeDeadFigureToHand(string name)
        {
            string type;
            foreach (var Fig in Figure.listOfFigures)
            {
                type = Fig.GetType().ToString();
                type = type.Substring(type.IndexOf(".") + 1);
                if(type==name && Fig.Column==byte.MaxValue)
                {
                    return Fig;
                }
            }
            return null;
        }
        private static Figure getTargetLocation(out byte Row, out byte Column, Figure A)
        {
            Row = 10;
            Column = 10;
            while (Row != 9 || Column != 9)
            {
                Board.getTargetCoordinates(out Row, out Column);
                if (A.Ressurect(Row, Column))
                {
                    return A;
                }
                Figure.printAllowedFieldsTab();    //for debug
            }
            return null;
        }
    }

    
    class RessurectionConsole : BoardConsole
    {   
        protected static void ListOfDeadFigures(byte Knight, byte Rook, byte Bishop, byte Pawn, byte Lance, byte Silver, byte Gold)
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("What do u want to ressurect?");
            Console.WriteLine("0 - Knight x " + Knight);
            Console.WriteLine("1 - Rook x " + Rook);
            Console.WriteLine("2 - Bishop x " + Bishop);
            Console.WriteLine("3 - Pawn x " + Pawn);
            Console.WriteLine("4 - Lance x " + Lance);
            Console.WriteLine("5 - Silver x " + Silver);
            Console.WriteLine("6 - Gold x " + Gold);
            Console.WriteLine("7 - I changed my mind");
        }
        protected static void ThereIsNoFigureText()
        {
            Console.WriteLine();
            Console.WriteLine("There is no figure of that type to choose. Please try again after pressing any key");
            Console.ReadKey();
        }
    }
}
