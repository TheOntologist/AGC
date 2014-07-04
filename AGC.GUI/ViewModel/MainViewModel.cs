using GalaSoft.MvvmLight;
using AGC.Library;
using System;

namespace AGC.GUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static GoogleCalendar calendar;

        #endregion

        #region Constructor

        public MainViewModel(GoogleCalendar googleCalendar)
        {
            try
            {
                log.Debug("Loading MainWindow view model...");
                calendar = googleCalendar;
                log.Debug("MainWindow view model was succssfully loaded");
            }
            catch(Exception ex)
            {
                log.Error("Failed to load MainWindow view model:", ex);
            }            
        }

        #endregion
    }
}