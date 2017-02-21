using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics.Drawables;
using System.Threading.Tasks;

namespace ShogiP1
{
    [Activity(Label = "ShogiP1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ImageButton[,] btTable;

        private Button resurrectButton;
        private Button undoButton;

        private static bool firstClick = true;
        private static bool figureResurrection = false; 

        private static byte actualRow;
        private static byte actualColumn;
        private static byte targetRow;
        private static byte targetColumn;

        public string resurrectFigureName;

        public enum Msg : byte
        {
            Undo = 0,
            LoadBoard = 1,
            VLoadBoard = 10,
            Move = 2,
            VMoveAsk = 20,
            VmoveGo = 21,
            Promote = 3,
            VPromote = 30,
            Ressurect = 4,
            VRessurect = 40,
            PossibleMoves = 5,
            VPossibleMoves = 50,
            PossibleRessurection = 6,
            VPossibleRessurection = 60,
            AnyMove = 7,
            VAnyMoveTrue = 70,
            VAnyMoveFalse = 71,
            DeadOfType = 8,
            VDeadOfTypeTrue = 80,
            VDeadOfTypeFalse = 81,
            ClearGreen = 9,
            VClearGreen = 90,

        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            Program.SetUpProgram();
            LoadButtons();
            refreshBoard();
        }

        private void LoadButtons()
        {
            btTable = new ImageButton[9, 9];
            for (int w = 0; w < 9; w++)
            {
                for (int k = 0; k < 9; k++)
                {
                    //Declaration of table of imageButtons in loop
                    string buttonID = "ImageButton" + w + k;
                    int resID = Resources.GetIdentifier(buttonID, "id", PackageName);
                    btTable[w, k] = FindViewById<ImageButton>(resID);

                    //Add tags; Tags contain position of button; It use tags to send position of button to the event of clicked button
                    string tag = "" + w + k;
                    btTable[w, k].SetTag(resID, tag);

                    //Add events
                    btTable[w, k].Click += Field_Click;
                }
            }
            undoButton = FindViewById<Button>(Resource.Id.buttonUndo);
            resurrectButton = FindViewById<Button>(Resource.Id.buttonResurrect);
            undoButton.Click += UndoButton_Click;
            resurrectButton.Click += ResurrectButton_Click;
        }

        private async void ResurrectButton_Click(object sender, System.EventArgs e)
        {
            //resurrect list
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_Resurrection resDialog = new dialog_Resurrection();
            resDialog.Show(transaction, "dialog fragment");
            dialog_Resurrection._continue = false;
            while (!dialog_Resurrection._continue)
            {
                await Task.Delay(100);
            }
            resurrectFigureName = dialog_Resurrection.resFigure;
            if(GameManager.DoSomething((byte)Msg.PossibleRessurection, 10, 10, 10, 10, resurrectFigureName) == (byte)Msg.VPossibleRessurection)
            {
                firstClick = false;
                figureResurrection = true;
            }
            refreshBoard();
        }


        private async System.Threading.Tasks.Task<bool> PromoteAction()
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_Promotion promDialog = new dialog_Promotion();
            promDialog.Show(transaction, "dialog fragment");
            //We maust stay there before button isUnclicked
            dialog_Promotion._continue = false;
            while (!dialog_Promotion._continue)
            {
                await Task.Delay(100);
            }
            if (promDialog.promoting == true)
            {
                return true;
            };
            return false;
        }

        private void UndoButton_Click(object sender, System.EventArgs e)
        {
            if (GameManager.DoSomething((byte)Msg.Undo) == 0)
                refreshBoard();
        }

        private async void Field_Click(object sender, System.EventArgs e)
        {
            string btId = (sender as ImageButton).GetTag((sender as ImageButton).Id).ToString();
            btId.ToCharArray();
            byte row = (byte)System.Char.GetNumericValue(btId[0]);
            byte column = (byte)System.Char.GetNumericValue(btId[1]);
            byte promotinginfo;
            Figure actual = Figure.FindFigure((byte)row, (byte)column);
            if (firstClick) //If we take Figure from board
            {
                if (actual == null) //We dont like working on null's and we can't move field
                    return;
                if (actual.IsBlack != GameManager.BlackToMove)
                    return;
                actualRow = row;
                actualColumn = column;
                firstClick = false;
                if (GameManager.DoSomething((byte)Msg.AnyMove, actualRow, actualColumn) == 71) //We want to know fast if we want unclick
                    firstClick = true;
                else
                {
                    GameManager.DoSomething((byte)Msg.PossibleMoves, actualRow, actualColumn);
                    refreshBoard();
                }
            }
            else //If we we place Figure on Board:
            {
                if (Figure.TableToFrotend[row, column] == true) // If it's legal move
                {
                    targetRow = row;
                    targetColumn = column;
                    firstClick = true;
                    if(figureResurrection == false)
                    {
                        promotinginfo = GameManager.DoSomething((byte)Msg.Move, actualRow, actualColumn, targetRow, targetColumn);
                        GameManager.DoSomething((byte)Msg.ClearGreen);
                        refreshBoard();
                        if (promotinginfo == (byte)Msg.VMoveAsk)
                        {
                            bool prAc = await PromoteAction();
                            if (prAc)
                            {
                                GameManager.DoSomething((byte)Msg.Promote, 10, 10, targetRow, targetColumn);
                                refreshBoard();
                            }
                            else
                                GameManager.NextMove();
                        }
                    }
                    else
                    {
                        GameManager.DoSomething((byte)Msg.Ressurect, 255, 255, targetRow, targetColumn, resurrectFigureName);
                        GameManager.DoSomething((byte)Msg.ClearGreen);
                        refreshBoard();
                        figureResurrection = false;
                    }
                }
                else //If it's illegal move
                {
                    GameManager.DoSomething((byte)Msg.ClearGreen);
                    firstClick = true;
                    figureResurrection = false;
                    refreshBoard();
                }
            }
        }


        private void refreshBoard()
        {
            for (int w = 0; w < 9; w++)
            {
                for (int k = 0; k < 9; k++)
                {
                    Figure actual = Figure.FindFigure((byte)w, (byte)k);
                    if (actual == null)
                    {
                        if (Figure.TableToFrotend[w, k] == false)
                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Field);
                        else
                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZField);
                    }
                    else
                    {
                        string type = actual.GetType().ToString();
                        type = type.Substring(type.IndexOf(".") + 1);
                        if (!actual.IsBlack)
                        {
                            if (!actual.IsPromoted)
                            {
                                if (!Figure.TableToFrotend[w, k])
                                {
                                    switch (type) //black, no promoted, yellow
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Bishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Gold);
                                            break;
                                        case "King":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.King);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Knight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Lance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Pawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Rook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Silver);
                                            break;
                                    }
                                }
                                else //black, no promoted green
                                {
                                    switch (type)
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZGold);
                                            break;
                                        case "King":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZKing);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZSilver);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (!Figure.TableToFrotend[w, k])
                                {
                                    switch (type) //black, promoted, yellow
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PGold);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PSilver);
                                            break;
                                    }
                                }
                                else //black, promoted green
                                {
                                    switch (type)
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPGold);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ZPSilver);
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!actual.IsPromoted)
                            {
                                if (!Figure.TableToFrotend[w, k])
                                {
                                    switch (type) //red, no promoted, yellow
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OGold);
                                            break;
                                        case "King":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OKing);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.ORook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OSilver);
                                            break;
                                    }
                                }
                                else //red, no promoted green
                                {
                                    switch (type)
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZGold);
                                            break;
                                        case "King":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZKing);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZSilver);
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                if (!Figure.TableToFrotend[w, k])
                                {
                                    switch (type) //red, promoted, yellow
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPGold);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OPSilver);
                                            break;
                                    }
                                }
                                else //red, promoted green
                                {
                                    switch (type)
                                    {
                                        case "Bishop":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPBishop);
                                            break;
                                        case "Gold":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPGold);
                                            break;
                                        case "Knight":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPKnight);
                                            break;
                                        case "Lance":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPLance);
                                            break;
                                        case "Pawn":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPPawn);
                                            break;
                                        case "Rook":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPRook);
                                            break;
                                        case "Silver":
                                            btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.OZPSilver);
                                            break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}