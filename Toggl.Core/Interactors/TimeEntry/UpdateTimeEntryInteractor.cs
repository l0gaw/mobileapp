﻿using System;
using System.Reactive.Linq;
using Toggl.Core.DataSources;
using Toggl.Core.DTOs;
using Toggl.Core.Extensions;
using Toggl.Core.Models;
using Toggl.Core.Models.Interfaces;
using Toggl.Core.Sync;
using Toggl.Shared;
using Toggl.Shared.Extensions;
using Toggl.Storage;

namespace Toggl.Core.Interactors
{
    internal class UpdateTimeEntryInteractor : IInteractor<IObservable<IThreadSafeTimeEntry>>
    {
        private readonly EditTimeEntryDto dto;
        private readonly ITimeService timeService;
        private readonly ITogglDataSource dataSource;
        private readonly IInteractorFactory interactorFactory;
        private readonly ISyncManager syncManager;

        public UpdateTimeEntryInteractor(
            ITimeService timeService,
            ITogglDataSource dataSource,
            IInteractorFactory interactorFactory,
            ISyncManager syncManager,
            EditTimeEntryDto dto)
        {
            Ensure.Argument.IsNotNull(dto, nameof(dto));
            Ensure.Argument.IsNotNull(dataSource, nameof(dataSource));
            Ensure.Argument.IsNotNull(timeService, nameof(timeService));
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));
            Ensure.Argument.IsNotNull(syncManager, nameof(syncManager));

            this.dto = dto;
            this.dataSource = dataSource;
            this.timeService = timeService;
            this.interactorFactory = interactorFactory;
            this.syncManager = syncManager;
        }

        public IObservable<IThreadSafeTimeEntry> Execute()
            => interactorFactory.GetTimeEntryById(dto.Id)
                .Execute()
                .Select(createUpdatedTimeEntry)
                .SelectMany(dataSource.TimeEntries.Update)
                .Do(syncManager.InitiatePushSync);

        private TimeEntry createUpdatedTimeEntry(IThreadSafeTimeEntry timeEntry)
            => TimeEntry.Builder.Create(dto.Id)
                .SetDescription(dto.Description)
                .SetDuration(dto.StopTime.HasValue ? (long?)(dto.StopTime.Value - dto.StartTime).TotalSeconds : null)
                .SetTagIds(dto.TagIds)
                .SetStart(dto.StartTime)
                .SetTaskId(dto.TaskId)
                .SetBillable(dto.Billable)
                .SetProjectId(dto.ProjectId)
                .SetWorkspaceId(dto.WorkspaceId)
                .SetUserId(timeEntry.UserId)
                .SetIsDeleted(timeEntry.IsDeleted)
                .SetServerDeletedAt(timeEntry.ServerDeletedAt)
                .SetAt(timeService.CurrentDateTime)
                .SetSyncStatus(SyncStatus.SyncNeeded)
                .Build();
    }
}
