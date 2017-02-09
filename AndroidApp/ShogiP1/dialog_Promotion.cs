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

using System.Threading.Tasks;

namespace ShogiP1
{
    class dialog_Promotion : DialogFragment
    {
        static Button[] btPromotion;

        public bool promoting;

        public static bool _continue = false;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _continue = false;

            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.promotion_dialog, container, false);

            LoadPromotionButtons(view);

            return view;

        }

        private void LoadPromotionButtons(View view)
        {
            btPromotion = new Button[2];
            btPromotion[0] = view.FindViewById<Button>(Resource.Id.pownResurrectionButton);
            btPromotion[0].Click += (object sender, EventArgs args) =>
            {
                promoting = true;
                _continue = true;
                this.Dismiss();
            };
            btPromotion[1] = view.FindViewById<Button>(Resource.Id.lanceResurrectionButton);
            btPromotion[1].Click += (object sender, EventArgs args) =>
            {
                promoting=false;
                _continue = true;
                this.Dismiss();
            };
        }
    }
}