using System.Linq;
using Xamarin.Auth;

namespace GWG
{
    public class CredentialsService
    {
        public string UserName
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(GWG.MainActivity.AppName).FirstOrDefault();
                return (account != null) ? account.Username : null;
            }
        }

        public string REDCapID
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(GWG.MainActivity.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["REDCapID"] : null;
            }
        }

        public string PIN
        {
            get
            {
                var account = AccountStore.Create().FindAccountsForService(GWG.MainActivity.AppName).FirstOrDefault();
                return (account != null) ? account.Properties["PIN"] : null;
            }
        }

        public void SaveCredentials(string userName, string redCapID, string pin)
        {
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(pin))
            {
                Account account = new Account
                {
                    Username = userName
                };
                account.Properties.Add("REDCapID", redCapID);
                account.Properties.Add("PIN", pin);
                AccountStore.Create().Save(account, GWG.MainActivity.AppName);
            }

        }

        public void DeleteCredentials()
        {
            var account = AccountStore.Create().FindAccountsForService(GWG.MainActivity.AppName).FirstOrDefault();
            if (account != null)
            {
                AccountStore.Create().Delete(account, GWG.MainActivity.AppName);
            }
        }

        public bool DoCredentialsExist()
        {
            return AccountStore.Create().FindAccountsForService(GWG.MainActivity.AppName).Any() ? true : false;
        }
    }
}