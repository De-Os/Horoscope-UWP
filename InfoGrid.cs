using System;
using System.Collections.Generic;
using System.Globalization;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Horoscope
{
    public class InfoGrid : Grid
    {
        public InfoGrid(Libs.Horoscope.Response data, string sign)
        {
            this.Transitions.Add(new Windows.UI.Xaml.Media.Animation.PopupThemeTransition());

            this.Children.Add(new ProgressRing
            {
                Width = 50,
                Height = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                IsActive = true
            });

            this.Load(data, sign);
        }

        private void Load(Libs.Horoscope.Response data, string sign)
        {
            var pivot = new Pivot
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            foreach (var type in typeof(Libs.Horoscope.Response).GetFields())
            {
                var name = type.Name;

                if (name == "Year")
                {
                    foreach (var y in (List<Libs.Horoscope.Response.Data>)type.GetValue(data))
                    {
                        AddPivotItem(DateTime.ParseExact(y.Date, "yyyyMMdd", CultureInfo.InvariantCulture).Year.ToString(), y.Text);
                    }
                }
                else
                {
                    var val = (Libs.Horoscope.Response.Data)type.GetValue(data);
                    AddPivotItem(App.LocString(name), val.Text, name == "Today");
                }
            }

            Composite();
            this.ActualThemeChanged += (a, b) => Composite();

            void AddPivotItem(string title, string text, bool selection = false)
            {
                var item = new PivotItem
                {
                    Header = title,
                    Content = new TextBlock
                    {
                        Margin = new Thickness(10),
                        FontSize = 20,
                        Text = text,
                        TextWrapping = TextWrapping.WrapWholeWords
                    }
                };
                pivot.Items.Add(item);
                if (selection) pivot.SelectedItem = item;
            }

            void Composite()
            {
                this.Children.Clear();
                this.RowDefinitions.Clear();

                this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
                this.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

                var darkTheme = new UISettings().GetColorValue(UIColorType.Background).ToString() == "#FF000000";

                var image = new Image
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    MaxHeight = 300,
                    Source = new BitmapImage(new Uri("ms-appx:///Assets/Pictures/" + (darkTheme ? "Light" : "Dark") + "/" + sign.ToLower() + ".png"))
                };

                Grid.SetRow(image, 0);
                Grid.SetRow(pivot, 1);

                this.Children.Add(image);
                this.Children.Add(pivot);
            }
        }
    }
}
