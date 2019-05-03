using System.Reactive.Subjects;
using Android.Content;
using Android.Content.PM;

namespace Toggl.Droid.Helper
{
    public interface IPermissionRequesterComponent
    {
        Subject<bool> CalendarAuthorizationSubject { get; set; }
        void RequestPermissions(string[] strings, int calendarAuthCode);
     
        Permission CheckPermission(string permission);

        void StartActivityIntent(Intent intent);
    }
}
