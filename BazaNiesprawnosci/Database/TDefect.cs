using BazaNiesprawnosci.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BazaNiesprawnosci
{ /// <summary>
/// DO POPRAWY
/// </summary>
    public class TDefect : INotifyPropertyChanged, IDbTable
    {
       // private static string[] minimalList = {""}

        private static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.primary_key_no_insert,
            }},
            {"FlightHours",new DbObject()
            {
                DBName = "FlightHours",
                DBRootName = "FlightHours",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,

            }},
            {"WorkHours",new DbObject()
            {
                DBName = "WorkHours",
                DBRootName = "WorkHours",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,

            }},
            {"AircraftId",new DbObject()
            {
                DBName = "AircraftId",
                DBRootName = "TailNumber",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TAircraft

            }},
            {"Symptoms",new DbObject()
            {
                DBName = "Symptoms",
                DBRootName = "Symptoms",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                IsNullable = false

            }},
            {"SolutionProcedure",new DbObject()
            {
                DBName = "SolutionProcedure",
                DBRootName = "Name",
                DBTableRootName = "TSolutionProcedure",
                KeyType = KeyTypes.reference,
                IsNullable = false

            }},
            {"Solution",new DbObject()
            {
                DBName = "Solution",
                DBRootName = "Solution",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                IsNullable = false
            }},
            {"Circumstances",new DbObject()
            {
                DBName = "Circumstances",
                DBRootName = "Name",
                DBTableRootName = "TCircumstances",
                KeyType = KeyTypes.reference,
                IsNullable = false

            }},
            {"Reason",new DbObject()
            {
                DBName = "Reason",
                DBRootName = "Reason",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                IsNullable = false

            }},
            {"DateStartInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateStartInt",
                DBRootName = "DateStartInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,
                IsNullable = false

            }},
            {"DateEndInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateEndInt",
                DBRootName = "DateEndInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,
                IsNullable = false

            }},
            {"Classification",new DbObject()
            {
                DBName = "Classification",
                DBRootName = "Name",
                DBTableRootName = "TClassification",
                KeyType = KeyTypes.reference,
                
            }},
            {"GroundStopReason",new DbObject()
            {
                DBName = "GroundStopReason",
                DBRootName = "Name",
                DBTableRootName = "TGroundStopReason",
                KeyType = KeyTypes.reference,
                
            }},
            {"WorkerCheckId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerCheckId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TWorker

            }},
            {"WorkerFixId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerFixId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TWorker

            }},
            {"WorkerDetectId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerDetectId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TWorker
            }},
            {"ItemProcedure",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemProcedure",
                DBRootName = "Name",
                DBTableRootName = "TItemProcedure",
                KeyType = KeyTypes.reference,
                
            }},
            {"EffectCompletingTask",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "EffectCompletingTask",
                DBRootName = "Name",
                DBTableRootName = "TEffectCompletingTask",
                KeyType = KeyTypes.reference,
                
            }},
            {"EffectFlightSafety",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "EffectFlightSafety",
                DBRootName = "Name",
                DBTableRootName = "TEffectFlightSafety",
                KeyType = KeyTypes.reference,
                
            }},
            {"ItemNewId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemNewId",
                DBRootName = "Id",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TItemNew

            }},
            {"ItemDefectId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemDefectId",
                DBRootName = "Id",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TItemDefect
                
            }},
            {"ItemNew",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemNewId",
                DBRootName = "Id",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.table_value,

            }},
            {"ItemDefect",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemDefectId",
                DBRootName = "Id",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.table_value,
            }},
            {"WorkerFix",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerFixId",
                DBRootName = "Id",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.table_value,

            }},
            {"WorkerDetect",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerDetectId",
                DBRootName = "Id",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.table_value,

            }},
            {"WorkerCheck",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerCheckId",
                DBRootName = "Id",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.table_value,

            }},
            {"Aircraft",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "AircraftId",
                DBRootName = "Id",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.table_value,

            }},
        };

        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"FlightHours",0},
            {"WorkHours",0},
            {"AircraftId",""},
            {"Symptoms",""},
            {"Solution",""},
            {"Circumstances",""},
            {"Reason",""},
            {"DateStartInt",0},
            {"DateEndInt",0},
            {"Classification",""},
            {"GroundStopReason",""},
            {"WorkerCheckId",""},
            {"WorkerDetectId",""},
            {"WorkerFixId",""},
            {"ItemProcedure",""},
            {"SolutionProcedure",""},
            {"EffectCompletingTask",""},
            {"EffectFlightSafety",""},
            {"ItemNewId",0},
            {"ItemDefectId",0},
            {"Aircraft",new TAircraft() },
            {"ItemNew",new TItem() },
            {"ItemDefect",new TItem() },
            {"WorkerFix",new TWorker() },
            {"WorkerCheck",new TWorker() },
            {"WorkerDetect",new TWorker() },

        };

        private static readonly RecordTypes recordType = RecordTypes.TDefect;

        private PropertyDictionary<string, object> PropertyExample = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"FlightHours",25},
            {"WorkHours",2},
            {"AircraftId","012"},
            {"Symptoms","Coś się działo złego"},
            {"Solution","Jakoś to naprawiono, zajrzano tu i tam"},
            {"Circumstances","Obsługa przedlotowa"},
            {"Reason","Zła pogoda"},
            {"DateStartInt",731800},
            {"DateEndInt",731800},
            {"Classification","It"},
            {"GroundStopReason","Organizacyjna"},
            {"WorkerCheckId","Pajszczyk"},
            {"WorkerDetectId","Pajszczyk"},
            {"WorkerFixId","Pajszczyk"},
            {"ItemProcedure","Naprawa"},
            {"SolutionProcedure","Inne"},
            {"EffectCompletingTask","Przerwanie startu"},
            {"EffectFlightSafety","Incydent"},
            {"ItemNewId",0},
            {"ItemDefectId",0},
            {"Aircraft",new TAircraft(){ Id=2,TailNumber="012" } },
            {"ItemNew",new TItem() },
            {"ItemDefect",new TItem() },
            {"WorkerFix",new TWorker() { Id=1,Surname="Pajszczyk"} },
            {"WorkerCheck",new TWorker(){ Id=1,Surname="Pajszczyk"}  },
            {"WorkerDetect",new TWorker(){ Id=1,Surname="Pajszczyk"}  },
        };

        private PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"FlightHours",0},
            {"WorkHours",0},
            {"AircraftId",""},
            {"Symptoms",""},
            {"Solution",""},
            {"Circumstances",""},
            {"Reason",""},
            {"DateStartInt",0},
            {"DateEndInt",0},
            {"Classification",""},
            {"GroundStopReason",""},
            {"WorkerCheckId",""},
            {"WorkerDetectId",""},
            {"WorkerFixId",""},
            {"ItemProcedure",""},
            {"SolutionProcedure",""},
            {"EffectCompletingTask",""},
            {"EffectFlightSafety",""},
            {"ItemNewId",0},
            {"ItemDefectId",0},
            {"Aircraft",new TAircraft() },
            {"ItemNew",new TItem() },
            {"ItemDefect",new TItem() },
            {"WorkerFix",new TWorker() },
            {"WorkerCheck",new TWorker() },
            {"WorkerDetect",new TWorker() },

        };

        public event PropertyChangedEventHandler PropertyChanged;

        #region interface_idefecttable

        public object this[string index]
        {
            get
            {
                return Property.ContainsKey(index) ? Property[index] : null;
            }
            set
            {
                if (Property.ContainsKey(index))
                    Property[index] = value;
                else
                    throw new KeyNotFoundException(index);
            }
        }
        public virtual Dictionary<string, DbObject> DbProperty { get => dbProperty; }
        public virtual RecordTypes RecordType { get => recordType; }
        public virtual string TableName { get => RecordType.ToString(); }

        #endregion

        private bool isLoaded = false;

        private string _dateStart = "";
        private string _dateEnd = "";

        private DateTime dateStart;
        private DateTime dateEnd;

        #region properties
        public int Id
        {
            get
            {
                return (int)Property["Id"];
            }
            set
            {
                Property["Id"] = value;
            }
        }

        [DisplayName("Samolot")]
        public string AircraftId
        {
            get
            {
                return ((TAircraft)Property["Aircraft"]).TailNumber;
            }
            set
            {
                Property["AircraftId"] = value;
                if (((TAircraft)Property["Aircraft"]).TailNumber != value)
                {
                    //["Aircraft"] = new TAircraft() { TailNumber = value };
                    if (MainSettings.Default.LoadAircraftDataOnChange)
                    {
                        var temp = new TAircraft() { TailNumber = value };
                        ///database wczytaj aircrafta i go dodaj 
                    }
                    else
                    {
                        ((TAircraft)Property["Aircraft"]).Reset();
                        ((TAircraft)Property["Aircraft"]).TailNumber = value;
                    }
                }
            }
        }

        [DisplayName("Nalot")]
        public int FlightHours
        {
            get
            {
                return (int)Property["FlightHours"];
            }
            set
            {
                Property["FlightHours"] = value;
               // NotifyPropertyChanged("FlightHours");
            }
        }

        [DisplayName("Objawy niesprawności")]
        public string Symptoms
        {
            get
            {
                return (string)Property["Symptoms"];
            }
            set
            {
                Property["Symptoms"] = value;
             //   NotifyPropertyChanged("Symptoms");
            }
        }

        [DisplayName("Sposób usnięcia niesprawności")]
        public string Solution
        {
            get
            {
                return (string)Property["Solution"];
            }
            set
            {
                Property["Solution"] = value;
              //  NotifyPropertyChanged("Solution");
            }
        }

        [DisplayName("Okoliczności wykrycia niesprawności")]
        public string Circumstances
        {
            get
            {
                return (string)Property["Circumstances"]; ;
            }
            set
            {
                Property["Circumstances"] = value;
           //     NotifyPropertyChanged("Circumstances");
            }
        }

        [DisplayName("Przyczyna niesprawności")]
        public string Reason
        {
            get
            {
                return (string)Property["Reason"];
            }
            set
            {
                Property["Reason"] = value;
           //     NotifyPropertyChanged("Reason");
            }
        }

        public int ItemDefectId
        {
            get
            {
                return ItemDefect.Id;
            }
            set
            {
                ItemDefect.Id = value;
                this["ItemDefectId"] = value;
                //      NotifyPropertyChanged("ItemName");
            }
        }

        public int ItemNewId
        {
            get
            {
                return ItemNew.Id;
            }
            set
            {
                ItemNew.Id = value;
                this["ItemNewId"] = value;
                //      NotifyPropertyChanged("ItemName");
            }
        }

        public string ItemName
        {
            get
            {
                return ItemDefect.Name;
            }
            set
            {
                ItemDefect.Name = value;
                ItemNew.Name = value;
          //      NotifyPropertyChanged("ItemName");
            }
        }
        public string IdPartNumber
        {
            get
            {
                return ItemDefect.PartNumber;
            }
            set
            {
                ItemDefect.PartNumber = value;
        //        NotifyPropertyChanged("IdPartNumber");
            }
        }
        public string InPartNumber
        {
            get
            {
                return ItemNew.PartNumber;
            }
            set
            {
                ItemNew.PartNumber = value;
           //     NotifyPropertyChanged("InPartNumber");
            }
        }
        public string IdSerialNumber
        {
            get
            {
                return ItemDefect.SerialNumber;
            }
            set
            {
                ItemDefect.SerialNumber = value;
           //     NotifyPropertyChanged("IdSerialNumber");
            }
        }
        public string InSerialNumber
        {
            get
            {
                return ItemNew.SerialNumber;
            }
            set
            {
                ItemNew.SerialNumber = value;
                NotifyPropertyChanged("InSerialNumber");
            }
        }
        public int IdFlightHours
        {
            get
            {
                return ItemDefect.FlightHours;
            }
            set
            {
                ItemDefect.FlightHours = value;
            //    NotifyPropertyChanged("IdFlightHours");
            }
        }

        public string DateStart
        {
            get
            {
                if ((int)Property["DateStartInt"] != 0)
                {
                    return IntToDate((int)Property["DateStartInt"]).ToString("dd.MM.yyyy");
                }
                else
                    return _dateStart;
            }
            set
            {
                if (((string)value).Length > 8)
                {
                    if (DateTime.TryParse(value, out DateTime date))
                    {
                        _dateStart = value;
                        Property["DateStartInt"] = DateToInt(date);
                    //    NotifyPropertyChanged("DateStart");
                    }
                    else
                    {
                        _dateStart = value;
                        Property["DateStartInt"] = 0;
                   //     NotifyPropertyChanged("DateStart");
                    }
                }
                else
                {
                    _dateStart = value;
                    Property["DateStartInt"] = 0;
                   // NotifyPropertyChanged("DateStart");
                }
            }
        }
        public string DateEnd
        {
            get
            {
                if ((int)Property["DateEndInt"] != 0)
                {
                    return IntToDate((int)Property["DateEndInt"]).ToString("dd.MM.yyyy");
                }
                else
                    return _dateEnd;

            }
            set
            {
                if (((string)value).Length > 8)
                {
                    if (DateTime.TryParse(value, out DateTime date))
                    {
                        _dateEnd = value;
                        Property["DateEndInt"] = DateToInt(date);
                        ItemNew.Date = Property["DateEndInt"].ToString();
                        ItemDefect.Date = Property["DateEndInt"].ToString();
                        //      NotifyPropertyChanged("DateEnd");
                    }
                    else
                    {
                        _dateEnd = value;
                        Property["DateEndInt"] = 0;
                        ItemNew.Date = Property["DateEndInt"].ToString();
                        ItemDefect.Date = Property["DateEndInt"].ToString();
                        //         NotifyPropertyChanged("DateEnd");
                    }
                }
                else
                {
                    _dateEnd = value;
                    Property["DateEndInt"] = 0;
                    ItemNew.Date = Property["DateEndInt"].ToString();
                    ItemDefect.Date = Property["DateEndInt"].ToString();
                    //    NotifyPropertyChanged("DateEnd");
                }
            }
        }
        [Browsable(false)]
        public int DateStartInt
        {
            get
            {
                return (int)Property["DateStartInt"];
            }
            set
            {
                _dateStart = IntToDate(value).ToString("dd.MM.yyyy");
                Property["DateStartInt"] = value;
                NotifyPropertyChanged("DateStart");
                NotifyPropertyChanged("DateStartInt");
            }
        }
        [Browsable(false)]
        public int DateEndInt
        {
            get
            {
                return (int)Property["DateEndInt"];
            }
            set
            {
                _dateEnd = IntToDate(value).ToString("dd.MM.yyyy");
                Property["DateEndInt"] = value;
                NotifyPropertyChanged("DateEnd");
                NotifyPropertyChanged("DateEndInt");
            }
        }

        public string Classification
        {
            get => (string)Property["Classification"]; set { Property["Classification"] = value;// NotifyPropertyChanged("Classification"); 
            }
        }

        public string GroundStopReason
        {
            get => (string)Property["GroundStopReason"]; set
            {
                Property["GroundStopReason"] = value;// NotifyPropertyChanged("GroundStopReason"); 
            }
        }

        public string ItemProcedure
        {
            get => (string)Property["ItemProcedure"]; set { Property["ItemProcedure"] = value; //NotifyPropertyChanged("ItemProcedure");
            }
        }

        public string SolutionProcedure
        {
            get => (string)Property["SolutionProcedure"]; set { Property["SolutionProcedure"] = value;// NotifyPropertyChanged("SolutionProcedure");
            }
        }

        public string EffectCompletingTask
        {
            get => (string)Property["EffectCompletingTask"]; set { Property["EffectCompletingTask"] = value;// NotifyPropertyChanged("EffectCompletingTask");
            }
        }

        public string EffectFlightSafety
        {
            get => (string)Property["EffectFlightSafety"]; set { Property["EffectFlightSafety"] = value; //NotifyPropertyChanged("EffectFlightSafety");
            }
        }

        public int WorkHours
        {
            get => (int)Property["WorkHours"]; set { Property["WorkHours"] = value; //NotifyPropertyChanged("WorkHours");
            }
        }
        public string Speciality
        {
            get => (string)Property["Speciality"]; set { Property["Speciality"] = value;
                //NotifyPropertyChanged("Speciality");
            }
        }

        public TAircraft Aircraft
        {
                get
                {
                    if (Property["Aircraft"] == null)
                    {
                        Property["Aircraft"] = new TAircraft();
                        ((TAircraft)Property["Aircraft"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                    }
                    return (TAircraft)Property["Aircraft"];
                }
                set => Property["Aircraft"] = value; //NotifyPropertyChanged("WorkerCheck");
            }
        

        public TWorker WorkerCheck
        {
            get
            {
                if (Property["WorkerCheck"] == null)
                {
                    Property["WorkerCheck"] = new TWorker();
                    ((TWorker)Property["WorkerCheck"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                }
                return (TWorker)Property["WorkerCheck"];
            }
            set => Property["WorkerCheck"] = value; //NotifyPropertyChanged("WorkerCheck");
        }

        public TWorker WorkerFix
        {
            get
            {
                if (Property["WorkerFix"] == null)
                {
                    Property["WorkerFix"] = new TWorker();
                    ((TWorker)Property["WorkerFix"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                }
                return (TWorker)Property["WorkerFix"];
            }
            set => Property["WorkerFix"] = value; //NotifyPropertyChanged("WorkerFix");
        }

        public TWorker WorkerDetect
        {
            get
            {
                if (Property["WorkerDetect"] == null)
                {
                    Property["WorkerDetect"] = new TWorker();
                    ((TWorker)Property["WorkerDetect"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                }
                return (TWorker)Property["WorkerDetect"];
            }
            set => Property["WorkerDetect"] = value; //NotifyPropertyChanged("WorkerDetect");

        }


        public TItem ItemNew
        {
            get
            {
                if (Property["ItemNew"] == null)
                {
                    Property["ItemNew"] = new TItem();
                    ((TItem)Property["ItemNew"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                }
                return ((TItem)Property["ItemNew"]);
            }
            set => Property["ItemNew"] = value;
        }

        public TItem ItemDefect
        {
            get
            {
                if (Property["ItemDefect"] == null)
                {
                    Property["ItemDefect"] = new TItem();
                    ((TItem)Property["ItemDefect"]).PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
                }
                return ((TItem)Property["ItemDefect"]);
            }
            set => Property["ItemDefect"] = value;              
        }
        #endregion

        public TDefect()
        {
            Property.PropertyChanged += (obj, args) => NotifyPropertyChanged(args.PropertyName);
        }



        public bool IsItemReady(RecordTypes record)
        {
            if (record == RecordTypes.TItemNew)
                return ItemNew.IsMinimal();
            else if (record == RecordTypes.TItemDefect)
                return ItemDefect.IsMinimal();
            else
                throw new InvalidOperationException();
        }

        public PropertyDictionary<string, object> GetData(bool insert)
        {
            var temp = new PropertyDictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in Property)
            {
                if (kv.Value == (object)""
                    || (kv.Value == (object)0)
                    || (kv.Value == null))
                {
                    if (!dbProperty[kv.Key].IsNullable)
                        return null; // dopisac ze za malo argumentow itd jakis event by sie przydal
                }
                else
                {
                    if (!((dbProperty[kv.Key].KeyType == KeyTypes.primary_key_no_insert) && insert))
                    {
                        temp.Add(kv);
                    }
                }
            }
            return temp;
        }

        public static DateTime IntToDate(int date)
        {
            if (date == 0)
                return DateTime.MinValue;
            int years1 = Math.Abs(date / 1461) * 4;
            int years2 = Math.Abs((date % 1461) / 365);
            int years = years2 + years1;
            int date1 = date % ((years1 / 4) * 1461 + years2 * 365);
            return new DateTime(years, 1, 1).AddDays(date1 - 1);
        }

        public static int DateToInt(DateTime data)
        {
            int days2 = Math.Abs(data.Year / 4) * 1461;
            int days3 = Math.Abs(data.Year % 4) * 365;
            return (days2 + days3 + data.DayOfYear);
        }

        public static bool StrEquals(string a, string b)
        {
            if (string.Compare(a, b) == 0)
                return true;
            else
                return false;
        }


        #region Derived Methods

        public void Assign(IDbTable _data)
        {
            TDefect data = (TDefect)_data;
            foreach (KeyValuePair<string, object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }

        public bool IsMinimal()
        {
            return true;
        }

        public bool IsComplete()
        {
            return true;
        }
        public IDbTable Duplicate()
        {
            var temp = new TDefect();
            foreach (KeyValuePair<string, object> obj in Property)
            {
                temp.Property[obj.Key] = obj.Value;
            }
            return temp;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        #endregion

        public TDefect GetExample()
        {
            var temp = new TDefect();
            foreach(KeyValuePair<string,object> kv in PropertyExample)
            {
                temp.Property[kv.Key] = kv.Value;
            }
            return temp;
        }
        public TDefect Clone()
        {
            return (TDefect) this.MemberwiseClone();
        }
    }

    public class SearchDefect : TDefect
    {
        private static readonly Dictionary<string, DbObject> filterProperty = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.primary_key_no_insert,

            }},
            {"WorkHours",new DbObject()
            {
                DBName = "WorkHours",
                DBRootName = "WorkHours",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,

            }},
            {"AircraftNumber",new DbObject()
            {
                DBName = "AircraftId",
                DBRootName = "Name",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.reference_key_id,
                FilterType = FilterTypes.list
            }},
            {"Symptoms",new DbObject()
            {
                DBName = "Symptoms",
                DBRootName = "Symptoms",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.single_contains
            }},
            {"SolutionProcedure",new DbObject()
            {
                DBName = "SolutionProcedure",
                DBRootName = "Name",
                DBTableRootName = "TSolutionProcedure",
                KeyType = KeyTypes.reference,

            }},
            {"Solution",new DbObject()
            {
                DBName = "Solution",
                DBRootName = "Solution",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                IsNullable = true,
                FilterType = FilterTypes.single_contains
            }},
            {"Circumstances",new DbObject()
            {
                DBName = "Circumstances",
                DBRootName = "Name",
                DBTableRootName = "TCircumstances",
                KeyType = KeyTypes.reference,
                FilterType = FilterTypes.single_contains
            }},
            {"Reason",new DbObject()
            {
                DBName = "Reason",
                DBRootName = "Reason",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.single_contains
            }},
            {"Classification",new DbObject()
            {
                DBName = "Classification",
                DBRootName = "Name",
                DBTableRootName = "TClassification",
                KeyType = KeyTypes.reference,
                
            }},
            {"GroundStopReason",new DbObject()
            {
                DBName = "GroundStopReason",
                DBRootName = "Name",
                DBTableRootName = "TGroundStopReason",
                KeyType = KeyTypes.reference,
                
            }},
            {"WorkerCheckId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerCheckId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                
            }},
            {"WorkerFixId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerFixId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                
            }},
            {"WorkerDetectId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "WorkerDetectId",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.reference_key_id,
                
            }},
            {"ItemProcedure",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemProcedure",
                DBRootName = "Name",
                DBTableRootName = "TItemProcedure",
                KeyType = KeyTypes.reference,
                
            }},
            {"EffectCompletingTask",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "EffectCompletingTask",
                DBRootName = "Name",
                DBTableRootName = "TEffectCompletingTask",
                KeyType = KeyTypes.reference,
                
            }},
            {"EffectFlightSafety",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "EffectFlightSafety",
                DBRootName = "Name",
                DBTableRootName = "TEffectFlightSafety",
                KeyType = KeyTypes.reference,
                
            }},
            {"ItemNewId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemNewId",
                DBRootName = "Id",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.reference_key_id,
                
            }},
            {"ItemDefectId",new DbObject() // narazie false wiadomo dlaczego
            {
                DBName = "ItemDefectId",
                DBRootName = "Id",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.reference_key_id,
                
            }},
            {"DateStartInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateStartInt",
                DBRootName = "DateStartInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.number_larger_than
            }},
            {"DateEndInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateEndInt",
                DBRootName = "DateEndInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.number_larger_than
            }},
            {"DateStartToInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateStartInt",
                DBRootName = "DateStartInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value, 
                FilterType = FilterTypes.number_lower_than
                
            }},
            {"DateEndToInt",new DbObject() // TYMCZASOWO WAROTSC DOMYSLNA 0
            {
                DBName = "DateEndInt",
                DBRootName = "DateEndInt",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.number_lower_than
            }},
            {"FlightHours",new DbObject()
            {
                DBName = "FlightHours",
                DBRootName = "FlightHours",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,
                FilterType = FilterTypes.number_larger_than
            }},
            {"FlightHoursTo",new DbObject()
            {
                DBName = "FlightHours",
                DBRootName = "FlightHours",
                DBTableRootName = "TDefect",
                KeyType = KeyTypes.numeric_value,
                FilterType = FilterTypes.number_lower_than
            }},
        };

        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>()
        {

           {"Id",0},
            {"FlightHours",0},
            {"WorkHours",0},
            {"AircraftNumber",""},
            {"Symptoms",""},
            {"Solution",""},
            {"Circumstances",""},
            {"Reason",""},
            {"DateStartInt",0},
            {"DateEndInt",0},
            {"Classification",""},
            {"GroundStopReason",""},
            {"WorkerCheckId",""},
            {"WorkerDetectId",""},
            {"WorkerFixId",""},
            {"ItemProcedure",""},
            {"SolutionProcedure",""},
            {"EffectCompletingTask",""},
            {"EffectFlightSafety",""},
            {"ItemNewId",0},
            {"ItemDefectId",0},
            {"Aircraft",null },
            {"ItemNew",null },
            {"ItemDefect",null },
            {"WorkerFix",null },
            {"WorkerCheck",null },
            {"WorkerDetect",null },
            {"DateEndToInt",0},
            {"DateStartToInt",0},

        };

        private PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"FlightHours",0},
            {"WorkHours",0},
            {"AircraftId",""},
            {"Symptoms",""},
            {"Solution",""},
            {"Circumstances",""},
            {"Reason",""},
            {"DateStartInt",0},
            {"DateEndInt",0},
            {"Classification",""},
            {"GroundStopReason",""},
            {"WorkerCheckId",""},
            {"WorkerDetectId",""},
            {"WorkerFixId",""},
            {"ItemProcedure",""},
            {"SolutionProcedure",""},
            {"EffectCompletingTask",""},
            {"EffectFlightSafety",""},
            {"ItemNewId",0},
            {"ItemDefectId",0},
            {"Aircraft",null },
            {"ItemNew",null },
            {"ItemDefect",null },
            {"WorkerFix",null },
            {"WorkerCheck",null },
            {"WorkerDetect",null },
            {"DateEndToInt",0},
            {"DateStartToInt",0},

        };

        #region interface_idefecttable
       // public override Dictionary<string, DbObject> DbProperty { get => dbProperty; }


        #endregion


 /*       public override Dictionary<string, DbObject> DbProperty { get {
                if (filterProperty == null) {
                    filterProperty = new Dictionary<string, FilterDbObject>();
                    foreach (KeyValuePair<string, DbObject> kv in dbProperty)
                    {
                        filterProperty.Add(kv.Key, new FilterDbObject(kv.Value,)
                    }
                }
            } }
            */
       // public Dictionary<string, FilterDbObject> filterProperty;

        private DateTime dateStartTo;
        private DateTime dateEndTo;
        private string _dateStartTo = "";
        private string _dateEndTo = "";
        private List<TItemCategory> categoryList = new List<TItemCategory>();
        private List<string> aircraftList = new List<string>();

        public string DateEndTo
        {
            get
            {
                if (_dateEndTo.Length < 8)
                    return _dateEndTo;
                if (dateEndTo != null && dateEndTo.Year > 2000)
                    return dateEndTo.ToString("dd.MM.yyyy");
                else
                    return _dateEndTo;

            }
            set
            {
                if (((string)value).Length > 8)
                {
                    if (DateTime.TryParse(value, out dateEndTo))
                    {
                        if (dateEndTo.Year < 2000)
                        {
                            _dateEndTo = value;
                        }
                        NotifyPropertyChanged("DateEndTo");
                    }
                    else
                    {
                        _dateEndTo = value;
                        NotifyPropertyChanged("DateEndTo");
                    }
                }
                else
                {
                    _dateEndTo = value;
                    NotifyPropertyChanged("DateEndTo");
                }
            }
        }
        public string DateStartTo
        {
            get
            {
                if (_dateStartTo.Length < 8)
                    return _dateStartTo;
                if (dateStartTo != null && dateStartTo.Year > 2000)
                    return dateStartTo.ToString("dd.MM.yyyy");
                else
                    return _dateStartTo;

            }
            set
            {
                if (value.Length > 8)
                {
                    if (DateTime.TryParse(value, out dateStartTo))
                    {
                        if (dateStartTo.Year < 2000)
                        {
                            _dateStartTo = value;
                        }
                        NotifyPropertyChanged("DateStartTo");
                    }
                    else
                    {
                        _dateStartTo = value;
                        NotifyPropertyChanged("DateStartTo");
                    }

                }
                else
                {
                    _dateStartTo = value;
                    NotifyPropertyChanged("DateStartTo");
                }
            }
        }
        [Browsable(false)]
        public int DateStartToInt
        {
            get
            {
                var temp = DateToInt(dateStartTo);
                if (temp < 730000)
                    return 0;
                else
                    return temp;
            }
            set
            {
                dateStartTo = IntToDate(value);
                NotifyPropertyChanged("DateStart");
                NotifyPropertyChanged("DateStartInt");
            }
        }
        [Browsable(false)]
        public int DateEndToInt
        {
            get
            {
                var temp = DateToInt(dateEndTo);
                if (temp < 730000)
                    return 0;
                else
                    return temp;
            }
            set
            {
                dateEndTo = IntToDate(value);
                NotifyPropertyChanged("DateEnd");
                NotifyPropertyChanged("DateEndInt");
            }
        }
        [Browsable(false)]
        public List<string> AircraftList
        {
            get
            {
                return aircraftList;
            }
            set
            {
                aircraftList = value;
                NotifyPropertyChanged("AircraftList");
            }
        }

    }

    public class DList : List<TDefect>
    { }

   
}
