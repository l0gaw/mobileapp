using System;

namespace Toggl.Core.UI.Services
{
    public interface IPermissionsService : IPermissionRequester
    {
        IObservable<bool> CalendarPermissionGranted { get; }

        IObservable<bool> NotificationPermissionGranted { get; }
    }
}
