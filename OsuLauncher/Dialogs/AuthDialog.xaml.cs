using System.Text.RegularExpressions;
using System.Windows;
using HandyControl.Controls;
using Microsoft.Web.WebView2.Core;
using OsuLauncher.Helpers;
using Window = System.Windows.Window;

namespace OsuLauncher.Dialogs;

public partial class AuthDialog : Window
{
    public AuthDialog()
    {
        InitializeComponent();
        
        AuthBrowser.NavigationStarting += AuthBrowserOnNavigationStarting;
        AuthBrowser.NavigationCompleted += AuthBrowserOnNavigationCompleted;
    }

    private void AuthBrowserOnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
    {
        var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
        if (!tokenRegex.IsMatch(e.Uri))
            return;
        var authCode = tokenRegex.Match(e.Uri).Value;
    }

    private void AuthBrowserOnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        if (AuthBrowser.Source.AbsoluteUri != "https://osu.ppy.sh/oauth/authorize?client_id=21770&redirect_uri=http%3A%2F%2Flocalhost%3A7040&response_type=code&scope=public+identify&state=randomval")
        {
            var tokenRegex = new Regex("(?<=code=)(.*)(?=&state)");
            if (!tokenRegex.IsMatch(AuthBrowser.Source.AbsoluteUri))
                return;
            var authCode = tokenRegex.Match(AuthBrowser.Source.AbsoluteUri).Value;
            Clipboard.SetText(authCode);
            DialogHelper.ShowDialog(typeof(AuthCodeDialog));
            this.Close();
        }
    }
}