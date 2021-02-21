using System.Windows;

namespace ULogView
{
    public partial class MainWindow : Window
    {
        public MainWindow() => InitializeComponent();

        void Window_Loaded(object sender, RoutedEventArgs e) => LogServer.start();

        void Button_Click(object sender, RoutedEventArgs e) => LogServer.sendEvent("Das war es");

        void OnDropFile(string file) =>
            LogServer.indexFile(file);

        void webView_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
            => DropFile.Initialize(this, OnDropFile);
    }
}
