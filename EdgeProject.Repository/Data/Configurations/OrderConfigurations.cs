using EdgeProject.Core.Entities.Order_Aggregation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdgeProject.Repository.Data.Configurations
{
    public class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(O => O.ShippingAddress, SA => SA.WithOwner());

            builder.Property(O=>O.Status)
                .HasConversion(OS=>OS.ToString(),
                OS=> (OrderStatus) Enum.Parse(typeof(OrderStatus), OS));
            builder.Property(O=>O.SubTotal).HasColumnType("decimal(18,2)");

            builder.HasOne(O => O.DeliveryMethod)
                .WithMany().OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(O=>O.Items)
                .WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
