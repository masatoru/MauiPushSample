using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiPushSample
{
    public class Constants
    {
        public const string ListenConnectionString = "Endpoint=sb://{Your Hub Namespace}.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey={Your SharedAccessKey}";
        public const string NotificationHubName = "{Your Hub Name}";
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
        public static string[] SubscriptionTags { get; set; } = { "sports", "music" };

    }
}
