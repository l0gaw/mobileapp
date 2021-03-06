﻿using System;
using CoreGraphics;
using Foundation;
using Toggl.iOS.Extensions;
using Toggl.iOS.Extensions.Reactive;
using Toggl.Core;
using Toggl.Core.UI.Helper;
using Toggl.Core.UI.ViewModels;
using Toggl.iOS.Presentation.Attributes;
using Toggl.Shared.Extensions;
using UIKit;
using static Toggl.iOS.Extensions.RangeExtensions;

namespace Toggl.iOS.ViewControllers
{
    [ModalDialogPresentation]
    public sealed partial class TermsOfServiceViewController
        : ReactiveViewController<TermsOfServiceViewModel>
    {
        private const int fontSize = 15;

        private readonly NSRange termsOfServiceTextRange = new NSRange(56, 16);
        private readonly NSRange privacyPolicyRange = new NSRange(77, 14);

        private readonly UIStringAttributes normalTextAttributes = new UIStringAttributes
        {
            Font = UIFont.SystemFontOfSize(fontSize)
        };

        private readonly UIStringAttributes highlitedTextAttributes = new UIStringAttributes
        {
            Font = UIFont.SystemFontOfSize(fontSize),
            ForegroundColor = Colors.Signup.HighlightedText.ToNativeColor()
        };

        public TermsOfServiceViewController() : base(nameof(TermsOfServiceViewController))
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            TitleLabel.Text = Resources.ReviewTheTerms;
            AcceptButton.SetTitle(Resources.IAgree, UIControlState.Normal);

            var height = UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Pad
                ? 260
                : View.Frame.Height;

            PreferredContentSize = new CGSize(View.Frame.Width, height);

            prepareTextView();

            AcceptButton.Rx()
                .BindAction(ViewModel.Close, _ => true)
                .DisposedBy(DisposeBag);

            CloseButton.Rx()
                .BindAction(ViewModel.Close, _ => false)
                .DisposedBy(DisposeBag);
        }

        private void prepareTextView()
        {
            TextView.TextContainerInset = UIEdgeInsets.Zero;
            TextView.TextContainer.LineFragmentPadding = 0;

            var text = new NSMutableAttributedString(Resources.TermsOfServiceDialogMessage, normalTextAttributes);
            text.AddAttributes(highlitedTextAttributes, termsOfServiceTextRange);
            text.AddAttributes(highlitedTextAttributes, privacyPolicyRange);
            TextView.AttributedText = text;

            TextView.AddGestureRecognizer(new UITapGestureRecognizer(onTextViewTapped));
        }

        private void onTextViewTapped(UITapGestureRecognizer recognizer)
        {
            var layoutManager = TextView.LayoutManager;
            var location = recognizer.LocationInView(TextView);
            location.X -= TextView.TextContainerInset.Left;
            location.Y -= TextView.TextContainerInset.Top;

            nfloat _ = 0;
            var characterIndex = layoutManager.CharacterIndexForPoint(location, TextView.TextContainer, ref _);

            if (termsOfServiceTextRange.ContainsNumber(characterIndex))
                ViewModel.ViewTermsOfService.Execute();

            if (privacyPolicyRange.ContainsNumber(characterIndex))
                ViewModel.ViewPrivacyPolicy.Execute();
        }
    }
}
