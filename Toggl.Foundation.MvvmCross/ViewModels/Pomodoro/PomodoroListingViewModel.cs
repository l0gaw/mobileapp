﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Toggl.Foundation.Analytics;
using Toggl.Foundation.DataSources;
using Toggl.Foundation.Diagnostics;
using Toggl.Foundation.Interactors;
using Toggl.Foundation.Login;
using Toggl.Foundation.MvvmCross.Services;
using Toggl.Foundation.Services;
using Toggl.Foundation.Sync;
using Toggl.Multivac;
using Toggl.PrimeRadiant.Settings;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;
using Toggl.Multivac.Extensions;
using Toggl.Foundation.MvvmCross.ViewModels.Pomodoro;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using Toggl.Foundation.MvvmCross.Extensions;

namespace Toggl.Foundation.MvvmCross.ViewModels
{
    [Preserve(AllMembers = true)]
    public sealed class PomodoroListingViewModel : MvxViewModel
    {
        private readonly IPomodoroStorage pomodoroStorage;
        private readonly IMvxNavigationService navigationService;
        private readonly ISchedulerProvider schedulerProvider;
        private readonly IRxActionFactory rxActionFactory;

        public UIAction Close { get; }

        private PomodoroConfiguration pomodoroConfig;

        private ISubject<IReadOnlyList<PomodoroWorkflow>> workflowsSubject
            = new BehaviorSubject<IReadOnlyList<PomodoroWorkflow>>(new List<PomodoroWorkflow>());

        public IObservable<IReadOnlyList<PomodoroWorkflow>> Workflows { get; private set; }

        public PomodoroListingViewModel(
            IMvxNavigationService navigationService,
            IPomodoroStorage pomodoroStorage,
            ISchedulerProvider schedulerProvider,
            IRxActionFactory rxActionFactory)
        {
            Ensure.Argument.IsNotNull(navigationService, nameof(navigationService));
            Ensure.Argument.IsNotNull(pomodoroStorage, nameof(pomodoroStorage));
            Ensure.Argument.IsNotNull(schedulerProvider, nameof(schedulerProvider));
            Ensure.Argument.IsNotNull(rxActionFactory, nameof(rxActionFactory));

            this.navigationService = navigationService;
            this.pomodoroStorage = pomodoroStorage;
            this.schedulerProvider = schedulerProvider;
            this.rxActionFactory = rxActionFactory;

            Close = rxActionFactory.FromAsync(close);

            Workflows = workflowsSubject
                .AsDriver(new List<PomodoroWorkflow>(), schedulerProvider);

            initializeConfiguration();
        }

        private void initializeConfiguration()
        {
            var configurationXml = pomodoroStorage.GetPomodoroConfigurationXml();
            pomodoroConfig = PomodoroConfiguration.FromXml(configurationXml);

            if (pomodoroConfig == null)
            {
                pomodoroConfig = PomodoroConfiguration.Default;
                pomodoroStorage.Save(pomodoroConfig.ToXml());
            }

            workflowsSubject.OnNext(pomodoroConfig.Workflows);
        }

        private Task close() => navigationService.Close(this);
    }
}
