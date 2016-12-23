using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    static class GameManager
    {
        public static bool BlackToMove = true;
        public static int MoveCounter = 0;
        private static string incrementationFileName()
        {
            string path = "D:\\savedgame";
            path = path.Insert(path.Length, MoveCounter.ToString());
            return path;
        }
        public static void LoadTable()
        {
            Figure.listOfFigures.Clear();
            bool tempIsPromoted, tempIsBlack;
            byte tempRow, tempColumn;
            string tempFigureType;
            string path = incrementationFileName();
            string line;
            if (!(System.IO.File.Exists(path)))
            {
                System.IO.File.Create(path).Dispose();
            }
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                tempRow = Convert.ToByte(line.Substring(0, line.IndexOf(" ")));
                line = line.Substring(line.IndexOf(" ") + 1);
                tempColumn = Convert.ToByte(line.Substring(0, line.IndexOf(" ")));
                line = line.Substring(line.IndexOf(" ") + 1);
                tempIsBlack = Convert.ToBoolean(line.Substring(0, line.IndexOf(" ")));
                line = line.Substring(line.IndexOf(" ") + 1);
                tempIsPromoted = Convert.ToBoolean(line.Substring(0, line.IndexOf(" ")));
                line = line.Substring(line.IndexOf(".") + 1);
                tempFigureType = line;
                switch (tempFigureType)
                {
                    case "Bishop":
                        Figure.listOfFigures.Add(new Bishop(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Gold":
                        Figure.listOfFigures.Add(new Gold(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "King":
                        Figure.listOfFigures.Add(new King(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Knight":
                        Figure.listOfFigures.Add(new Knight(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Lance":
                        Figure.listOfFigures.Add(new Lance(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Pawn":
                        Figure.listOfFigures.Add(new Pawn(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Rook":
                        Figure.listOfFigures.Add(new Rook(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                    case "Silver":
                        Figure.listOfFigures.Add(new Silver(tempRow, tempColumn, tempIsBlack, tempIsPromoted));
                        break;
                }
            }

            file.Close();
        }

        public static void SaveTable()
        {
            string path = incrementationFileName();
            System.IO.StreamWriter file;
            if (!(System.IO.File.Exists(path)))
            {
                System.IO.File.Create(path).Dispose();
            }
            file = new System.IO.StreamWriter(path);
            foreach (var Figtest in Figure.listOfFigures)
            {
                file.WriteLine(Figtest.Row.ToString() + " " + Figtest.Column.ToString() + " " + Figtest.IsBlack.ToString() + " " + Figtest.IsPromoted.ToString() + " " + Figtest.GetType().ToString());
            }
            file.Close();
        }

        public static void undoMove()
        {
            MoveCounter -= 1;
            incrementationFileName();
            LoadTable();
            if (BlackToMove) BlackToMove = false;
            else BlackToMove = true;
        }
    }
}
