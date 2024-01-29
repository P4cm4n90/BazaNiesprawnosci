using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Globalization;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
   
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private DateTime dateComparator = new DateTime(1990,1,1);
        private int windowHeight = ((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height * 0.8));
        private int windowWidth = ((int)(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 0.8));
 
        private int acInfoId = 0;
        private bool schDateStartTo = false;
        private bool schDateEndTo = false;
        private bool addDateEnd = false;
        private bool fixEntryMode = false;
        private int selectedIndex = -1;

        public bool FixEntryMode
        {
            get { return fixEntryMode; }
            set { 
                fixEntryMode = value; 
                if(fixEntryMode)
                {
                    bAddEntry.Content = "Popraw wpis";
                    bChangeEntry.Content = "Anuluj";
                    AppendNewInfo(Properties.Resources.FixModeEnabled);
                }
                else
                {
                    bAddEntry.Content = "Dodaj wpis";
                    bChangeEntry.Content = "Zmień wpis";
                    AppendNewInfo(Properties.Resources.FixModeDisabled);
                }
            }
        }

        #region UI BINDING

        private string progressInfo = "";

        private TDefect defectAdd = new TDefect();
        private TDefect defectSelected = new TDefect();
        private SearchDefect defectSearch = new SearchDefect();

        public string ProgressInfo
        {
            get
            {
                return progressInfo;
            }
            set
            {
                progressInfo = value;
                NotifyPropertyChanged("ProgressInfo");
            }
        }
        public TDefect DefectSelected
        {
            get
            {
                return defectSelected;
            }
            set
            {
                defectSelected = (TDefect)value;
                NotifyPropertyChanged("DefectSelected");
            }
        }
        public TDefect DefectAdd {
            get
            {
                return defectAdd;
            }
            set
            {
            
                defectAdd = value;
                NotifyPropertyChanged("DefectAdd");
            }

        }

        public SearchDefect DefectSearch
        {
            get
            {
                return defectSearch;
            }
            set
            {
                defectSearch = value;
               NotifyPropertyChanged("DefectSearch");
            }
        }



          
    
            /// <summary>
            /// Date of defect to be added
            /// </summary>
           

        #endregion
        #region HANDLING CONSOLE ACTION

            private int infoPostion = 0;
            private int InfoPosition
            {
                get { return infoPostion; }
                set { infoPostion = value; InfoTotalPostition = value; }
            }

            private int InfoTotalPostition = 0;

            private Queue<string> InfoQueue;

            /// <summary>
            /// Timer that count time between click on cDateSearch
            /// </summary>
        #endregion
        #region HANDLING DEFECT ACTION
            private ObservableCollection<TDefect> defectToAdd;
            private DList DefectsFound;
            public ObservableCollection<TDefect> DefectToAdd
            {
                get
                {
                    return defectToAdd;
                }
                set
                {
                    defectToAdd = value;
                    NotifyPropertyChanged("DefectToAdd");
                }
            }

        public bool SchDateStartTo { get => schDateStartTo; set => schDateStartTo = value; }




        #endregion
        #region UI DATA 
        private WResults SearchResults;

        #endregion

        public MainWindow()
        {

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd.MM.yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            /*
                  if ((AcInfoList = AircraftInfoList.DeSerialize()) == null)
                      AcInfoList = new AircraftInfoList();
                  else
                      Settings.SynchronizeSettings = true;*/

            InitializeComponent();
            uWindow.Height = windowHeight;
            uWindow.Width = windowWidth;
            SearchResults = new WResults();
            SearchResults.OnDefectChange += FixEntryEvent;
            SearchResults.OnDefectChange += DeleteEntryEvent;
            Initialization();
            InitializationDatabase();
            this.DataContext = this;
            tbProgressInfo.DataContext = this;

        }

        /// <summary>
        /// EXTENSION OF THE CONSTRUCTOR
        /// </summary>
        private void Initialization()
        {
            ///do zmiany w zaleznosci od zmiany parametrow
            DefectToAdd = new ObservableCollection<TDefect>();
            dgTable.ItemsSource = DefectToAdd;
            InfoQueue = new Queue<string>(50);
            

            DefectToAdd.CollectionChanged += (sender, args) => { NotifyPropertyChanged("DefectToAdd");
            dgTable.Items.Refresh();
            
            };
            DbManager.OnStatusChange += DataBaseInfo;
            SearchResults.OnStatusChange += DataBaseInfo;
            ConsoleItemDescription.OnStatusChange += DataBaseInfo;
            ConsoleAircraft.OnStatusChange += DataBaseInfo;
            ConsoleWorker.OnStatusChange += DataBaseInfo;
            ConsoleItem.OnStatusChange += DataBaseInfo;
            DbExcelWrap.OnProgressChange += AppendProgress;
            tbSearchSolution.Text = Properties.Resources.SearchSolutionStd;
            tbSearchSymptoms.Text = Properties.Resources.SearchSymptomsStd;
        }

        public void InitializationDatabase()
        {
            MainSettings.Default.Upgrade();
            if(!String.IsNullOrEmpty(MainSettings.Default.DatabaseFilepath))
            {
                DbManager.InitializeDatabase(false, false);
                SetReady();
            }
            

        }

        private void AppendProgress(object sender, ProgressEventArgs eargs)
        {
            int progress = (eargs.ActualValue / eargs.FinalValue);
            ProgressInfo = $"{eargs.Info} : {progress}";
        }

        private void AppendNewInfo(string info)
        {
            if (InfoQueue.Count >= InfoPosition + 1)
            {
                try
                {
                    InfoQueue.Dequeue();
                    InfoTotalPostition++;
                    InfoQueue.Enqueue(InfoTotalPostition + ": " + info);
                    rtbConsole.Document.Blocks.Clear();
                    foreach (string s in InfoQueue)
                    {
                        rtbConsole.AppendText(s + Environment.NewLine);
                    }
                }
                catch(InvalidOperationException ex)
                {
                    return;
                }
            }
            else
            {
   
                InfoQueue.Enqueue(InfoPosition+": "+info);
                InfoPosition++;
                rtbConsole.Document.Blocks.Clear();
                foreach(string s in InfoQueue)
                {
                    rtbConsole.AppendText(s + Environment.NewLine);
                }
            }
            

        }
        private void SetReady()
        {
            gAdd.IsEnabled = true;
            gSearch.IsEnabled = true;
            bLoadDataBase.IsEnabled = true;
            bSaveDataBase.IsEnabled = true;
        }

        #region CUSTOM EVENTS
        private void DataBaseInfo(object sender, TextEventArgs eargs)
        {
            AppendNewInfo(eargs.Info);
        }
        private void FixEntryEvent(object sender,DefectChange dea)
        {
            if(dea.DefType == DefectChangeType.Fix)
            {
                this.Activate();
                gAdd.Focus();
                gAdd.BringIntoView();
                
               foreach(TDefect def in DefectsFound)
               {
                   if(def.Id == dea.Id)
                   {
                       DefectAdd = def;
                       break;
                   }
               }
               FixEntryMode = true;
           }
        }

        private void DeleteEntryEvent(object sender, DefectChange dea)
        {
            if (dea.DefType == DefectChangeType.Delete)
            {
                this.Activate();
                gAdd.Focus();
                TDefect defRemove = SearchResults.data.First<TDefect>(x => x.Id == dea.Id);
                if(DbManager.RemoveData(defRemove) == 0)
                    SearchResults.data.Remove(defRemove);

            }
            
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion

        #region STATIC

        public static bool StrEqual(string a,string b)
        {
            if (string.Compare(a, b) == 0)
                return true;
            else
                return false;
        }

        private static DateTime DaysToDate(int days)
        {
            int years = Math.Abs(days / 1461) + Math.Abs((days % 1461) / 365);
            return new DateTime(years, 1, 1).AddDays((days % years) - 1);

        }

        private static int DateToDays(DateTime date)
        {
            int days = Math.Abs(date.Year / 4) * 1461 + Math.Abs((date.Year % 4)) * 365 + date.DayOfYear;
            return days;
        }

        #endregion

        #region Handling Window Events

        // BUTTONS CLICK 

        private void bReset_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void bSearch_Click(object sender, RoutedEventArgs e)
        {

            throw new NotImplementedException();

        }
        private void bCreateDataBase_Click(object sender, RoutedEventArgs e)
        {
            var tempBox = new WPasswdBox();
          //  tempBox.DataContext = tempBox;
            tempBox.OnStatusChange += DataBaseInfo;
            tempBox.Closing += delegate
            {
                if (tempBox.result)
                {
                    if (DbManager.CreateDatabase())
                    {
                        AppendNewInfo(Properties.Resources.DataBaseCreateOk);
                        SetReady();
                        
                    }
                }
                else
                {
                    AppendNewInfo(Properties.Resources.DateBasePasswordError);
                }
                tempBox.OnStatusChange -= DataBaseInfo;
            };
            tempBox.Show();
        }
        private void bChooseDataBase_Click(object sender, RoutedEventArgs e)
        {
            if (DbManager.InitializeDatabase(false,true))
                SetReady();
            else
                AppendNewInfo(Properties.Resources.DataBaseInitializeError);


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bSaveDataBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DefectToAdd.Count > 0)
                {
                    try
                    {
                        foreach (TDefect def in DefectToAdd)
                        {
                            throw new NotImplementedException();
                        }
                        DefectToAdd.Clear();
                    }
                    catch
                    {
                        AppendNewInfo("Błąd. Nie można zapisać defektu do bazy danych");
                    }
                }
                else
                {
                    AppendNewInfo(Properties.Resources.DataBaseSaveError);
                }
            }
            catch(Exception ex)
            {
                AppendNewInfo("Nie uzupełniono wymaganych pól");
            }

        }
        /// <summary>
        /// Wczytuje cala baze danych
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bLoadDataBase_Click(object sender, RoutedEventArgs e)
        {
            var temp = DbManager.ReadWholeDatabase(new TDefect());
            if (temp != null) { if (temp.Count > 0)
            {
                    var collection = new ObservableCollection<TDefect>();
                    foreach (IDbTable tab in temp)
                    {
                        collection.Add((TDefect)tab);
                    }
                    SearchResults.ShowResults(collection);
                }
                else
                { AppendNewInfo(Properties.Resources.SearchAllNotFoundDefect); }
            }
            else { AppendNewInfo(Properties.Resources.SearchAllNotFoundDefect); }

            
        }

        private void bSearch_Click_1(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void bAddEntry_Click(object sender, RoutedEventArgs e)
        {
            if (!FixEntryMode)
            {
                int res = DbManager.AddNewData(DefectAdd);
                if (res >= 0)
                {
                    DefectAdd.Id = res;
                    DefectToAdd.Add(DefectAdd.Clone());
                    if (MainSettings.Default.defectAddClearAfterSaving)
                        defectAdd = new TDefect();
                }
                if (res == -2 || res == -1)
                {
                    AppendNewInfo("Jakis bład dodawania");
                }
            }
            else
            {
                int res = DbManager.UpdateData(DefectAdd);
                if (res == -2 || res == -1)
                {
                    AppendNewInfo("Jakis bład dodawania");
                }
                else
                {
                    AppendNewInfo(Properties.Resources.DefectSuccChanged + $"{DefectAdd.Id}");
                }
            }
        }

        private void bResetList_Click(object sender, RoutedEventArgs e)
        {
            DefectToAdd.Clear();
        }

        private void bChangeEntry_Click(object sender, RoutedEventArgs e)
        {
            FixEntryMode = !FixEntryMode;
            if (FixEntryMode)
                DefectAdd = DefectSelected;
            else
                if (MainSettings.Default.defectClearAfterAbortEdit)
                DefectAdd = new TDefect();
            
        }

        private void bResetEntry_Click(object sender, RoutedEventArgs e)
        {
            DefectAdd = new TDefect();
        }

        private void bSaveSettings_Click(object sender, RoutedEventArgs e)
        {
            //temporary
            // AcInfoList.Serialize();
            // Text="{Binding Source={StaticResource MainSettings.Default}, Path=DatabaseFilepath}" 
        }

        // TEXTBOX EVENT HANDLING



     
        /// <summary>
        /// ////////////////////////
        /// /
        /// 
        /// 
        /// 
        /// DOKONCZYC TA METODA
        /// 
        /// 
        /// 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbDateSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
               // DateTime temp;
                if (sender.Equals(tbDateStartSearchFrom))
                {
                    if ((tbDateStartSearchFrom.Text.Length < 10) ||
                     ((tbDateStartSearchFrom.Text.Length < 9) && (tbDateStartSearchFrom.Text.ToCharArray(0, 2)[1] != '.')))
                    {
                        if (tbDateStartSearchFrom.Background != Brushes.White)
                            tbDateStartSearchFrom.Background = Brushes.White;
                    }
                    else
                    {
                        if (DateTime.TryParse(tbDateStartSearchFrom.Text, out dateComparator))
                        {
                            if (dateComparator.Year < 2000)
                            {
                                tbDateStartSearchFrom.Background = Brushes.LightPink;
                            }
                            else
                            {
                                /// check if start date is bigger than end date
                                if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartInt > DefectSearch.DateEndInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightPink;
                                    tbDateEndSearchFrom.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateEndHigher");
                                }
                                else if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartInt < DefectSearch.DateEndInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                    tbDateEndSearchFrom.Background = Brushes.LightGreen;
                                }
                                else if ((DefectSearch.DateEndInt == 0) && (DefectSearch.DateStartInt < DefectSearch.DateEndInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                }

                                 if(DefectSearch.DateStartToInt != 0 && (DefectSearch.DateStartInt > DefectSearch.DateStartToInt))
                                 {
                                     tbDateStartSearchFrom.Background = Brushes.LightPink;
                                     tbDateStartSearchTo.Background = Brushes.LightPink;
                                     AppendNewInfo("Properties.Resources.DateToHigher");
                                 }
                                 else if (DefectSearch.DateStartToInt != 0 && (DefectSearch.DateStartInt < DefectSearch.DateStartToInt))
                                 {
                                     tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                     tbDateStartSearchTo.Background = Brushes.LightGreen;
                                 }
                                 else if (DefectSearch.DateStartToInt == 0 && (DefectSearch.DateStartInt < DefectSearch.DateStartToInt))
                                 {
                                     tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                 }
                           
                            }
                        }
                        else
                        {
                            tbDateStartSearchFrom.Background = Brushes.LightPink;
                        }
                    }
                }
                else if (sender.Equals(tbDateStartSearchTo))
                {
                    if ((tbDateStartSearchTo.Text.Length < 10)  ||
                     ((tbDateStartSearchTo.Text.Length < 9) && (tbDateStartSearchTo.Text.ToCharArray(0, 2)[1] != '.')))
                    {
                        if (tbDateStartSearchTo.Background != Brushes.White)
                            tbDateStartSearchTo.Background = Brushes.White;
                    }
                    else
                    {

                        if (DateTime.TryParse(tbDateStartSearchTo.Text, out dateComparator))
                        {
                            if (dateComparator.Year < 2000)
                            {
                                tbDateStartSearchTo.Background = Brushes.LightPink;
                            }
                            else
                            {
                                if ((DefectSearch.DateEndToInt != 0) && (DefectSearch.DateStartToInt > DefectSearch.DateEndToInt))
                                {
                                    tbDateStartSearchTo.Background = Brushes.LightPink;
                                    tbDateEndSearchTo.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateEndHigher");
                                }
                                else if ((DefectSearch.DateEndToInt != 0) && (DefectSearch.DateStartToInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateStartSearchTo.Background = Brushes.LightGreen;
                                    tbDateEndSearchTo.Background = Brushes.LightGreen;
                                }
                                else if ((DefectSearch.DateEndToInt == 0) && (DefectSearch.DateStartToInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateStartSearchTo.Background = Brushes.LightGreen;
                                }
                                if (DefectSearch.DateStartToInt != 0 && (DefectSearch.DateStartInt > DefectSearch.DateStartToInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightPink;
                                    tbDateStartSearchTo.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateToHigher");
                                }
                                else if (DefectSearch.DateStartToInt != 0 && (DefectSearch.DateStartInt < DefectSearch.DateStartToInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                    tbDateStartSearchTo.Background = Brushes.LightGreen;
                                }


                            }
                        }
                        else
                        {
                            tbDateStartSearchTo.Background = Brushes.LightPink;
                        }
                    }
                }
                else if(sender.Equals(tbDateEndSearchFrom))
                {
                    if ((tbDateEndSearchFrom.Text.Length < 10) ||
                     ((tbDateEndSearchFrom.Text.Length < 9) && (tbDateStartSearchFrom.Text.ToCharArray(0, 2)[1] != '.')))
                    {
                        if (tbDateEndSearchFrom.Background != Brushes.White)
                            tbDateEndSearchFrom.Background = Brushes.White;
                    }
                    else
                    {
                        if (DateTime.TryParse(tbDateEndSearchFrom.Text, out dateComparator))
                        {
                            if (dateComparator.Year < 2000)
                            {
                                tbDateEndSearchFrom.Background = Brushes.LightPink;
                            }
                            else
                            {
                                if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartInt > DefectSearch.DateEndInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightPink;
                                    tbDateEndSearchFrom.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateEndHigher");
                                }
                                else if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartInt < DefectSearch.DateEndInt))
                                {
                                    tbDateStartSearchFrom.Background = Brushes.LightGreen;
                                    tbDateEndSearchFrom.Background = Brushes.LightGreen;
                                }


                                if (DefectSearch.DateEndToInt != 0 && (DefectSearch.DateEndInt > DefectSearch.DateEndToInt))
                                {
                                    tbDateEndSearchFrom.Background = Brushes.LightPink;
                                    tbDateEndSearchTo.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateToHigher");
                                }
                                else if (DefectSearch.DateEndToInt != 0 && (DefectSearch.DateEndInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateEndSearchFrom.Background = Brushes.LightGreen;
                                    tbDateEndSearchTo.Background = Brushes.LightGreen;
                                }
                                else if (DefectSearch.DateEndToInt == 0 && (DefectSearch.DateEndInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateEndSearchFrom.Background = Brushes.LightGreen;
                                }
                                //DateEnSearchFrom = dateComparator;

                            }
                        }
                        else
                        {
                            tbDateEndSearchFrom.Background = Brushes.LightPink;
                        }
                    }
                }
                else if(sender.Equals(tbDateEndSearchTo))
                {
                    if (tbDateEndSearchTo.Text.Length < 10 || 
                     ((tbDateEndSearchTo.Text.Length < 9) && (tbDateStartSearchTo.Text.ToCharArray(0,2)[1] != '.')))
                    {
                        if (tbDateEndSearchTo.Background != Brushes.White)
                            tbDateEndSearchTo.Background = Brushes.White;
                    }
                    else
                    {
                        if (DateTime.TryParse(tbDateEndSearchTo.Text, out dateComparator))
                        {
                            if (dateComparator.Year < 2000)
                            {
                                tbDateEndSearchTo.Background = Brushes.LightPink;
                            }
                            else
                            {
                                if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartToInt > DefectSearch.DateEndToInt))
                                {
                                    tbDateStartSearchTo.Background = Brushes.LightPink;
                                    tbDateEndSearchTo.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateEndHigher");
                                }
                                else if ((DefectSearch.DateEndInt != 0) && (DefectSearch.DateStartToInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateStartSearchTo.Background = Brushes.LightGreen;
                                    tbDateEndSearchTo.Background = Brushes.LightGreen;
                                }


                                if (DefectSearch.DateEndToInt != 0 && (DefectSearch.DateEndInt > DefectSearch.DateEndToInt))
                                {
                                    tbDateEndSearchFrom.Background = Brushes.LightPink;
                                    tbDateEndSearchTo.Background = Brushes.LightPink;
                                    AppendNewInfo("Properties.Resources.DateToHigher");
                                }
                                else if (DefectSearch.DateEndToInt != 0 && (DefectSearch.DateEndInt < DefectSearch.DateEndToInt))
                                {
                                    tbDateEndSearchFrom.Background = Brushes.LightGreen;
                                    tbDateEndSearchTo.Background = Brushes.LightGreen;
                                }

                            }
                        }
                        else
                        {
                            tbDateEndSearchTo.Background = Brushes.LightPink;
                        }
                    }
                }
            
        }



        private void tbDateSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if(sender.Equals(tbDateStartSearchFrom))
            {
                SchDateStartTo = false;
            }
            else if(sender.Equals(tbDateStartSearchTo))
            {
                SchDateStartTo = true;
            }
            else if(sender.Equals(tbDateEndSearchFrom))
            {
                schDateEndTo = false;
            }
            else if(sender.Equals(tbDateEndSearchTo))
            {
                schDateEndTo = true;
            }
        }

        // LISTBOX EVENT HANDLING

      
      

        // WINDOW EVENT HANDLING

        private void uWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
                    
        }

        protected override void OnClosed(EventArgs e)
        {
           /* if(Settings.SynchronizeSettings)
            {
                AcInfoList.Serialize();
            }*/
            base.OnClosed(e);

            Application.Current.Shutdown();
        }


        private void cbSpecType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

       #endregion



        private void dgTable_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var displayName = GetPropertyDisplayName(e.PropertyDescriptor);
            
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
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

        private void uWindow_Closing(object sender, CancelEventArgs e)
        {
            // AcInfoList.Serialize();
            MainSettings.Default.Save();
        }

       

        private void dgTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if((selectedIndex = dgTable.SelectedIndex) != -1)
            {
                DefectAdd = (TDefect)DefectSelected;
                dgTable.SelectedIndex = -1;
            }
        }

        private void lbAircraftSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void bGetExample_Click(object sender, RoutedEventArgs e)
        {
            DefectAdd = DefectAdd.GetExample();
        }

        private void bCopyDataFromExcelClick(object sender, RoutedEventArgs e)
        {
            var listItemDesc = DbExcelWrap.OpenExcelFile().Result;
            if (listItemDesc != null)
            {
                if(listItemDesc.Count > 0)
                {
                    foreach(TItemDescription item in listItemDesc)
                    {
                        DbManager.AddNewData(item);
                           
                        
                    }                   
                }
            }
        }
    }

   

}
   

