using Borne2017.DataAccessObject;
using Borne2017.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPBorne.Exceptions;
using Windows.Devices.Geolocation;

namespace Borne2017.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private IDialogService dialogService;

        //*******************************************************
        //*                  RECUPERATION DONNEES               *
        //*******************************************************

        private ObservableCollection<ApplicationUser> _cafes;

        public ObservableCollection<ApplicationUser> Cafes
        {
            get { return _cafes; }
            set
            {
                if (_cafes == value)
                    return;
                _cafes = value;
                RaisePropertyChanged("Cafes");
            }
        }

        private ObservableCollection<CafeWithCharities> _users;

        public ObservableCollection<CafeWithCharities> Users
        {
            get { return _users; }
            set
            {
                if (_users == value)
                    return;
                _users = value;

                ObservableCollection<ApplicationUser> cafesValidate = new ObservableCollection<ApplicationUser>();

                foreach (CafeWithCharities user in _users)
                {
                    cafesValidate.Add(user.Cafe);
                }

                Cafes = cafesValidate;

                RaisePropertyChanged("Users");
            }
        }
        //*******************************************************
        //*              FIN RECUPERATION DONNEES               *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                      INFO CAFE                      *
        //*******************************************************
        private TimeSpan _closingHour;

        public TimeSpan ClosingHour
        {
            get { return _closingHour; }
            set
            {
                _closingHour = value;
                RaisePropertyChanged("ClosingHour");
            }
        }

        private TimeSpan _openingHour;

        public TimeSpan OpeningHour
        {
            get { return _openingHour; }
            set
            {
                _openingHour = value;
                RaisePropertyChanged("OpeningHour");
            }
        }
        //*******************************************************
        //*                     FIN INFO CAFE                   *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                     SELECTED CAFE                   *
        //*******************************************************
        public ApplicationUser _selectedCafe;

        public ApplicationUser SelectedCafe
        {
            get { return _selectedCafe; }
            set
            {
                _selectedCafe = value;
                int dayOfWeek = (int)(DateTime.Now.DayOfWeek);
                

                if (_selectedCafe != null)
                {
                    ButtonBookIsEnabled = true;
                    OpeningHour = _selectedCafe.TimeTables.ElementAt(dayOfWeek).OpeningHour;
                    ClosingHour = _selectedCafe.TimeTables.ElementAt(dayOfWeek).ClosingHour;
                }
                else
                {
                    ButtonBookIsEnabled = false;
                    OpeningHour = new TimeSpan(1, 0, 0, 0);
                    ClosingHour = new TimeSpan(1, 0, 0, 0);
                    Cancel();
                }
                RaisePropertyChanged("SelectedCafe");
                if(_selectedCafe != null)
                    InitializePosition(); 
            }
        }

        //*******************************************************
        //*                   FIN SELECTED CAFE                 *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                     CONSTRUCTEUR                    *
        //*******************************************************
        public MainViewModel(IDialogService dialogService)
        {
            InitializeAsync();
            this.dialogService = dialogService;
            StartApp();
            CafeLocation = new Geopoint(new BasicGeoposition()
            {
                Latitude = 50.4673883,
                Longitude = 4.871985399999971
            });
        }
        //*******************************************************
        //*                 FIN CONSTRUCTEUR                    *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                      INITIALIZE                     *
        //*******************************************************
        private async Task InitializeAsync()
        {
            try
            {
                var service = new CafeService();
                var user = await service.GetCafe();
                Users = new ObservableCollection<CafeWithCharities>(user);
            }
            catch (DataNotReachableException e)
            {
                ShowErrorDatabaseConnectionMessage(e);
            }
        }

        private async void InitializePosition()
        {
            try
            {
                var service = new PositionService();

                LatitudeAndLongitude cafePosition = await service.GetLatitudeAndLongitude(SelectedCafe.Number + ", " + SelectedCafe.Street);

                CafeLocation = new Geopoint(new BasicGeoposition()
                {
                    Latitude = cafePosition.Latitude,
                    Longitude = cafePosition.Longitude
                });
            }
            catch (DataNotReachableException e)
            {
                ShowErrorDatabaseConnectionMessage(e);
            }
        }
        //*******************************************************
        //*                  FIN INITIALIZE                     *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*               BUTTON VISIBILITY                     *
        //*******************************************************       
        private string _confirmButtonVisibility;

        public string ConfirmButtonVisibility
        {
            get { return _confirmButtonVisibility; }
            set
            {
                _confirmButtonVisibility = value;
                RaisePropertyChanged("ConfirmButtonVisibility");
            }
        }

        private string _cancelButtonVisibility;

        public string CancelButtonVisibility
        {
            get { return _cancelButtonVisibility; }
            set
            {
                _cancelButtonVisibility = value;
                RaisePropertyChanged("CancelButtonVisibility");
            }
        }

        private string _bookButtonVisibility;

        public string BookButtonVisibility
        {
            get { return _bookButtonVisibility; }
            set
            {
                _bookButtonVisibility = value;
                RaisePropertyChanged("BookButtonVisibility");
            }
        }

        private string _textReservationNameVisibility;

        public string TextReservationNameVisibility
        {
            get { return _textReservationNameVisibility; }
            set
            {
                _textReservationNameVisibility = value;
                RaisePropertyChanged("TextReservationNameVisibility");
            }
        }

        private string _textBoxReservationNameVisibility;

        public string TextBoxReservationNameVisibility
        {
            get { return _textBoxReservationNameVisibility; }
            set
            {
                _textBoxReservationNameVisibility = value;
                RaisePropertyChanged("TextBoxReservationNameVisibility");
            }
        }

        private string _informationVisibility;

        public string InformationVisibility
        {
            get { return _informationVisibility; }
            set
            {
                _informationVisibility = value;
                RaisePropertyChanged("InformationVisibility");
            }
        }
        //*******************************************************
        //*             FIN BUTTON VISIBILITY                   *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                     BUTTON ENABLED                  *
        //*******************************************************
        private bool _buttonBookIsEnabled;

        public bool ButtonBookIsEnabled
        {
            get { return _buttonBookIsEnabled; }
            set
            {
                _buttonBookIsEnabled = value;
                RaisePropertyChanged("ButtonBookIsEnabled");
            }
        }

        private bool _buttonConfirmIsEnabled;

        public bool ButtonConfirmIsEnabled
        {
            get { return _buttonConfirmIsEnabled; }
            set
            {
                _buttonConfirmIsEnabled = value;
                RaisePropertyChanged("ButtonConfirmIsEnabled");
            }
        }

        //*******************************************************
        //*                    MISE A JOUR PAGE                 *
        //*******************************************************
        private void StartApp()
        {
            ConfirmButtonVisibility = "Collapsed";
            CancelButtonVisibility = "Collapsed";
            BookButtonVisibility = "Visible";
            TextBoxReservationNameVisibility = "Collapsed";
            TextReservationNameVisibility = "Collapsed";
            InformationVisibility = "Collapsed";

            ValueNameTextBox = "";

            ButtonBookIsEnabled = false;
            ButtonConfirmIsEnabled = false;

            SelectedCafe = null;
        }

        private void Booking()
        {
            ConfirmButtonVisibility = "Visible";
            CancelButtonVisibility = "Visible";
            TextBoxReservationNameVisibility = "Visible";
            TextReservationNameVisibility = "Visible";
            BookButtonVisibility = "Collapsed";
            InformationVisibility = "Visible";

            ButtonConfirmIsEnabled = false;
        }

        private void Cancel()
        {
            ConfirmButtonVisibility = "Collapsed";
            CancelButtonVisibility = "Collapsed";
            TextBoxReservationNameVisibility = "Collapsed";
            TextReservationNameVisibility = "Collapsed";
            BookButtonVisibility = "Visible";
            InformationVisibility = "Collapsed";

            ValueNameTextBox = "";

            ButtonConfirmIsEnabled = false;
            ButtonBookIsEnabled = false;

            InitializeAsync();
        }
        //*******************************************************
        //*                FIN MISE A JOUR PAGE                 *
        //*******************************************************

        //================================================================


        //*******************************************************
        //*                       TEXTBOX                       *
        //*******************************************************
        private string _valueNameTextBox;

        public string ValueNameTextBox
        {
            get { return _valueNameTextBox; }
            set
            {
                _valueNameTextBox = value;

                //Utile pour quand quelqu'un écrit juste des espaces
                bool result = value.ToCharArray().All(c => c == ' ');

                string nameValue = _valueNameTextBox.Replace(" ", "");
                int nameLenght = nameValue.Length;

                if (String.IsNullOrEmpty(_valueNameTextBox) || result || nameLenght < 6)
                    ButtonConfirmIsEnabled = false;
                else
                    ButtonConfirmIsEnabled = true;

                RaisePropertyChanged("ValueNameTextBox");
            }
        }
        //*******************************************************
        //*                      FIN TEXTBOX                    *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                        ACTION BUTTON                *
        //*******************************************************
        private ICommand _bookCommand;

        public ICommand BookCommand
        {
            get
            {
                if( this._bookCommand == null)
                {
                    this._bookCommand = new RelayCommand(BookAction);
                }
                return this._bookCommand;
            }
        }

        private void BookAction()
        {       
            Booking();
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if(this._cancelCommand == null)
                {
                    this._cancelCommand = new RelayCommand(CancelAction);
                }
                return this._cancelCommand;
            }
        }

        private void CancelAction()
        {
            Cancel();
        }

        private ICommand _confirmCommand;

        public ICommand ConfirmCommand
        {
            get
            {
                if(this._confirmCommand == null)
                {
                    this._confirmCommand = new RelayCommand(ConfirmAction);
                }
                return this._confirmCommand;
            }
        }

        public async void ConfirmAction()
        {
            try
            {
                var service = new CafeService();
                await service.ProceedBooking(SelectedCafe.CafeName, 1, ValueNameTextBox);

                await dialogService.ShowMessageBox("La réservation a bien été effectuée au nom de " + _valueNameTextBox, "Réservation effectuée");
                Cancel();        
            }
            catch(DataUpdateNotPossibleException e)
            {
                ShowErrorUpdateNotPossible(e);
            }
        }

        private ICommand _actualizeCommand;

        public ICommand ActualizeCommand
        {
            get
            {
                if(_actualizeCommand == null)
                {
                    this._actualizeCommand = new RelayCommand(ActualizeAction);
                }

                return this._actualizeCommand;
            }
        }

        public void ActualizeAction()
        {
            InitializeAsync();
            CafeLocation = new Geopoint(new BasicGeoposition() { Latitude = 50.4673883, Longitude = 4.871985399999971 });
            Cancel();
        }
        //*******************************************************
        //*                    FIN ACTION BUTTON                *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                     GEOLOCALISATION                 *
        //*******************************************************
        private Geopoint _cafeLocation;

        public Geopoint CafeLocation
        {
            get { return _cafeLocation; }
            set
            {
                _cafeLocation = value;
                RaisePropertyChanged("CafeLocation");
            }
        }

        //*******************************************************
        //*                 FIN GEOLOCALISATION                 *
        //*******************************************************

        //================================================================

        //*******************************************************
        //*                   MESSAGES ERREUR                   *
        //*******************************************************

        private async void ShowErrorUpdateNotPossible(DataUpdateNotPossibleException e)
        {
            await dialogService.ShowMessageBox(e.GetMessage(),
                                "Problème");
        }

        private async void ShowErrorDatabaseConnectionMessage(DataNotReachableException e)
        {
            await dialogService.ShowMessage(e.GetMessage() + "\n Voulez-vous essayer d'actualiser ?",
                "Erreur",
                buttonConfirmText: "Oui", buttonCancelText: "Annuler",
                afterHideCallback: (confirmed) =>
                {
                    if (confirmed)
                    {
                        ActualizeAction();
                    }
                });
        }
        //*******************************************************
        //*               FIN MESSAGES ERREUR                   *
        //*******************************************************
    }
}

/*await dialogService.ShowMessage("Une réservation a bien été effectuée au nom de ",
                        "Réservation",
                        buttonConfirmText: "Ok", buttonCancelText: "Annuler",
                                afterHideCallback: (confirmed) =>
                                {
                                    if (confirmed)
                                    {
                                        // User has pressed the "confirm" button.
                                        // ...
                                    }
                                    else
                                    {
                                        // User has pressed the "cancel" button
                                        // (or has discared the dialog box).
                                        // ...
                                    }
                                });*/
