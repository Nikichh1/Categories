using Categories.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Categories.Migrations;

[DbContext(typeof(ApplicationDbContext))]
public partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.10")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("Categories.Models.Category", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<int>("DisplayOrder").HasColumnType("int");
            b.HasKey("Id");
            b.ToTable("Categories");
        });

        modelBuilder.Entity("Categories.Models.Product", b =>
        {
            b.Property<int>("Id").ValueGeneratedOnAdd().HasColumnType("int")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
            b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");
            b.Property<int>("IdCategory").HasColumnType("int");
            b.HasKey("Id");
            b.HasIndex("IdCategory");
            b.ToTable("Products");
        });

        modelBuilder.Entity("Categories.Models.Product", b =>
        {
            b.HasOne("Categories.Models.Category", "Category")
                .WithMany("Products")
                .HasForeignKey("IdCategory")
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            b.Navigation("Category");
        });

        modelBuilder.Entity("Categories.Models.Category", b =>
        {
            b.Navigation("Products");
        });
    }
}
