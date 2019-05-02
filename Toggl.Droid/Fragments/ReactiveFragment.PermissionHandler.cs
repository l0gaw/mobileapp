using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Android.Support.V4.Content;
using Toggl.Droid.Helper;

namespace Toggl.Droid.Fragments
{
    public abstract partial class ReactiveFragment<TViewModel>
    {
        private const int calendarAuthCode = 500;
        
        public Action<int, string[], Permission[]> OnPermissionChangedCallback { get; set; }

        private Subject<bool> calendarAuthorizationSubject;

        public IObservable<bool> RequestCalendarAuthorization(bool force = false)
            => Observable.Defer(() =>
            {
                if (checkPermissions(Manifest.Permission.ReadCalendar))
                    return Observable.Return(true);

                if (calendarAuthorizationSubject != null)
                    return calendarAuthorizationSubject.AsObservable();

                
                OnPermissionChangedCallback = onPermissionChanged;
                calendarAuthorizationSubject = new Subject<bool>();
                RequestPermissions(new[] { Manifest.Permission.ReadCalendar, Manifest.Permission.WriteCalendar }, calendarAuthCode);

                return calendarAuthorizationSubject.AsObservable();
            });

        private void onPermissionChanged(int requestCode, string[] permissions, Permission[] grantResults)
        {
            if (requestCode != calendarAuthCode)
            {
                calendarAuthorizationSubject?.OnNext(false);
                calendarAuthorizationSubject?.OnCompleted();
                calendarAuthorizationSubject = null;
            }

            var permissionWasGranted = grantResults.Any() && grantResults.First() == Permission.Granted;
            calendarAuthorizationSubject?.OnNext(permissionWasGranted);
            calendarAuthorizationSubject?.OnCompleted();
            calendarAuthorizationSubject = null;
        }

        public IObservable<bool> RequestNotificationAuthorization(bool force = false)
            => Observable.Return(true);

        public void OpenAppSettings()
        {
            var settingsIntent = new Intent();
            settingsIntent.SetAction(Settings.ActionApplicationDetailsSettings);
            settingsIntent.AddCategory(Intent.CategoryDefault);
            settingsIntent.SetData(Android.Net.Uri.Parse("package:com.toggl.giskard"));
            settingsIntent.AddFlags(ActivityFlags.NewTask);
            settingsIntent.AddFlags(ActivityFlags.NoHistory);
            settingsIntent.AddFlags(ActivityFlags.ExcludeFromRecents);
            
            StartActivity(settingsIntent);
        }

        private bool checkPermissions(params string[] permissionsToCheck)
        {
            foreach (var permission in permissionsToCheck)
            {
                if (MarshmallowApis.AreAvailable)
                {
                    if (ContextCompat.CheckSelfPermission(Activity, permission) != Permission.Granted)
                        return false;
                }
                else
                {
                    if (PermissionChecker.CheckSelfPermission(Activity, permission) != PermissionChecker.PermissionGranted)
                        return false;
                }
            }

            return true;
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            OnPermissionChangedCallback?.Invoke(requestCode, permissions, grantResults);
            OnPermissionChangedCallback = null;
        }
    }
}