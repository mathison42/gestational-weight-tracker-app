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
using Android.Support.V7.Widget;
using Java.Util;

namespace GWG.Resources.adapter
{
    class RecyclerAdapter : RecyclerView.Adapter
    {
        private List<DateWeight> weightData = new List<DateWeight>();

        public RecyclerAdapter(List<DateWeight> weightData)
        {
            this.weightData = weightData;
        }
        public override int ItemCount
        {
            get
            {
                return weightData.Count; 
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            RecyclerViewHolder vh = holder as RecyclerViewHolder;
            var item = weightData[position];
            vh.mText1.Text = new DateTime(item.mDate).ToString();
            vh.mText2.Text = item.mWeight.ToString();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.weightItem, parent, false);

            return new RecyclerViewHolder(itemView);
        }

        public class RecyclerViewHolder : RecyclerView.ViewHolder
        {
            public TextView mText1 { get; private set; }
            public TextView mText2 { get; private set; }

            public RecyclerViewHolder(View itemView) : base(itemView)
            {
                mText1 = itemView.FindViewById<TextView>(Resource.Id.text1);
                mText2 = itemView.FindViewById<TextView>(Resource.Id.text2);
            }

        }
    }
}