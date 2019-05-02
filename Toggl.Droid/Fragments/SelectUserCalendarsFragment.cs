﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Toggl.Core.UI.ViewModels.Calendar;
using Toggl.Droid.Adapters;
using Toggl.Droid.Extensions.Reactive;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Fragments
{
    public sealed partial class SelectUserCalendarsFragment : ReactiveDialogFragment<SelectUserCalendarsViewModel>
    {
        private UserCalendarsRecyclerAdapter userCalendarsAdapter;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var contextThemeWrapper = new ContextThemeWrapper(Activity, Resource.Style.TogglDialog);
            var wrappedInflater = inflater.CloneInContext(contextThemeWrapper);

            var view = wrappedInflater.Inflate(Resource.Layout.SelectUserCalendarsFragment, container, false);
            InitializeViews(view);

            setupRecyclerView();

            cancelButton
                .Rx()
                .BindAction(ViewModel.Close)
                .DisposedBy(DisposeBag);

            doneButton
                .Rx()
                .BindAction(ViewModel.Done)
                .DisposedBy(DisposeBag);

            ViewModel
                .Calendars
                .Select(calendars => calendars.ToList())
                .Subscribe(userCalendarsAdapter.Rx().Items())
                .DisposedBy(DisposeBag);

            userCalendarsAdapter
                .ItemTapObservable
                .Subscribe(ViewModel.SelectCalendar.Inputs)
                .DisposedBy(DisposeBag);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            var layoutParams = Dialog.Window.Attributes;
            layoutParams.Width = ViewGroup.LayoutParams.MatchParent;
            layoutParams.Height = ViewGroup.LayoutParams.WrapContent;
            Dialog.Window.Attributes = layoutParams;
        }

        private void setupRecyclerView()
        {
            userCalendarsAdapter = new UserCalendarsRecyclerAdapter();
            recyclerView.SetAdapter(userCalendarsAdapter);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
        }
    }
}
