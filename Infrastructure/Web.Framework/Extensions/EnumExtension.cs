using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class EnumExtension
    {
        public static int ToInt(this Enum e)
        {
            return Convert.ToInt32(e);
        }

        public static bool IsDefinedEnum(this Enum e)
        {
            Array values = Enum.GetValues(e.GetType());

            foreach (object value in values)
            {
                if (value.Equals(e))
                    return true;
            }

            return false;
        }

        public static T ToEnum<T>(this object obj)
        { 
            var t = (T)Enum.Parse(typeof(T), obj.ToString());
            return t;
        }
    }
}
