using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{


    public class TItem : IDbTable, INotifyPropertyChanged, IComparable<TItem>
    {
        private bool itemDefect = false;
        public bool ItemDefect
        {
            get
            {
                return itemDefect;
            }
            set
            {
                itemDefect = value;
                NotifyPropertyChanged("ItemDefect");
            }
        }
        private static readonly Dictionary<string, DbObject> dbPropertyItemNew = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.primary_key_no_insert,

            }},
            {"PartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.reference,
                FilterType = FilterTypes.single_contains,
                RecordType = RecordTypes.TItemDesc

            }},
            {"SerialNumber",new DbObject()
            {
                DBName = "SerialNumber",
                DBRootName = "SerialNumber",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.normal_value,

            }},
            {"FlightHours",new DbObject()
            {
                DBName = "FlightHours",
                DBRootName = "FlightHours",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.numeric_value,
            }},
            {"Date",new DbObject()
            {
                DBName = "Date",
                DBRootName = "Date",
                DBTableRootName = "TItemNew",
                KeyType = KeyTypes.numeric_value,

            }}
        };

        private static readonly Dictionary<string, DbObject> dbPropertyItemDefect = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.primary_key_no_insert,



            }},
            {"PartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.reference,
                FilterType = FilterTypes.single_contains,
                RecordType = RecordTypes.TItemDesc

            }},
            {"SerialNumber",new DbObject()
            {
                DBName = "SerialNumber",
                DBRootName = "SerialNumber",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.normal_value,

            }},
            {"FlightHours",new DbObject()
            {
                DBName = "FlightHours",
                DBRootName = "FlightHours",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.numeric_value,

            }},
            {"Date",new DbObject()
            {
                DBName = "Date",
                DBRootName = "Date",
                DBTableRootName = "TItemDefect",
                KeyType = KeyTypes.numeric_value,

            }},
        };

        private RecordTypes recordType = RecordTypes.TItemNew;

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
        public Dictionary<string, DbObject> DbProperty {
            get
            {
                if (ItemDefect)
                    return dbPropertyItemDefect;
                else
                    return dbPropertyItemNew;
            }
        }
        public RecordTypes RecordType { get
            {
                if (itemDefect)
                    return (recordType = RecordTypes.TItemDefect);
                else
                    return (recordType = RecordTypes.TItemNew);
            }
        }

        public string TableName { get => RecordType.ToString(); }

        #endregion

        public PropertyDictionary<string, object> GetData(bool insert)
        {
            var temp = new PropertyDictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in Property)
            {
                if (kv.Value == (object)""
                    || (kv.Value == (object)0)
                    || (kv.Value == null))
                {
                    if (!DbProperty[kv.Key].IsNullable)
                        return null; // dopisac ze za malo argumentow itd jakis event by sie przydal
                }
                else
                {
                    if (!((DbProperty[kv.Key].KeyType == KeyTypes.primary_key_no_insert) && insert))
                    {
                        temp.Add(kv);
                    }
                }
            }
            return temp;
        }
        public void Assign(IDbTable _data)
        {
            TItem data = (TItem)_data;
            foreach (KeyValuePair<string, object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }
        public IDbTable Duplicate()
        {
            
            return new TItem()
            {
                Id = Id,
                PartNumber = PartNumber,
                SerialNumber = SerialNumber,
                FlightHours = FlightHours,
                Date = Date,
                ItemDefect = ItemDefect,
                ItemDescription = (TItemDescription) ItemDescription.Duplicate()

            };
        }

        // BRAKUJE
        // EVENTU JAK NIE BEDZIE DALO SIE WYLUSKAC ITEMDESCRIPTION
        // ODP. METODY SPRAWDZAJACEJ CZY JEST KOMPLETNY WPIS ZROBIONY
        //
        //
        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>(4)
        {
            { "Id",0},
            {"PartNumber","" },
            {"SerialNumber","" },
            {"FlightHours",0 },
            {"Date", 0 }
        };
        protected PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>(4)
        {
            { "Id",0},
            {"PartNumber","" },
            {"SerialNumber","" },
            {"FlightHours",0 },
            {"Date", 0 }
        };

        public int Id
        {
            get => (int)Property["Id"];
            set => Property["Id"] = value;
        }
        public string PartNumber
        {
            get => ItemDescription.PartNumber;
            set  {
                Property["PartNumber"] = value;
                ItemDescription.PartNumber = (string) Property["PartNumber"];
            }
        }

        public string SerialNumber {
            get => (string)Property["SerialNumber"];
            set => Property["SerialNumber"] = value; }

        public int FlightHours {
            get => (int)Property["FlightHours"];
            set => Property["FlightHours"] = value; }

        public string Name
        {
            get { return ItemDescription.Name; }
            set { ItemDescription.Name = value; NotifyPropertyChanged("Name"); }
        }

        private string date = "";
        public string Date
        {
            get
            {
                if (((int)Property["Date"]) == 0)
                    return date;
                DateTime temp;
                if ((temp = TDefect.IntToDate((int)Property["Date"])) == DateTime.MinValue)
                    return date;
                else                  
                return date = TDefect.IntToDate((int)Property["Date"]).ToString("dd.MM.yyyy");
            }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    Property["Date"] = 0;
                else if (value.Length >= 9)
                {
                    if (DateTime.TryParse(value, out DateTime tempDate))
                    {
                        Property["Date"] = TDefect.DateToInt(tempDate);
                        date = tempDate.ToString("dd.MM.yyyy");
                    }
                    else
                    {
                        Property["Date"] = 0;
                        date = value;
                    }
                }
                else
                {
                    date = value;
                    Property["Date"] = 0;
                }
            }
        }

        public TItem()
        {
            Property.PropertyChanged += (obj, args) => { NotifyPropertyChanged(args.PropertyName); };
            Property.LockProperties = true;
        }

        private TItemDescription itemDescription;

        public TItemDescription ItemDescription /// poprawa implementcaji
        {
            get
            {
                return itemDescription ?? (itemDescription = new TItemDescription() { PartNumber = (string)Property["PartNumber"] });               
            }
            set
            {
                itemDescription = value;
                NotifyPropertyChanged("ItemDescription");
            }

        }

        public int CompareTo(TItem other)
        {
            if (Id != 0 || other.Id != 0)
            {
                if (Id == other.Id) return 0;
                return Id > other.Id ? 1 : -1;
            }
            if (StrEqual(PartNumber, other.PartNumber) && StrEqual(SerialNumber, other.SerialNumber))
                return 0;
            else
                return 1;
        }
        public bool IsNullOrEmpty()
        {
            if (String.IsNullOrEmpty(PartNumber) && String.IsNullOrEmpty(SerialNumber) && (FlightHours == 0))
                return true;
            else
                return false;

        }

        public void Clear()
        {
            foreach (KeyValuePair<string, object> kv in PropertyDefault)
            {
                Property[kv.Key] = PropertyDefault[kv.Key];
            }
        }

        public bool IsMinimal()
        {
            if (String.IsNullOrEmpty(PartNumber) && String.IsNullOrEmpty(SerialNumber) && (Id == 0))
                return false;
            else
                return true;
        }

        public bool IsComplete()
        {
            if (!String.IsNullOrEmpty(PartNumber) && !String.IsNullOrEmpty(SerialNumber) &&
                !((Date == 0.ToString()) || String.IsNullOrEmpty(Date)))
            {
                return true;
            }
            else
                return false;
        }



        public object Clone()
        {
            return this.MemberwiseClone();
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

        public override string ToString()
        {
            if (FlightHours == 0)
                return String.Format("P/N: {0}, S/N: {0}", PartNumber, SerialNumber);
            else
                return String.Format("P/N: {0}, S/N: {1}, FH: {2}", PartNumber, SerialNumber, FlightHours);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string _propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));
        }
    }


}
