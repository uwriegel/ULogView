using System;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Web.WebView2.Core;

namespace ULogView
{
    public partial class MainWindow : Window
    {
#if DEBUG
        public static Uri Url { get; } = new Uri("http://localhost:3000/");
#else
        public static Uri Url { get; } = new Uri("http://localhost:9865/");
#endif
        public MainWindow() 
        {
            InitializeComponent();
            InitializeAsync();

            async void InitializeAsync()
            {
                await webView.EnsureCoreWebView2Async();
                webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            }
        }

        void Window_Loaded(object sender, RoutedEventArgs e) => LogServer.start();

        void OnDropFile(string file) => Task.Run(() => LogServer.indexFile(file));

        void webView_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
            => DropFile.Initialize(this, OnDropFile);
	}
}

// TODO AND Restriction
// TODO Cancel Restriction toggle

