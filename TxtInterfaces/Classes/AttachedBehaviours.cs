using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TxtInterfaces.Classes
{
    public static class AttachedBehaviours
    {
        public static DependencyProperty LoadedCommandProperty
          = DependencyProperty.RegisterAttached(
               "LoadedCommand",
               typeof(ICommand),
               typeof(AttachedBehaviours),
               new PropertyMetadata(null, OnLoadedCommandChanged));

        private static void OnLoadedCommandChanged
             (DependencyObject depObj, DependencyPropertyChangedEventArgs e)
        {
            if (depObj is FrameworkElement frameworkElement && e.NewValue is ICommand)
            {
                frameworkElement.Loaded
                  += (o, args) =>
                  {
                      (e.NewValue as ICommand).Execute(null);
                  };
            }
        }

        public static ICommand GetLoadedCommand(DependencyObject depObj)
        {
            return (ICommand)depObj.GetValue(LoadedCommandProperty);
        }

        public static void SetLoadedCommand(
            DependencyObject depObj,
            ICommand value)
        {
            depObj.SetValue(LoadedCommandProperty, value);
        }
    }
}
