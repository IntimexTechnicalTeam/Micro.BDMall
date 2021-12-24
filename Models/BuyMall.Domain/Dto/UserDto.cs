using BDMall.Enums;
using BDMall.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Domain
{
    public class UserDto
    {
        public Guid Id { get; set; }
    
        public string Account { get; set; }

        public string Email { get; set; }

        //[Column(TypeName = "varchar")]
        public string Password { get; set; }

        public string Name { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsTempUser { get; set; }

        public string Mobile { get; set; }
     
        public LoginType LoginType { get; set; }
    
        //public AppTypeEnum ComeFrom { get; set; }

        //public string ComeFromDisplay
        //{
        //    get
        //    {

        //        return ComeFrom.ToString();
        //    }
        //}

        public bool OptOut { get; set; }


        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

      
        public Language Language { get; set; }

   
        public string DateTimeFormat { get; set; }

        public Guid MerchantId { get; set; }

       
        public DateTime LastLogin { get; set; }

     
        public bool IsMerchant
        {
            get
            {
                return MerchantId != Guid.Empty;
            }
        }
    
        public string MerchantName { get; set; }

        //public SimpleCurrency Currency { get; set; }

        public bool IsLogin { get; set; }

        public string IPAddress { get; set; }

        public string LoginSerialNO { get; set; }

        public string AppNO { get; set; }

        public DateTime LastAccessTime { get; set; }

        public string Token { get; set; }
    }
}
