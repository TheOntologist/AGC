using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class UpdateEventOptionsViewModel : ViewModelBase
    {
        private readonly IGoogleCalendar calendar;
        private readonly IRepository repository;
        private RecurrenceSettings recurrence;

        private CalendarEvent selectedEvent;

        public RelayCommand UpdateOnlyInstanceCommand { get; private set; }
        public RelayCommand UpdateAllEventsCommand { get; private set; }

        public UpdateEventOptionsViewModel(IGoogleCalendar googleCalendar, IRepository commonRepository)
        {
            calendar = googleCalendar;
            repository = commonRepository;

            selectedEvent = repository.GetCurrentEvent();

            UpdateOnlyInstanceCommand = new RelayCommand(UpdateOnlyInstance);
            UpdateAllEventsCommand = new RelayCommand(UpdateAllEvents);
        }

        private void UpdateOnlyInstance()
        {
            CalendarEventUpdater updateEvent = new CalendarEventUpdater(GoogleCalendar.ActionType.single, selectedEvent);
            repository.SetEventUpdater(updateEvent);
            CloseWindow();
        }

        private void UpdateAllEvents()
        {
            recurrence = calendar.GetRecurrenceSettings(selectedEvent);
            CalendarEventUpdater updateEvent = new CalendarEventUpdater(GoogleCalendar.ActionType.all, selectedEvent, recurrence);
            repository.SetEventUpdater(updateEvent);
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows[1].Close();
        }
    }
}