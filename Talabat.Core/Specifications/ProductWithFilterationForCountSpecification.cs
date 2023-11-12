using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams productSpec)
            : base(product =>
                        (!productSpec.BrandId.HasValue || product.ProductBrandId == productSpec.BrandId)&&
                        (!productSpec.TypeId.HasValue || product.ProductTypeId == productSpec.TypeId)&&
                        (string.IsNullOrEmpty(productSpec.Search) || product.Name.ToLower().Contains(productSpec.Search))
                  )
        {
            
        }
    }
}
