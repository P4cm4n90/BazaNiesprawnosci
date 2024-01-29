using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Collections.ObjectModel;
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
    /// Logika interakcji dla klasy CWorker.xaml
    /// </summary>
    public partial class CWorker : UserControl, INotifyPropertyChanged
    {
        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;
        public event PropertyChangedEventHandler PropertyChanged;

        private string bAddText = "Dodaj";
        private string bEditText = "Edytuj";

        public string BAddText { get => bAddText; set { bAddText = value; NotifyPropertyChanged("BAddText"); } }
        public string BEditText { get => bEditText; set { bEditText = value; NotifyPropertyChanged("BEditText"); } }

        private TWorker selectedWorker;
        private TWorker workerToUse;

        private ObservableCollection<TWorker> workersCollection;
        public ObservableCollection<TWorker> WorkersCollection { get {
                if(workersCollection == null) { workersCollection = new ObservableCollection<TWorker>(); }
                return workersCollection; } set { workersCollection = value;  NotifyPropertyChanged("WorkersCollection"); } }

        public TWorker SelectedWorker { get {
                if(selectedWorker == null) { selectedWorker = new TWorker(); }
                return selectedWorker; } set { selectedWorker = value;  NotifyPropertyChanged("SelectedWorker"); } }
        public TWorker WorkerToUse { get {
                if (workerToUse == null) { workerToUse = new TWorker(); }
                return workerToUse; } set{ workerToUse = value; NotifyPropertyChanged("WorkerToUse"); } }

        private bool editMode;
        public bool EditMode
        {
            get => editMode; set
            {
                editMode = value;
                if (editMode)
                {
                    BAddText = "Zatwierdź zmiany";
                    BEditText = "Anuluj zmiany";
                }
                else
                {
                    BAddText = "Dodaj nowy";
                    BEditText = "Edytuj";
                }

            }
        }

        public CWorker()
        {
            InitializeComponent();
            DataContext = this;
            WorkersCollection.CollectionChanged += (sender, args) => {
                NotifyPropertyChanged("WorkersCollection");
                dgItemData.Items.Refresh(); };
            
        }

        private void ResetItemsList(object sender, RoutedEventArgs e)
        {
            WorkersCollection = new ObservableCollection<TWorker>();
        }

        private void RemoveWorker(object sender, RoutedEventArgs e)
        {
            if (DbManager.RemoveData(SelectedWorker) == 0)
            {
                var temp = SelectedWorker;
                dgItemData.SelectedIndex = -1;
                WorkersCollection.Remove(temp);
            }
        }

        private void SearchWorker(object sender, RoutedEventArgs e)
        {
            if (WorkerToUse.IsMinimal())
            {
                var temp = DbManager.ReadDatabase(WorkerToUse);
                if (temp == null)
                    return;
                else
                    foreach (TWorker item in temp) { WorkersCollection.Add(item); }
            }
            else
                LoadlAll(sender, e);
        }

        private void EditWorker(object sender, RoutedEventArgs e)
        {
            WorkerToUse = SelectedWorker;
            EditMode = true;
            
        }

        private void AddNewWorker(object sender, RoutedEventArgs e)
        {
            if (EditMode)
            {
                DbManager.UpdateData(WorkerToUse);
                WorkersCollection.ElementAt(WorkersCollection.IndexOf(SelectedWorker)).Assign(WorkerToUse);
            }
            else
            {
                if (WorkerToUse.IsComplete())
                {
                    var id = DbManager.AddNewData(WorkerToUse);
                    WorkerToUse.Id = id;
                    WorkersCollection.Add(WorkerToUse);
                }
                else
                    NotifyStatusChanged(Properties.Resources.WorkerAddWarning);
            }
        }


        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyStatusChanged(string text)
        {
            OnStatusChange?.Invoke(this, new TextEventArgs(text));
        }

        private static bool StrEqual(object a, object b)
        {
            if (String.Compare((string)a, (string)b) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void LoadlAll(object sender, RoutedEventArgs e)
        {
            var temp = DbManager.ReadWholeDatabase(new TWorker());
            if (temp == null)
            {
                NotifyStatusChanged(Properties.Resources.SearchAllNotFoundWorker);
                return;
            }
            else
                foreach (TWorker item in temp) { WorkersCollection.Add(item); }
        }
    }
}
