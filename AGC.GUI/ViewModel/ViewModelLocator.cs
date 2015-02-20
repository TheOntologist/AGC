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

        public AnotherCalendarViewModel AnotherCalendar
        {
            get
            {
                log.Debug("Get AnotherCalendarViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<AnotherCalendarViewModel>();
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

        public SettingsViewModel Settings
        {
            get
            {
                log.Debug("Get SettingsViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<SettingsViewModel>();
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

        public DeleteEventOptionsViewModel DeleteEvent
        {
            get
            {
                log.Debug("Get DeleteEventOptionsViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<DeleteEventOptionsViewModel>(); 
            }
        }

        public UpdateEventOptionsViewModel UpdateEventOptions
        {
            get
            {
                log.Debug("Get UpdateEventOptionsViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<UpdateEventOptionsViewModel>();
            }
        }

        public UpdateEventViewModel UpdateEvent
        {
            get
            {
                log.Debug("Get UpdateEventViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<UpdateEventViewModel>();
            }
        }

        public SortingAndFilteringViewModel SortingAndFiltering
        {
            get
            {
                log.Debug("Get SortingAndFilteringViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<SortingAndFilteringViewModel>();
            }
        }

        public SoundsViewModel Sounds
        {
            get
            {
                log.Debug("Get SoundsViewModel from ViewModelLocator");
                return _bootStrapper.Container.Resolve<SoundsViewModel>();
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