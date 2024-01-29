using BazaNiesprawnosci;
using System;
using System.Collections.Generic;

namespace BazaNiesprawnosci.Database
{
    public interface IDbTable
    {
        RecordTypes RecordType { get; }
        string TableName { get; }
        PropertyDictionary<string, object> GetData(bool insert);
        object this[string s] { get; set;}
       // PropertyDictionary<string, object> GetData();
        Dictionary<string, DbObject> DbProperty { get; }
        IDbTable Duplicate();
        void Assign(IDbTable data);

        bool IsComplete();
        bool IsMinimal();
        
    }

}