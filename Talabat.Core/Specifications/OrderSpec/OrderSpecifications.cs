using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecifications : BaseSpecification<Order>
    {
        public OrderSpecifications(string email)
            : base(order => order.BuyerEmail == email)
        {
            Includes.Add(order => order.DeliveryMethod);
            Includes.Add(order => order.Items);
            AddOrderByDescending(order => order.OrderDate);
        }
        public OrderSpecifications(int id , string email)
            : base(order => order.BuyerEmail == email && order.Id == id)
        {
            Includes.Add(order => order.DeliveryMethod);
            Includes.Add(order => order.Items);
        }
    }
}
