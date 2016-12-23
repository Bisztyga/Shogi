using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Figure.listOfFigures = new List<Figure>();
            //Figure.listOfFigures.Add(new Lance(0, 0, true, false));//0
            //Figure.listOfFigures.Add(new Knight(0, 1, true, false));//1
            //Figure.listOfFigures.Add(new Silver(0, 2, true, false));//2
            //Figure.listOfFigures.Add(new Gold(0, 3, true, false));//3
            //Figure.listOfFigures.Add(new King(0, 4, true, false));//4
            //Figure.listOfFigures.Add(new Gold(0, 5, true, false));//5
            //Figure.listOfFigures.Add(new Silver(0, 6, true, false));//6
            //Figure.listOfFigures.Add(new Knight(0, 7, true, false));//7
            //Figure.listOfFigures.Add(new Lance(0, 8, true, false));//8
            //Figure.listOfFigures.Add(new Rook(1, 1, true, false));//9
            //Figure.listOfFigures.Add(new Bishop(1, 7, true, false));//10
            //Figure.listOfFigures.Add(new Pawn(2, 0, true, false));//11
            //Figure.listOfFigures.Add(new Pawn(2, 1, true, false));//12
            //Figure.listOfFigures.Add(new Pawn(2, 2, true, false));//13
            //Figure.listOfFigures.Add(new Pawn(2, 3, true, false));//14
            //Figure.listOfFigures.Add(new Pawn(2, 4, true, false));//15
            //Figure.listOfFigures.Add(new Pawn(2, 5, true, false));//16
            //Figure.listOfFigures.Add(new Pawn(2, 6, true, false));//17
            //Figure.listOfFigures.Add(new Pawn(2, 7, true, false));//18
            //Figure.listOfFigures.Add(new Pawn(2, 8, true, false));//19
            //Figure.listOfFigures.Add(new Pawn(6, 0, false, false));//20
            //Figure.listOfFigures.Add(new Pawn(6, 1, false, false));//21
            //Figure.listOfFigures.Add(new Pawn(6, 2, false, false));//22
            //Figure.listOfFigures.Add(new Pawn(6, 3, false, false));//23
            //Figure.listOfFigures.Add(new Pawn(6, 4, false, false));//24
            //Figure.listOfFigures.Add(new Pawn(6, 5, false, false));//25
            //Figure.listOfFigures.Add(new Pawn(6, 6, false, false));//26
            //Figure.listOfFigures.Add(new Pawn(6, 7, false, false));//27
            //Figure.listOfFigures.Add(new Pawn(6, 8, false, false));//28
            //Figure.listOfFigures.Add(new Bishop(7, 1, false, false));//29
            //Figure.listOfFigures.Add(new Rook(7, 7, false, false));//30
            //Figure.listOfFigures.Add(new Lance(8, 0, false, false));//31
            //Figure.listOfFigures.Add(new Knight(8, 1, false, false));//32
            //Figure.listOfFigures.Add(new Silver(8, 2, false, false));//33
            //Figure.listOfFigures.Add(new Gold(8, 3, false, false));//34
            //Figure.listOfFigures.Add(new King(8, 4, false, false));//35
            //Figure.listOfFigures.Add(new Gold(8, 5, false, false));//36
            //Figure.listOfFigures.Add(new Silver(8, 6, false, false));//37
            //Figure.listOfFigures.Add(new Knight(8, 7, false, false));//38
            //Figure.listOfFigures.Add(new Lance(8, 8, false, false));//39
            GameManager.LoadTable();
            GameManager.SaveTable();

            //string[,] tab = new string[9, 9];
            ////for (int i = 0; i < 9; i++)
            ////    for (int j = 0; j < 9; j++)
            ////    {
            ////        tab[i, j] = "O";
            ////    }
            ////foreach (Figure Figtest in Figure.listOfFigures)
            ////{
            ////    tab[Figtest.Row, Figtest.Column] = "X";
            ////    //System.Console.WriteLine("Wiersz elementu to: {0} a kolumna to: {1} ", Figtest.Row, Figtest.Column);
            ////}

            ////for (int i = 0; i < 9; i++)
            ////{
            ////    for (int j = 0; j < 9; j++)
            ////    {
            ////        Console.Write(tab[i, j]);
            ////    }
            ////    Console.WriteLine();
            ////}

            Board.ControlTheGame();

        }
    }
}