using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TxtInterfaces.ViewModel;

namespace TxtInterfaces
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static MainViewModel _mainViewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainViewModel;
        }
        void NBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            throw new NotImplementedException();
        }
        void NBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        static public void ChangeEnabled(bool ena)
        {
            _mainViewModel.Enabled = ena;
        }
        static public void ChangeCursor(Cursor cur)
        {
            _mainViewModel.Curs = cur;
        }
        static public void ChangeContent(string content)
        {
            _mainViewModel.Content = content;
        }
    }
}
