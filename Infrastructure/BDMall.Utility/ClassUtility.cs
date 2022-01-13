using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class ClassUtility
    {
        /// <summary>
        /// 獲取當前方法的名稱
        /// </summary>
        /// <returns></returns>
        public static string GetMethodName()
        {
            try
            {
                var method = new StackFrame(1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息
                var property = (
                            from p in method.DeclaringType.GetProperties(
                            BindingFlags.Instance |
                            BindingFlags.Static |
                            BindingFlags.Public |
                            BindingFlags.NonPublic)
                            where p.GetGetMethod(true) == method || p.GetSetMethod(true) == method
                            select p).FirstOrDefault();
                return property == null ? method.Name : property.Name;
            }
            catch (Exception ex)
            {
                return "";
                // throw;
            }

        }

        /// <summary>
        /// 獲取當前類類型的全稱
        /// </summary>
        /// <returns></returns>
        public static string GetTypeFullName()
        {
            var method = new StackFrame(1).GetMethod(); // 这里忽略1层堆栈，也就忽略了当前方法GetMethodName，这样拿到的就正好是外部调用GetMethodName的方法信息

            return method.DeclaringType.FullName;
        }

        /// <summary>
        /// 獲取調用當前方法的類類型的全稱
        /// </summary>
        /// <returns></returns>
        public static string GetCallerTypeFullName()
        {
            var method = new StackFrame(2).GetMethod(); // 这里忽略2层堆栈 

            return method.DeclaringType.FullName;
        }


        /// <summary>
        /// 复制对象的属性值
        /// </summary>
        /// <typeparam name="TSource">数据源对象类型</typeparam>
        /// <typeparam name="TResult">返回结果数据对象类型</typeparam>
        /// <param name="target">数据源对象</param>
        /// <returns></returns>
        public static TResult CopyValue<TSource, TResult>(TSource target)
        {
            TResult result = default(TResult);

            if (target == null)
            {
                return result;
            }

            object propertyValueObject;
            string classPropertyName = string.Empty;
            string classPropertyValue = string.Empty;
            string dataBaseColumnName = string.Empty;


            result = Activator.CreateInstance<TResult>();


            Type pInfoType_source = target.GetType();
            PropertyInfo[] pInfos_source = pInfoType_source.GetProperties();
            PropertyInfo pInfo_source;

            Type pInfoType = result.GetType();
            PropertyInfo[] pInfos = pInfoType.GetProperties();
            PropertyInfo pInfo;

            if (pInfos.Length == 0)
            {
                return result;
            }

            for (int i = 0; i < pInfos_source.Length; i++)
            {
                propertyValueObject = null;
                pInfo_source = pInfos_source[i];
                classPropertyName = pInfo_source.Name;

                for (int j = 0; j < pInfos.Length; j++)
                {
                    pInfo = pInfos[j];
                    if (pInfo_source.Name.Trim() == pInfo.Name.Trim())
                    {
                        Type prop = pInfo.PropertyType;



                        Type prop_s = pInfo_source.PropertyType;



                        #region By justin

                        if (prop_s.ToString() == "System.Guid" && prop.ToString() == "System.String")
                        {
                            propertyValueObject = pInfo_source.GetValue(target, null).ToString();
                        }
                        else if (prop.ToString() == "System.Guid" && prop_s.ToString() == "System.String")
                        {
                            propertyValueObject = new Guid(pInfo_source.GetValue(target, null).ToString());
                        }
                        else
                        {
                            propertyValueObject = pInfo_source.GetValue(target, null);

                        }
                        #endregion

                        try
                        {
                            pInfo.SetValue(result, propertyValueObject, null);
                        }
                        catch
                        {

                        }

                        break;
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// 将Model实体值赋予LingQ实体
        /// </summary>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static TTarget CopyValue<TTarget, TSource>(TTarget target, TSource source)
        {
            TTarget targetObject = target;

            if (source == null)
            {
                return targetObject;
            }

            object propertyValueObject;
            string classPropertyName = string.Empty;
            string classPropertyValue = string.Empty;
            string dataBaseColumnName = string.Empty;

            string errPropertyName = string.Empty;


            try
            {
                Type sourceType = source.GetType();
                PropertyInfo[] sourceProperties = sourceType.GetProperties();
                PropertyInfo pInfo_source;

                Type targetType = targetObject.GetType();
                PropertyInfo[] targetProperties = targetType.GetProperties();
                PropertyInfo pInfo_target;

                if (targetProperties.Length == 0)
                {
                    return targetObject;
                }

                for (int i = 0; i < targetProperties.Length; i++)
                {
                    propertyValueObject = null;
                    pInfo_target = targetProperties[i];
                    classPropertyName = pInfo_target.Name;

                    for (int j = 0; j < sourceProperties.Length; j++)
                    {
                        pInfo_source = sourceProperties[j];
                        //获得目标属性
                        errPropertyName = pInfo_target.Name.Trim();
                        if (pInfo_source.Name.Trim().ToLower() == pInfo_target.Name.Trim().ToLower())
                        {
                            //提取来源属性值
                            propertyValueObject = pInfo_source.GetValue(source, null);
                            //将来源属性值赋予目标属性
                            pInfo_target.SetValue(targetObject, propertyValueObject, null);

                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //infoObject = null;
                string message = errPropertyName + ":" + ex.Source;
                throw new Exception(message, ex);
            }

            return targetObject;
        }
        public static void SetValue<T>(T obj, string key, object value)
        {
            Type targetType = obj.GetType();
            PropertyInfo[] targetProperties = targetType.GetProperties();
            foreach (var item in targetProperties)
            {
                if (item.Name.ToLower() == key.Trim().ToLower())
                {
                    if (value.GetType().Name == typeof(Boolean).Name)
                    {
                        value = value.ToString();
                    }
                    //将来源属性值赋予目标属性
                    item.SetValue(obj, value, null);
                    break;
                }
            }
        }
        public static void SetValue<T, T2>(T obj, Dictionary<string, object> values)
        {
            Type targetType = obj.GetType();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            var keys = values.Keys;
            foreach (var item in targetProperties)
            {
                if (keys.Contains(item.Name.Trim()))
                {
                    if (item.PropertyType.Name == typeof(Boolean).Name)//轉換布爾值
                    {
                        var val = false;
                        bool.TryParse(values[item.Name].ToString(), out val);
                        item.SetValue(obj, val, null);
                    }
                    else
                    {
                        //将来源属性值赋予目标属性
                        item.SetValue(obj, values[item.Name], null);
                    }

                }
            }
        }
    }
}
