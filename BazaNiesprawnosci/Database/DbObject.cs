
namespace BazaNiesprawnosci
{


    public class DbObject
    {

        private string dBName = "" ;
                 
        private string dBRootName = "";
        private string dBTableRootName = "";
        private bool isNullable = true;
        private KeyTypes keyType = KeyTypes.normal_value;
        private FilterTypes filterType = FilterTypes.single;
        private RecordTypes recordType = RecordTypes.Unassigned;

        
        
        /// <summary>
        /// Nazwa wpisu w bazie danych, lub odwołania do wpisu.
        /// </summary>
        public string DBName { get => dBName; set => dBName = value; }
        public string DBNameParam
        {
            get => "@Param" + DBName;
        }
        /// <summary>
        /// Nazwa wpisu przechowującego informacje w pierwotnej tabeli
        /// </summary>
        public string DBRootName { get => dBRootName; set => dBRootName = value; }
        /// <summary>
        /// Nazwa tabeli pierwotnej w której znajduje się wpis
        /// </summary>
        public string DBTableRootName { get => dBTableRootName; set => dBTableRootName = value; }
        public KeyTypes KeyType { get => keyType; set => keyType = value; }
        public FilterTypes FilterType { get => filterType; set => filterType = value; }
        public RecordTypes RecordType { get => recordType; set => recordType = value; }
        public bool IsInsertable
        {
            get
            {
                if ((KeyType == KeyTypes.aux_value)
                    || (KeyType == KeyTypes.primary_key_no_insert)
                        || (KeyType == KeyTypes.primary_key_stored))
                {
                    return false;
                }
                else return true;
            }
        }
        public bool IsUpdateable
        {
            get
            {
                if ((KeyType == KeyTypes.primary_key_no_insert) || (KeyType == KeyTypes.aux_value))
                    return false;
                else return true;
            }
        }

        public bool IsSelectable
        {
            get
            {
                if (KeyType == KeyTypes.aux_value)
                    return false;
                else
                    return true;
            }
        }

        public bool IsNullable { get => isNullable; set => isNullable = value; }
    }

    public enum KeyTypes
    {
        /// <summary>
        /// Odwołanie do Id w wpisu w innej tablicy
        /// </summary>
        reference_key_id,
        /// <summary>
        /// Odwołanie do wartości komórki w innej tablicy posiadające tę samą wartość
        /// </summary>
        reference,
        /// <summary>
        /// Zapasowe id, użyte w razie potrzeby zmiany id wpisu
        /// </summary>
        primary_key_stored,
        /// <summary>
        /// Id wpisu definiowane przez uzytkownika
        /// </summary>
        primary_key,
        /// <summary>
        /// Numeryczne id definiowane przez bazę danych
        /// </summary>
        primary_key_no_insert,
        /// <summary>
        /// Zwykła wartość danej komórki
        /// </summary>
        normal_value,
        /// <summary>
        /// Pomocnicza wartość do wyszukiwania wpisów
        /// </summary>
        aux_value,
        /// <summary>
        /// Wartosc określająca caly wpis w tablicy 
        /// </summary>
        table_value,
        /// wartosc normalna numeryczna
        numeric_value


    }

    public enum FilterTypes
    {
        number_larger_than,
        number_lower_than,
        list,
        single_contains,
        single
    }

  
}