using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Xml.Serialization;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    [Serializable]
    [XmlRoot("Category")]
    public class TItemCategory : INotifyPropertyChanged, IComparable<TItemCategory>, IDbTable
    {
        [NonSerialized]
        public static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TCategory",
                KeyType = KeyTypes.primary_key
                
            }},
            {"Name",new DbObject()
            {
                DBName = "Name",
                DBRootName = "Name",
                DBTableRootName = "TCategory",
                KeyType = KeyTypes.normal_value,
                


            }},
        };
        [NonSerialized]
        public static readonly RecordTypes recordType = RecordTypes.TCategory;

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

        [XmlIgnoreAttribute]
        public Dictionary<string, DbObject> DbProperty { get => dbProperty; }
        [XmlIgnoreAttribute]
        public RecordTypes RecordType { get => recordType; }
        [XmlIgnoreAttribute]
        public string TableName { get => RecordType.ToString(); }

        #endregion
        [NonSerialized]
        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>()
        {
            {"Id",0 },
            {"Name","" }
        };
        [NonSerialized]
        private PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"Id",0 },
            {"Name","" }
        };


        [XmlAttribute("Id")]
        public int Id
        {
            get => (int)Property["Id"];
            set => Property["Id"] = value;
        }

        [XmlAttribute("Name")]
        public string Name
        {
            get => (string)Property["Name"];
            set => Property["Name"] = value;
            
        }
        public TItemCategory(int _number, string _name)
        {
            Id = _number;
            Name = _name;
            Property.LockProperties = true;
            Property.PropertyChanged += (obj, args) => { NotifyPropertyChanged(args.PropertyName); };
        }

        public TItemCategory()
        {
            Property.LockProperties = true;
            Property.PropertyChanged += (obj, args) => { NotifyPropertyChanged(args.PropertyName); };
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
                    if (!((dbProperty[kv.Key].KeyType == KeyTypes.primary_key_no_insert) && insert) 
                        && (dbProperty[kv.Key].KeyType != KeyTypes.aux_value))
                    {
                        temp.Add(kv);
                    }
                }
            }
            return temp;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Id, Name);
        }
        public int CompareTo(TItemCategory _ata)
        {
            if((string.Compare(Name, _ata.Name)==0) && (Id == _ata.Id))
            {
                return 0;
            }
            else
            {
                if (Id > _ata.Id)
                    return -1;
                else
                    return 1;
            }
            
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
            if (String.IsNullOrEmpty(Name) && Id == 0)
                return false;
            else
                return true;
        }
        public bool IsComplete()
        {
            if (String.IsNullOrEmpty(Name) || Id == 0)
                return false;
            else
                return true;
        }
        public void Assign(IDbTable _data)
        {
            TItemCategory data = (TItemCategory)_data;
            foreach (KeyValuePair<string, object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }
        public IDbTable Duplicate()
        {
            return new TItemCategory()
            {
                Id = Id,
                Name = Name,


            };
        }
        public TItemCategory Clone(bool random)
        {
            return (TItemCategory)Clone();
        }
        public object Clone()
        {
            return MemberwiseClone();
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }


}
