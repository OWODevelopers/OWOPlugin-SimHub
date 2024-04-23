using System.Threading.Tasks;
using System.Windows.Controls;
using OWOGame;

namespace OWOPluginSimHub.View
{
    /// <summary>
    /// Logique d'interaction pour SettingsControlDemo.xaml
    /// </summary>
    public partial class SettingsControlDemo : UserControl
    {
        public DataPluginDemo Plugin { get; }

        public SettingsControlDemo()
        {
            InitializeComponent();
        }

        public SettingsControlDemo(DataPluginDemo plugin) : this()
        {
            this.Plugin = plugin;
        }
        
        void Connect(object sender, System.Windows.RoutedEventArgs e)
        {
                OWO.AutoConnect();

                ConnectionStatus.Text = "Looking for an OWO Skin";
                DisconnectButton.Visibility = System.Windows.Visibility.Visible;
                ConnectButton.Visibility = System.Windows.Visibility.Collapsed;

                WaitForConnection();
        }

        void Disconnect(object sender, System.Windows.RoutedEventArgs e)
        {
            OWO.Disconnect();

            ConnectionStatus.Text = "";
            DisconnectButton.Visibility = System.Windows.Visibility.Collapsed;
            ConnectButton.Visibility = System.Windows.Visibility.Visible;
        }

        async void WaitForConnection()
        {
            while (OWO.ConnectionState != ConnectionState.Connected) 
            {
                await Task.Delay(1000);
            }

            ConnectionStatus.Text = "Connected";
        }
    }
}