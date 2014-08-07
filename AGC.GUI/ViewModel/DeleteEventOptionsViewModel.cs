using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Windows;

namespace AGC.GUI.ViewModel
{
    public class DeleteEventOptionsViewModel : ViewModelBase
    {
        private readonly IGoogleCalendar calendar;
        private readonly IRepository repository;

        private CalendarEvent selectedEvent;

        public RelayCommand DeleteOnlyInstanceCommand { get; private set; }
        public RelayCommand DeleteFollowingEventsCommand { get; private set; }
        public RelayCommand DeleteAllEventsCommand { get; private set; }
        public RelayCommand CancelDeleteCommand { get; private set; }

        public DeleteEventOptionsViewModel(IGoogleCalendar googleCalendar, IRepository commonRepository)
        {
            calendar = googleCalendar;
            repository = commonRepository;
            selectedEvent = repository.GetCurrentEvent();

            DeleteOnlyInstanceCommand = new RelayCommand(DeleteOnlyInstance);
            DeleteFollowingEventsCommand = new RelayCommand(DeleteFollowingEvents);
            DeleteAllEventsCommand = new RelayCommand(DeleteAllEvents);
            CancelDeleteCommand = new RelayCommand(CloseWindow);

        }

        private void DeleteOnlyInstance()
        {
            calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.single);
            CloseWindow();
        }

        private void DeleteFollowingEvents()
        {
            calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.following);
            CloseWindow();
        }

        private void DeleteAllEvents()
        {
            calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.all);
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows[1].Close();
        }
    }
}