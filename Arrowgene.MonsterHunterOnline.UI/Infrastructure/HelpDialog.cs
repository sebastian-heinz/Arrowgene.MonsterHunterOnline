using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;

namespace Arrowgene.MonsterHunterOnline.UI.Infrastructure;

internal sealed class HelpDialog : Window
{
    public HelpDialog(string title, string content)
    {
        Title = title;
        Width = 560;
        Height = 520;
        MinWidth = 400;
        MinHeight = 300;
        CanResize = true;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Background = new SolidColorBrush(Color.Parse("#F2F1EC"));

        Button closeButton = new Button
        {
            Content = "Close",
            MinWidth = 96,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        closeButton.Click += (_, _) => Close();

        Content = new DockPanel
        {
            Margin = new Thickness(20),
            LastChildFill = true,
            Children =
            {
                DockTo(new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Spacing = 10,
                    Margin = new Thickness(0, 12, 0, 0),
                    Children = { closeButton }
                }, Dock.Bottom),
                new Border
                {
                    Background = new SolidColorBrush(Colors.White),
                    BorderBrush = new SolidColorBrush(Color.Parse("#D7D3CC")),
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(8),
                    Padding = new Thickness(16),
                    Child = new ScrollViewer
                    {
                        HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                        VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto,
                        Content = new SelectableTextBlock
                        {
                            Text = content,
                            FontSize = 12.5,
                            LineHeight = 20,
                            Foreground = new SolidColorBrush(Color.Parse("#0F172A")),
                            TextWrapping = TextWrapping.Wrap,
                            FontFamily = new FontFamily("Segoe UI, San Francisco, Helvetica Neue, sans-serif")
                        }
                    }
                }
            }
        };
    }

    private static T DockTo<T>(T control, global::Avalonia.Controls.Dock dock) where T : Control
    {
        DockPanel.SetDock(control, dock);
        return control;
    }
}
