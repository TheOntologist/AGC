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
        private readonly IMessanger messanger;

        private CalendarEvent selectedEvent;

        public RelayCommand DeleteOnlyInstanceCommand { get; private set; }
        public RelayCommand DeleteFollowingEventsCommand { get; private set; }
        public RelayCommand DeleteAllEventsCommand { get; private set; }
        public RelayCommand CancelDeleteCommand { get; private set; }

        public DeleteEventOptionsViewModel(IGoogleCalendar googleCalendar, IRepository commonRepository, IMessanger commonMessanger)
        {
            calendar = googleCalendar;
            repository = commonRepository;
            selectedEvent = repository.GetCurrentEvent();
            messanger = commonMessanger;

            DeleteOnlyInstanceCommand = new RelayCommand(DeleteOnlyInstance);
            DeleteFollowingEventsCommand = new RelayCommand(DeleteFollowingEvents);
            DeleteAllEventsCommand = new RelayCommand(DeleteAllEvents);
            CancelDeleteCommand = new RelayCommand(CloseWindow);

        }

        private void DeleteOnlyInstance()
        {
            if (calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.single))
            {
                messanger.Delete("Deleted", false);
            }
            else
            {
                messanger.Error("Failed to delete single event", false);
            }
            CloseWindow();
        }

        private void DeleteFollowingEvents()
        {
            if (calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.following))
            {
                messanger.Delete("Deleted", false);
            }
            else
            {
                messanger.Error("Failed to delete following events", false);
            }
            CloseWindow();
        }

        private void DeleteAllEvents()
        {
            if (calendar.DeleteEvent(selectedEvent, GoogleCalendar.ActionType.all))
            {
                messanger.Delete("Deleted", false);
            }
            else
            {
                messanger.Error("Failed to delete all events in the series", false);
            }
            CloseWindow();
        }

        private void CloseWindow()
        {
            Application.Current.Windows[1].Close();
        }
    }
}