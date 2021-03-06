﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using GWG.Resources.redcap;
using System.Threading;
using System.Threading.Tasks;

namespace GWG
{
    public class REDCapEventArg : EventArgs
    {
        private string mREDCapId;
        private string mPIN;

        public string REDCapId
        {
            get { return mREDCapId; }
            set { mREDCapId = value; }
        }

        public string PIN
        {
            get { return mPIN; }
            set { mPIN = value; }
        }

        public REDCapEventArg(string rEDCapId, string pIN) : base()
        {
            REDCapId = rEDCapId;
            PIN      = pIN;
        }

    }
        
    class dialog_SignUp1 : DialogFragment
    {
        private EditText mTxtREDCapId;
        private EditText mTxtPin;
        private EditText mTxtPinRepeat;
        private TextView mViewFailureReason;
        private Button mBtnREDCap;

        public event EventHandler<REDCapEventArg> mREDCapComplete;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.dialog_sign_up_1, container, false);

            mTxtREDCapId = view.FindViewById<EditText>(Resource.Id.txtREDCapId);
            mTxtPin = view.FindViewById<EditText>(Resource.Id.txtPin);
            mTxtPinRepeat = view.FindViewById<EditText>(Resource.Id.txtPinRepeat);
            mViewFailureReason = view.FindViewById<TextView>(Resource.Id.viewFailureReason);
            mBtnREDCap = view.FindViewById<Button>(Resource.Id.btnREDCap);

            mBtnREDCap.Click += MBtnREDCap_ClickAsync;
            return view;

        }

        private async void MBtnREDCap_ClickAsync(object sender, EventArgs e)
        {
            string id = mTxtREDCapId.Text;
            string pin1 = mTxtPin.Text;
            string pin2 = mTxtPinRepeat.Text;

            mViewFailureReason.Text = "Confirming...";
            mViewFailureReason.SetTextColor(Android.Graphics.Color.DarkGreen);

            if (string.IsNullOrEmpty(id) || !(await new REDCapHelper(id).DoesREDCapIdExistAsync()))
            {
                // Invalid REDCapId
                mViewFailureReason.Text = "Invalid REDCap ID";
                mViewFailureReason.SetTextColor(Android.Graphics.Color.Red);
            }
            else if (pin1 != pin2)
            {
                // PINs do not equal
                mViewFailureReason.Text = "PINs do not match";
                mViewFailureReason.SetTextColor(Android.Graphics.Color.Red);
            }
            else if (pin1.Length < 4)
            {
                // PIN is not at least 4 digits
                mViewFailureReason.Text = "PIN must be a minimum of 4 digits";
                mViewFailureReason.SetTextColor(Android.Graphics.Color.Red);
            }
            else
            {
                // User has clicked the sign up button and entered a valid pin
                mREDCapComplete.Invoke(this, new REDCapEventArg(id, pin1));
                this.Dismiss();
            }
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle); //Sets the title bar to invisible
            base.OnActivityCreated(savedInstanceState);
            Dialog.Window.Attributes.WindowAnimations = Resource.Style.dialog_animation; //set the animation
        }
    }
}