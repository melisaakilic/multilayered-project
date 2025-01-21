namespace Multiple_Layered_DataAccess.Library.EntityConfigurations
{
    internal class OrderProductEntityConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasKey(x => new { x.OrderId, x.ProductId });

            builder.Property(x => x.Quantity)
                .IsRequired()
                .HasComment("Adet");

            builder.HasOne<Order>()
                .WithMany()
                .HasForeignKey(x => x.OrderId)
                .HasConstraintName("FK_OrderProduct_Order");

            builder.HasOne<Product>()
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .HasConstraintName("FK_OrderProduct_Product");

            builder.ToTable("OrderProducts", x => x.HasComment("Sipariş-Ürün İlişki Tablosu"));
        }
    }
}
