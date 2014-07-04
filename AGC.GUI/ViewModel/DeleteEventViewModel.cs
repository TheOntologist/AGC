using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class DeleteEventViewModel : ViewModelBase
    {
        private readonly IGoogleCalendar calendar;
        private readonly IRepository repository;

        private CalendarEvent selectedEvent;

        public RelayCommand DeleteOnlyInstanceCommand { get; private set; }
        public RelayCommand DeleteAllEventsCommand { get; private set; }

        public DeleteEventViewModel(IGoogleCalendar googleCalendar, IRepository commonRepository)
        {
            calendar = googleCalendar;
            repository = commonRepository;
            selectedEvent = repository.GetCurrentEvent();

            DeleteOnlyInstanceCommand = new RelayCommand(DeleteOnlyInstance);
            DeleteAllEventsCommand = new RelayCommand(DeleteAllEvents);
        }

        private void DeleteOnlyInstance()
        {
            calendar.DeleteSingleInstanceOfRecurringEvent(selectedEvent);
            CloseWindow();
        }

        private void DeleteAllEvents()
        {
            calendar.DeleteAllInstancesOfRecurringEvent(selectedEvent);
            CloseWindow();
        }
        
        private void CloseWindow()
        {
            Application.Current.Windows[1].Close();
        }
    }
}