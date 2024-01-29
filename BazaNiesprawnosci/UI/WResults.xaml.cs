using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;



namespace BazaNiesprawnosci
{
    /// <summary>
    /// Interaction logic for WResults.xaml
    /// </summary>

    public partial class WResults : Window, INotifyPropertyChanged
    {

        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;

        public delegate void DefectChangeHandler(object sender, DefectChange dc);
        public event DefectChangeHandler OnDefectChange;

        public event PropertyChangedEventHandler PropertyChanged;

        private DateTime date = new DateTime(1990, 1, 1);
        public ObservableCollection<TDefect> data;
        private TDefect defectSelected;
        private bool isShown = false;

        public TDefect DefectSelected { get => defectSelected; set { defectSelected = value; NotifyPropertyChanged("DefectSelected"); } }

        public WResults()
        {
            InitializeComponent();


        }

        public void ShowResults(ObservableCollection<TDefect> defects)
        {
            data = defects;
            //dgTable.DataContext = data;
            dgTable.ItemsSource = data;
            if(!isShown){
            this.Show();
                
            isShown = true;
            }
            SetWindows(0);
           
        }


        private void dgTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { //selected index chuju
           // var _data = (DList)DataContext;
            
            var number = dgTable.SelectedIndex;
            SetWindows(number);
            
        }
        /// <summary>
        /// do poprawy
        /// </summary>
        /// <param name="number"></param>
        private void SetWindows(int number)
        {
            try
            {
                if (number != -1 && number < data.Count)
                { //popraw
                    this.DataContext = data[number];
                }
            }
            catch { }
        }
        private void bRemoveEntry_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Czy napewno chcesz usunąc wpis", "Usuwanie wpisu", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                if(!ValidateSelection())
                {
                    MessageBox.Show("Przed próba usunięcia wybierz wpis");
                    return;
                }
               // var data = (DList)DataContext;
                var number = dgTable.SelectedIndex;
                var id = data[number].Id;              
                NotifyDefectChange(DefectChangeType.Delete, data[number].Id);
               // data.RemoveAt(number);
                dgTable.UpdateLayout();
            }           
        }

        private void bFixEntry_Click(object sender, RoutedEventArgs e)
        {
                if(!ValidateSelection())
                {
                    MessageBox.Show("Przed próba zmiany wybierz wpis");
                    return;
                }
            var number = dgTable.SelectedIndex;
            NotifyDefectChange(DefectChangeType.Fix, data[number].Id);
            this.DataContext = null;
            dgTable.SelectedIndex = -1;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            isShown = false;
            this.Hide();
            base.OnClosing(e);
        }

        private bool ValidateSelection()
        {
            if(dgTable.SelectedIndex == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

        }

        public static string GetPropertyDisplayName(object descriptor)
        {
            var pd = descriptor as PropertyDescriptor;

            if (pd != null)
            {
                // Check for DisplayName attribute and set the column header accordingly
                var displayName = pd.Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute;

                if (displayName != null && displayName != DisplayNameAttribute.Default)
                {
                    return displayName.DisplayName;
                }

            }
            else
            {
                var pi = descriptor as PropertyInfo;

                if (pi != null)
                {
                    // Check for DisplayName attribute and set the column header accordingly
                    Object[] attributes = pi.GetCustomAttributes(typeof(DisplayNameAttribute), true);
                    for (int i = 0; i < attributes.Length; ++i)
                    {
                        var displayName = attributes[i] as DisplayNameAttribute;
                        if (displayName != null && displayName != DisplayNameAttribute.Default)
                        {
                            return displayName.DisplayName;
                        }
                    }
                }
            }

            return null;
        }

        private void dgTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);

            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }
        }

        public void NotifyDefectChange(DefectChangeType type, int _id)
        {
            if (OnDefectChange != null)
                OnDefectChange(this, new DefectChange(type, _id));
        }


    }

    public class DefectEventArgs : EventArgs
    {
        public TDefect def { get; set;}
        public bool dbEntry {get; set;}
        public DefectEventArgs(TDefect _def, bool _dbEntry) : base()
        {
            def = (TDefect)_def.Duplicate();
            dbEntry = _dbEntry;
            
        }

        
    }

    public class DefectChange : EventArgs
    {
        public DefectChangeType DefType { get; set; }
        public int Id { get; set; }

        public DefectChange(DefectChangeType _type, int id)
            : base()
        {
            DefType = _type;
            Id = id;

        }
    }

    public enum DefectChangeType
    {
        Fix,
        Delete
    }




}
