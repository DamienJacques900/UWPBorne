using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UWPBorne.DataAccessObject;
using UWPBorne.Exceptions;
using UWPBorne.Model;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;

namespace UWPBorne.ViewModel
{
    class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        //collections

        private ObservableCollection<CafeWithCharities> _users = null;

        public ObservableCollection<CafeWithCharities> Users
        {
            get { return _users; }
            set
            {
                if (_users == value)
                    return;
                _users = value;

                ObservableCollection<ApplicationUser> appUsers = new ObservableCollection<ApplicationUser>();

                foreach (CafeWithCharities user in _users)
                {
                    appUsers.Add(user.Cafe);
                }

                Cafes = appUsers;

                RaisePropertyChanged("Users");
            }
        }

        private ObservableCollection<ApplicationUser> _cafes = null;

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

        //boutons

        private bool _isBookingButtonEnabled;

        public bool IsBookingButtonEnabled
        {
            get
            {
                return _isBookingButtonEnabled;
            }   
            set
            {
                _isBookingButtonEnabled = value;
                RaisePropertyChanged("IsBookingButtonEnabled");
            }
        }

        private string _bookingButtonVisibility;

        public string BookingButtonVisibility
        {
            get
            {
                return _bookingButtonVisibility;
            }

            set
            {
                _bookingButtonVisibility = value;
                RaisePropertyChanged("BookingButtonVisibility");
            }
        }

        private bool _isConfirmButtonEnabled;

        public bool IsConfirmButtonEnabled
        {
            get
            {
                return _isConfirmButtonEnabled;
            }
            set
            {
                _isConfirmButtonEnabled = value;
                RaisePropertyChanged("IsConfirmButtonEnabled");
            }
        }
        private string _confirmButtonVisibility;

        public string ConfirmButtonVisibility
        {
            get
            {
                return _confirmButtonVisibility;
            }

            set
            {
                _confirmButtonVisibility = value;
                RaisePropertyChanged("ConfirmButtonVisibility");
            }
        }

        private string _cancelButtonVisibility;

        public string CancelButtonVisibility
        {
            get
            {
                return _cancelButtonVisibility;
            }

            set
            {
                _cancelButtonVisibility = value;
                RaisePropertyChanged("CancelButtonVisibility");
            }
        }

        //Text

        private string _nameBlockVisibility;

        public string NameBlockVisibility
        {
            get
            {
                return _nameBlockVisibility;
            }

            set
            {
                _nameBlockVisibility = value;
                RaisePropertyChanged("NameBlockVisibility");
            }
        }

        private string _nameBoxVisibility;

        public string NameBoxVisibility
        {
            get
            {
                return _nameBoxVisibility;
            }

            set
            {
                _nameBoxVisibility = value;
                RaisePropertyChanged("NameBoxVisibility");
            }
        }

        private string _nameBoxContent;

        public string NameBoxContent
        {
            get
            {
                return _nameBoxContent;
            }

            set
            {
                _nameBoxContent = value;
                bool result = value.ToCharArray().All(c => c == ' ');

                if (String.IsNullOrEmpty(_nameBoxContent) || result)
                    IsConfirmButtonEnabled = false;
                else
                    IsConfirmButtonEnabled = true;
                RaisePropertyChanged("NameBoxContent");
            }
        }

        //objects

        private TimeTable _timeTable = null;

        public TimeTable TimeTable
        {
            get { return _timeTable; }
            set
            {
                if (_timeTable == value)
                    return;
                _timeTable = value;
                RaisePropertyChanged("TimeTable");
            }
        }


        private ApplicationUser _selectedItem;

        public ApplicationUser SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                int dayOfWeek = (int)(DateTime.Now.DayOfWeek);
                dayOfWeek = (dayOfWeek == 0) ? 7 : dayOfWeek;

                if (_selectedItem == null)
                {
                    IsBookingButtonEnabled = false;
                    OpeningHour = new TimeSpan(1,0,0,0);
                    ClosingHour = new TimeSpan(1,0,0,0);
                }
                else
                {
                    IsBookingButtonEnabled = true;
                    OpeningHour = value.TimeTables.ElementAt(dayOfWeek - 1).OpeningHour;
                    ClosingHour = value.TimeTables.ElementAt(dayOfWeek - 1).ClosingHour;
                }


                if (BookingButtonVisibility == "Collapsed")
                {
                    CancelAction();
                }

                RaisePropertyChanged("SelectedItem");
                if(_selectedItem != null)
                    InitializePosition();

            }
        }

        private TimeSpan _openingHour;

        public TimeSpan OpeningHour
        {
            get
            {
                return _openingHour;
            }
            set
            {
                _openingHour = value;
                RaisePropertyChanged("OpeningHour");
            }
        }

        private TimeSpan _closingHour;

        public TimeSpan ClosingHour
        {
            get
            {
                return _closingHour;
            }
            set
            {
                _closingHour = value;
                RaisePropertyChanged("ClosingHour");
            }
        }

        private Geopoint _cafeLocation;


        public Geopoint CafeLocation
        {
            get
            {
                return _cafeLocation;
            }
            set
            {
                _cafeLocation = value;
                RaisePropertyChanged("CafeLocation");
            }
        }

        //Others

        private IDialogService dialogService;

        private ICommand _reservationCommand;

        public ICommand ReservationCommand
        {
            get
            {
                if (this._reservationCommand == null)
                {
                    this._reservationCommand = new RelayCommand(BookingAction);
                }
                return this._reservationCommand;
            }
        }

        private ICommand _cancelCommand;

        public ICommand CancelCommand
        {
            get
            {
                if (this._cancelCommand == null)
                {
                    this._cancelCommand = new RelayCommand(CancelAction);
                }
                return this._cancelCommand;
            }
        }

        private ICommand _confirmCommand;

        public ICommand ConfirmCommand
        {
            get
            {
                if (this._confirmCommand == null)
                {
                    this._confirmCommand = new RelayCommand(ConfirmAction);
                }
                return this._confirmCommand;
            }
        }

        private ICommand _refreshCommand;

        public ICommand RefreshCommand
        {
            get
            {
                if (this._refreshCommand == null)
                {
                    this._refreshCommand = new RelayCommand(RefreshAction);
                }
                return this._refreshCommand;
            }
        }


        public MainViewModel(IDialogService dialogService)
        {
            InitializeAsync();

            this.dialogService = dialogService;

            CancelAction();

            CafeLocation = new Geopoint(new BasicGeoposition() { Latitude = 50.4673883, Longitude = 4.871985399999971 });

        }

        private async Task InitializeAsync()
        {
            try
            {
                var service = new CafeService();
                var user = await service.GetCafe();
                Users = new ObservableCollection<CafeWithCharities>(user);

            }
            catch(DataNotReachableException e)
            {
                ShowErrorDatabaseConnectionMessage(e);
            }
        }

        private async void ShowErrorDatabaseConnectionMessage(DataNotReachableException e)
        {
            await dialogService.ShowMessage(e.GetMessage() + "\n Voulez-vous essayer d'actualiser ?",
                "Erreur",
                buttonConfirmText: "Onameboui", buttonCancelText: "Annuler",
                afterHideCallback: (confirmed) =>
                {
                    if (confirmed)
                    {
                        RefreshAction();
                    }
                });
        }

        private async void InitializePosition()
        {
            try
            {
                var service = new PositionService();
                LatitudeAndLongitude coord = await service.GetLatitudeAndLongitude(SelectedItem.Number + ", " + SelectedItem.Street);

                CafeLocation = new Geopoint(new BasicGeoposition() { Latitude = coord.Latitude, Longitude = coord.Longitude });
            }
            catch (DataNotReachableException e)
            {
                ShowErrorDatabaseConnectionMessage(e);
            }
        }

        
        private void BookingAction()
        {
            BookingButtonVisibility = "Collapsed";
            NameBlockVisibility = "Visible";
            NameBoxVisibility = "Visible";
            CancelButtonVisibility = "Visible";
            ConfirmButtonVisibility = "Visible";
        }

        private void CancelAction()
        {
            BookingButtonVisibility = "Visible";
            NameBlockVisibility = "Collapsed";
            NameBoxVisibility = "Collapsed";
            CancelButtonVisibility = "Collapsed";
            ConfirmButtonVisibility = "Collapsed";
            SelectedItem = null;

            NameBoxContent = "";
        }

        private void RefreshAction()
        {
            InitializeAsync();
            CafeLocation = new Geopoint(new BasicGeoposition() { Latitude = 50.4673883, Longitude = 4.871985399999971 });
            CancelAction();
        }

        private async void ConfirmAction()
        {
            try
            {
                var service = new CafeService();
                await service.ProceedBooking(SelectedItem.CafeName, 1, NameBoxContent);

                var yesCommand = new UICommand("Ok", cmd => { });

                var dialog = new MessageDialog("Réservation effectuée !", "Réservation OK");

                dialog.Options = MessageDialogOptions.None;
                dialog.Commands.Add(yesCommand);

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 0;

                var command = await dialog.ShowAsync();
                RefreshAction();

            }
            catch(DataUpdateNotPossibleException e)
            {
                await dialogService.ShowMessage(e.GetMessage(),
                        "Erreur",
                        buttonConfirmText: "OK", buttonCancelText: "Annuler",
                        afterHideCallback: (confirmed) =>
                        {
                       
                        });
                RefreshAction();
            }
        }


    }
}
