using AGC.GUI.ViewModel;
using AGC.Library;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGC.GUI.Common
{
    public class Bootstrapper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IUnityContainer Container { get; set; }

        public Bootstrapper()
        {
            try
            {
                log.Debug("Creating Bootstrapper...");
                Container = new UnityContainer();

                ConfigureContainer();
                log.Debug("Bootstrapper was successfully created");
            }
            catch(Exception ex)
            {
                log.Error("Failed to create Bootstrapper:", ex);
            }

        }

        /// <summary>
        /// We register here every service / interface / viewmodel.
        /// </summary>
        private void ConfigureContainer()
        {
            log.Debug("Configuring container...");

            Container.RegisterInstance<IRecurrenceSettings>(new RecurrenceSettings());
            log.Debug("Registered instance of RecurrenceSettings");

            Container.RegisterInstance<ICalendarEventService>(new CalendarEventService());
            log.Debug("Registered instance of CalendarEventService");

            Container.RegisterInstance<ITimeIntervals>(new TimeIntervals());
            log.Debug("Registered instance of TimeIntervals");

            Container.RegisterInstance<IGoogleCalendar>(new GoogleCalendar());
            log.Debug("Registered instance of GoogleCalendar");

            Container.RegisterInstance<IRepository>(new Repository());
            log.Debug("Registered instance of Repository");

            Container.RegisterType<MainViewModel>();
            log.Debug("Registered type of MainViewModel");

            Container.RegisterType<EventsListViewModel>();
            log.Debug("Registered type of EventsListViewModel");

            Container.RegisterType<AddQuickEventViewModel>();
            log.Debug("Registered type of AddQuickEventViewModel");

            Container.RegisterType<SettingsViewModel>();
            log.Debug("Registered type of SettingsViewModel");

            Container.RegisterType<DeleteEventOptionsViewModel>();
            log.Debug("Registered type of DeleteEventOptionsViewModel");

            Container.RegisterType<UpdateEventOptionsViewModel>();
            log.Debug("Registered type of UpdateEventOptionsViewModel");

            log.Debug("Container was successfully configured");
        }
    }
}
