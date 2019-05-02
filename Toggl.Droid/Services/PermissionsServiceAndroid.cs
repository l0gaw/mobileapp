﻿using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Provider;
using Android.Support.V4.Content;
using Toggl.Core.UI.Services;
using Toggl.Droid.Helper;
using Toggl.Shared;
using Uri = Android.Net.Uri;

namespace Toggl.Droid.Services
{
    [Preserve(AllMembers = true)]
    public sealed class PermissionsServiceAndroid : IPermissionsService
    {
        private const int calendarAuthCode = 500;

        private Subject<bool> calendarAuthorizationSubject;

        public IObservable<bool> CalendarPermissionGranted
            => Observable.Start(() => checkPermissions(Manifest.Permission.ReadCalendar));

        public IObservable<bool> NotificationPermissionGranted
            => Observable.Return(true);

        public IObservable<bool> RequestCalendarAuthorization(bool force = false)
            => Observable.Throw<bool>(new NotImplementedException("This now needs to happen in the IView interface"));

        public IObservable<bool> RequestNotificationAuthorization(bool force = false)
            => Observable.Return(true);

        public void OpenAppSettings()
        {
            var settingsIntent = new Intent();
            settingsIntent.SetAction(Settings.ActionApplicationDetailsSettings);
            settingsIntent.AddCategory(Intent.CategoryDefault);
            settingsIntent.SetData(Uri.Parse("package:com.toggl.giskard"));
            settingsIntent.AddFlags(ActivityFlags.NewTask);
            settingsIntent.AddFlags(ActivityFlags.NoHistory);
            settingsIntent.AddFlags(ActivityFlags.ExcludeFromRecents);
            
            Application.Context.StartActivity(settingsIntent);
        }

        private bool checkPermissions(params string[] permissionsToCheck)
        {
            foreach (var permission in permissionsToCheck)
            {
                if (MarshmallowApis.AreAvailable)
                {
                    if (ContextCompat.CheckSelfPermission(Application.Context, permission) != Permission.Granted)
                        return false;
                }
                else
                {
                    if (PermissionChecker.CheckSelfPermission(Application.Context, permission) != PermissionChecker.PermissionGranted)
                        return false;
                }
            }

            return true;
        }
    }
}
