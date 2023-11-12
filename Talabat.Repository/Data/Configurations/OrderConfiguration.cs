using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregation;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // to make order has all of Address Properties
            // relation between order and shipping address 1-1 total participation from two sides
            // which means they will be in one entity
            builder.OwnsOne(order => order.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            // order status will be stored in database as string and here its value int
            // this to make convert between these types when deal with it
            builder.Property(order => order.Status)
                   .HasConversion(
                            orderStatus => orderStatus.ToString(),
                            orderStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), orderStatus));
            builder.Property(order => order.SubTotal).HasColumnType("decimal(18,2)");
            builder.HasOne(order => order.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
