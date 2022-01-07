using BDMall.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Utility
{
    public class TreeUtil
    { /// <summary>
      /// 將樹狀結構的樹轉為列表結構
      /// </summary>
      /// <param name="tree"></param>
      /// <returns></returns>
        public static List<TreeNode> TreeToList(List<TreeNode> tree)
        {
            List<TreeNode> list = new List<TreeNode>();

            list = GenList(tree);

            return list;
        }

        public static List<TreeNode> GenList(List<TreeNode> tree)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (TreeNode item in tree)
            {
                //if (item.IsChange == true)
                //{
                list.Add(item);
                if (item.Children != null)
                {
                    list.AddRange(GenList(item.Children));
                }
                //}
            }

            return list;

        }

    }
}
