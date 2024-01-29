using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BazaNiesprawnosci
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class SqlParamAttribute : Attribute
    {
        public string Name { get; set; }
        public string RootName { get; set; }
        public string TableRootName {get; set;}
        public KeyTypes KeyType { get; set; }
        public RecordTypes RecordType { get; set; }
        //public bool Nullable { get => isNullable; set => isNullable = value; }
        public bool IsInsertable { get => isInsertable; set => isInsertable = value; }


        public SqlParamAttribute(string _Name, string _RootName, string _TableName, KeyTypes _KeyType, bool _IsInsertable)
        {
            Name = _Name;
            RootName = _RootName;
            TableRootName = _TableName;
            KeyType = _KeyType;
            IsInsertable = _IsInsertable;
            RecordType = RecordTypes.Unassigned;
        }

        public SqlParamAttribute(string _Name, string _RootName, string _TableName, KeyTypes _KeyType, RecordTypes _RecordType, bool _IsInsertable)
        {
            Name = _Name;
            RootName = _RootName;
            TableRootName = _TableName;
            KeyType = _KeyType;
            IsInsertable = _IsInsertable;
            RecordType = _RecordType;

        }

        public SqlParamAttribute(string _Name, string _RootName, string _TableName, KeyTypes _KeyType)
        {
            Name = _Name;
            RootName = _RootName;
            TableRootName = _TableName;
            KeyType = _KeyType;
            RecordType = RecordTypes.Unassigned;

    }

    }

    public enum KeyTypes
    {
        /// <summary>
        /// Reprezenatacja w glownej tablicy wystepuje pod primary_key_id_ref o typie int
        /// </summary>
        primary_key_id_ref,
        /// <summary>
        /// Reprezentacja w glownej tablicy wystepuje pod ta sama wartoscia
        /// </summary>
        foreign_id,
        primary_key_changes,
        primary_key,
        primary_key_autoincrease,
        no_key
    }

    
}
