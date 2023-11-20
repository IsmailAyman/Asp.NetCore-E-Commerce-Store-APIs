using EdgeProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification:BaseSpecification<Product>
    {

        public ProductWithFilterationForCountSpecification(ProductSpecParams productSpec) :
           base(P =>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
           (!productSpec.Brandid.HasValue || P.ProductBrandId == productSpec.Brandid) &&
           (!productSpec.Typeid.HasValue || P.ProductTypeId == productSpec.Typeid))
        {
         
        }
    }
}
