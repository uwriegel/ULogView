using System.Windows;

namespace ULogView
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow() => InitializeComponent();

		void Window_Loaded(object sender, RoutedEventArgs e)
		{
			LogServer.start();
		}
	}
}
