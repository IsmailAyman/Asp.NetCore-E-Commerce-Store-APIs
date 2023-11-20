using EdgeProject.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Core.Specifications
{
    public class ProductSpecification:BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams productSpec) :
            base(P=>
            (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search)) &&
            (!productSpec.Brandid.HasValue || P.ProductBrandId == productSpec.Brandid) &&
            (!productSpec.Typeid.HasValue || P.ProductTypeId == productSpec.Typeid))
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);

            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break; 
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            ApplyPagination(productSpec.PageSize*(productSpec.PageIndex-1), productSpec.PageSize);
        }

        public ProductSpecification(int id):base(P=>P.Id == id)
        {
            Includes.Add(P => P.ProductBrand);
            Includes.Add(P => P.ProductType);
        }
    }
}
