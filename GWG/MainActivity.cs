using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using System.Threading;
using Android.Content;
using Android.Accounts;
using GWG.Resources.redcap;
using System.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Android.Content.PM;

namespace GWG
{
    [Activity(Label = "GWG", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        public static string AppName { get { return "GWG"; } }
        CredentialsService storeService;

        private EditText mLoginPIN;
        private TextView mViewPIN;
        private Button mBtnSignUp;
        private Button mBtnLogin;
        private ImageView mImgLock;
        private ProgressBar mProgressBar;

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);

            storeService = new CredentialsService();

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            Window.SetSoftInputMode(SoftInput.StateAlwaysHidden);
            mLoginPIN = FindViewById<EditText>(Resource.Id.loginPIN);
            mViewPIN = FindViewById<TextView>(Resource.Id.viewPIN);
            mBtnSignUp = FindViewById<Button>(Resource.Id.btnSignUp);
            mBtnLogin  = FindViewById<Button>(Resource.Id.btnLogin);
            mImgLock   = FindViewById<ImageView>(Resource.Id.imgLock);
            mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);


            mBtnSignUp.Click += MBtnSignUp_Click;
            mBtnLogin.Click  += MBtnLogin_Click;

            REDCapHelper rch = new REDCapHelper("123567");
            //rch.GetVersion();

        }

        private async void MBtnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string pin = mLoginPIN.Text;
                if (!string.IsNullOrWhiteSpace(pin))
                {
                    bool doCredentialsExist = storeService.DoCredentialsExist();
                    if (!doCredentialsExist)
                    {
                        mViewPIN.Text = "Please create an account";
                        mViewPIN.SetTextColor(Android.Graphics.Color.Red);

                    } else
                    {
                        // Confirm User and PIN
                        // Console.WriteLine("Pin: " + pin);
                        // Console.WriteLine("Saved PIN: " + storeService.PIN);
                        if (pin == storeService.PIN)
                        {

                            REDCapHelper rch = new REDCapHelper(storeService.REDCapID);
                            REDCapResult result = await rch.GetProfile();
                            result.parseJson2DateWeightList();
                            

                            if (result.redcapid != null)
                            {
                                if (result.redcapid == storeService.REDCapID)
                                {
                                    result.printRecord();

                                    // Reset Values
                                    mLoginPIN.Text = "";
                                    mViewPIN.Text = "Enter PIN";
                                    mViewPIN.SetTextColor(Android.Graphics.Color.ForestGreen);

                                    var intent = new Intent(this, typeof(MainToolbarActivity));
                                    intent.PutExtra("record", JsonConvert.SerializeObject(result));
                                    StartActivity(intent);
                                }
                                else if (result.redcapid != storeService.REDCapID)
                                {
                                    mViewPIN.Text = "Error: Please contact study cordinator";
                                    mViewPIN.SetTextColor(Android.Graphics.Color.Red);
                                }
                            }
                            else
                            {
                                mViewPIN.Text = "Error: Try again";
                                mViewPIN.SetTextColor(Android.Graphics.Color.Red);
                            }
                        }
                        else
                        {
                            mViewPIN.Text = "Invalid PIN";
                            mViewPIN.SetTextColor(Android.Graphics.Color.Red);
                        }
                    }
                } else
                {
                    mViewPIN.Text = "Enter PIN";
                    mViewPIN.SetTextColor(Android.Graphics.Color.Red);
                }

            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                mViewPIN.Text = "Error: Try again";
                mViewPIN.SetTextColor(Android.Graphics.Color.Red);
            }
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
            string id = e.REDCapId;
            Console.WriteLine("ID: " + id);
            Console.WriteLine("Stored UserName: " + storeService.UserName);
            Console.WriteLine("Stored REDCapID: " + storeService.REDCapID);
            Console.WriteLine("New ID: " + id);
            
            if (!storeService.DoCredentialsExist() || storeService.REDCapID == id)
            {

                mImgLock.Visibility = Android.Views.ViewStates.Invisible;
                mProgressBar.Visibility = Android.Views.ViewStates.Visible;
                //mImgLock.SetImageDrawable(GetDrawable(Resource.Drawable.ProgressBarStyle));
                // Confirm that REDCap ID is valid....
                bool validRedCapId = false;
                Thread thread = new Thread(() =>
                {
                    validRedCapId = ConfirmRedCAPId(id);
                });
                thread.Start();
                thread.Join();

                // Save Password
                if (validRedCapId)
                {
                    storeService.SaveCredentials("user", id, e.PIN);
                    mViewPIN.Text = "Enter PIN";
                    mViewPIN.SetTextColor(Android.Graphics.Color.ForestGreen);
                }
                else
                {
                    // Invalid Red Cap ID
                    mViewPIN.Text = "Invalid Red Cap ID";
                    mViewPIN.SetTextColor(Android.Graphics.Color.Red);
                }
            }
            else
            {
                // If attempting to reset PIN
                mViewPIN.Text = "REDCap ID does not match original ID";
                mViewPIN.SetTextColor(Android.Graphics.Color.Red);
            }
        }

        private bool ConfirmRedCAPId(string id)
        {
            Thread.Sleep(3000);
            RunOnUiThread(() =>
            {
                mProgressBar.Visibility = Android.Views.ViewStates.Invisible;
                mImgLock.Visibility = Android.Views.ViewStates.Visible;
            });

            return true;
        }
    }
}

