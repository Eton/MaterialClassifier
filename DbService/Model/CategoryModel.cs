using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbService.Model
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string MainCategory { get; set; }
        public string SubCategory { get; set; }
        public int MaterialCount { get; set; }
    }
}
