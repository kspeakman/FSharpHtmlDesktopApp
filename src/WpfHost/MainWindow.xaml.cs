using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Windows;

namespace WpfHost
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Init();
        }

        private async void Init()
        {
            // required for Fixed Version Runtime
            webView.CreationProperties =
                new CoreWebView2CreationProperties
                {
                    BrowserExecutableFolder = "WebView2.92.0.902.84.x64"
                };

            // ensure WebView2 resources are available
            await webView.EnsureCoreWebView2Async();

            // stuff that must be set after webView "ensured"
            webView.CoreWebView2.Settings.AreHostObjectsAllowed = false;

            webView.CoreWebView2.Settings.AreBrowserAcceleratorKeysEnabled = false;
            webView.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
            webView.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
            webView.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
#if !DEBUG
            webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            webView.CoreWebView2.Settings.AreDevToolsEnabled = false;
            webView.CoreWebView2.Settings.IsStatusBarEnabled = false;
#endif
            webView.CoreWebView2.WebMessageReceived += HandleMessage;
            webView.Source = new Uri("http://localhost:8080");
        }

        private readonly Api.EffectConfig config =
            new(close: Application.Current.Shutdown);

        private void HandleMessage(object sender, CoreWebView2WebMessageReceivedEventArgs args)
        {
            string request = args.TryGetWebMessageAsString();
            string response = Api.Request.handle(config, request);
            webView.CoreWebView2.PostWebMessageAsString(response);
        }
    }
}
