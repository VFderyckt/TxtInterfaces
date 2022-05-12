using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TxtInterfaces.Framework
{
    public static class RaisePropertyChangedHelper
    {
        public static void Raise<O, P>(this PropertyChangedEventHandler ev, O sender, Expression<Func<O, P>> expr)
        {
            ev.Invoke(sender, new PropertyChangedEventArgs(((MemberExpression)expr.Body).Member.Name));
        }
    }
}
