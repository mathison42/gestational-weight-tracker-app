using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.Threading;
using Android.Content;

namespace GWG
{
    [Activity(Label = "GWG", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button mBtnSignUp;
        private Button mBtnLogin;
        private ImageView mImgLock;
        private ProgressBar mProgressBar;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnLogin  = FindViewById<Button>(Resource.Id.btnLogin);
            mImgLock   = FindViewById<ImageView>(Resource.Id.imgLock);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);


            mBtnSignUp.Click += MBtnSignUp_Click;
            mBtnLogin.Click  += MBtnLogin_Click;
        }

        private void MBtnLogin_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(MainToolbarActivity));
            StartActivity(intent);
        }

        private void MBtnSignUp_Click(object sender, EventArgs e)
        {
            //Pull up dialog
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            dialog_SignUp1 signUpDialog = new dialog_SignUp1();
            signUpDialog.Show(transaction, "dialog fragment");

            signUpDialog.mREDCapComplete += SignUpDialog_mREDCapComplete;
        }

        private void SignUpDialog_mREDCapComplete(object sender, REDCapEventArg e)
        {
            mImgLock.Visibility = Android.Views.ViewStates.Invisible;
            mProgressBar.Visibility = Android.Views.ViewStates.Visible;
            //mImgLock.SetImageDrawable(GetDrawable(Resource.Drawable.ProgressBarStyle));
            string id = e.REDCapId;
            Console.WriteLine("ID: " + id);
            // Confirm that REDCap ID is valid....
            Thread thread = new Thread(() => ConfirmRedCAPId(id));
            thread.Start();

        }

        private void ConfirmRedCAPId(string id)
        {
            Thread.Sleep(3000);
            RunOnUiThread(() =>
            {
                mProgressBar.Visibility = Android.Views.ViewStates.Invisible;
                mImgLock.Visibility = Android.Views.ViewStates.Visible;
            });
        }
    }
}

