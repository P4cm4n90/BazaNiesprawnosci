using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SQLite;
using System.Data;
using System.Reflection;
using Microsoft.Win32;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    public static class DbManager
    {
        private readonly static object[] dbEmpty = { "{}", DBNull.Value, null, "" ,0 };

        private static MainSettings Settings = MainSettings.Default;
        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public static event DataBaseStatusHandler OnStatusChange;


        private static string applicationPath = System.IO.Path.GetDirectoryName((new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath);
        public static string FileConnection
        {
            get
            {
                return string.Format("Data Source = {0}; Version=3;foreign keys=true;", @Settings.DatabaseFilepath);
               
            }
        }


    
        #pragma warning disable 1060
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);
        #pragma warning restore 1060

        public static bool InitializeDatabase(bool createNew, bool wantToChoose)
        {
            try
            {
              //  if (propertyDict == null)
                 //   propertyDict = prop;
                FileDialog _file;
                if (createNew)
                {
                    _file = new SaveFileDialog()
                    {
                        InitialDirectory = applicationPath,
                        Filter = "Pliki bazy danych|*.sqlite"
                    };
                    ((SaveFileDialog)_file).OverwritePrompt = false;
                    if (_file.ShowDialog() == true)
                    {
                        Settings.DatabaseFilepath = _file.FileName;
                        if (!File.Exists(_file.FileName))
                        {
                            SQLiteConnection.CreateFile(@Settings.DatabaseFilepath);
                            MainSettings.Default.Save();
                        }
                        else
                        {
                            NotifyStatusChange(Properties.Resources.DataBaseCreateOverwrite);
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if(wantToChoose)
                {
                   
                    _file = new OpenFileDialog()
                    {
                        InitialDirectory = applicationPath,
                        Filter = "Pliki bazy danych|*.sqlite"
                    };
                    if (_file.ShowDialog() == true)
                    {
                        string _fullPath = _file.FileName;
                        string _tempFileName = ((OpenFileDialog)_file).SafeFileName;
                        Settings.DatabaseFilepath = _fullPath.Replace(_tempFileName, "") + _tempFileName;
                        MainSettings.Default.Save();
                    }
                    else
                    {
                        return false;
                    }
                }
                using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
                {
                    connection.Open();
                    connection.Close();
                    if (!createNew)
                    {
                        NotifyStatusChange($"{Properties.Resources.DatabaseInitializeOk}. Scieżka do bazy danych: {Settings.DatabaseFilepath}");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                NotifyStatusChange("Nie można połączyć z bazą danych. Tekst błędu :" + ex.Message);
                return false;
            }
        }

        public static bool CreateDatabase()
        {
            InitializeDatabase(true, true);
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(connection))
                    {

                        cmd.CommandText = @"CREATE TABLE TCategory (  Id INTEGER NOT NULL PRIMARY KEY, " +
                                                                      "Name TEXT NOT NULL, " +
                                                                      "Additional TEXT)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TCircumstance ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TAircraft ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                                                        "TailNumber TEXT NOT NULL, " +
                                                                        "SerialNumber TEXT, " +
                                                                        "Type TEXT TEXT NOT NULL)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TEffectFlightSafety ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TEffectCompletingTask ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TSolutionProcedure ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TItemProcedure ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TGroundStopReason ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TClassification ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TSpeciality ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TJobType ( Name TEXT NOT NULL PRIMARY KEY)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TItemsSubstitutes ( Sub1 TEXT NOT NULL PRIMARY KEY REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                        "Sub2 TEXT NOT NULL REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                        "Sub3 TEXT REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                        "Sub4 TEXT REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TItemDesc ( "+
                                                                    "PartNumber TEXT NOT NULL PRIMARY KEY, " +
                                                                    "Name TEXT NOT NULL, " +
                                                                    "Jim TEXT UNIQUE, "+
                                                                    "CategoryId INTEGER REFERENCES TCategory(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "Speciality TEXT REFERENCES TSpeciality(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TItemDefect ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "+
                                                                "PartNumber TEXT NOT NULL REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                "SerialNumber TEXT, "+
                                                                "FlightHours INTEGER, "+
                                                                "Date INTEGER )";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TItemNew ( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, " +
                                                                "PartNumber TEXT NOT NULL REFERENCES TItemDesc(PartNumber) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                "SerialNumber TEXT, " +
                                                                "FlightHours INTEGER, " +
                                                                "Date INTEGER )";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TWorker( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "+
                                                                "Name TEXT, "+
                                                                "Surname TEXT NOT NULL, "+
                                                                "JobType TEXT REFERENCES TJobType(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                "Speciality TEXT REFERENCES TSpeciality(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT)";
                        cmd.ExecuteNonQuery();
                        cmd.CommandText = @"CREATE TABLE TDefect( Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, "+
                                                                 "AircraftId INTEGER NOT NULL REFERENCES TAircraft(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                 "FlightHours INTEGER, "+
                                                                 "DateStartInt INTEGER, "+
                                                                    "DateEndInt INTEGER, "+
                                                                    "Circumstance TEXT REFERENCES TCircumstance(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "Symptoms TEXT, "+
                                                                    "EffectFlightSafety TEXT REFERENCES TEffectFlightSafety(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "EffectCompletingTask TEXT REFERENCES TEffectCompletingTask(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "SolutionProcedure TEXT REFERENCES TSolutionProcedure(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "Solution TEXT, "+
                                                                    "Reason TEXT, "+
                                                                    "ItemProcedure TEXT REFERENCES TItemProcedure(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "ItemNewId INTEGER REFERENCES TItemNew(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                    "ItemDefectId INTEGER REFERENCES TItemDefect(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                    "WorkerDetectId INTEGER REFERENCES TWorker(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                    "WorkerFixId INTEGER REFERENCES TWorker(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                    "WorkerCheckId INTEGER REFERENCES TWorker(Id) ON UPDATE CASCADE ON DELETE SET DEFAULT, "+
                                                                    "GroundStopReason TEXT REFERENCES TGroundStopReason(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "Speciality TEXT REFERENCES TSpeciality(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "Classification TEXT REFERENCES TClassification(Name) ON UPDATE CASCADE ON DELETE SET DEFAULT, " +
                                                                    "WorkHours INTEGER)";
                
                        cmd.ExecuteNonQuery();
                        /// chwilowo dla c295
                        foreach(string s in MainSettings.Default.TAircraft)
                        {
                            cmd.CommandText = @"INSERT INTO TAircraft(TailNumber,Type) Values(@Param0,@Param1)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0",s));
                            cmd.Parameters.Add(new SQLiteParameter("@Param1", "C-295"));
                            cmd.ExecuteNonQuery();
                        }
                        foreach(string s in MainSettings.Default.TCircumstances)
                        {
                            cmd.CommandText = @"INSERT INTO TCircumstances(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TEffectFlightSafety)
                        {
                            cmd.CommandText = @"INSERT INTO TEffectFlightSafety(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TEffectCompletingTask)
                        {
                            cmd.CommandText = @"INSERT INTO TEffectCompletingTask(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TItemProcedure)
                        {
                            cmd.CommandText = @"INSERT INTO TItemProcedure(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.ff)
                        {
                            cmd.CommandText = @"INSERT INTO TGroundStopReason(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TSolutionProcedure)
                        {
                            cmd.CommandText = @"INSERT INTO TSolutionProcedure(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TClassification)
                        {
                            cmd.CommandText = @"INSERT INTO TClassification(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TJobType)
                        {
                            cmd.CommandText = @"INSERT INTO TJobType(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (string s in MainSettings.Default.TSpeciality)
                        {
                            cmd.CommandText = @"INSERT INTO TSpeciality(Name) Values(@Param0)";
                            cmd.Parameters.Add(new SQLiteParameter("@Param0", s));
                            cmd.ExecuteNonQuery();
                        }
                        foreach (TItemCategory ic in MainSettings.Default.TCategory)
                        {
                            cmd.CommandText = @"INSERT INTO TCategory(Id,Name) Values(@Id,@Name)";
                            cmd.Parameters.Add(new SQLiteParameter("@Id", ic.Id));
                            cmd.Parameters.Add(new SQLiteParameter("@Name", ic.Name));
                            cmd.ExecuteNonQuery();
                        }

                        NotifyStatusChange(Properties.Resources.DataBaseCreateOk);
                        return true;

                        
                    }
                }
                
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// returns 1 - if its ok;
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// 

        public static int WriteDatabase(object _data, RecordTypes type, QueryTypes queryType)
        {
            return 0;
        }
        /// <summary>
        /// Zwraca 1 kiedy znajdzie wartosc
        /// Zwraca 0 kiedy nie znajdzie wartosci
        /// Zwracac 2 kiedy wystąpi błąd
        /// </summary>
        /// <param name="_data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int CheckDatabase(IDbTable _data)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    var tempQuery = GetCheckQuery(_data);
                    cmd.CommandText = tempQuery.Command;
                    cmd.Parameters.AddRange(tempQuery.ParamCollection.ToArray());
                    try
                    {
                        if ((int.Parse(cmd.ExecuteScalar().ToString())) == 1)
                            return 1;
                        else
                            return 0;

                    }
                    catch (Exception ex)
                    {
                        return 2;
                    }
                }
            }
        }
        /// <summary>
        /// Returns 0 or itemid if item was successfully added into database
        /// returns -1 if there is an error
        /// returns -2 if there is not sufficient data for add
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static int AddNewData(IDbTable _data)
        {
            return AddNewData(_data, null, null);
        }
     
          
     
        public static int AddNewData(IDbTable _data,  SQLiteConnection _connection, SQLiteCommand _command)
        {
            var type = _data.RecordType;
            switch (type)
            {
                case (RecordTypes.TItemDesc):
                    {
                        using (SQLiteConnection connection = _connection ??  new SQLiteConnection(@FileConnection))
                        {
                            connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                var item = (TItemDescription)_data;
                                try
                                {
                                    if(!item.IsComplete())
                                    {
                                        NotifyStatusChange(Properties.Resources.ItemDescNoName);
                                        return -2;
                                    }
                                    var cmdData = GetInsertQuery(item);
                                    cmd.CommandText = cmdData.Command;
                                    foreach(SQLiteParameter sqp in cmdData.ParamCollection) { cmd.Parameters.Add(sqp); }
                                    cmd.ExecuteNonQuery();
                                }
                                catch (SQLiteException ex)
                                {
                                    if (ex.ResultCode == SQLiteErrorCode.Constraint_Unique || ex.ResultCode == SQLiteErrorCode.Constraint_PrimaryKey)
                                    {

                                    }
                                    //if (ex.ResultCode == SQLiteErrorCode.Constraint_ForeignKey)
                                    //     NotifyStatusChange(String.Format("W bazie agregatów nie znaleziono agregatu o P/N: {0} " +
                                    //        "Jeżeli nie popełniono błędu w pisowni - istnieje możliwość natychmiastowego dodania agregatu do bazy danych agregatów." +
                                    //        "Czy chcesz dodać agregat o p/n: {0} do bazy danych agregatów?", defect.PartNumber), DatabaseResultCodes.item_pn_not_found);

                                    return -1;
                                }
                                return 0;
                            }
                        }
                        
                               
                    };
                case (RecordTypes.TWorker):
                    {
                        using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
                        {
                            if(connection.State != ConnectionState.Open)
                                 connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                var item = (TWorker)_data;
                                try
                                {
                                    if (item.IsComplete())
                                    {
                                        var cmdData = GetInsertQuery(item);
                                        cmd.CommandText = cmdData.Command;
                                        cmd.Parameters.AddRange(cmdData.ParamCollection.ToArray());
                                        cmd.ExecuteNonQuery();
                                        return GetLastItemId(connection, cmd);
                                    }
                                    else
                                    {
                                        NotifyStatusChange(Properties.Resources.WorkerNoName);
                                        return -2;
                                    }
                                }
                                catch(SQLiteException ex)
                                {
                                  //  if (ex.ResultCode == SQLiteErrorCode.Constraint_ForeignKey)
                                 //       NotifyStatusChange(String.Format("W bazie specjalistów nie znaleziono osoby o nazwisku: {0}. " +
                                  //          "Jeżeli nie popełniono błędu w pisowni - istnieje możliwość dodania nowego specjalisty do bazy danych." +
                                  //          "Dodać specjaliste o nazwisku: {0}. ?", defect), DatabaseResultCodes.worker_not_found);
                                    return -1;
                                }
                                catch 
                                {
                                    return -1;
                                }
                            }
                        }
                    };
                case (RecordTypes.TCategory):
                    {
                        using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
                        {
                            if (connection.State != ConnectionState.Open)
                                connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                var item = (TItemCategory)_data;
                                try
                                {
                                    if(!item.IsComplete())
                                    {
                                        NotifyStatusChange(Properties.Resources.CategoryNotCompleted);
                                        return -2;
                                    }
                                    var cmdData = GetInsertQuery(item);
                                    cmd.CommandText = cmdData.Command;
                                    cmd.Parameters.AddRange(cmdData.ParamCollection.ToArray());
                                    cmd.ExecuteNonQuery();
                                    return GetLastItemId(connection,cmd);
                                }
                                catch(Exception ex)
                                {
                                    return 0;
                                }
                            }
                        }
                    };
                case (RecordTypes.TAircraft):
                    {
                        using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
                        {
                            if (connection.State != ConnectionState.Open)
                                connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                var item = (TAircraft)_data;
                                try
                                {
                                    if (!item.IsComplete())
                                    {
                                        NotifyStatusChange(Properties.Resources.AircraftNotCompleted);
                                        return -2;
                                    }
                                    var cmdData = GetInsertQuery(item);
                                    cmd.CommandText = cmdData.Command;
                                    cmd.Parameters.AddRange(cmdData.ParamCollection.ToArray());
                                    cmd.ExecuteNonQuery();
                                    return GetLastItemId(connection, cmd);

                                }
                                catch (SQLiteException ex)
                                {
                                    //  if (ex.ResultCode == SQLiteErrorCode.Constraint_ForeignKey)
                                    //       NotifyStatusChange(String.Format("W bazie specjalistów nie znaleziono osoby o nazwisku: {0}. " +
                                    //          "Jeżeli nie popełniono błędu w pisowni - istnieje możliwość dodania nowego specjalisty do bazy danych." +
                                    //          "Dodać specjaliste o nazwisku: {0}. ?", defect), DatabaseResultCodes.worker_not_found);
                                    return 0;
                                }
                                catch
                                {
                                    return 0;
                                }

                            }
                        }
                    };
                case (RecordTypes.TItemNew):
                case (RecordTypes.TItemDefect):
                    {
                        using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
                        {
                            if (connection.State != ConnectionState.Open)
                                connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                try
                                {
                                    
                                    var item = (TItem)_data;
                                    if(!item.IsComplete())
                                    {
                                        NotifyStatusChange(Properties.Resources.AircraftNotCompleted);
                                        return -2;
                                    }
                                    if ((CheckDatabase(item.ItemDescription) != 1) && MainSettings.Default.dbInsertItemDescIfNotFound)
                                    {
                                        if (MainSettings.Default.dbInsertItemDescIfNotFound)
                                        {
                                            AddNewData(item.ItemDescription);
                                            NotifyStatusChange(Properties.Resources.NotFoundItemDescInsert);
                                        }
                                        else
                                        {
                                            NotifyStatusChange(Properties.Resources.NotFoundItemDesc);
                                        }
                                    }

                                    var cmdData = GetInsertQuery(item);
                                    cmd.Parameters.AddRange(cmdData.ParamCollection.ToArray());
                                    cmd.CommandText = cmdData.Command;
                                    cmd.ExecuteNonQuery();
                                    return GetLastItemId(connection, cmd);
                                }
                                catch (SQLiteException ex)
                                {
                                    ///wtf?

                                    return 2;
                                }
                            }
                        }
                    };
                case (RecordTypes.TDefect):
                    {
                        using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
                        {
                            connection.Open();
                            using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                            {
                                var defect = (TDefect)_data;
                                try
                                {
                                    ///mozna by sprawdzic czy jest taki worker :-)
                                    int tempid = AddNewData(defect.ItemDefect, _connection, _command);
                                    defect.ItemDefectId = tempid == -2 ? 0 : tempid;
                                    tempid = AddNewData(defect.ItemNew, _connection, _command);
                                    defect.ItemNewId = tempid == -2 ? 0 : tempid;
                                    var cmdData = GetInsertQuery(defect);
                                    cmd.CommandText = cmdData.Command;
                                    foreach(SQLiteParameter sqp in cmdData.ParamCollection) { cmd.Parameters.Add(sqp); }
                                    cmd.ExecuteNonQuery();
                                    return GetLastItemId(connection, cmd);
                                    /*

                                    #region items
                                    if(!String.IsNullOrEmpty(defect.AircraftNumber))
                                    {

                                    }
                                    /// ZAKODOWANE ROWNANIE PARTNUMBEROW!
                                    if (defect.ItemDefect.IsMinimal() || defect.ItemNew.IsMinimal())
                                    {
                                        if(defect.ItemDefect.IsMinimal() && defect.ItemNew.IsMinimal())
                                        {
                                            defect.ItemDefect.Id = AddNewData(defect.ItemDefect, connection, cmd);
                                            defect.ItemNew.Id = AddNewData(defect.ItemNew, connection, cmd);
                                        }
                                        else if(defect.ItemDefect.IsMinimal() && !defect.ItemNew.IsMinimal())
                                        {
                                            defect.ItemNew.PartNumber = defect.ItemDefect.PartNumber;
                                            defect.ItemDefect.Id = AddNewData(defect.ItemDefect, connection, cmd);
                                            defect.ItemNew.Id = AddNewData(defect.ItemNew, connection, cmd);
                                        }
                                        else if(!defect.ItemDefect.IsMinimal() && defect.ItemNew.IsMinimal())
                                        {
                                            defect.ItemDefect.PartNumber = defect.ItemNew.PartNumber;
                                            defect.ItemDefect.Id = AddNewData(defect.ItemDefect, connection, cmd);
                                            defect.ItemNew.Id = AddNewData(defect.ItemNew, connection, cmd);
                                        }
                                       
                                        cmd.CommandText = @"UPDATE TDefect SET ItemNewId=@ItemNewId WHERE Id=@Id";
                                        cmd.Parameters.Add(new SQLiteParameter("@ItemNewId", defect.ItemNew.Id));
                                        cmd.Parameters.Add(new SQLiteParameter("@Id", Id));
                                        cmd.ExecuteNonQuery();
                                        cmd.CommandText = @"UPDATE TDefect SET ItemDefectId=@ItemNewId WHERE Id=@Id";
                                        cmd.Parameters.Add(new SQLiteParameter("@ItemDefectId", defect.ItemDefect.Id));
                                        cmd.Parameters.Add(new SQLiteParameter("@Id", Id));
                                        cmd.ExecuteNonQuery();
                                    }

                                    #endregion

                                    #region workers insert/update

                                    SaveWorker("WorkerDetectId", defect.WorkerDetect);
                                    SaveWorker("WorkerFixId", defect.WorkerFix);
                                    SaveWorker("WorkerCheckId", defect.WorkerCheck);
                                                                      
                                    bool SaveWorker(string column, TWorker worker)
                                    {
                                        if (worker.IsMinimal())
                                        {
                                            cmd.CommandText = @"SELECT Id FROM TWorkers WHERE Surname=@Surname";
                                            cmd.Parameters.Add(new SQLiteParameter("@Surname", worker));

                                            int wId = 0;
                                            if (MainSettings.Default.dbInsertWorkerIfNotFound)
                                            {
                                                wId = Convert.ToInt32(cmd.ExecuteScalar() ?? AddNewData(worker, connection, cmd));
                                                cmd.CommandText = String.Format(@"UPDATE TDefect SET {0}=@WorkerId WHERE Id=@Id", column);
                                                cmd.Parameters.Add(new SQLiteParameter("@WorkerId", wId));
                                                cmd.Parameters.Add(new SQLiteParameter("@Id", Id));
                                                cmd.ExecuteNonQuery();
                                                NotifyStatusChange(worker.ToString(), DatabaseResultCodes.worker_not_found_inserted);
                                                return true;
                                            }
                                            else
                                            {
                                                wId = Convert.ToInt32(cmd.ExecuteScalar() ?? 0);
                                                if (wId == 0)
                                                {
                                                    NotifyStatusChange(worker.ToString(), DatabaseResultCodes.worker_not_found);
                                                    return false;
                                                }
                                                else
                                                    return true;
                                            }
                                        }
                                        else
                                            return false;
                                    }
                                    #endregion
 
                                    return Id;*/
                                }
                                catch
                                {
                                    //narazie nie wiem
                                }
                                return 0;
                            } }
                        
                    };

                case (RecordTypes.TItemSubstitute):
                    {
                        /*var list = (List<string>)_data;

                        using (SQLiteConnection connection = _connection)
                        {
                            if (connection.State != ConnectionState.Open)
                                connection.Open();

                            using (SQLiteCommand cmd = _command)
                            {
                                switch (list.Count)
                                {

                                    case 2:
                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2) " +
                                                                              "Values(@Sub1, @Sub2)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[0]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[1]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2) " +
                                                                        "Values(@Sub1, @Sub2)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[0]));
                                        cmd.ExecuteNonQuery();

                                        break;
                                    case 3:
                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3) " +
                                                                              "Values(@Sub1, @Sub2, @Sub3)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[0]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[2]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3) " +
                                                                                     "Values(@Sub1, @Sub2, @Sub3)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[2]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[0]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3) " +
                                                                                        "Values(@Sub1, @Sub2, @Sub3)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[2]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[0]));
                                        cmd.ExecuteNonQuery();
                                        break;
                                    case 4:
                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3, Sub4) " +
                                                                                            "Values(@Sub1, @Sub2, @Sub3, @Sub4)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[0]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[2]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub3", list[3]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3, Sub4) " +
                                         "Values(@Sub1, @Sub2, @Sub3, @Sub4)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[2]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[3]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub3", list[0]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3, Sub4) " +
                                         "Values(@Sub1, @Sub2, @Sub3, @Sub4)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[2]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[3]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[0]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub3", list[1]));
                                        cmd.ExecuteNonQuery();

                                        cmd.CommandText = @"INSERT OR IGNORE INTO TItemsSubstitutes(Sub1, Sub2, Sub3, Sub4) " +
                                         "Values(@Sub1, @Sub2, @Sub3, @Sub4)";
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub1", list[3]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[0]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub2", list[1]));
                                        cmd.Parameters.Add(new SQLiteParameter("@Sub3", list[2]));
                                        cmd.ExecuteNonQuery();

                                        break;
                                    default:
                                        break;
                                }
                            }
                        } */
                        throw new NotImplementedException();
                    return 0;
                    }

                default:
                    {
                        break;
                    };
}
            return 0;
        }
        /// <summary>
        /// Zwraca 0 jezeli uda sie usunac
        /// Zwraca 1 jak jest jakis blad
        /// </summary>
        /// <param name="_data"></param>
        /// <returns></returns>
        public static int RemoveData(IDbTable _data)
        {
            using (SQLiteConnection connection =  new SQLiteConnection(@FileConnection))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    var tempQuery = GetRemoveQuery(_data);
                    cmd.CommandText = tempQuery.Command;
                    cmd.Parameters.AddRange(tempQuery.ParamCollection.ToArray());
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        public static int UpdateData(IDbTable _data)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
            {
                connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {

                    var tempQuery = GetUpdateQuery(_data);
                    cmd.CommandText = tempQuery.Command;
                    cmd.Parameters.AddRange(tempQuery.ParamCollection.ToArray());
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }

        public static int UpdateData(IDbTable _data, SQLiteConnection _connection, SQLiteCommand _command)
        {
            using (SQLiteConnection connection = _connection ?? new SQLiteConnection(@FileConnection))
            {
                if(connection.State != ConnectionState.Open)
                    connection.Open();
                using (SQLiteCommand cmd = _command ?? new SQLiteCommand(connection))
                {
                    switch (_data.RecordType)
                    {
                        case RecordTypes.TAircraft:
                            {
                                break;
                            };
                        case RecordTypes.TDefect:
                            {
                                TDefect def = (TDefect)_data;
                                UpdateData(def.ItemDefect,connection,cmd);
                                UpdateData(def.ItemNew, connection, cmd);
                                break;
                            };
                        case RecordTypes.TWorker:
                            {
                                TWorker worker = (TWorker)_data;
                                if(CheckDatabase(worker) != 0)
                                {
                                    if (!Settings.dbInsertWorkerIfNotFound)
                                        NotifyStatusChange(Properties.Resources.NotFoundWorker, DatabaseResultCodes.worker_not_found);
                                    else
                                        throw new NotFiniteNumberException();
                                    ///po poprawy w pozniejsyzm czasie

                                }
                                break;
                            };
                        case (RecordTypes.TItemNew | RecordTypes.TItemDefect):
                            {
                                TItem item = (TItem)_data;
                                if (CheckDatabase(item.ItemDescription) !=0)
                                {
                                    if (!Settings.dbInsertItemDescIfNotFound)
                                        NotifyStatusChange(Properties.Resources.NotFoundItemDesc, DatabaseResultCodes.item_not_found);
                                    else
                                        throw new NotImplementedException();
                                    /// do poprawy i dopisanie bo narazie nie wiem jak to zrobic.
                                }
                                break;
                            }

                    }
                    var tempQuery = GetUpdateQuery(_data);
                    cmd.CommandText = tempQuery.Command;
                    cmd.Parameters.AddRange(tempQuery.ParamCollection.ToArray());
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return 1;
                    }
                    return 0;
                }
            }
        }


        private static SqlCommandData GetInsertQuery(IDbTable item)
        {           
            List<SQLiteParameter> listParameter = new List<SQLiteParameter>();
           
            var data = item.GetData(true); /// TRZEA BY SIE ZASTANOWIC JAKIE ZABEZPIECZNIE PRZED MALA ILOSCIA DANYCH
            var listProp = new List<string>();
            var listParam = new List<string>();
            foreach (KeyValuePair<string, object> kv in data)
            {
                if (dbEmpty.Contains(data[kv.Key]))
                    continue;
                var dbProp = item.DbProperty[kv.Key];   
                if(dbProp.KeyType == KeyTypes.primary_key_no_insert || dbProp.KeyType == KeyTypes.aux_value || dbProp.KeyType == KeyTypes.primary_key_stored || dbProp.KeyType == KeyTypes.table_value)
                    continue;
                listProp.Add(dbProp.DBName);
                if (dbProp.KeyType == KeyTypes.reference_key_id && !((dbProp.RecordType == RecordTypes.TItemDefect) || (dbProp.RecordType == RecordTypes.TItemNew)))
                { 
                    listParam.Add($"(SELECT Id FROM {dbProp.DBTableRootName} WHERE {dbProp.DBRootName}={dbProp.DBNameParam})");
                }
                else
                    listParam.Add(dbProp.DBNameParam);

                listParameter.Add(new SQLiteParameter(dbProp.DBNameParam, kv.Value));
            }
            var command = $@"INSERT OR IGNORE INTO {item.TableName} ({String.Join(", ", listProp)}) Values({String.Join(", ", listParam)})";
             return new SqlCommandData()
             {
                 Command = command,
                 ParamCollection = listParameter
             };

        }

        private static SqlCommandData GetUpdateQuery(IDbTable item)
        {
            List<SQLiteParameter> listParameter = new List<SQLiteParameter>();

            var data = item.GetData(false); /// TRZEA BY SIE ZASTANOWIC JAKIE ZABEZPIECZNIE PRZED MALA ILOSCIA DANYCH
            var listProp = new List<string>();

            string idName = "";

            foreach (KeyValuePair<string, object> kv in data)
            {
                var dbProp = item.DbProperty[kv.Key];
                if (dbProp.KeyType == KeyTypes.primary_key_no_insert ||
                    dbProp.KeyType == KeyTypes.primary_key)
                {
                    idName = $@"{dbProp.DBName} = {dbProp.DBNameParam}";
                }
                else if (dbProp.KeyType == KeyTypes.reference_key_id && 
                    !((dbProp.RecordType == RecordTypes.TCategory) ||
                    (dbProp.RecordType == RecordTypes.TItemDefect) || 
                    (dbProp.RecordType == RecordTypes.TItemNew)))
                {
                    listProp.Add(String.Format("(SELECT Id FROM {0} WHERE {1}={2})", dbProp.DBTableRootName, dbProp.DBRootName, dbProp.DBNameParam));
                }
                else
                    listProp.Add($@"{dbProp.DBName} = {dbProp.DBNameParam}");
        
                listParameter.Add(new SQLiteParameter(dbProp.DBNameParam, kv.Value));

            }
            if(String.IsNullOrEmpty(idName))
            {
                NotifyStatusChange("Nie wybrano rekordu do aktualizacji", DatabaseResultCodes.no_id_to_update); // how to handle that
                return null;
            }
            var command = $@"UPDATE {item.TableName} SET {String.Join(", ", listProp)} WHERE {idName}";
            return new SqlCommandData()
            {
                Command = command,
                ParamCollection = listParameter
            };

        }

        private static SqlCommandData GetRemoveQuery(IDbTable item)
        {
            List<SQLiteParameter> listParameter = new List<SQLiteParameter>();

            KeyValuePair<string, object> idKeyValue;
            if ((idKeyValue = item.GetData(false).FirstOrDefault(c => ((item.DbProperty[c.Key].KeyType == KeyTypes.primary_key) ||
                                              (item.DbProperty[c.Key].KeyType == KeyTypes.primary_key_no_insert)))).
                                              Equals(default(KeyValuePair<string, object>)))
            {
                NotifyStatusChange("Błąd. Nie można usunąć wpisu. Brak Id wpisu.", DatabaseResultCodes.item_not_found); // tu mozna zrobic klase statyczna ktora bedzie przydzialala message :P
                return null;
            }
            else
            {
                var dbProp = item.DbProperty[idKeyValue.Key];
                var idName = String.Format("{0} = {1}", dbProp.DBName, dbProp.DBNameParam);
                listParameter.Add(new SQLiteParameter(dbProp.DBNameParam, idKeyValue.Value));
                var command = String.Format(@"DELETE FROM {0} WHERE {1}", item.TableName, idName);
                return new SqlCommandData()
                {
                    Command = command,
                    ParamCollection = listParameter
                };
            }

        }

        private static SqlCommandData GetSelectAllQuery(IDbTable item)
        {
            return new SqlCommandData()
            {
                Command = $@"SELECT * FROM {item.TableName}",
                ParamCollection = new List<SQLiteParameter>()
            };
        }
        private static SqlCommandData GetSelectQuery(IDbTable item)
        {
            List<SQLiteParameter> listParameter = new List<SQLiteParameter>();

            var data = item.GetData(false); /// TRZEA BY SIE ZASTANOWIC JAKIE ZABEZPIECZNIE PRZED MALA ILOSCIA DANYCH
            var listProp = new List<string>();

            foreach (KeyValuePair<string, object> kv in data)
            {
                if (dbEmpty.Contains(data[kv.Key]))
                    continue;
                var dbProp = item.DbProperty[kv.Key];
                string sign = "=";
                var param = dbProp.DBNameParam;

                if(dbProp.KeyType == KeyTypes.aux_value)
                {
                    if (dbProp.FilterType == FilterTypes.number_larger_than)
                        sign = ">=";
                    else if (dbProp.FilterType == FilterTypes.number_lower_than)
                        sign = "<=";
                }
                if (dbProp.KeyType == KeyTypes.reference_key_id && dbProp.FilterType != FilterTypes.list)
                {
                    param = $"(SELECT Id FROM {dbProp.DBTableRootName} WHERE {dbProp.DBRootName} {sign} {dbProp.DBNameParam})";
                }
                if (dbProp.FilterType == FilterTypes.single_contains)
                {
                    listProp.Add($"{dbProp.DBName} LIKE %{param}%");
                }
                else if(dbProp.FilterType == FilterTypes.single)
                {
                    listProp.Add($"{dbProp.DBName} {sign} {param}");
                }
                else if(dbProp.FilterType == FilterTypes.list)
                {
                    throw new NotImplementedException();
                  /*  var lst = new List<string>();
                    var tempList = (List<string>)data[kv.Key];
                    foreach (string s in tempList)
                    {
                        if (dbProp.KeyType == KeyTypes.reference_key_id)
                            param = $"(SELECT Id FROM {dbProp.DBTableRootName} WHERE {dbProp.DBRootName}={$"@Param{s}"})";                                                                                                    
                        else
                            param = $"@Param{s}";
                        
                        lst.Add($"{dbProp.DBName} = {param}");
                        listParameter.Add(new SQLiteParameter(String.Format("@Param{0}", s), s));
                    }
                    listProp.Add($"({String.Join(" OR ", lst)})");
                    continue;*/
                }
                listParameter.Add(new SQLiteParameter(dbProp.DBNameParam, kv.Value));

            }
            var command = $@"SELECT * FROM {item.TableName} WHERE {String.Join(" AND ", listProp)}";

            return new SqlCommandData()
            {
                Command = command,
                ParamCollection = listParameter
            };

        }

        private static SqlCommandData GetCheckQuery(IDbTable item)
        {
            List<SQLiteParameter> listParameter = new List<SQLiteParameter>();

            KeyValuePair<string, object> idKeyValue;
            idKeyValue = item.GetData(false).FirstOrDefault(c => ((item.DbProperty[c.Key].KeyType == KeyTypes.primary_key) ||
                                              (item.DbProperty[c.Key].KeyType == KeyTypes.primary_key_no_insert)));
            if(idKeyValue.Equals(default(KeyValuePair<string,object>)))
            {
                //TYMCZASOWO
                switch (item.RecordType)
                {
                    case RecordTypes.TAircraft:
                        {
                            var dupa = new TAircraft();
                            idKeyValue = new KeyValuePair<string, object>("TailNumber", ((TAircraft)item).TailNumber);
                            break;
                        };
                    case RecordTypes.TDefect:
                        {

                            break;
                        };
                    case RecordTypes.TWorker:
                        {
                            idKeyValue = new KeyValuePair<string, object>("Surname", ((TWorker)item).Surname);
                            break;
                        };


                }
                NotifyStatusChange("Błąd. Nie można usunąć wpisu. Brak Id wpisu.", DatabaseResultCodes.item_not_found); // tu mozna zrobic klase statyczna ktora bedzie przydzialala message :P
                return null;
            }
            else
            {
                var dbProp = item.DbProperty[idKeyValue.Key];
                var idName = String.Format("{0} = {1}", dbProp.DBName, dbProp.DBNameParam);
                listParameter.Add(new SQLiteParameter(dbProp.DBNameParam, idKeyValue.Value));
                var command = $@"SELECT EXISTS(SELECT * FROM {item.TableName} WHERE {idName} LIMIT 1)";
                return new SqlCommandData()
                {
                    Command = command,
                    ParamCollection = listParameter
                };
            }
        }

        private static int GetLastItemId(SQLiteConnection _connection, SQLiteCommand _cmd)
        {
            using (SQLiteConnection connection = _connection)
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (SQLiteCommand cmd = _cmd)
                {
                    int temp = 0;
                    try
                    {
                        cmd.CommandText = @"SELECT last_insert_rowid()";
                        var dupa = cmd.ExecuteScalar();
                        temp = Int32.Parse((string)dupa.ToString());

                    }
                    catch (InvalidCastException ex)
                    {
                        cmd.CommandText = @"SELECT last_insert_rowid()";
                        temp = Convert.ToInt32((cmd.ExecuteScalar() ?? 0));
                    }
                    catch (NullReferenceException)
                    {
                        temp = 0;
                    }
                    catch { temp = 0; }
                    return temp;
                }
            }   
        }

   

        public static List<IDbTable> ReadDatabase(IDbTable _filter)
        {
            
            using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    var qdata = GetSelectQuery(_filter);
                    cmd.CommandText = qdata.Command;
                    foreach (SQLiteParameter par in qdata.ParamCollection) cmd.Parameters.Add(par);
                    var filters = _filter.DbProperty;
                    var result = new List<IDbTable>();
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            IDbTable example = null;
                            switch (_filter.RecordType)
                            {
                                case RecordTypes.TAircraft:
                                    example = new TAircraft();
                                    break;
                                case RecordTypes.TCategory:
                                    example = new TItemCategory();
                                    break;
                                case RecordTypes.TDefect:
                                    example = new TDefect();
                                    break;
                                case RecordTypes.TItemDefect:
                                    example = new TItem() { ItemDefect = true };
                                    break;
                                case RecordTypes.TItemDesc:
                                    example = new TItemDescription();
                                    break;
                                case RecordTypes.TItemNew:
                                    example = new TItem() { ItemDefect = true };
                                    break;
                                case RecordTypes.TItemSubstitute:
                                    
                                    break;
                                case RecordTypes.TWorker:
                                    example = new TWorker();
                                    break;
                                default:
                                    break;
                            
                            }
                            while (rdr.Read())
                            {
                                var temp = example.Duplicate();

                                foreach (KeyValuePair<string, DbObject> kv in filters)
                                {
                                    var _value = rdr[kv.Value.DBName];
                                    if (!dbEmpty.Contains(_value))
                                    {
                                        if ((kv.Value.KeyType == KeyTypes.reference_key_id) ||
                                            ((kv.Value.KeyType == KeyTypes.reference) && (kv.Value.RecordType == RecordTypes.TItemDesc)))
                                        {
                                            switch (kv.Value.RecordType)
                                            {
                                                case RecordTypes.TAircraft:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var aircraft = new TAircraft() { Id = Int32.Parse(_value.ToString()) };
                                                        aircraft = (TAircraft)ReadDatabase(aircraft)[0];
                                                        ((TDefect)temp).Aircraft = (TAircraft) aircraft.Duplicate();
                                                    }
                                                    break;
                                                case RecordTypes.TCategory:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var category = new TItemCategory() { Id = Int32.Parse(_value.ToString()) };
                                                        category = (TItemCategory)ReadDatabase(category)[0];
                                                        ((TItemDescription)temp).Category = (TItemCategory)(category.Duplicate());
                                                    }
                                                    break;
                                                case RecordTypes.TItemDefect:
                                                    {
                                                        var item = new TItem() {
                                                            Id = Int32.Parse(_value.ToString()),
                                                            ItemDefect = true};
                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TDefect)temp).ItemDefect = new TItem(){ ItemDefect = true };
                                                        else
                                                            ((TDefect)temp).ItemDefect = (TItem)itemlst[0].Duplicate();
                                                        break;
                                                    }
                                                case RecordTypes.TItemNew:
                                                    {
                                                        var item = new TItem() { Id = Int32.Parse(_value.ToString()),ItemDefect = false };
                                                    
                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TDefect)temp).ItemNew = new TItem() { ItemDefect = false };
                                                        else
                                                            ((TDefect)temp).ItemNew = (TItem)itemlst[0].Duplicate();
                                                        break;
                                                    };
                                                case RecordTypes.TItemDesc:
                                                    {
                                                        var item = new TItemDescription() { PartNumber = (string)_value };
                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TItem)temp).ItemDescription = new TItemDescription();
                                                        else
                                                            ((TItem)temp).ItemDescription = (TItemDescription)itemlst[0].Duplicate();
                                                        break;
                                                    }
                                                case RecordTypes.TWorker:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var worker = new TWorker() { Id = Int32.Parse(_value.ToString()) };
                                                        worker = (TWorker)ReadDatabase(worker)[0];
                                                        var workerKey = kv.Key.Replace("Id", "");
                                                        ((TDefect)temp)[workerKey] = worker.Clone();
                                                    }
                                                    break;
                                            }
                                        }
                                        else if (kv.Value.KeyType == KeyTypes.primary_key_no_insert ||
                                            kv.Value.KeyType == KeyTypes.numeric_value ||
                                            ((kv.Value.KeyType == KeyTypes.primary_key) && (_filter.RecordType == RecordTypes.TCategory)))
                                        {
                                            temp[kv.Key] = Int32.Parse(_value.ToString());
                                        }
                                        else if ((kv.Value.KeyType == KeyTypes.normal_value)
                                            || (kv.Value.KeyType == KeyTypes.primary_key) ||
                                            ((kv.Value.KeyType == KeyTypes.reference) && (kv.Value.RecordType != RecordTypes.TItemDesc)))
                                        {
                                                temp[kv.Key] = _value;
                                        }
                                        //    temp.Add(kv.Key, _value);
                                    }

                                }
                                result.Add(temp.Duplicate());
                                ///for testing
                            }
                        }
                        catch(Exception ex)
                        {
                            ///cos tu trazeba dopisac
                            return null;
                        }
                        if (result.Count > 0)
                            return result;
                        else
                            return null;
                    }
                    }
                }
         }   

        public static List<IDbTable> ReadWholeDatabase(IDbTable _filter)
        {
            using (SQLiteConnection connection = new SQLiteConnection(@FileConnection))
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(connection))
                {
                    var qdata = GetSelectAllQuery(_filter);
                    cmd.CommandText = qdata.Command;
                    foreach (SQLiteParameter par in qdata.ParamCollection) cmd.Parameters.Add(par);
                    var filters = _filter.DbProperty;
                    var result = new List<IDbTable>();
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        try
                        {
                            IDbTable example = null;
                            switch (_filter.RecordType)
                            {
                                case RecordTypes.TAircraft:
                                    example = new TAircraft();
                                    break;
                                case RecordTypes.TCategory:
                                    example = new TItemCategory();
                                    break;
                                case RecordTypes.TDefect:
                                    example = new TDefect();
                                    break;
                                case RecordTypes.TItemDefect:
                                    example = new TItem() { ItemDefect = true };
                                    break;
                                case RecordTypes.TItemDesc:
                                    example = new TItemDescription();
                                    break;
                                case RecordTypes.TItemNew:
                                    example = new TItem() { ItemDefect = false };
                                    break;
                                case RecordTypes.TItemSubstitute:

                                    break;
                                case RecordTypes.TWorker:
                                    example = new TWorker();
                                    break;
                                default:
                                    break;

                            }
                            while (rdr.Read())
                            {
                                var temp = example.Duplicate();

                                foreach (KeyValuePair<string, DbObject> kv in filters)
                                {
                                    var _value = rdr[kv.Value.DBName];
                                    if (!dbEmpty.Contains(_value))
                                    {
                                        if ((kv.Value.KeyType == KeyTypes.reference_key_id) ||
                                            ((kv.Value.KeyType == KeyTypes.reference) && (kv.Value.RecordType == RecordTypes.TItemDesc)))
                                        {
                                            switch (kv.Value.RecordType)
                                            {
                                                case RecordTypes.TAircraft:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var aircraft = new TAircraft() { Id = Int32.Parse(_value.ToString()) };
                                                        aircraft = (TAircraft)ReadDatabase(aircraft)[0];
                                                        ((TDefect)temp).Aircraft = (TAircraft) aircraft.Duplicate();
                                                    }
                                                    break;
                                                case RecordTypes.TCategory:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var category = new TItemCategory() { Id = Int32.Parse(_value.ToString()) };
                                                        category = (TItemCategory)ReadDatabase(category)[0];
                                                        ((TItemDescription)temp).Category = (TItemCategory)(category.Duplicate());
                                                    }
                                                    break;
                                                case RecordTypes.TItemDefect:
                                                    {
                                                        var item = new TItem()
                                                        {
                                                            Id = Int32.Parse(_value.ToString()),
                                                            ItemDefect = true
                                                        };
                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TDefect)temp).ItemDefect = new TItem() { ItemDefect = true };
                                                        else
                                                            ((TDefect)temp).ItemDefect = (TItem)itemlst[0].Duplicate();
                                                        break;
                                                    }
                                                case RecordTypes.TItemNew:
                                                    {
                                                        var item = new TItem() { Id = Int32.Parse(_value.ToString()), ItemDefect = false };

                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TDefect)temp).ItemNew = new TItem() { ItemDefect = false };
                                                        else
                                                            ((TDefect)temp).ItemNew = (TItem)itemlst[0].Duplicate();
                                                        break;
                                                    };
                                                case RecordTypes.TItemDesc:
                                                    {
                                                        var item = new TItemDescription() { PartNumber = (string)_value };
                                                        var itemlst = ReadDatabase(item);
                                                        if (itemlst == null)
                                                            ((TItem)temp).ItemDescription = new TItemDescription();
                                                        else
                                                            ((TItem)temp).ItemDescription = (TItemDescription)itemlst[0].Duplicate();
                                                        break;
                                                    }
                                                case RecordTypes.TWorker:
                                                    {
                                                        /// TYMCZASOWE ROZWIAZANIE
                                                        var worker = new TWorker() { Id = Convert.ToInt32(_value) };
                                                        worker = (TWorker)ReadDatabase(worker)[0];
                                                        var workerKey = kv.Key.Replace("Id", "");
                                                        ((TDefect)temp)[workerKey] = worker.Clone();
                                                    }
                                                    break;
                                            }
                                        }
                                        else if (kv.Value.KeyType == KeyTypes.primary_key_no_insert ||
                                            kv.Value.KeyType == KeyTypes.numeric_value ||
                                            ((kv.Value.KeyType == KeyTypes.primary_key) && (_filter.RecordType == RecordTypes.TCategory)))
                                        {
                                            temp[kv.Key] = Int32.Parse(_value.ToString());
                                        }
                                        else if ((kv.Value.KeyType == KeyTypes.normal_value)
                                            || (kv.Value.KeyType == KeyTypes.primary_key_no_insert)
                                            || (kv.Value.KeyType == KeyTypes.primary_key) ||
                                            ((kv.Value.KeyType == KeyTypes.reference) && (kv.Value.RecordType != RecordTypes.TItemDesc)))
                                        {

                                            temp[kv.Key] = _value;
                                        }
                                        else
                                        {
                                            int i = 12;
                                            i = i + 2;
                                        }
                                        //    temp.Add(kv.Key, _value);
                                    }

                                }
                                result.Add(temp.Duplicate());
                                ///for testing
                            }
                        }
                        catch (Exception ex)
                        {
                            ///cos tu trazeba dopisac
                            return null;
                        }
                        if(result.Count >0)
                            return result;
                        else
                            return null;
                    }
                }
            }
        }
        



       // public static Dictionary<string, object> ReadDatabase(IDbTable _filter, SQLiteConnection _connection, SQLiteCommand _command)
       // {
            
      //  }

        private static void NotifyStatusChange(string text)
        {
            OnStatusChange?.Invoke("DbManager", new TextEventArgs(text));
        }

        private static void NotifyStatusChange(string text, DatabaseResultCodes code)
        {
            OnStatusChange?.Invoke("DbManager", new TextEventArgs(text));
        }

        private static bool StrEqual(object a, object b)
        {
            if(String.Compare((string)a,(string)b) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ContainsAll(this string source, params string[] values)
        {
            return values.All(x => source.Contains(x));
        }

        public static bool ContainsAny(string source, string[] values)
        {
            foreach(string str in values)
            {
                if(source.Contains(str))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool StrEqual(object a, string b)
        {
            if(String.Compare((string)a,b) == 0)
            {
                return true;
            }
            else
            {
 
                return false;
            }
        }

        public static void Dispose()
        {
            throw new NotImplementedException();
        }

        public class SqlCommandData
        {
            public List<SQLiteParameter> ParamCollection { get; set; }
            public string Command { get; set; }

        }
    }

    

    public enum RecordTypes
    {
        TDefect,
        TItemDesc,
        TItemNew,
        TItemDefect,
        TCategory,
        TAircraft,
        TWorker,
        TClassification,
        Unassigned,
        TItemSubstitute
    }

    public enum QueryTypes
    {
        Insert,
        Update,
        Remove
    }

    public enum DatabaseResultCodes
    {
        item_not_found,
        worker_not_found,
        item_already_exists,
        worker_not_found_inserted,
        item_not_found_inserted,
        no_id_to_update,
        none
    }

    public enum RecordConvertResults
    {
        ok,
        none,
        item_part_number_not_found,
        item_name_not_found,
        worker_surname_not_found,
        worker_surname_found_many
    }

    public enum AddingFlags
    {
        None,
        Add_Missing
    }

    public class TextEventArgs : EventArgs
    {
        public DatabaseResultCodes ResultCode { get; set; }
        public RecordConvertResults RecordConvertResult { get; set; }
        public string Info { get; set; }
        public TextEventArgs(string _info)
        {
            Info = _info;
            ResultCode = DatabaseResultCodes.none;
            RecordConvertResult =  RecordConvertResults.none;
        }

        public TextEventArgs(string _info, DatabaseResultCodes codes)
        {
            Info = _info;
            ResultCode = codes;
            RecordConvertResult = RecordConvertResults.none;
        }
        public TextEventArgs(string _info, RecordConvertResults resCodes)
        {
            Info = _info;
            ResultCode = DatabaseResultCodes.none;
            RecordConvertResult = resCodes;
        }
    }
    
    public class ProgressEventArgs : EventArgs
    {
        public int ActualValue { get; set; }
        public int FinalValue { get; set; }

        public string Info { get; set; }

        public ProgressEventArgs(string _info, int _actVal, int _finalVal)
        {
            Info = _info;
            ActualValue = _actVal;
            FinalValue = _finalVal;
        }
    }


    
 


}
 