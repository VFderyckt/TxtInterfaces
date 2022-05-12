using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using TxtInterfaces.Data;
namespace TxtInterfaces.Classes
{
    public class StringEmptyConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //return string.IsNullOrEmpty(value.ToString()) ? parameter : value;
            return value;
        }

        public object ConvertBack(
              object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.IsNullOrEmpty(value.ToString()) ? parameter : value;
            //throw new NotSupportedException();
        }

    }
    public class RevBoolConverter : IValueConverter
    {
        //reverse boolean
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(
              object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanConverter is a OneWay converter.");
        }

    }
    public class RevBooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(values.Any(a => (bool)a==false));
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("RevBooleanAndConverter is a OneWay converter.");
        }
    }
    public class BooleanAndConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (values.Any(a => (bool)a == false));
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BooleanAndConverter is a OneWay converter.");
        }
    }
    public class BoolToVisConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return values.Any(a => (bool)a == false)? Visibility.Visible:Visibility.Collapsed;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("BoolToVisConverter is a OneWay converter.");
        }
    }
    public class EnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value is sp_Interface_GetSAPvsTxtResult?string.IsNullOrEmpty(((sp_Interface_GetSAPvsTxtResult)value).SAPReqSegment):string.IsNullOrEmpty((string)value);
        }
        public object ConvertBack(
              object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("EnableConverter is a OneWay converter.");
        }
    }
    public class CmdTxtConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            List<string> _lst = new List<string> { "Add", "Update" };
            return _lst[(value is bool) ? ((bool)value ? 0 : 1) : string.IsNullOrEmpty((string)value)?0:1]; 
        }
        public object ConvertBack(
              object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("CmdTxtConverter is a OneWay converter.");
        }
    }
    public class GetDevPlantConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string _res = "";
            if (values[0] is bool && values[6] != null)
            { 
                _res = "Force To Default Plant ";
                if (!(bool)values[0])
                {
                    List<sp_Interface_GetSAPvsTxtResult> _lst = (List<sp_Interface_GetSAPvsTxtResult>)values[1];
                    _res += "(" + _lst.Where(a => a.Fcst_model == values[2].ToString() && a.TxtChannel == values[3].ToString() &&
                     a.TxtOrderType == values[4].ToString() && a.TxtCollection == values[5].ToString() && (bool)a.ExportToSAP).Select(a => a.SAPPlant).FirstOrDefault() + ")";
                }
                else
                {
                    _res += values[6].ToString() != "" ? "(" + values[6].ToString() + ")":"";
                }
            }
            return _res;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("GetDevPlantConverter is a OneWay converter.");
        }
    }
}
