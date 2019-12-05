namespace CarcassonneDiscovery.UserClient
{
    using CarcassonneDiscovery.SimulationLibrary;
    using System.Windows;

    /// <summary>
    /// Waiting lobby window.
    /// </summary>
    public partial class WaitingLobby : Window
    {
        /// <summary>
        /// Controller for the window.
        /// </summary>
        private UserClientController _ParentController;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="parentController">Controller for the window.</param>
        public WaitingLobby(UserClientController parentController)
        {
            InitializeComponent();

            _ParentController = parentController;


            // Set default values
            tbIP.Text = "127.0.0.1";
            tbPort.Text = "4263";
            tbName.Text = "Player";
            cbColor.SelectedIndex = 0;

            btnRegister.Click += (o, ev) =>
            {
                _ParentController.RegisterAs(tbName.Text, (PlayerColor)cbColor.SelectedValue);
            };

            btnConnect.Click += (o, ev) =>
            {
                if (!int.TryParse(tbPort.Text, out int port))
                {
                    MessageBox.Show("Invalid number of port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _ParentController.ConnectToServer(tbIP.Text, port);
            };

            Closed += (o, ev) => parentController.WaitingLobbyWindowClosed();
        }

        /// <summary>
        /// Sets whether player can send connection message.
        /// </summary>
        /// <param name="enabled">Is player enabled to connect as a player?</param>
        public void SetPlayerRegisteringEnabled(bool enabled)
        {
            Dispatcher.Invoke(() => {
                tbName.IsEnabled = enabled;
                cbColor.IsEnabled = enabled;
                btnRegister.IsEnabled = enabled;
            });
        }

        /// <summary>
        /// Sets whether server can connect to the server.
        /// </summary>
        /// <param name="enabled">Is player enabled to connect to the server?</param>
        public void SetServerConnectingEnabled(bool enabled)
        {
            Dispatcher.Invoke(() => {
                tbIP.IsEnabled = enabled;
                tbPort.IsEnabled = enabled;
                btnConnect.IsEnabled = enabled;
            });
        }
    }
}
