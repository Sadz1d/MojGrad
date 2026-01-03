//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Market.Domain.Entities.Reports;

//public class ProblemCategoryConfiguration
//    : IEntityTypeConfiguration<ProblemCategoryEntity>
//{
//    public void Configure(EntityTypeBuilder<ProblemCategoryEntity> builder)
//    {
//        builder.ToTable("ProblemCategories");

//        builder.Property(x => x.Name)
//            .IsRequired()
//            .HasMaxLength(ProblemCategoryEntity.Constraints.NameMaxLength);

//        builder.Property(x => x.Description)
//            .HasMaxLength(ProblemCategoryEntity.Constraints.DescriptionMaxLength);
//    }
//}
using Market.Domain.Entities.Reports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProblemCategoryConfiguration
    : IEntityTypeConfiguration<ProblemCategoryEntity>
{
    public void Configure(EntityTypeBuilder<ProblemCategoryEntity> builder)
    {
        builder.ToTable("ProblemCategories");

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(ProblemCategoryEntity.Constraints.NameMaxLength);

        builder.Property(x => x.Description)
            .HasMaxLength(ProblemCategoryEntity.Constraints.DescriptionMaxLength);

        builder.Property(x => x.IsEnabled)
            .HasDefaultValue(true);
    }
}
