
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

        public DbSet<OrderDetail> OrderDetails { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductClickRateSummry> ProductClickRateSummrys { get; set; }

        public DbSet<CodeMaster> CodeMasters { get; set; }

        public DbSet<ProductSalesSummry> ProductSalesSummrys { get; set; }

        public DbSet<ProductSku> ProductSkus { get; set; }

        public DbSet<Merchant> Merchants { get; set; }

        public DbSet<ProductAttribute> ProductAttributes { get; set; }
        public DbSet<ProductAttributeValue> ProductAttributeValues { get; set; }

        public DbSet<ProductCatalog> ProductCatalogs { get; set; }

        public DbSet<ProductCatalogAttr> ProductCatalogAttrs { get; set; }

        public DbSet<ProductCatalogParent> ProductCatalogParents { get; set; }

        public DbSet<ProductAttr> ProductAttrs { get; set; }

        public DbSet<MerchantPromotion> MerchantPromotions { get; set; }

        public DbSet<MerchantStatistic> MerchantStatistics { get; set; }    

        public DbSet<CustomUrl> CustomUrls { get; set; }

        public DbSet<MerchantECShipInfo> MerchantECShipInfos { get; set; }

        public DbSet<ExpressCompany> ExpressCompanies { get; set; }

        public DbSet<MerchantActiveShipMethod> MerchantActiveShipMethods { get; set; }
    }
}