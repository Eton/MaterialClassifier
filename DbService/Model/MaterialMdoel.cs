using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService.Model
{
    public class MaterialMdoel : Material
    {
        public DateTime LastUsedTime { get; set; }
    }
}
