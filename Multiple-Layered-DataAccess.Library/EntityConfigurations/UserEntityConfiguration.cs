namespace Multiple_Layered_DataAccess.Library.EntityConfigurations
{
    internal class UserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(x => x.FirstName)
           .IsRequired()
           .HasMaxLength(100)
           .HasComment("Birincil Ad");

            builder.Property(x => x.LastName)
                .IsRequired()
                .HasMaxLength(100)
                .HasComment("İkincil Ad");

            builder.HasMany(x => x.Orders)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Users_Orders");

            builder.ToTable("Users", x => x.HasComment("Müşterilerin Tutulduğu Tablo"));
        }
    }
}
