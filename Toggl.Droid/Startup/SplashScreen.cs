using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Toggl.Core;
using Toggl.Core.UI;
using Toggl.Core.UI.Parameters;
using Toggl.Core.UI.ViewModels;
using Toggl.Droid.BroadcastReceivers;
using Toggl.Droid.Helper;
using Toggl.Networking;
using static Android.Content.Intent;

namespace Toggl.Droid
{
    [Activity(Label = "Toggl for Devs",
              MainLauncher = true,
              Icon = "@mipmap/ic_launcher",
              Theme = "@style/Theme.Splash",
              ScreenOrientation = ScreenOrientation.Portrait,
              ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
    [IntentFilter(
        new[] { "android.intent.action.VIEW", "android.intent.action.EDIT" },
        Categories = new[] { "android.intent.category.BROWSABLE", "android.intent.category.DEFAULT" },
        DataSchemes = new[] { "toggl" },
        DataHost = "*")]
    [IntentFilter(
        new[] { "android.intent.action.PROCESS_TEXT" },
        Categories = new[] { "android.intent.category.DEFAULT" },
        DataMimeType = "text/plain")]
    public class SplashScreen : AppCompatActivity
    {
        private const ApiEnvironment environment =
#if USE_PRODUCTION_API
                        ApiEnvironment.Production;
#else
                        ApiEnvironment.Staging;
#endif

        protected override void OnCreate(Bundle bundle)
        {
#if !USE_PRODUCTION_API
            System.Net.ServicePointManager.ServerCertificateValidationCallback
                  += (sender, certificate, chain, sslPolicyErrors) => true;
#endif
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SplashScreen);
            
            var applicationContext = Application.Context;
            var packageInfo = applicationContext.PackageManager.GetPackageInfo(applicationContext.PackageName, 0);

            AndroidDependencyContainer.EnsureInitialized(environment, Platform.Giskard, packageInfo.VersionName);

            var dependencyContainer = AndroidDependencyContainer.Instance;
            ApplicationContext.RegisterReceiver(new TimezoneChangedBroadcastReceiver(dependencyContainer.TimeService),
                new IntentFilter(ActionTimezoneChanged));

            //createApplicationLifecycleObserver(dependencyContainer.BackgroundService);

            var app = new App<LoginViewModel, CredentialsParameter>(AndroidDependencyContainer.Instance);
            app.Initialize().ContinueWith(_ =>
            {
                Finish();
            });


            //MVEXIT: Reimplementing url based navigation is meant to be done on another PR
            //var navigationUrl = Intent.Data?.ToString() ?? getTrackUrlFromProcessedText();
            //var navigationService = AndroidDependencyContainer.Instance.NavigationService;
            //if (string.IsNullOrEmpty(navigationUrl))
            //{
            //    Finish();
            //    return;
            //}

            //navigationService.Navigate(navigationUrl).ContinueWith(_ =>
            //{
            //    Finish();
            //});
        }

        private string getTrackUrlFromProcessedText()
        {
            if (MarshmallowApis.AreNotAvailable)
                return null;

            var description = Intent.GetStringExtra(ExtraProcessText);
            if (string.IsNullOrWhiteSpace(description))
                return null;

            var applicationUrl = ApplicationUrls.Main.Track(description);
            return applicationUrl;
        }

        // MVEXIT: Fix before merging into develop
        //protected override IMvxAndroidCurrentTopActivity CreateAndroidCurrentTopActivity()
        //{
        //    var mvxApplication = MvxAndroidApplication.Instance;
        //    var activityLifecycleCallbacksManager = new QueryableMvxLifecycleMonitorCurrentTopActivity();
        //    mvxApplication.RegisterActivityLifecycleCallbacks(activityLifecycleCallbacksManager);
        //    return activityLifecycleCallbacksManager;
        //}

        //private void createApplicationLifecycleObserver(IBackgroundService backgroundService)
        //{
        //    var mvxApplication = MvxAndroidApplication.Instance;
        //    var appLifecycleObserver = new ApplicationLifecycleObserver(backgroundService);
        //    mvxApplication.RegisterActivityLifecycleCallbacks(appLifecycleObserver);
        //    mvxApplication.RegisterComponentCallbacks(appLifecycleObserver);
        //}
    }
}
