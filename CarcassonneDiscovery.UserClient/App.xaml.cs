namespace CarcassonneDiscovery.UserClient
{
    using System.Windows;

    /// <summary>
    /// Main application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Main controller of the application.
        /// </summary>
        private UserClientController Controller;

        /// <summary>
        /// De-facto application entrypoint.
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Controller = new UserClientController();
            Controller.Start();
        }
    }
}
