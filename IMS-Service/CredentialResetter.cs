using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IMS_Interface;
using IMS_Library;
using WindowManager;

namespace IMS_Service
{
    public sealed class CredentialResetter : Login.ILocalCredentialsResetter
    {
        private object Locker = new object();
        private Task CurrentTask;

        /// <summary>
        /// Prompts the user of the local machine to reset the IMS admin console credentials.
        /// </summary>
        /// <returns>A <see cref="Task"/> object representing the asynchronous operation.</returns>
        public Task ResetCredentialsAsync()
        {
            lock(Locker)
            {
                if(CurrentTask is null)
                {
                    CurrentTask = ResetCredentials();
                }
                return CurrentTask;
            }
        }

        private async Task ResetCredentials()
        {
            await Task.Run(() => {
                MsgBoxResult result = Interaction.MsgBox("A credentials reset for IMS was requested using the IMS remote interface.  Would you like to reset the IMS admin console username/password?", "IMS Credentials Reset", MsgBoxStyle.YesNo);
                if(result == MsgBoxResult.Yes)
                {
                    IMSSettings settings = IMS.Instance.CurrentSettings.Clone() as IMSSettings;
                    settings.Username = null;
                    settings.PasswordHash = null;
                    IMS.Instance.ChangeSettings(settings);
                }
                lock (Locker) {
                    CurrentTask = null;
                }
            });
        }
    }
}
