using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BazaNiesprawnosci
{
    public class DbField
    {
        private object dbProperty;
        private string dbName;
        private string dbRootName;
        private RecordTypes recordType;
        private KeyTypes keyType;



        public SQLiteParameter ParamValue
        {
            get
            {                
                return new SQLiteParameter(String.Format("@Param{0}", dbName), DbProperty);
            }
        }

        public object DbProperty { get => dbProperty; set => dbProperty = value; }
        public string DbName { get => dbName; set => dbName = value; }
        public string DbRootName { get => dbRootName; set => dbRootName = value; }
        public RecordTypes RecordType { get => recordType; set => recordType = value; }
        public KeyTypes KeyType { get => keyType; set => keyType = value; }

        public DbField(object _DbProperty,SqlParamAttribute attribute)
        {
            DbProperty = _DbProperty;
            DbName = attribute.Name;
            DbRootName = attribute.RootName;
            RecordType = attribute.RecordType;

        }
    }
}
