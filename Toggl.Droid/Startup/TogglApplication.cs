using System;
using Android.App;
using Android.Runtime;

namespace Toggl.Droid
{
    [Application(AllowBackup = false)]
    public class TogglApplication : Application
    {
        public TogglApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            Firebase.FirebaseApp.InitializeApp(this);
#if USE_ANALYTICS
            Microsoft.AppCenter.AppCenter.Start(
                "{TOGGL_APP_CENTER_ID_DROID}",
                typeof(Microsoft.AppCenter.Crashes.Crashes),
                typeof(Microsoft.AppCenter.Analytics.Analytics));
#endif
        }
    }
}
