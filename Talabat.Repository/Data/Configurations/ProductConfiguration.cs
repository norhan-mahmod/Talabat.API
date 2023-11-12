using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(product => product.Name).IsRequired().HasMaxLength(100);
            builder.Property(product => product.Description).IsRequired();
            builder.Property(product => product.PictureUrl).IsRequired();
            builder.Property(product => product.Price).IsRequired().HasColumnType("decimal(18,2)");

            builder.HasOne(product => product.ProductBrand)
                   .WithMany()
                   .HasForeignKey(product => product.ProductBrandId);
            builder.HasOne(product => product.ProductType)
                   .WithMany()
                   .HasForeignKey(product => product.ProductTypeId);
        }
    }
}
