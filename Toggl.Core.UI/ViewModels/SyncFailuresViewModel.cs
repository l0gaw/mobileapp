using System;
using System.Collections.Immutable;
using System.Reactive.Linq;
using Toggl.Core.Interactors;
using Toggl.Core.Models;
using Toggl.Shared;

namespace Toggl.Core.UI.ViewModels
{
    [Preserve(AllMembers = true)]
    public class SyncFailuresViewModel : ViewModel
    {
        public IImmutableList<SyncFailureItem> SyncFailures { get; private set; }

        private readonly IInteractorFactory interactorFactory;

        public SyncFailuresViewModel(IInteractorFactory interactorFactory)
        {
            Ensure.Argument.IsNotNull(interactorFactory, nameof(interactorFactory));

            this.interactorFactory = interactorFactory;
        }

        public override void Initialize()
        {
            interactorFactory
                .GetItemsThatFailedToSync()
                .Execute()
                .FirstAsync()
                .Subscribe(syncFailures => SyncFailures = syncFailures.ToImmutableList());
        }
    }
}
