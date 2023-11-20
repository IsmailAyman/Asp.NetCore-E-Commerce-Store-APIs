using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Specifications
{
    public class ProductSpecParams
    {
        private int MaxPageSize = 10;
        private int pageSize = 5;
        public int PageSize {
            get { return pageSize; } 
            set { pageSize = value > MaxPageSize ? MaxPageSize : value; } 
            }
        public int PageIndex { get; set; } = 1; 
        public string? Sort {  get; set; }
        public int? Brandid { get; set; }
        public int? Typeid { get; set; }

        private string search {  get; set; }
        public string? Search { get { return search; } set { search = value.ToLower();} }
    }
}
