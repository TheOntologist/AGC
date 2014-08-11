using AGC.GUI.Common;
using AGC.Library;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace AGC.GUI.ViewModel
{

    public class AddQuickEventViewModel : ViewModelBase
    {
        #region Constants

        private const string QUICK_EVENT_TEXT_EXAMPLE = "Example: Dinner with Michael 7 p.m. tomorrow";

        #endregion

        #region Private Properties

        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IGoogleCalendar calendar;
        private readonly IRepository repository;
        private QuickEventsTemplates quickEventsTemplates;

        #endregion

        #region Commands

        public RelayCommand SelectTemplateCommand { get; private set; }
        public RelayCommand MoveUpTemplateCommand { get; private set; }
        public RelayCommand MoveDownTemplateCommand { get; private set; }
        public RelayCommand RemoveTemplateCommand { get; private set; }
        public RelayCommand AddQuickEventCommand { get; private set; }
        public RelayCommand SaveAsTemplateCommand { get; private set; }

        #endregion

        #region Constructor

        public AddQuickEventViewModel(IGoogleCalendar googleCalendar, IRepository commonRepository)
        {
            try
            {
                log.Debug("Loading AddQuickEvent view model...");

                calendar = googleCalendar;
                repository = commonRepository;
                quickEventsTemplates = repository.GetQuickEventsTemplates();
                LoadData();

                SelectTemplateCommand = new RelayCommand(SelectTemplate);
                MoveUpTemplateCommand = new RelayCommand(MoveUpTemplate);
                MoveDownTemplateCommand = new RelayCommand(MoveDownTemplate);
                RemoveTemplateCommand = new RelayCommand(RemoveTemplate);
                AddQuickEventCommand = new RelayCommand(AddQuickEvent);
                SaveAsTemplateCommand = new RelayCommand(SaveAsTemplate);

                log.Debug("AddQuickEvent view model was succssfully loaded");
            }
            catch (Exception ex)
            {
                log.Error("Failed to load AddQuickEvent view model:", ex);
            }
        }

        #endregion

        #region Public Properties

        public const string TempaltesPropertyName = "Tempaltes";
        private ObservableCollection<string> _tempaltes = new ObservableCollection<string>();
        public ObservableCollection<string> Tempaltes
        {
            get
            {
                return _tempaltes;
            }

            set
            {
                if (_tempaltes == value)
                {
                    return;
                }

                RaisePropertyChanging(TempaltesPropertyName);
                _tempaltes = value;
                RaisePropertyChanged(TempaltesPropertyName);
            }
        }

        public const string SelectedTemplatePropertyName = "SelectedTemplate";
        private string _selectedTemplate = string.Empty;
        public string SelectedTemplate
        {
            get
            {
                return _selectedTemplate;
            }

            set
            {
                if (_selectedTemplate == value)
                {
                    return;
                }

                RaisePropertyChanging(SelectedTemplatePropertyName);
                _selectedTemplate = value;
                RaisePropertyChanged(SelectedTemplatePropertyName);
            }
        }

        public const string EditTemplatePropertyName = "EditTemplate";
        private bool _editTemplate = false;
        public bool EditTemplate
        {
            get
            {
                return _editTemplate;
            }

            set
            {
                if (_editTemplate == value)
                {
                    return;
                }

                RaisePropertyChanging(EditTemplatePropertyName);
                _editTemplate = value;
                RaisePropertyChanged(EditTemplatePropertyName);
            }
        }

        public const string IsTemplateSelectedPropertyName = "IsTemplateSelected";
        private bool _isTemplateSelected = false;
        public bool IsTemplateSelected
        {
            get
            {
                return _isTemplateSelected;
            }

            set
            {
                if (_isTemplateSelected == value)
                {
                    return;
                }

                RaisePropertyChanging(IsTemplateSelectedPropertyName);
                _isTemplateSelected = value;
                RaisePropertyChanged(IsTemplateSelectedPropertyName);
            }
        }

        public const string QuickEventTextPropertyName = "QuickEventText";
        private string _quickEventText = QUICK_EVENT_TEXT_EXAMPLE;
        public string QuickEventText
        {
            get
            {
                return _quickEventText;
            }

            set
            {
                if (_quickEventText == value)
                {
                    return;
                }

                RaisePropertyChanging(QuickEventTextPropertyName);
                _quickEventText = value;
                RaisePropertyChanged(QuickEventTextPropertyName);
            }
        }

        #endregion

        #region Private Methods

        private void LoadData()
        {
            foreach (var item in quickEventsTemplates.Templates)
            {
                Tempaltes.Add(item);
            }

            SelectedTemplate = Tempaltes.Count > 0 ? Tempaltes[0] : string.Empty;
        }

        private void SelectTemplate()
        {
            QuickEventText = SelectedTemplate;
            IsTemplateSelected = true;
        }

        private void MoveUpTemplate()
        {
            if (Tempaltes.Count < 1)
            {
                return;
            }

            int selectedTemplateIndex = Tempaltes.IndexOf(SelectedTemplate);

            if (selectedTemplateIndex == 0)
            {
                MessageBox.Show(Application.Current.MainWindow, "First position reached", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                return;
            }

            string buffer = Tempaltes[selectedTemplateIndex - 1];
            Tempaltes[selectedTemplateIndex - 1] = SelectedTemplate;
            Tempaltes[selectedTemplateIndex] = buffer;
            SelectedTemplate = Tempaltes[selectedTemplateIndex - 1];
            SaveChanges();
        }

        private void MoveDownTemplate()
        {
            if (Tempaltes.Count < 1)
            {
                return;
            }

            int selectedTemplateIndex = Tempaltes.IndexOf(SelectedTemplate);

            if (selectedTemplateIndex == Tempaltes.Count - 1)
            {
                MessageBox.Show(Application.Current.MainWindow, "Last position reached", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
                return;
            }

            string buffer = Tempaltes[selectedTemplateIndex + 1];
            Tempaltes[selectedTemplateIndex + 1] = SelectedTemplate;
            Tempaltes[selectedTemplateIndex] = buffer;
            SelectedTemplate = Tempaltes[selectedTemplateIndex + 1];

            SaveChanges();
        }

        private void RemoveTemplate()
        {
            Tempaltes.Remove(SelectedTemplate);
            MessageBox.Show(Application.Current.MainWindow, "Removed", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            SelectedTemplate = Tempaltes.Count > 0 ? Tempaltes[0] : string.Empty;
            SaveChanges();

        }

        private void AddQuickEvent()
        {
            if (calendar.AddQuickEvent(QuickEventText))
            {
                MessageBox.Show(Application.Current.MainWindow, "Added", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            }
            else
            {
                MessageBox.Show(Application.Current.MainWindow, "Failed to add quick event. Please check log file for a detailed information about the error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
            }
        }

        private void SaveAsTemplate()
        {
            Tempaltes.Add(QuickEventText);
            MessageBox.Show(Application.Current.MainWindow, "Saved", "Information", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.OK);
            SaveChanges();
        }

        private void SaveChanges()
        {
            List<string> newTemplates = new List<string>();
            foreach (var item in Tempaltes)
            {
                newTemplates.Add(item);
            }
            quickEventsTemplates.Templates = newTemplates;
            quickEventsTemplates.Save();
            repository.SetQuickEventsTemplates(quickEventsTemplates);
        }

        #endregion
    }
}