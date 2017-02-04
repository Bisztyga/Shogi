using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ShogiP1
{
    class dialog_Resurrection : DialogFragment
    {
        Button[] btResurrection;

        private string resFigure;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.resurrection_dialog, container, false);

            LoadResurrectionButtons(view);

            return view;

        }

        private void LoadResurrectionButtons(View view)
        {
            btResurrection = new Button[8];
            btResurrection[0] = view.FindViewById<Button>(Resource.Id.pownResurrectionButton);
            btResurrection[0].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Pown";
                this.Dismiss();
            };
            btResurrection[1] = view.FindViewById<Button>(Resource.Id.lanceResurrectionButton);
            btResurrection[1].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Lance";
                this.Dismiss();
            };
            btResurrection[2] = view.FindViewById<Button>(Resource.Id.bishopResurrectionButton);
            btResurrection[2].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Bishop";
                this.Dismiss();
            };
            btResurrection[3] = view.FindViewById<Button>(Resource.Id.silverResurrectionButton);
            btResurrection[3].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Silver";
                this.Dismiss();
            };
            btResurrection[4] = view.FindViewById<Button>(Resource.Id.goldResurrectionButton);
            btResurrection[4].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Gold";
                this.Dismiss();
            };
            btResurrection[5] = view.FindViewById<Button>(Resource.Id.knightResurrectionButton);
            btResurrection[5].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Knight";
                this.Dismiss();
            };
            btResurrection[6] = view.FindViewById<Button>(Resource.Id.rookResurrectionButton);
            btResurrection[6].Click += (object sender, EventArgs args) =>
            {
                resFigure = "Rook";
                this.Dismiss();
            };
        }
    }
}