
using System;
using BDMall.Model;
using BDMall.Model.SystemMNG;
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
        public DbSet<SystemEmail> SystemEmails { get; set; }

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

        public DbSet<ProductStatistics> ProductStatistics { get; set; }

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

        public DbSet<MerchantPromotionBanner> MerchantPromotionBanners { get; set; }

        public DbSet<MerchantPromotionProduct> MerchantPromotionProducts { get; set; }

        public DbSet<ApproveHistory> ApproveHistories { get; set;}

        public DbSet<ScheduleJob> ScheduleJobs { get; set; }

        public DbSet<MemberLoginRecord> MemberLoginRecords { get; set; }

        public DbSet<CurrencyExchangeRate> CurrencyExchangeRates { get; set; }

        public DbSet<MailServer> MailServers { get; set; }
        public DbSet<CustomMenu> CustomMenus { get; set; }
        public DbSet<CustomMenuDetail> CustomMenuDetails { get; set; }

        public DbSet<CMSContent> CMSContents { get; set; }

        public DbSet<CMSCategory> CMSCategories { get; set; }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Province> Provinces { get; set; }

        public DbSet<RnpForm> RnpForms { get; set; }
        public DbSet<RnpAnswer> RnpAnswers { get; set; }
        public DbSet<RnpPayment> RnpPayments { get; set; }
        public DbSet<RnpQuestion> RnpQuestions { get; set; }
        public DbSet<RnpSubmit> RnpSubmits { get; set; }
        public DbSet<RnpSubmitData> RnpSubmitDatas { get; set; }
        
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<ProductAttrValue> ProductAttrValues { get; set; }

        public DbSet<ProductCommission> ProductCommissions { get; set; }

        public DbSet<ProductRefuseDelivery> ProductRefuseDeliveries { get; set;}

        public DbSet<ProductRelatedItem> ProductRelatedItems { get; set; }
    }
}