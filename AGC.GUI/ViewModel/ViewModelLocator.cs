using AGC.GUI.Common;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;

namespace AGC.GUI.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static Bootstrapper _bootStrapper;

        static ViewModelLocator()
        {       
            try
            {
                log.Debug("Creating ViewModelLocator...");
                if (_bootStrapper == null)
                    _bootStrapper = new Bootstrapper();
                log.Debug("ViewModelLocator was successfully created");
            }
            catch(Exception ex)
            {
                log.Error("Failed to create ViewModelLocator:", ex);
            }
        }

        public MainViewModel Main
        {
            get 
            {
                log.Debug("Get MainViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<MainViewModel>(); 
            }
        }

        public EventsListViewModel EventsList
        {
            get 
            {
                log.Debug("Get EventsListViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<EventsListViewModel>(); 
            }
        }

        public EventsCreateViewModel EventsCreate
        {
            get 
            {
                log.Debug("Get EventsCreateViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<EventsCreateViewModel>(); 
            }
        }

        public AddQuickEventViewModel AddQuickEvent
        {
            get
            {
                log.Debug("Get AddQuickEventViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<AddQuickEventViewModel>();
            }
        }

        public DeleteEventViewModel DeleteEvent
        {
            get
            {
                log.Debug("Get DeleteEventViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<DeleteEventViewModel>(); 
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
        }
    }
}