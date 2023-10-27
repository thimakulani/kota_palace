using Google.Cloud.Firestore;

namespace Kota_Palace_Admin.Data
{
    public static class FirestoreInstance
    {
        
        public static FirestoreDb GetInstance(IWebHostEnvironment _appEnvironment)
        {
            string path = _appEnvironment.WebRootPath + @"/account_service.json";
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            FirestoreDb firestoreDb =  FirestoreDb.Create("local-kota-15d60");
            return firestoreDb;
        }
    }
}
