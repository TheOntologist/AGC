using System.Windows;
using AGC.GUI.ViewModel;
using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace AGC.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            try
            {
                log.Debug("Start AGC");
                log.Debug("Loading MainWindow view...");
                InitializeComponent();
                Closing += (s, e) => ViewModelLocator.Cleanup();
                log.Debug("MainWindow view was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load MainWindow view:", ex);
            }
        }
    }
}