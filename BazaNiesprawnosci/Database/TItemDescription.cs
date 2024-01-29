using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    public class TItemDescription : IDbTable, INotifyPropertyChanged, IComparable<TItemDescription>
    {
        private static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"PartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.primary_key,
                IsNullable = false,
                RecordType = RecordTypes.TItemDesc

            }},
            {"Name",new DbObject()
            {
                DBName = "Name",
                DBRootName = "Name",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.normal_value,
                IsNullable = false
                
            }},
            {"Jim",new DbObject()
            {
                DBName = "Jim",
                DBRootName = "Jim",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.normal_value,
                
            }},
            {"Speciality",new DbObject()
            {
                DBName = "Speciality",
                DBRootName = "Name",
                DBTableRootName = "TSpeciality",
                KeyType = KeyTypes.reference,
                
            }},
            {"CategoryId",new DbObject()
            {
                DBName = "CategoryId",
                DBRootName = "Id",
                DBTableRootName = "TCategory",
                KeyType = KeyTypes.reference_key_id,
                RecordType = RecordTypes.TCategory

            }},
            {"StoredPartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.primary_key_stored,
               
            }}, 
            {"Category",new DbObject()
            {
                DBName = "CategoryId",
                DBRootName = "Id",
                DBTableRootName = "TCategory",
                KeyType = KeyTypes.aux_value,

            }},

        };
        private static readonly object[] nullProperties = { "", 0, null, };

        private static readonly RecordTypes recordType = RecordTypes.TItemDesc;

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
        public Dictionary<string, DbObject> DbProperty { get => dbProperty; }
        public RecordTypes RecordType { get => recordType; }
        public string TableName { get => RecordType.ToString(); }

        public PropertyDictionary<string, object> GetData(bool insert)
        {
            var temp = new PropertyDictionary<string, object>();
            if (insert && !IsComplete())
                return null;
            foreach (KeyValuePair<string, object> kv in Property)
            {
                if(!nullProperties.Contains(kv.Value))
                {
                    if (!((dbProperty[kv.Key].KeyType == KeyTypes.primary_key_no_insert) && insert) && (dbProperty[kv.Key].KeyType != KeyTypes.aux_value))
                    {
                        temp.Add(kv);
                    }
                }
            }
            return temp;
        }

        #endregion

        protected static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>
        {
            {"PartNumber",""},
            {"Name",""},
            {"Jim",""},
            {"Speciality",""},
            {"CategoryId",0 },
            {"StoredPartNumber", ""},
            {"Category", new TItemCategory()}
        };

        protected PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"PartNumber",""},
            {"Name",""},
            {"Jim",""},
            {"Speciality",""},
            {"CategoryId",0 },
            {"StoredPartNumber", ""},
            {"Category", new TItemCategory()}
        };
        
        private ObservableCollection<string> subPartNumbers; // ????
               

        public string PartNumber {
            get => (string)Property["PartNumber"];
            set =>Property["PartNumber"] = value; }

        public string Name {
            get => (string)Property["Name"];
            set => Property["Name"] = value;   }

        public int CategoryId {
            get => (int) Property["CategoryId"];
            set => Property["CategoryId"] = value; }

        public string Jim {
            get => (string)Property["Jim"];
            set => Property["Jim"] = value;  }

        public string StoredPartNumber {
            get => (string)Property["StoredPartNumber"];
            set => Property["StoredPartNumber"] = value; }

        public string Speciality {
            get => (string)Property["Speciality"];
            set => Property["Speciality"] = value; }


        
        public int SubPartQuantity
        {
            get
            {
                int i = 0;
                foreach(string s in SubPartNumbers)
                {
                    if (!string.IsNullOrEmpty(s))
                             i++;
                }
                return i;
            }
        }

        public ObservableCollection<string> SubPartNumbers
        {
            get
            {
                if (subPartNumbers == null)
                {
                    subPartNumbers = new ObservableCollection<string>(new List<string>() { "", "", "" }) {  };

                    subPartNumbers.CollectionChanged += (sender, args) =>
                    {
                        if (String.IsNullOrEmpty(subPartNumbers[0]))
                        {
                            if (String.IsNullOrEmpty(subPartNumbers[1]))
                            {
                                if (!String.IsNullOrEmpty(subPartNumbers[2]))
                                {
                                    subPartNumbers[0] = subPartNumbers[2];
                                    subPartNumbers[2] = "";
                                }

                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(subPartNumbers[2]))
                                {
                                    subPartNumbers[0] = subPartNumbers[1];
                                    subPartNumbers[1] = subPartNumbers[2];
                                    subPartNumbers[2] = "";
                                }
                                else
                                {
                                    subPartNumbers[0] = subPartNumbers[1];
                                    subPartNumbers[1] = "";
                                }
                            }
                        }
                        else
                        {
                            if (String.IsNullOrEmpty(subPartNumbers[1]))
                            {
                                if (!String.IsNullOrEmpty(subPartNumbers[2]))
                                {
                                    subPartNumbers[1] = subPartNumbers[2];
                                    subPartNumbers[2] = "";
                                }

                            }
                        }

                        NotifyPropertyChanged("SubPartNumbers");
                    };
                }
                return subPartNumbers;
            }
            set { subPartNumbers = value; NotifyPropertyChanged("SubPartNumbers"); }
        
        } /// <summary>
        /// poprawic
        /// </summary>

        public TItemDescription()
        {
            Property.PropertyChanged += (obj, args) => {
                    NotifyPropertyChanged(args.PropertyName);                 
                };
        }
        
        public TItemCategory Category
        {
            get
            {
                return (TItemCategory)Property["Category"];
            }
            set
            {
                if (value == null) return;
                ((TItemCategory)Property["Category"]).Assign(value);
                CategoryId = ((TItemCategory)Property["Category"]).Id;
                NotifyPropertyChanged("Category");
            }
        }

        public PropertyDictionary<string, object> DataToWrite()
        {
            var temp = new PropertyDictionary<string, object>();
            foreach (KeyValuePair<string, object> kv in Property)
            {
                if (!dbProperty[kv.Key].IsNullable
                    && ((kv.Value != (object)"")
                    || (kv.Value != (object)0)
                    || (kv.Value != null)))
                    return null; // dopisac ze za malo argumentow itd jakis event by sie przydal
                if ((kv.Value != (object)"")
                    || (kv.Value != (object)0)
                    || (kv.Value != null)
                    || dbProperty[kv.Key].KeyType == KeyTypes.primary_key_no_insert)
                {
                    temp.Add(kv);
                }
            }
            return temp;
        }


        public virtual bool IsComplete()
        {
            if (String.IsNullOrEmpty(PartNumber) || String.IsNullOrEmpty(Name) || String.IsNullOrEmpty(Jim))
                return false;
            else
                return true;
        }

        public virtual bool IsMinimal()
        {
            if (String.IsNullOrEmpty(PartNumber) || String.IsNullOrEmpty(Name))
                return false;
            else
                return true;
        }

        public List<string> GetAllPartNumbers()
        {
            var list = new List<string>{
                PartNumber };
            foreach (string s in SubPartNumbers)
            {
                if(!String.IsNullOrEmpty(s))
                {
                    list.Add(s);
                }
            }
            return list;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// 0 - sa rowni
        /// 1 - nie sa rowni
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(TItemDescription other)
        {
            if (StrEqual(PartNumber, other.PartNumber) && StrEqual(Name, other.Name) && StrEqual(Speciality, other.Speciality) &&
                StrEqual(Jim, other.Jim))
                return 0;
            else
                return 1;
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, object> kv in PropertyDefault)
            {
                Property[kv.Key] = PropertyDefault[kv.Key];
            }
        }

        public void Assign(IDbTable _data)
        {
            TItemDescription data = (TItemDescription)_data;
            foreach (KeyValuePair<string, object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }

        public IDbTable Duplicate()
        {
           
            return new TItemDescription
            {
                PartNumber = PartNumber,
                Name = Name,
                Jim = Jim,
                CategoryId = CategoryId,
                Speciality = Speciality,
                StoredPartNumber = StoredPartNumber,
                Category = (TItemCategory)Category.Duplicate(),
                
            };
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
    }

    public class TItemDescriptionSearch: TItemDescription
    {
        private static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"PartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.primary_key,
                
            }},
            {"Name",new DbObject()
            {
                DBName = "Name",
                DBRootName = "Name",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.normal_value,
                
            }},
            {"Jim",new DbObject()
            {
                DBName = "Jim",
                DBRootName = "Jim",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.normal_value,
                
            }},
            {"Speciality",new DbObject()
            {
                DBName = "Speciality",
                DBRootName = "Name",
                DBTableRootName = "TSpeciality",
                KeyType = KeyTypes.reference,
                
            }},
            {"CategoryId",new DbObject()
            {
                DBName = "CategoryId",
                DBRootName = "Id",
                DBTableRootName = "TCategory",
                KeyType = KeyTypes.reference,
                
            }},
            {"StoredPartNumber",new DbObject()
            {
                DBName = "PartNumber",
                DBRootName = "PartNumber",
                DBTableRootName = "TItemDescription",
                KeyType = KeyTypes.primary_key_stored,
                
            }},
        };
    }
}
