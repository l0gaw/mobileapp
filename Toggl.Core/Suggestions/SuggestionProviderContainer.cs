﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using Toggl.Shared;

namespace Toggl.Core.Suggestions
{
    public sealed class SuggestionProviderContainer : ISuggestionProviderContainer
    {
        public ReadOnlyCollection<ISuggestionProvider> Providers { get; }

        public SuggestionProviderContainer(params ISuggestionProvider[] providers)
        {
            Ensure.Argument.IsNotNull(providers, nameof(providers));
            if (providers.Length == 0)
                throw new ArgumentException("At least one ISuggestionProvider is required", nameof(providers));

            Providers = new ReadOnlyCollection<ISuggestionProvider>(providers.ToList());
        }
    }
}
