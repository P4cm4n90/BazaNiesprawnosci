using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BazaNiesprawnosci.Database
{
    static class DbExcelWrap
    {
        public delegate void DataBaseStatusHandler(object sender, TextEventArgs eargs);
        public static event DataBaseStatusHandler OnStatusChange;

        public delegate void ProgressStatusHandler(object sender, ProgressEventArgs eargs);
        public static event ProgressStatusHandler OnProgressChange;

        private const string ActionName = "Wczytywanie bazy z excel";

        private static int vPartNumber = 6;
        private static int vAltPartNumber = 7;
        private static int vName = 4;
        private static int vJim = 2;
        private static Excel.Worksheet ExcSheet;
        private static string[] ForbiddenNames = {"multim", "żarówka" ,"zawleczka", "łożysko", "śruba", "wkręt", "klej", "tester", "rurka",
            "uszczelniacz", "podkładka", "końcówka", "grzejnik", "pas", "podstawka", "torba", "śrubokręt", "bit","klucz", "nasadka", "wkrętak",
            "zestaw", "komputer", "osłona", "sworzeń", "gum","kula", "kulka","zaciskacz", "taśma", "kołnierz", "metalizacja", "łożysko", "pompka", "pompa",
        "łączówka", "bezpiecznik", "pierścień", "zawór", "zawor", "regulator", "nakrętka", "wąż", "wkładka", "spręż","uszczelka", "kołek", "zacisk", "szyba","koło",
        "amortyzator","łopata", "tarcza", "podnośnik", "kątomierz", "narzęd", "zatrzask", "stożek", "rolka", "przegub", "uszczelniak", "uszczelka",
        "obejma", "mocowanie", "pokrowiec", "filtr", "platforma", "metalizacja", "stanowisko", "rura", "tłok", "imadło", "tuleja", "schody", "wózek",
        "nakrętka", "śmigło", "wkładka", "bloka", "bloku", "schody", "boroskop", "pierścień", "pokrowiec", "podpora", "pomiar", "zworka", "nalepka", "mocowanie",
        "nici", "komplet", "pochłaniacz", "wybijacz", "wylot", "wlot", "interfejs", "karta", "element", "ustalacz", "tuleja", "lakier", "farba", "pędz", "zamek",
        "nit", "zagłuszka", "pokrętło", "krążek", "oddziela","redu", "dysz","zawl","baza", "przekładnia" };
        private static string ChooseExcelFile()
        {
            OpenFileDialog _file = new OpenFileDialog();
            _file.Filter = "Office Files|*.xls";
            if (_file.ShowDialog() == DialogResult.OK)
            {
                string _fullPath = _file.FileName;
                string _fileName = _file.SafeFileName;
                return _fullPath.Replace(_fileName, "") + _file.SafeFileName;
            }
            else
            {
                return string.Empty;
            }

        }

        public static async Task<List<TItemDescription>> OpenExcelFile()
        {
           
                var _filepath = ChooseExcelFile();
                if (string.IsNullOrWhiteSpace(_filepath))
                    return null;
                var ExcApplication = new Excel.Application();
                var ExcWorkbook = ExcApplication.Workbooks.Open(_filepath, 0, false, 5, "", "",
                false, Excel.XlPlatform.xlWindows, "", true, false,
                0, true, false, false);
                ExcSheet = (Excel.Worksheet)ExcWorkbook.Worksheets["CASA"];
                var itemsList = new List<TItemDescription>();

                int rows = 3866;

            try { 
                var tasks = new List<Task>();
            
                for (int m = 5; m < rows; m++)
                {
                    var i = new int();
                    i = m;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        var item = new TItemDescription
                        {
                            Jim = ReadCell(i, vJim),
                            PartNumber = ReadCell(i, vPartNumber),
                            Name = ReadCell(i, vName)
                        };
                        var altPartNumber = ReadCell(i, vAltPartNumber);
                        bool copy = true;
                        foreach (string forb in ForbiddenNames)
                        {
                            if (item.Name.ToLower().Contains(forb) || String.IsNullOrWhiteSpace(item.Name))
                               
                                copy = false;
                        }
                        if (copy)
                        {
                            itemsList.Add((TItemDescription)item.Duplicate());

                            if (!String.IsNullOrWhiteSpace(altPartNumber))
                            {
                                itemsList.Add((TItemDescription)item.Duplicate());
                                item.PartNumber = altPartNumber;

                            }
                        }
                    }));
                    if ((i % 50 == 0) || (i==3866))
                    {
                        Task.WaitAll(tasks.ToArray());
                        tasks.Clear();
                    }
                   // Task.WaitAll(tasks.ToArray());
                   
                }
                Task.WaitAll(tasks.ToArray());
                return itemsList;
            }
            catch (Exception e)
            {
                ExcSheet.Delete();
                return null;
            }
            finally
            {
                ExcSheet.Delete();
                ExcWorkbook.Close();
                ExcApplication.Quit();
                Marshal.ReleaseComObject(ExcSheet);
                Marshal.ReleaseComObject(ExcWorkbook);
                Marshal.ReleaseComObject(ExcApplication);
            }


        }

        private static string ReadCell(int k, int w)
        {
            var data  = ExcSheet.Cells[k, w].Value ?? "";
            return data.ToString();

        }

        private static void NotifyProgressChange(string text, int valAct, int valFinal)
        {
            OnProgressChange?.Invoke("DbExcelWrap", new ProgressEventArgs(text,valAct,valFinal));
        
        }
    }
}
