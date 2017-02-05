using Android.App;
using Android.Widget;
using Android.OS;

namespace ShogiP1
{
    [Activity(Label = "ShogiP1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        ImageButton[,] btTable;

        private Button resurrectButton;
        private Button undoButton;

        private static bool firstClick = true;

        private static byte actualRow;
        private static byte actualColumn;
        private static byte targetRow;
        private static byte targetColumn;

        public string resurrectFigureName;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView (Resource.Layout.Main);
            Program.SetUpProgram();
            LoadButtons();
            refreshBoard();
        }

        private void LoadButtons()
        {
            btTable = new ImageButton[9,9];
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

        private void ResurrectButton_Click(object sender, System.EventArgs e)
        {
            //resurrect list
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_Resurrection resDialog = new dialog_Resurrection();
            resDialog.Show(transaction, "dialog fragment");
        }

        private void UndoButton_Click(object sender, System.EventArgs e)
        {
            //undoMove
        }

        private void Field_Click(object sender, System.EventArgs e)
        {
            string btId = (sender as ImageButton).GetTag((sender as ImageButton).Id).ToString();
            btId.ToCharArray();
            byte row = (byte)System.Char.GetNumericValue(btId[0]);
            byte column = (byte)System.Char.GetNumericValue(btId[1]);

            if (firstClick)
            {
                actualRow = row;
                actualColumn = column;
                firstClick = false;
            }
            else
            {
                targetRow = row;
                targetColumn = column;
                //if(DoMove)GameManager.DoSomething(2, actualRow, actualColumn, targetRow, targetColumn, "DoMove");
                firstClick = true;
            }
        }

        private void refreshBoard()
        {
            for (int w = 0; w < 9; w++)
            {
                for (int k = 0; k < 9; k++)
                {
                    Figure actual = Figure.FindFigure((byte)w, (byte)k);
                    if (actual == null) btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.Field);
                    else
                    {
                        string type = actual.GetType().ToString();
                        type = type.Substring(type.IndexOf(".") + 1);
                        if (actual.IsBlack)
                        {
                            switch (type)
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
                        else
                        {
                            switch (type)
                            {
                                case "Bishop":
                                    btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PBishop);
                                    break;
                                case "Gold":
                                    btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PGold);
                                    break;
                                case "King":
                                    btTable[w, k].Background = Resources.GetDrawable(Resource.Drawable.PKing);
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
                    }
                }
            }
        }
    }
}

