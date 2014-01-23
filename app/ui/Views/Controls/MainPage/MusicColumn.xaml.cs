﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

// Pour en savoir plus sur le modèle d'élément Contrôle utilisateur, consultez la page http://go.microsoft.com/fwlink/?LinkId=234236
using VLC_WINRT.Utility.Commands;
using VLC_WINRT.Utility.Helpers;
using VLC_WINRT.ViewModels;
using VLC_WINRT.ViewModels.MainPage;

namespace VLC_WINRT.Views.Controls.MainPage
{
    public sealed partial class MusicColumn : UserControl
    {
        private int _currentSection;
        public MusicColumn()
        {
            this.InitializeComponent();
            this.Loaded += (sender, args) =>
            {
                UIAnimationHelper.FadeOut(TracksPanel);
                for (int i = 1; i < SectionsGrid.Children.Count; i++)
                {
                    UIAnimationHelper.FadeOut(SectionsGrid.Children[i]);
                }
            };
            this.SizeChanged += OnSizeChanged;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                if (sizeChangedEventArgs.NewSize.Width < 1080)
                {
                    SectionsGrid.Margin = new Thickness(40, 0, 0, 0);
                    ArtistListView.Visibility = Visibility.Collapsed;
                    AlbumPlaylistListView.Visibility = Visibility.Collapsed;
                }
                else
                {
                    SectionsGrid.Margin = new Thickness(50, 0, 0, 0);
                    ArtistListView.Visibility = Visibility.Visible;
                    AlbumPlaylistListView.Visibility = Visibility.Visible;
                }


                if (sizeChangedEventArgs.NewSize.Width == 320)
                {
                    AlbumsByArtistListView.ItemTemplate = LittleSizedAlbumDataTemplate;
                    SectionsGrid.Margin = new Thickness(0);
                }
                else
                {
                    AlbumsByArtistListView.ItemTemplate = NormalSizedAlbumDataTemplate;
                    SectionsGrid.Margin = new Thickness(50, 0, 0, 0);
                }
            });
        }

        private void AlbumGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var album = e.ClickedItem as MusicLibraryViewModel.AlbumItem;

            AlbumPlaylistListView.Header = album;
            AlbumPlaylistListView.ItemsSource = album.Tracks;

            if (Window.Current.Bounds.Width < 1080)
            {
                new PlayAlbumCommand().Execute(album);
            }
            //Locator.MainPageVM.MusicVM.CurrentArtist.CurrentAlbumIndex = Locator.MainPageVM.MusicVM.CurrentArtist.Albums.IndexOf(album);
        }

        private void OnSelectedArtist_Changed(object sender, SelectionChangedEventArgs e)
        {
            var artist = e.AddedItems[0] as MusicLibraryViewModel.ArtistItemViewModel;
            AlbumsByArtistListView.ScrollIntoView(artist);
        }

        private void SectionsHeaderListView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var i = ((Model.Panel)e.ClickedItem).Index;
            ChangedSectionsHeadersState(i);
        }
        private void ChangedSectionsHeadersState(int i)
        {
            if (i == _currentSection) return;
            UIAnimationHelper.FadeOut(SectionsGrid.Children[_currentSection]);
            UIAnimationHelper.FadeIn(SectionsGrid.Children[i]);
            _currentSection = i;
            for (int j = 0; j < SectionsHeaderListView.Items.Count; j++)
                Locator.MainPageVM.MusicLibraryVm.Panels[j].Opacity = 0.4;
            Locator.MainPageVM.MusicLibraryVm.Panels[i].Opacity = 1;
        }

        private async void AlbumsByArtistSemanticZoom_OnViewChangeCompleted(object sender, SemanticZoomViewChangedEventArgs e)
        {

        }
    }
}