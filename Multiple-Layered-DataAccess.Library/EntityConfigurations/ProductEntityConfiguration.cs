namespace Multiple_Layered_DataAccess.Library.EntityConfigurations
{
    internal class ProductEntityConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200)
                .HasComment("Ürün Adı");

            builder.Property(x => x.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasComment("Ürün Fiyatı");

            builder.Property(x => x.Stock)
                .IsRequired()
                .HasComment("Ürün Stoğu");


            builder.ToTable("Products", x => x.HasComment("Ürünlerin Tutulduğu Tablo"));
        }
    }
}
