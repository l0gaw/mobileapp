﻿using System;
using Android.App;
using Android.App.Job;

namespace Toggl.Droid.Services
{
    [Service(Exported = true, 
             Permission = "android.permission.BIND_JOB_SERVICE",
             Name = "com.toggl.giskard.BackgroundSyncJobSchedulerService")]
    public class BackgroundSyncJobSchedulerService : JobService
    {
        public const int JobId = 1;

        private IDisposable disposable;

        public override bool OnStartJob(JobParameters @params)
        {
            // Background sync is temporary disabled due to a crash that is hard to reproduce
            // Calling JobFinished and eturning early here stops the background job from running
            JobFinished(@params, false);
            return true;

            var dependencyContainer = AndroidDependencyContainer.Instance;
            if (!dependencyContainer.UserAccessManager.CheckIfLoggedIn())
                return false;

            disposable = dependencyContainer.InteractorFactory
                .RunBackgroundSync()
                .Execute()
                .Subscribe(_ => JobFinished(@params, false));

            return true;
        }

        public override bool OnStopJob(JobParameters @params)
        {
            AndroidDependencyContainer
                .Instance.AnalyticsService
                .BackgroundSyncMustStopExcecution.Track();
            return true;
        }
    }
}
