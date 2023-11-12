using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        // we have 4 types of filteration
        // 1. filter with brandId && typeId
            // where(product => product.ProductBrandId == productSpec.BrandId
            //               && product.ProductTypeId == productSpec.TypeId)
        // 2. filter with typeId only
            // where(product => true && product.ProductTypeId == productSpec.TypeId)
        // 3. filter with brandId only
            // where(product => product.ProductBrandId == productSpec.BrandId && true)
        // 4. no filter (get all products)
            // where(product => true && true)
        public ProductSpecification(ProductSpecParams productSpec)
            : base( product =>
                        (!productSpec.BrandId.HasValue || product.ProductBrandId == productSpec.BrandId) &&
                        (!productSpec.TypeId.HasValue || product.ProductTypeId == productSpec.TypeId) &&
                        (string.IsNullOrEmpty(productSpec.Search) || product.Name.ToLower().Contains(productSpec.Search))
                  )
        {
            Includes.Add(product => product.ProductBrand);
            Includes.Add(product => product.ProductType);
            if (!string.IsNullOrEmpty(productSpec.Sort))
            {
                switch (productSpec.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(product => product.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(product => product.Price);
                        break;
                    default:
                        AddOrderBy(product => product.Name);
                        break;
                }
            }
            ApplyPagination(productSpec.PageSize * (productSpec.PageIndex - 1), productSpec.PageSize);
        }
        public ProductSpecification(int id) : base(Product => Product.Id == id)
        {
            Includes.Add(product => product.ProductBrand);
            Includes.Add(product => product.ProductType);
        }
    }
}
