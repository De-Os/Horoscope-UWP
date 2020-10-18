using System;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace Horoscope
{
    public sealed partial class MainPage : Page
    {
        private readonly Libs.Horoscope Api = new Libs.Horoscope();

        public MainPage()
        {
            this.InitializeComponent();

            this.Navigation.Header = App.LocString("Horoscope");

            this.LoadMenuItems();
            this.Navigation.SelectedItem = this.Navigation.MenuItems[0];
            this.ActualThemeChanged += (a, b) => this.LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            var darkTheme = new UISettings().GetColorValue(UIColorType.Background).ToString() == "#FF000000";
            this.Navigation.MenuItems.Clear();
            foreach (Libs.Horoscope.Signs sign in typeof(Libs.Horoscope.Signs).GetEnumValues())
            {
                this.Navigation.MenuItems.Add(new SignItem(sign));
            }
            this.Navigation.SelectionChanged += this.OnNavSelection;
        }

        private void OnNavSelection(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var selected = args.SelectedItem as SignItem;

            Task.Factory.StartNew(() =>
            {
                var response = this.Api.GetBySign(selected.Sign);
                App.UIThread(() =>
                {
                    this.Frame.Content = new InfoGrid(response, selected.SignName);
                    this.Navigation.Header = App.LocString(selected.SignName);
                });
            });
        }

        private class SignItem : NavigationViewItem
        {
            public Libs.Horoscope.Signs Sign;
            public string SignName;

            public SignItem(Libs.Horoscope.Signs sign)
            {
                this.Sign = sign;
                this.SignName = typeof(Libs.Horoscope.Signs).GetEnumName(sign);

                this.Content = new TextBlock
                {
                    Text = App.LocString(this.SignName)
                };
                this.Icon = new BitmapIcon
                {
                    UriSource = new Uri("ms-appx:///Assets/Pictures/" + (new UISettings().GetColorValue(UIColorType.Background).ToString() == "#FF000000" ? "Light" : "Dark") + "/sb_" + this.SignName.ToLower() + ".png")
                };
            }
        }
    }
}
