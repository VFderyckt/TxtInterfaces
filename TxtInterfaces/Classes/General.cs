using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TxtInterfaces.Data;
namespace TxtInterfaces.Classes
{
    class General
    {
        public static void Enable(bool on) { MainWindow.ChangeEnabled(on); }
        public static void Curs(Cursor cur) { MainWindow.ChangeCursor(cur); }
        public static void Content(string content) { MainWindow.ChangeContent(content); }
        public static sp_Interface_GetParamsResult parameters;
        public static void ChangeActivity(Boolean On)
        {
            Enable(On);
            Curs(On ? Cursors.Arrow : Cursors.Wait);
        }
        public static int CurrentFFC = 0;
    }
}
