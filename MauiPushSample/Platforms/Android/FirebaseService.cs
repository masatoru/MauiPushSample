using Android.App;
using Android.Content;
using Android.Mtp;
using Android.OS;
using Android.Util;
using Firebase.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAzure.Messaging;

namespace MauiPushSample.Platforms.Android
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class FirebaseService : FirebaseMessagingService
    {
        public override void OnMessageReceived(RemoteMessage message)
        {
            base.OnMessageReceived(message);
            string messageBody = string.Empty;

            if (message.GetNotification() != null)
            {
                messageBody = message.GetNotification().Body;
            }

            // NOTE: test messages sent via the Azure portal will be received here
            else
            {
                messageBody = message.Data.Values.First();
            }

            // convert the incoming message to a local notification
            // SendLocalNotification(messageBody);

            // send the incoming message directly to the MainPage
            SendMessageToMainPage(messageBody);
        }

        public override void OnNewToken(string token)
        {
            SendRegistrationToServer(token);
        }

        void SendMessageToMainPage(string body)
        {
            var appShell = App.Current.MainPage as AppShell;
            var mainPage = appShell.CurrentPage as MainPage;

            mainPage.AddMessage(body);

            Log.Debug("firebase", $"Message:{body}");
        }

        void SendRegistrationToServer(string token)
        {
            try
            {
                NotificationHub hub = new NotificationHub(Constants.NotificationHubName, Constants.ListenConnectionString, this);

                // register device with Azure Notification Hub using the token from FCM
                Registration registration = hub.Register(token, Constants.SubscriptionTags);

                // subscribe to the SubscriptionTags list with a simple template.
                string pnsHandle = registration.PNSHandle;
                TemplateRegistration templateReg = hub.RegisterTemplate(pnsHandle, "defaultTemplate", Constants.FCMTemplateBody, Constants.SubscriptionTags);
            }
            catch (Exception e)
            {
                Log.Error("firebase", $"Error registering device: {e.Message}");
            }
        }
    }
}
