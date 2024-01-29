using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Windows.Data;
using BazaNiesprawnosci.Database;

namespace BazaNiesprawnosci
{
    public class TextNumericConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                if(((int)value) == 0)                
                    return "";                
                else
                    return (object)value.ToString();
            }
            else
            {
                return "";
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                int intValue = 0;
                bool isNumeric;
                if (value is int)
                {
                    return value.ToString();
                }
                if (isNumeric = Int32.TryParse((string)value, out intValue))
                {
                    return (object)intValue;
                }
                else
                {
                    return (object)0;
                }
            }
            else
            {
                return (object)0;
            }
        }
    }

    public class AircraftConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TAircraft)
            {
                return ((TAircraft)value).TailNumber;
            }
            else
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter is TAircraft)
            {
                ((TAircraft)parameter).TailNumber = (string)value;
                return ((TAircraft)parameter);
            }
            else
            {
                return "";
            }
        }
    }

    public class CollectionListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                System.Collections.IList tempList = (List<TItemCategory>)value;
                return tempList;
            }
            else
            {
                return null;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                return ((System.Collections.IList)value).Cast<TItemCategory>().ToList<TItemCategory>();
            }
            else
            {
                return null;
            }
        }
    }

    public class FlightHoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                if (((int)value) == 0)
                    return "";
                else
                    return (object)value.ToString()+" FH";
            }
            else
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                value = ((string)value).Replace(" FH", "");
                bool isNumeric;
                if (value is int)
                {
                    return value.ToString();
                }
                if (isNumeric = Int32.TryParse((string)value, out int intValue))
                {
                    return (object)intValue;
                }
                else
                {
                    return (object)0;
                }
            }
            else
            {
                return (object)0;
            }
        }
    }

    public class CategoryFullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TItemCategory)
            {
                if ((value) == null)
                    return "";
                else
                    return (string)value.ToString();
            }
            else
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                
                string svalue = (string)value;
                svalue.Replace(":", "");
                svalue.Replace(" ", "");
                int cnt = svalue.Length;
                
                string temp = svalue.Substring(0, 2);
                string temp2 = svalue.Substring(2, cnt-2);
                return new TItemCategory(int.Parse(temp), temp2);
            }
            else
            {
                return "";
            }
        }
    }

    public class CategoryIdConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is TItemCategory)
            {
                if ((value) == null)
                    return "";
                else
                    return (string)value.ToString();
            }
            else
            {
                return "";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                string svalue = (string)value;
                string temp = svalue.Substring(0, 2);
                return new TItemCategory(int.Parse(temp), MainSettings.Default.TCategory.First(s => (s.Id == int.Parse(temp))).Name);
            }
            else
            {
                return "";
            }
        }
    }
}
