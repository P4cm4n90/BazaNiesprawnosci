using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BazaNiesprawnosci.Database;
using System.ComponentModel;

namespace BazaNiesprawnosci
{

    public class TWorker : IDbTable, INotifyPropertyChanged, IComparable<TWorker>, ICloneable
    {


        private static readonly Dictionary<string, DbObject> dbProperty = new Dictionary<string, DbObject>()
        {
            {"Id",new DbObject()
            {
                DBName = "Id",
                DBRootName = "Id",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.primary_key_no_insert,
                RecordType = RecordTypes.TWorker

            }},
            {"Surname",new DbObject()
            {
                DBName = "Surname",
                DBRootName = "Surname",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.normal_value,
                RecordType = RecordTypes.TWorker

            }},
            {"Name",new DbObject()
            {
                DBName = "Name",
                DBRootName = "Name",
                DBTableRootName = "TWorker",
                KeyType = KeyTypes.normal_value,
                RecordType = RecordTypes.TWorker

            }},
            {"JobType",new DbObject()
            {
                DBName = "JobType",
                DBRootName = "Name",
                DBTableRootName = "TJobType",
                KeyType = KeyTypes.reference,
                RecordType = RecordTypes.TWorker

            }},
            {"Speciality",new DbObject()
            {
                DBName = "Speciality",
                DBRootName = "Name",
                DBTableRootName = "TSpeciality",
                KeyType = KeyTypes.reference,
                RecordType = RecordTypes.TWorker

            }},
        };

        private static readonly PropertyDictionary<string, object> PropertyDefault = new PropertyDictionary<string, object>
        {
            {"Id",0},
            {"Surname",""},
            {"Name",""},
            {"JobType",""},
            {"Speciality","" },
        };

        private static readonly RecordTypes recordType = RecordTypes.TWorker;

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

        private PropertyDictionary<string, object> Property = new PropertyDictionary<string, object>()
        {
            {"Id",0},
            {"Surname",""},
            {"Name",""},
            {"JobType",""},
            {"Speciality","" },
        };

        public int Id {
            get => (int)Property["Id"];
            set => Property["Id"] = value;} 
        public string Surname {
            get => (string)Property["Surname"];
            set => Property["Surname"] = value; }
        public string Name {
            get => (string)Property["Name"];
            set => Property["Name"] = value;
        }
        public string JobType {
            get => (string)Property["JobType"];
            set => Property["JobType"] = value;
        }
        public string Speciality {
            get => (string)Property["Speciality"];
            set => Property["Speciality"] = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TWorker()
        {
            Property.PropertyChanged += (obj, args) => { NotifyPropertyChanged(args.PropertyName); };
            Property.LockProperties = true;
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
            if (!String.IsNullOrEmpty(Surname))
                return true;
            else
                return false;
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int CompareTo(TWorker _worker)
        {
            if (StrEqual(Name, _worker.Name) && StrEqual(Surname, _worker.Surname) && StrEqual(JobType, _worker.JobType) && StrEqual(Speciality, _worker.Speciality))
                return 0;
            else
                return 1;
        }

        public bool IsComplete()
        {
            if (String.IsNullOrEmpty(Surname) || String.IsNullOrEmpty(Name))
                return false;
            else
                return true;
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

        public void Clear()
        {
            foreach (KeyValuePair<string, object> kv in PropertyDefault)
            {
                Property[kv.Key] = PropertyDefault[kv.Key];
            }
        }

        public void Assign(IDbTable _data)
        {
            TWorker data = (TWorker)_data;
            foreach (KeyValuePair<string, object> kv in data.Property)
            {
                Property[kv.Key] = kv.Value;
            }
        }

        public IDbTable Duplicate()
        {
            var temp = new TWorker();
            foreach(KeyValuePair<string,object> obj in Property)
            {
                temp.Property[obj.Key] = obj.Value;
            }
            return temp;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public override string ToString()
        {
            var result = "";
            foreach (KeyValuePair<string, object> key in Property)
            {
                if (StrEqual(key.Key, "Id") && ((int)key.Value != 0)) {
                    result = String.Format("{2} {0}: {1}", key.Key, key.Value, result);
                    continue; }
                if(!String.IsNullOrEmpty((string)key.Value))
                    result = String.Format("{1} {0}", key.Value, result);
            }
            return result;
        }
    }
}