using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace BazaNiesprawnosci
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class SqlTableAttribute : Attribute
    {
        public RecordTypes TableType { get; set; }
        public string TableName { get; set; }

        public SqlTableAttribute(RecordTypes _TableType, string _tableName)
        {
            TableType = _TableType;
            TableName = _tableName;

        }

    }
}