using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intimex.Common
{
    public class EnumHelper
    {
        public static List<KeyValue> EnumToList<T>()
        {
            var list = new List<KeyValue>();

            foreach (var e in System.Enum.GetValues(typeof(T)))
            {
                var m = new KeyValue();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Text = da.Description;
                }
                m.Id = e.ToString();
                m.Text = e.ToString();
                list.Add(m);
            }
            return list.OrderBy(p => p.Id).ToList();
        }
    }
}
