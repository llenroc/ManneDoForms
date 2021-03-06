﻿using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using ManneDoForms.Components.PhotoViewer.View;
using XLabs.Ioc;
using ManneDoForms.Common.Filesystem;
using ManneDoForms.Common.Api;

namespace ManneDoForms.Samples
{
    public class SingleImagePage : ContentPage
    {
        private PhotoView _photoView;

        public SingleImagePage()
        {
            Content = new SpinnerView() { IsBusy = true };
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Content = CreateSinglePhotoViewer();

            await LoadSingleImage("bild1.jpg", "https://raw.githubusercontent.com/Manne990/PhotoViewerTest/master/iOS/Resources/bild1.jpg");
        }

        private async Task LoadSingleImage(string imageName, string imageUrl)
        {
            var fileSystem = DependencyService.Get<IFileSystem>();

            if (fileSystem.FileExists(imageName))
            {
                ShowImage(imageName);
            }
            else
            {
                var api = Resolver.Resolve<IApi>();

                await api.DownloadAndSaveFile(imageUrl, imageName);

                ShowImage(imageName);
            }
        }

        private void ShowImage(string imageName)
        {
            _photoView.ImageName = imageName;
        }

        private View CreateSinglePhotoViewer()
        {
            // Create a signle PhotoView
            _photoView = new PhotoView() { BackgroundColor = Color.Gray };

            _photoView.Tap += OnPhotoTab;

            // Add the PhotoView to a layout
            var layout = new RelativeLayout();

            layout.Children.Add(_photoView,
                xConstraint: Constraint.Constant(0),
                yConstraint: Constraint.Constant(0),
                widthConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Width;
                    }),
                heightConstraint: Constraint.RelativeToParent((parent) =>
                    {
                        return parent.Height;
                    }));

            return layout;
        }

        public async void OnPhotoTab(object sender, EventArgs args)
        {
            await DisplayAlert("Photo Tapped", "You Tapped the photo", "OK");
        }
    }
}