﻿using System;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Toggl.Core.Suggestions;
using Toggl.Droid.Extensions;

namespace Toggl.Droid.ViewHolders
{
    public sealed class MainLogSuggestionItemViewHolder : BaseRecyclerViewHolder<Suggestion>
    {
        private readonly int firstItemMargin;
        private readonly int lastItemMargin;
        private readonly int defaultCardSize;

        public bool IsSingleItem { get; set; }
        public bool IsFirstItem { get; set; }
        public bool IsLastItem { get; set; }

        private TextView timeEntriesLogCellDescriptionLabel;
        private TextView timeEntriesLogCellProjectLabel;
        private TextView timeEntriesLogCellClientLabel;
        private ImageView timeEntriesLogCellContinueImage;

        public void RecalculateMargins()
        {
            var left = IsFirstItem ? firstItemMargin : 0;
            var right = IsSingleItem ? firstItemMargin : IsLastItem ? lastItemMargin : 0;

            var marginLayoutParams = ItemView.LayoutParameters as ViewGroup.MarginLayoutParams;
            var newLayoutParams = marginLayoutParams.WithMargins(left, null, right, null);
            newLayoutParams.Width = IsSingleItem ? ViewGroup.LayoutParams.MatchParent : defaultCardSize;
            ItemView.LayoutParameters = newLayoutParams;
        }

        public MainLogSuggestionItemViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public MainLogSuggestionItemViewHolder(View itemView, int firstItemMargin, int lastItemMargin, int defaultCardSize) : base(itemView)
        {
            this.firstItemMargin = firstItemMargin;
            this.lastItemMargin = lastItemMargin;
            this.defaultCardSize = defaultCardSize;
        }

        protected override void InitializeViews()
        {
            timeEntriesLogCellDescriptionLabel = ItemView.FindViewById<TextView>(Resource.Id.TimeEntriesLogCellDescriptionLabel);
            timeEntriesLogCellProjectLabel = ItemView.FindViewById<TextView>(Resource.Id.TimeEntriesLogCellProjectLabel);
            timeEntriesLogCellClientLabel = ItemView.FindViewById<TextView>(Resource.Id.TimeEntriesLogCellClientLabel);
            timeEntriesLogCellContinueImage = ItemView.FindViewById<ImageView>(Resource.Id.TimeEntriesLogCellContinueImage);
        }

        protected override void UpdateView()
        {
            timeEntriesLogCellDescriptionLabel.Text = Item.Description;
            timeEntriesLogCellProjectLabel.Text = Item.ProjectName;
            timeEntriesLogCellProjectLabel.SetTextColor(Color.ParseColor(Item.ProjectColor));
            timeEntriesLogCellProjectLabel.Visibility = Item.HasProject.ToVisibility();
            timeEntriesLogCellClientLabel.Text = Item.ClientName;
            timeEntriesLogCellClientLabel.Visibility = Item.HasProject.ToVisibility();
        }
    }

}
