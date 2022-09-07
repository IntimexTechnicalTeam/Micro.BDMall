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

        /// <summary>
        /// 枚举转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>()
        {
            List<EnumEntity> list = new List<EnumEntity>();

            foreach (var e in System.Enum.GetValues(typeof(T)))
            {
                EnumEntity m = new EnumEntity();
                object[] objArr = e.GetType().GetField(e.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (objArr != null && objArr.Length > 0)
                {
                    DescriptionAttribute da = objArr[0] as DescriptionAttribute;
                    m.Desction = da.Description;
                }
                m.Id = Convert.ToInt32(e);
                m.Text = e.ToString();
                list.Add(m);
            }
            return list.OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// 枚举转成List(过滤某个值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>(int filterValue)
        {
            List<EnumEntity> list = EnumToList<T>();
            list.RemoveAll(p => p.Id == filterValue);
            return list;
        }
        /// <summary>
        /// 枚举转成List(过滤某些值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>(List<int> filterValueList)
        {
            List<EnumEntity> list = EnumToList<T>();
            list.RemoveAll(p => filterValueList.Contains(p.Id));
            return list;
        }
        /// <summary>
        /// 枚举转成List(过滤某些值)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<EnumEntity> EnumToList<T>(int[] filterValueList)
        {
            List<EnumEntity> list = EnumToList<T>();
            list.RemoveAll(p => filterValueList.Contains(p.Id));
            return list;
        }


        /// <summary>
        /// 根据枚举的值获取枚举的名称
        /// </summary>
        /// <param name="Status"></param>
        /// <returns></returns>
        public static string GetEnumName(Type enumType, object value)
        {
            return System.Enum.GetName(enumType, value);
        }
        //返回指定枚举中是否存在具有指定值的常数的指示。
        //ShoppingCarEnum.CarType enumcartype = ShoppingCarEnum.CarType.购物车;
        //            if (Enum.IsDefined(typeof(ShoppingCarEnum.CarType), cartype))
        //            {
        //                enumcartype = (ShoppingCarEnum.CarType)cartype;
        //            }

        /// <summary>
        /// 获取枚举描述信息
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumEntity">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetDescription<T>(T enumEntity)
        {
            string summary = string.Empty;

            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new Exception("类型T必须为枚举类型");
            }
            var field = type.GetField(System.Enum.GetName(type, enumEntity));
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute != null)
            {
                summary = attribute.Description;

            }
            return summary;
        }

        /// <summary>
        /// 获取枚举的默认值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="enumEntity">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetDefaultValue<T>(T enumEntity)
        {
            string summary = string.Empty;

            Type type = typeof(T);

            if (!type.IsEnum)
            {
                throw new Exception("类型T必须为枚举类型");
            }
            var field = type.GetField(System.Enum.GetName(type, enumEntity));
            DefaultValueAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DefaultValueAttribute)) as DefaultValueAttribute;
            if (attribute != null)
            {
                summary = attribute.Value.ToString();

            }
            return summary;
        }

    }


    public class EnumEntity
    {
        /// <summary>  
        /// 枚举的描述  
        /// </summary>  
        public string Desction { set; get; }

        /// <summary>  
        /// 枚举名称  
        /// </summary>  
        public string Text { set; get; }

        /// <summary>  
        /// 枚举对象的值  
        /// </summary>  
        public int Id { set; get; }
    }
}
