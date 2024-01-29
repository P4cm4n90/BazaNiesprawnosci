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
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    /// <summary>
    /// Logika interakcji dla klasy CItemDescription.xaml
    /// </summary>
    public partial class CItem : UserControl, INotifyPropertyChanged
    {
        private readonly static string tbDateInstall = "Data zabudowania";
        private readonly static string tbDateRemoval = "Data wybudowania";

        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;
        public event PropertyChangedEventHandler PropertyChanged;



        private TItem itemSelected;
        private TItem itemActive;

        private string bAddText = "Dodaj nowy";
        private string bEditText= "Edytuj";
        
        private bool enabledTbSub2 = false;
        private bool enabledTbSub3 = false;

        public bool EnabledTbSub2 { get => enabledTbSub2; set { enabledTbSub2 = value; NotifyPropertyChanged("EnabledTbSub2"); } }
        public bool EnabledTbSub3 { get => enabledTbSub3; set { enabledTbSub3 = value; NotifyPropertyChanged("EnabledTbSub3"); } }

        public TItem ItemActive { get => 
                itemActive ?? (itemActive = new TItem());
            set {
                itemActive = value; NotifyPropertyChanged("ItemActive"); } }
        public TItem ItemSelected { get {
                return itemSelected ?? (itemSelected = new TItem()); } set {
                itemSelected = value;
                NotifyPropertyChanged("ItemSelected"); } }

        private bool editMode;
        public bool EditMode { get => editMode; set {
                editMode = value;
                if(editMode)
                {
                    BAddText = "Zatwierdź zmiany";
                    BEditText = "Anuluj zmiany";
                }
                else
                {
                    BAddText = "Dodaj nowy";
                    BEditText = "Edytuj";
                }

            } }

        private string tbDate = tbDateInstall;
        public string TbDate
        {
            get => tbDate; 
            set {
                tbDate = value;
                NotifyPropertyChanged("TbDate");
            }
        }
        public string BAddText { get => bAddText; set { bAddText = value; NotifyPropertyChanged("BAddText"); } }
        public string BEditText { get => bEditText; set { bEditText = value; NotifyPropertyChanged("BEditText"); } }

        private ObservableCollection<TItem> itemsCollection;
        public ObservableCollection<TItem> ItemsCollection { get
            { if (itemsCollection == null) { itemsCollection = new ObservableCollection<TItem>(); }
                return itemsCollection; }
            set { itemsCollection = value; NotifyPropertyChanged("ItemsCollection"); } }



        public CItem()
        {
            InitializeComponent();
            DataContext = this;
            ItemsCollection.CollectionChanged += (sender, args) => {
                NotifyPropertyChanged("ItemsCollection");
                dgItemData.Items.Refresh();

            };

        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (!ItemActive.IsComplete())
                NotifyStatusChanged(Properties.Resources.ItemDescAddWarning);
            else
            {
                if (!EditMode)
                {
                        DbManager.AddNewData(ItemActive);
                        ItemsCollection.Add(ItemActive);
                }
                else
                {
                    DbManager.UpdateData(ItemActive);
                    ItemsCollection[ItemsCollection.IndexOf(ItemSelected)].Assign(ItemActive);
                    EditMode = false;
                }
            }
        }

        private void RemoveItem(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(ItemSelected.PartNumber))
                NotifyStatusChanged(Properties.Resources.ItemDescRemoveWarning);
            else
            {
                if (EditMode)
                    EditMode = false;
                DbManager.RemoveData(ItemSelected);
                var idx = ItemsCollection.IndexOf(ItemSelected);
                ItemsCollection.RemoveAt(idx);
            }
        }


        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (EditMode)
            {
                ItemActive = new TItem();
                EditMode = false;
            }
            else
            {
                EditMode = true;
                ItemActive = ItemSelected;
            }
            
        }

        private void SearchItem(object sender, RoutedEventArgs e)
        {

            if (ItemActive.IsNullOrEmpty())
            {
                NotifyStatusChanged(Properties.Resources.CAircraftSearchNoDataLoadAll);
                LoadAllItems(sender, e);
            }
            else
            {
                var results = DbManager.ReadDatabase(ItemActive);
                if (results != null)
                    foreach (IDbTable item in results) { ItemsCollection.Add((TItem)item); }
                else
                    NotifyStatusChanged(Properties.Resources.SearchNotFoundItemDesc);
               
            }
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            if (EditMode)
                EditMode = false;
            ItemActive = new TItem();
            ItemsCollection = new ObservableCollection<TItem>();
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
        /// <summary>
        /// do poprawy
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void LoadAllItems(object sender, RoutedEventArgs e)
        {
            var results = DbManager.ReadWholeDatabase(new TItem() { ItemDefect = false } );
            if (results != null)
                foreach (IDbTable item in results) { ItemsCollection.Add((TItem)item); }
            else
                NotifyStatusChanged(Properties.Resources.SearchNotFoundItem);

            results = DbManager.ReadWholeDatabase(new TItem() { ItemDefect = true });
            if (results != null)
                foreach (IDbTable item in results) { ItemsCollection.Add((TItem)item); }
            else
                NotifyStatusChanged(Properties.Resources.SearchNotFoundItemDefect);


        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
                TbDate = tbDateRemoval;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            TbDate = tbDateInstall;
        }

        private void dgItemData_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ItemActive = ItemSelected;
        }
    }
}
