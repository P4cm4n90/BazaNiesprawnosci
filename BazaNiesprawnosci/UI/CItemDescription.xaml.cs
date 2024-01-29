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
    public partial class CItemDescription : UserControl, INotifyPropertyChanged
    {
        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public event DataBaseStatusHandler OnStatusChange;
        public event PropertyChangedEventHandler PropertyChanged;



        private TItemDescription itemSelected;
        private TItemDescription itemActive;

        private string bAddText = "Dodaj nowy";
        private string bEditText= "Edytuj";

        private bool enabledTbSub2 = false;
        private bool enabledTbSub3 = false;

        public bool EnabledTbSub2 { get => enabledTbSub2; set { enabledTbSub2 = value; NotifyPropertyChanged("EnabledTbSub2"); } }
        public bool EnabledTbSub3 { get => enabledTbSub3; set { enabledTbSub3 = value; NotifyPropertyChanged("EnabledTbSub3"); } }

        public TItemDescription ItemActive { get => 
                itemActive ?? (itemActive = new TItemDescription());
            set { itemActive = value; NotifyPropertyChanged("ItemActive"); } }
        public TItemDescription ItemSelected { get { return itemSelected ?? (itemSelected = new TItemDescription()); } set { itemSelected = value; NotifyPropertyChanged("ItemSelected"); } }

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

        public string BAddText { get => bAddText; set { bAddText = value; NotifyPropertyChanged("BAddText"); } }
        public string BEditText { get => bEditText; set { bEditText = value; NotifyPropertyChanged("BEditText"); } }

        private ObservableCollection<TItemDescription> itemsCollection;
        public ObservableCollection<TItemDescription> ItemsCollection { get
            { if (itemsCollection == null) { itemsCollection = new ObservableCollection<TItemDescription>(); }
                return itemsCollection; }
            set { itemsCollection = value; NotifyPropertyChanged("ItemsCollection"); } }



        public CItemDescription()
        {
            InitializeComponent();
            DataContext = this;
            ItemsCollection.CollectionChanged += (sender, args) => {
                NotifyPropertyChanged("ItemsCollection");
                dgItemData.Items.Refresh();

            };

        }

        private void dgItemData_DoubleClick(object sender, MouseEventArgs eargs)
        {
            ItemActive = ItemSelected;
        }

        private void AddItem(object sender, RoutedEventArgs e)
        {
            if (!ItemActive.IsComplete())
                NotifyStatusChanged(Properties.Resources.ItemDescAddWarning);
            else
            {
                if (!EditMode)
                {
                    if (ItemActive.IsComplete())
                    {
                        DbManager.AddNewData(ItemActive);
                        ItemsCollection.Add(ItemActive);
                    }
                    else
                    {
                        NotifyStatusChanged(Properties.Resources.ItemDescAddWarning);
                    }
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
            if (String.IsNullOrEmpty(ItemActive.PartNumber))
            {
                NotifyStatusChanged(Properties.Resources.ItemDescRemoveWarning);

            }
            else
            {
                if (EditMode)
                    EditMode = false;
                else
                    if (DbManager.RemoveData(ItemActive) == 0)
                    NotifyStatusChanged(Properties.Resources.ItemDescRemoveOk);
               


                //DbManager.AddNewData(ItemActive, null, null);
            }
        }


        private void EditItem(object sender, RoutedEventArgs e)
        {
            if (EditMode)
            {
                ItemActive = new TItemDescription();
                EditMode = false;
            }
            else
            {
                EditMode = true;
                ItemActive.Assign(ItemSelected);
            }
            
        }

        private void SearchItem(object sender, RoutedEventArgs e)
        {

            if (!ItemActive.IsMinimal())
            {
                NotifyStatusChanged(Properties.Resources.CAircraftSearchNoDataLoadAll);
                LoadAllItems(sender, e);
            }
            else
            {
                var results = DbManager.ReadDatabase(ItemActive);
                if (results != null)
                    foreach (IDbTable item in results) { ItemsCollection.Add((TItemDescription)item); }
                else
                    NotifyStatusChanged(Properties.Resources.SearchNotFoundItemDesc);
               
            }
        }

        private void ClearList(object sender, RoutedEventArgs e)
        {
            if (EditMode)
                EditMode = false;
            ItemActive = new TItemDescription();
            ItemsCollection = new ObservableCollection<TItemDescription>();
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
        private void SubsChanged(object sender, TextChangedEventArgs e)
        {
                if(String.IsNullOrEmpty(ItemActive.SubPartNumbers[0]))
                {
                    if (String.IsNullOrEmpty(ItemActive.SubPartNumbers[1]))
                    {
                        if (String.IsNullOrEmpty(ItemActive.SubPartNumbers[2]))
                        {
                            EnabledTbSub2 = false;
                            EnabledTbSub3 = false;
                        }
                        else
                        {
                            EnabledTbSub3 = false;
                            EnabledTbSub2 = true;
                        }
                    }
                    else
                    {
                        EnabledTbSub3 = false;
                        EnabledTbSub2 = true;
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(ItemActive.SubPartNumbers[1]))
                    {
                            EnabledTbSub3 = false;
                    }
                    else
                    {
                        EnabledTbSub3 = true;
                    }
                }
        }

        private void LoadAllItems(object sender, RoutedEventArgs e)
        {
            var results = DbManager.ReadWholeDatabase(new TItemDescription());
            if (results != null)
                foreach (IDbTable item in results) { ItemsCollection.Add((TItemDescription)item); }
            else
                NotifyStatusChanged(Properties.Resources.SearchAllNotFoundItemDesc);

                }
        }
}
