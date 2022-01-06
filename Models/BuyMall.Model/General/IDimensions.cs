using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDMall.Model
{
    interface IDimensions
    {
        decimal Width { get; set; }
        decimal Heigth { get; set; }
        decimal Length { get; set; }
        int Unit { get; set; }
    }
}
