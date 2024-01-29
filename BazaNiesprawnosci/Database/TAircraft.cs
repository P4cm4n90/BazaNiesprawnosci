using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaNiesprawnosci.Database
{
    public class TAircraft: IDbTable, INotifyPropertyChanged, IComparable<TAircraft>
    {


        private static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.primary_key_no_insert,

            }},
            {"TailNumber",new DbObject()
            {
                DBName = "TailNumber",
                DBRootName = "TailNumber",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.normal_value,
                FilterType = FilterTypes.single,            
            }},
            {"SerialNumber",new DbObject()
            {
                DBName = "SerialNumber",
                DBRootName = "SerialNumber",
                DBTableRootName = "TAircraft",
                KeyType = KeyTypes.normal_value,

            }},
            {"Type",new DbObject()
            {
                DBName = "Type",
                DBRootName = "Type",
                DBTableRootName = "TAircraftTypes",
                KeyType = KeyTypes.reference,

            }},         
        };

        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>
        {
            {"Id",0},
            {"TailNumber",""},
            {"SerialNumber",""},
            {"Type",""},
        };

        private static readonly RecordTypes recordType = RecordTypes.TAircraft;

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

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        private PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"TailNumber",""},
            {"SerialNumber",""},
            {"Type",""},
        };

        public int Id
        {
            get => (int)Property["Id"];
            set => Property["Id"] = value;
        }
        public string TailNumber
        {
            get => (string)Property["TailNumber"];
            set => Property["TailNumber"] = value;
        }
        public string SerialNumber
        {
            get => (string)Property["SerialNumber"];
            set => Property["SerialNumber"] = value;
        }
        public string Type
        {
            get => (string)Property["Type"];
            set => Property["Type"] = value;
        }

        public TAircraft()
        {
            Property.PropertyChanged += (obj, args) => { NotifyPropertyChanged(args.PropertyName); };
            Property.LockProperties = true;
        }

        public void Assign(IDbTable _data)
        {
            TAircraft data = (TAircraft)_data;
            foreach(KeyValuePair<string,object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }

        public IDbTable Duplicate()
        {
            return new TAircraft()
            {
                Id = Id,
                TailNumber = TailNumber,
                SerialNumber = SerialNumber,
                Type = Type
            };
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

        public bool IsMinimal()
        {
            if (!String.IsNullOrEmpty(TailNumber) || !String.IsNullOrEmpty(SerialNumber) || !String.IsNullOrEmpty(Type))
                return true;
            else
                return false;
        }

        public void Reset()
        {
            TailNumber = "";
            SerialNumber = "";
            Type = "";
            Id = 0;
        }

        public bool IsComplete()
        {
            if (String.IsNullOrEmpty(TailNumber) && String.IsNullOrEmpty(SerialNumber) && String.IsNullOrEmpty(Type))
                return false;
            else
                return true;

        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(TAircraft other)
        {
            if (TailNumber.CompareTo(other.TailNumber) == 0)
                return 0;
            else
                return -1;
        }
    }
}
