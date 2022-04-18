using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ElectronicComponentsShop.Entities;

namespace ElectronicComponentsShop.Database
{
    public class ECSDbContext : DbContext
    {
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderState> OrderStates { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentType> PaymentTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public ECSDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CartItem>(action =>
            {
                action.HasKey(item => new { item.ProductId, item.UserId });
                action.HasOne(item => item.User).WithMany(user => user.CartItems).HasForeignKey(item => item.UserId);
                action.HasOne(item => item.Product).WithMany(product => product.CartItems).HasForeignKey(item => item.ProductId);
            });

            builder.Entity<Category>(action =>
            {
                action.HasKey(category => category.Id);
                action.Property(category => category.Id).ValueGeneratedOnAdd();
                action.Property(category => category.Name).HasMaxLength(250).IsRequired();
                action.Property(category => category.Slug).HasMaxLength(250).IsRequired();
                action.Property(category => category.ThumbnailURL).HasMaxLength(500);
            });

            builder.Entity<Favourite>(action =>
            {
                action.HasKey(fav => new { fav.ProductId, fav.UserId });
                action.HasOne(fav => fav.Product).WithMany(product => product.Favourites).HasForeignKey(fav => fav.ProductId);
                action.HasOne(fav => fav.User).WithMany(user => user.Favourites).HasForeignKey(fav => fav.UserId);
            });

            builder.Entity<Order>(action =>
            {
                action.HasKey(order => order.Id);
                action.Property(order => order.Id).ValueGeneratedOnAdd();
                action.HasOne(order => order.OrderState).WithMany(orderState => orderState.Orders).HasForeignKey(order => order.OrderStateId);
                action.HasOne(order => order.User).WithMany(user => user.Orders).HasForeignKey(order => order.UserId);
                action.Property(order => order.Address).HasMaxLength(200).IsRequired();
                action.Property(order => order.Province).HasMaxLength(50).IsRequired();
                action.Property(order => order.District).HasMaxLength(50).IsRequired();
                action.Property(order => order.Ward).HasMaxLength(50).IsRequired();
                action.Property(order => order.Note).HasMaxLength(500);
                action.HasOne(order => order.PaymentType).WithMany(paymentType => paymentType.Orders).HasForeignKey(order => order.PaymentTypeId);
            });

            builder.Entity<OrderItem>(action =>
            {
                action.HasKey(item => new { item.OrderId, item.ProductId });
                action.HasOne(item => item.Order).WithMany(order => order.Items).HasForeignKey(item => item.OrderId);
                action.HasOne(item => item.Product).WithMany(product => product.OrderItems).HasForeignKey(item => item.ProductId);
            });

            builder.Entity<OrderState>(action =>
            {
                action.HasKey(state => state.Id);
                action.Property(state => state.Id).ValueGeneratedOnAdd();
                action.Property(state => state.Name).HasMaxLength(30).IsRequired();
                action.Property(state => state.Description).HasMaxLength(500);
            });

            builder.Entity<Payment>(action =>
            {
                action.HasKey(payment => payment.Id);
                action.Property(payment => payment.Id).ValueGeneratedOnAdd();
                action.HasOne(payment => payment.Order).WithOne(order => order.Payment);
                action.HasOne(payment => payment.PaymentType).WithMany(paymentType => paymentType.Payments).HasForeignKey(payment => payment.PaymentTypeId);
            });

            builder.Entity<PaymentType>(action =>
            {
                action.HasKey(paymentType => paymentType.Id);
                action.Property(paymentType => paymentType.Name).HasMaxLength(50).IsRequired();
            });

            builder.Entity<Product>(action =>
            {
                action.HasKey(product => product.Id);
                action.Property(product => product.Id).ValueGeneratedOnAdd();
                action.HasOne(product => product.Category).WithMany(category => category.Products).HasForeignKey(product => product.CategoryId);
                action.Property(product => product.Name).HasMaxLength(250).IsRequired();
                action.Property(product => product.Slug).HasMaxLength(250).IsRequired();
                action.Property(product => product.Price).HasColumnType("money");
                action.Property(product => product.ThumbnailURL).HasMaxLength(500);
            });

            builder.Entity<ProductImage>(action =>
            {
                action.HasKey(image => image.Id);
                action.HasOne(image => image.Product).WithMany(product => product.Images).HasForeignKey(image => image.ProductId);
                action.Property(image => image.Id).ValueGeneratedOnAdd();
                action.Property(image => image.URL).HasMaxLength(500).IsRequired();
            });

            builder.Entity<Review>(action =>
            {
                action.HasKey(review => review.Id);
                action.Property(review => review.Id).ValueGeneratedOnAdd();
                action.HasOne(review => review.Product).WithMany(product => product.Reviews).HasForeignKey(review => review.ProductId);
                action.HasOne(review => review.User).WithMany(user => user.Reviews).HasForeignKey(review => review.UserId);
                action.Property(review => review.Content).HasMaxLength(500).IsRequired();
            });

            builder.Entity<Role>(action =>
            {
                action.HasKey(role => role.Id);
                action.Property(role => role.Id).ValueGeneratedOnAdd();
                action.Property(role => role.Name).HasMaxLength(50).IsRequired();
            });

            builder.Entity<User>(action =>
            {
                action.HasKey(user => user.Id);
                action.Property(user => user.Id).ValueGeneratedOnAdd();
                action.Property(user => user.PhoneNumber).HasMaxLength(10).IsRequired();
                action.Property(user => user.Password).HasMaxLength(20).IsRequired();
                action.Property(user => user.Email).HasMaxLength(320);
                action.Property(user => user.FirstName).HasMaxLength(20);
                action.Property(user => user.LastName).HasMaxLength(20);
            });

            builder.Entity<UserRole>(action =>
            {
                action.HasKey(userRole => new { userRole.UserId, userRole.RoleId });
                action.HasOne(userRole => userRole.User).WithMany(user => user.UserRoles).HasForeignKey(userRole => userRole.UserId);
                action.HasOne(userRole => userRole.Role).WithMany(role => role.UserRoles).HasForeignKey(userRole => userRole.RoleId);
            });
        }
    }
}
