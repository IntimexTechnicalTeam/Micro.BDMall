
using System;
using BDMall.Model;
using Microsoft.EntityFrameworkCore;

namespace BDMall.Repository
{
    public class MallDbContext : DbContext
    {
        public MallDbContext() 
        {
              
        }

        public MallDbContext(DbContextOptions<MallDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            

        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SystemMenu> SystemMenus { get; set; }

        public DbSet<Translation> Translations { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<InventoryReserved> InventoryReserveds { get; set; }

        public DbSet<InvTransactionDtl> InvTransactionDtls { get; set; }

        public DbSet<ProductQty> ProductQties { get; set; }

        public DbSet<PushMessage> PushMessages { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Order> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductClickRateSummry> ProductClickRateSummrys { get; set; }

        public DbSet<CodeMaster> CodeMasters { get; set; }

        public DbSet<ProductSalesSummry> ProductSalesSummrys { get; set; }
    }
}