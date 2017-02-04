using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShogiP1
{
    static class GameManager
    {

        public static byte DoSomething(byte code, byte StartRow=10, byte StartColumn=10, byte TargerRow=10, byte TargetColumn=10, string name="anything")
        {
            bool promoting;
            switch (code)
            {
                case 0: //Undo move
                    LogMessageToFile("Decided to undo move");
                    undoMove();
                    return 0;
                case 1: //Load board from file
                    LogMessageToFile("Decided to load board");
                    LoadTable(true, name);
                    return 10;
                case 2: // Move 
                    LogMessageToFile("Decided to move");
                    Figure FigureToMove = Figure.FindFigure(StartRow, StartColumn);
                    if (FigureToMove.IsBlack == BlackToMove)
                        promoting = FigureToMove.Move(TargerRow, TargetColumn);
                    else
                    {
                        LogMessageToFile("Wrong color of Figure");
                        return 20;
                    }
                    if (promoting == true)
                    {
                        return 21;
                    }
                    NextMove();
                    return 23;
                case 3: // Promote
                    LogMessageToFile("Decided to promote");
                    Figure FigureToPromote = Figure.FindFigure(StartRow, StartColumn);
                    FigureToPromote.Promote();
                    NextMove();
                    return 30;
                case 4: // Ressurect
                    LogMessageToFile("Decided to ressurect");
                    foreach (Figure FigureToRessurect in Figure.listOfFigures)
                    {
                        if (FigureToRessurect.Column==255 && FigureToRessurect.GetType().ToString().Contains(name) && FigureToRessurect.IsBlack==BlackToMove)
                        {
                            FigureToRessurect.Move(TargerRow, TargetColumn);
                            return 40;  
                        }
                    }
                    NextMove();
                    return 41;
                case 5: //Fill "TableToFrontend" with possible moves
                    Figure.clearMoveBoard(Figure.TableToFrotend);
                    Figure FigureTOCheckMoves = Figure.FindFigure(StartRow, StartColumn);
                    FigureTOCheckMoves.WhereToMove();
                    Figure.GiveFieldsToFrontend();
                    return 50;
                case 6: //Fill "TableToFrontend" with possible places to ressurection
                    Figure.clearMoveBoard(Figure.TableToFrotend);
                    foreach (Figure FigureToRessurect in Figure.listOfFigures)
                    {
                        if (FigureToRessurect.Column == 255 && FigureToRessurect.GetType().ToString().Contains(name) && FigureToRessurect.IsBlack == BlackToMove)
                        {
                            FigureToRessurect.WhereToRessurect();
                            Figure.GiveFieldsToFrontend();
                            return 60;
                        }
                    }
                    return 61;
                case 7:
                    if (Figure.IsAnyMovePossible(StartRow, StartColumn))
                        return 70;
                    else
                        return 71;
                case 8:
                    foreach (Figure FigureToRessurect in Figure.listOfFigures)
                    {
                        if (FigureToRessurect.Column == 255 && FigureToRessurect.GetType().ToString().Contains(name) && FigureToRessurect.IsBlack == BlackToMove)
                        {
                            return 80;
                        }
                    }
                    return 81;
                default:
                    return byte.MaxValue;
            }
        }
        public static bool BlackToMove = true;
        public static int MoveCounter = 0;
        public static string GetPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            //System.Environment.CurrentDirectory;
            //Environment.ExternalStorageDirectory;
            //Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (!path.EndsWith("\\")) path += "\\";
            return path;
        }
        public static void LogMessageToFile(string msg)
        {
            System.IO.StreamWriter sw = System.IO.File.AppendText(
                GetPath() + "My Log File.txt");
            try
            {
                string logLine = System.String.Format(
                    "{0:G}: {1}.", System.DateTime.Now, msg);
                sw.WriteLine(logLine);
            }
            finally
            {
                sw.Close();
            }
        }
        private static string incrementationFileName()
        {
            string path = GetPath();
            path = path.Insert(path.Length, MoveCounter.ToString());
            return path;
        }
        public static void LoadTable(bool load=false, string path="anything")
        {
            if(Figure.listOfFigures != null ) Figure.listOfFigures.Clear();
            bool tempIsPromoted, tempIsBlack;
            byte tempRow, tempColumn;
            string tempFigureType;
            if (load==false)
                path = incrementationFileName();
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
        private static void NextMove()
        {
            MoveCounter += 1;
            BlackToMove = BlackToMove ^ true; 
        }
    }
}
