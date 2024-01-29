using BazaNiesprawnosci.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BazaNiesprawnosci
{
    /// <summary>
    /// Logika interakcji dla klasy CAircraft.xaml
    /// </summary>
    public partial class CAircraft : UserControl, INotifyPropertyChanged
    {
        private const string beEditCancel = "Anuluj";
        private const string beEdit = "Edytuj";
        private const string baAddSet = "Wprowadź zmiany";
        private const string baAdd = "Dodaj statek";

        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;
        public event PropertyChangedEventHandler PropertyChanged;

        private TAircraft aircraftActive;
        private TAircraft aircraftSelected;

        private string beText = beEdit;
        private string baText = baAdd;

        private bool editMode;
        public TAircraft AircraftActive
        {
            get { return aircraftActive ?? (aircraftActive = new TAircraft()); }
            set { aircraftActive = value; NotifyPropertyChanged("AircraftActive"); }
        }

        public TAircraft AircraftSelected
        {
            get { return aircraftSelected ?? (aircraftSelected = new TAircraft()); }
            set
            {
                aircraftSelected = value; NotifyPropertyChanged("AircraftSelected");
            }
        }

        public ObservableCollection<TAircraft> AircraftCollection {
            get {               
                return aircraftCollection ?? (aircraftCollection = new ObservableCollection<TAircraft>());}
            set { aircraftCollection = value; } }

        public bool EditMode
        { get => editMode;
            set {
                editMode = value;
                if(EditMode)
                {
                    BaText = baAddSet;
                    BeText = beEditCancel;
                }
                else
                {
                    BaText = baAdd;
                    BeText = beEdit;
                }
                NotifyPropertyChanged("EditMode"); } }

        public string BeText { get => beText; set { beText = value;
                NotifyPropertyChanged("BeText"); } }
        public string BaText { get => baText; set { baText = value;
                NotifyPropertyChanged("BaText"); } }

        private ObservableCollection<TAircraft> aircraftCollection;

        public CAircraft()
        {
            InitializeComponent();
            DataContext = this;
            BaText = baAdd;
            BeText = beEdit;
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (AircraftSelected.Id != 0) {
                var result = MessageBox.Show(Properties.Resources.CAircraftRemoveFromDatabase, Properties.Resources.CAircraftRemoveCaption, MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    DbManager.RemoveData(AircraftSelected);
                    var temp = AircraftSelected;
                    dgItemData.SelectedIndex = -1;
                    AircraftCollection.Remove(AircraftSelected);                  
                }
                else if (result == MessageBoxResult.No)
                {
                    var temp = AircraftSelected;
                    dgItemData.SelectedIndex = -1;
                    AircraftCollection.Remove(AircraftSelected);
                }
            }
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if(EditMode)
            {
                DbManager.UpdateData(AircraftActive);
                AircraftCollection[AircraftCollection.IndexOf(AircraftSelected)].Assign(AircraftActive);
                EditMode = false;
            }
            else
            {
                if (AircraftActive.IsComplete())
                    DbManager.AddNewData(AircraftActive);
                else
                    NotifyStatusChanged(Properties.Resources.AircraftNotCompleted);
            }
        }

        private void SearchItem(object sender, RoutedEventArgs e)
        {
            if (!AircraftActive.IsMinimal())
            {
                NotifyStatusChanged(Properties.Resources.CAircraftSearchNoDataLoadAll);
                LoadAllItems(sender, e);
            }
            else
            {
                var results = DbManager.ReadDatabase(AircraftActive);
                foreach(IDbTable item in results) { AircraftCollection.Add((TAircraft)item); }
            }

        }

        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (!EditMode)
            {
                AircraftActive.Assign(AircraftSelected);
                EditMode = true;
            }
            else
                EditMode = false;
        }

        private void LoadAllItems(object sender, RoutedEventArgs e)
        {
            var results = DbManager.ReadWholeDatabase(new TAircraft());
            foreach (IDbTable item in results) { AircraftCollection.Add((TAircraft)item); }
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            AircraftCollection.Clear();
            dgItemData.UpdateLayout();
        }

        public bool IsComplete()
        {
            return AircraftActive.IsComplete();
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyStatusChanged(string text)
        {
            OnStatusChange?.Invoke(this, new TextEventArgs(text));
        }
    }
}
