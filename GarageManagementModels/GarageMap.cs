using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageManagementModels
{
    public class GarageMap
    {
        public GarageMap(EntityTypeBuilder<Garage> entityTypeBuilder) {
            entityTypeBuilder.HasKey(x => x.Id);
            entityTypeBuilder.ToTable("Garage");

            entityTypeBuilder.Property(x => x.Id).HasColumnName("id");
            entityTypeBuilder.Property(x => x.Name).HasColumnName("name");
            entityTypeBuilder.Property(x => x.Address).HasColumnName("address");

        
        }
    }
}
