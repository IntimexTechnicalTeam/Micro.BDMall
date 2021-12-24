using BDMall.Domain;
using BDMall.Enums;
using Intimex.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.BLL
{
    public class SettingBLL : BaseBLL, ISettingBLL
    {
        public SettingBLL(IServiceProvider services) : base(services)
        {
        }

        public List<KeyValue> GetApproveStatuses()
        {
            var statusList = new List<KeyValue>();
            foreach (ApproveType statusItm in Enum.GetValues(typeof(ApproveType)))
            {
                var status = new KeyValue()
                {
                    Id = ((int)statusItm).ToString(),
                };
                switch (statusItm)
                {
                    case ApproveType.WaitingApprove:
                        status.Text = Resources.Value.WaitingApprove;
                        break;
                    case ApproveType.Pass:
                        status.Text = Resources.Value.Pass;
                        break;
                    case ApproveType.Reject:
                        status.Text = Resources.Value.Reject;
                        break;
                    case ApproveType.Editing:
                        status.Text = Resources.Value.Editing;
                        break;
                    default:
                        status.Text = string.Empty;
                        break;
                }

                statusList.Add(status);
            }
            return statusList;
        }

        public List<KeyValue> GetSupportLanguages()
        {
            var langs = GetSupportLanguage();
            var list = langs.Select(s => new KeyValue
            {
                Text = s.Text,
                Id = ((Language)Enum.Parse(typeof(Language),s.Code)).ToInt().ToString(),
            });

            return list.ToList();
        }
    }
}
