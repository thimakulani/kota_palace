using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;

namespace Kota_Palace_Admin.Data
{
    public static class FirestoreInstance
    {

        public static FirestoreDb GetInstance(IWebHostEnvironment _appEnvironment)
        {
            string path = _appEnvironment.WebRootPath + @"/account_service.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb firestoreDb = FirestoreDb.Create("local-kota-15d60");
            return firestoreDb;
        }
        public static FirebaseMessaging GetFirebaseMessaging(IWebHostEnvironment _appEnvironment)
        {
            FirebaseMessaging messaging;
            string path = _appEnvironment.WebRootPath + @"/account_service.json";
            FirebaseAdmin.AppOptions options = new()
            {
                Credential = GoogleCredential.FromFile(path),
                ProjectId = "local-kota-15d60",
                ServiceAccountId = "firebase-adminsdk-6p3vo@local-kota-15d60.iam.gserviceaccount.com",

            };

            if (FirebaseAdmin.FirebaseApp.DefaultInstance == null)
            {
                var app = FirebaseAdmin.FirebaseApp.Create(options);
                messaging = FirebaseMessaging.GetMessaging(app);
            }
            else
            {
                messaging = FirebaseMessaging.DefaultInstance;//.FirebaseAuth.DefaultInstance;
            }

            return messaging;
        }
    }
}
