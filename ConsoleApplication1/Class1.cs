using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class MovingActions : MovingActionsConsole
    {
        public static void MakeAMove()
        {
            Figure A = ChooseFigure();
            if (A == null)
            {
                Board.actionAborted = true;
                return;
            }
            FigureChosenText(A);
            A.WhereToMove();
            bool hint = yesNo();
            clearConsole(hint);
            Console.ReadKey();
            byte targetRow, targetColumn;
            getTargetLocation(out targetRow, out targetColumn, A);
            if (A == null)
            {
                Board.actionAborted = true;
                return;
            }
            bool promoteflag = false;
            promoteflag = A.StayInPromotionZone();
            A.Move(targetRow, targetColumn);
            promoteflag = A.StayInPromotionZone();
            PromotingOptions(A, promoteflag);
            Board.NextTurn();
        }
        private static void ForcePromotion(Figure Promoted)
        {
            ForcePromotionText();
            Promoted.Promote();
        }
        public static void PromotingOptions(Figure BeingPromoting, bool promoteflag)
        {
            if(promoteflag)
            {
                if (BeingPromoting.IsLegalMovePossible())
                {
                    promotingProposition(BeingPromoting);
                }
                else
                {
                    ForcePromotion(BeingPromoting);
                }
            }
        }
        public static Figure ChooseFigure()
        {
            byte Row = 10;
            byte Column = 10;
            Figure A;
            while (Row != 9 || Column != 9)
            {
                getFigureCoordinates(out Row, out Column);
                A = Figure.FindFigure(Row, Column);
                if (A != null)
                {
                    if ((A.IsBlack ^ GameManager.BlackToMove) == false)
                    {
                        return A;
                    }
                    else
                    {
                        InvalidPlayerText();
                    }
                }
                else
                {
                    EmptyFieldText();
                }
            }
            return null;
        }//<--Podpięte pod MakeAMove()
        private static void promotingProposition(Figure A)
        {
            PromoteMaybeText();
            bool promote = yesNo();
            if (promote == true)
            {
                A.Promote();
            }
        }
        private static Figure getTargetLocation(out byte Row, out byte Column, Figure A)
        {
            Row = 10;
            Column = 10;
            while (Row != 9 || Column != 9)
            {
                Board.getTargetCoordinates(out Row, out Column);
                A.WhereToMove();
                if (Figure.IsThatMovePossible(Row, Column) == false)
                {
                    InvalidMoveText();
                }
                else
                {
                    return A;
                }
            }
            return null;
        }
        private static void getFigureCoordinates(out byte ReadedRow, out byte ReadedColumn)
        {
            ChooseRowText();
            ReadedRow = getcoordinate();
            ChooseColumnText();
            ReadedColumn = getcoordinate();
        }

    }
    class MovingActionsConsole : BoardConsole
    {
        protected static void ForcePromotionText()
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("Your Figure will be promoted due to lack of possible legal moves");
            Console.ReadKey();
        }
        protected static void DecidedToMoveText()
        {
            Console.WriteLine("You decided to move. Now you will be asked to give coordinates of figure u want to move.");
            Console.WriteLine("If you want to abort this action, press double 9");
            Console.WriteLine();
        }
        protected static void FigureChosenText(Figure A)
        {
            BoardConsole.clearConsole();
            Console.WriteLine();
            String TypeOfA = A.GetType().ToString();
            TypeOfA = TypeOfA.Substring(TypeOfA.IndexOf(".") + 1);
            Console.WriteLine("U choose " + TypeOfA + " which is placed on row" + A.Row.ToString() + " and column" + A.Column.ToString());
            Console.WriteLine("Do you want a hint where can u move?");
            Console.WriteLine();
            Console.WriteLine("0 - No, no hints");
            Console.WriteLine("1 - Yes, I do want a hint");
        }
        protected static void InvalidMoveText()
        {
            Console.WriteLine();
            Console.WriteLine("U cant move there. Try again");
            Console.ReadKey();
        }
        protected static void PromoteMaybeText()
        {
            clearConsole();
            Console.WriteLine();
            Console.WriteLine("Do you want to promote figure u just moved? 'Y' for 'Yes' and 'N' for 'No' ");
        }
        protected static void InvalidPlayerText()
        {
            Console.WriteLine();
            Console.WriteLine("There is not your figure! Try again");
            Console.ReadKey();
            clearConsole();
        }
        protected static void EmptyFieldText()
        {
            Console.WriteLine("There is nothing right there, lets try again");
            Console.ReadKey();
            clearConsole();
        }
    }

}
