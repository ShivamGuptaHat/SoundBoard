using SoundBoard.Assets.Models;
using SoundBoard.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoundBoard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private ObservableCollection<Sound> Sounds;
        private List<string> suggestion;
        private List<MenuItem> MenuItems;

        public MainPage()
        {
            this.InitializeComponent();
            Sounds = new ObservableCollection<Sound>();
            SoundManager.GetAllSound(Sounds);

            MenuItems = new List<MenuItem>();

            MenuItems.Add(new MenuItem { Icon="/Assets/Icons/animals.png",Category="Animals"});
            MenuItems.Add(new MenuItem { Icon = "/Assets/Icons/cartoon.png", Category ="Cartoons" });
            MenuItems.Add(new MenuItem { Icon = "/Assets/Icons/taunt.png", Category ="Taunts" });
            MenuItems.Add(new MenuItem { Icon = "/Assets/Icons/warning.png", Category = "Warnings"});
            BackButton.Visibility = Visibility.Collapsed;
        }

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen;

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            SoundManager.GetAllSound(Sounds);
            SoundTextBlock.Text = "All sounds";
            BackButton.Visibility = Visibility.Collapsed;
            MyListView.SelectedIndex = -1;
        }

        private void MyListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var items = (MenuItem)e.ClickedItem;
            SoundTextBlock.Text = items.Category;

            SoundManager.GetCategorySounds(items.Category,Sounds);
            BackButton.Visibility = Visibility.Visible;

        }

        private void SoundGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var sound=(Sound)e.ClickedItem;
            MyMediaElement.Source = new Uri(this.BaseUri,sound.AudioFile);

        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            SoundManager.GetAllSound(Sounds);
            suggestion = Sounds.Where(p => p.Name.StartsWith(sender.Text)).Select(p => p.Name).ToList();
            AutoSuggestBox.ItemsSource = suggestion;
        }

        private void SoundGridView_DragOver(object sender, DragEventArgs e)
        {
            
            e.AcceptedOperation = DataPackageOperation.Copy;

            e.DragUIOverride.Caption = "drop to create a custom sound and tile";
            e.DragUIOverride.IsCaptionVisible = true;
            e.DragUIOverride.IsContentVisible = true;
            e.DragUIOverride.IsGlyphVisible = true;
        }

        private async void SoundGridView_Drop(object sender, DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var items = await e.DataView.GetStorageItemsAsync();

                if (items.Any())
                {
                    var storageFile = items[0] as StorageFile;
                    var contentType = storageFile.ContentType;

                    StorageFolder folder = ApplicationData.Current.LocalFolder;


                  if (contentType == "audio/wav" || contentType == "audio/mpeg")
                    {
                        StorageFile newFile = await storageFile.CopyAsync(folder, storageFile.Name, NameCollisionOption.GenerateUniqueName);
                        MyMediaElement.SetSource(await storageFile.OpenAsync(FileAccessMode.Read), contentType);
                        MyMediaElement.Play();            
                    }
                }
            }
        }
        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            Sounds.Clear();
            SoundManager.GetSoundsByName(sender.Text , Sounds);
            SoundTextBlock.Text = sender.Text;
            MyListView.SelectedIndex = -1;
            BackButton.Visibility = Visibility.Visible;
            
        }
    }
}
