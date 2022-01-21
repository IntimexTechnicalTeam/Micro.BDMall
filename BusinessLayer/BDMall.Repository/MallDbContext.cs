
using System;
using System.Linq;
using System.Reflection;
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
            foreach (var item in modelBuilder.Model.GetEntityTypes())
            {
                var type = item.ClrType;
                var props = type.GetProperties().Where(c => c.IsDefined(typeof(DecimalPrecisionAttribute), true)).ToArray();
                foreach (var p in props)
                {
                    var precis = p.GetCustomAttribute<DecimalPrecisionAttribute>();
                    modelBuilder.Entity(type).Property(p.Name).HasColumnType($"decimal({precis.Precision},{precis.Scale})");
                }
            }
            base.OnModelCreating(modelBuilder);
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

        public DbSet<ApproveHistory> ApproveHistories { get; set; }

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
        public DbSet<InventoryChangeNotify> InventoryChangeNotifies { get; set; }

        public DbSet<ProductAttrValue> ProductAttrValues { get; set; }

        public DbSet<ProductCommission> ProductCommissions { get; set; }

        public DbSet<ProductRefuseDelivery> ProductRefuseDeliveries { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<EmailTempItem> EmailTempItems { get; set; }
        public DbSet<EmailTypeTempItem> EmailTypeTempItems { get; set; }

        public DbSet<ProductRelatedItem> ProductRelatedItems { get; set; }

        public DbSet<ProductDetail> ProductDetails { get; set; }

        public DbSet<ProductExtension> ProductExtensions { get; set; }

        public DbSet<ProductSpecification> ProductSpecifications { get; set; }

        public DbSet<ProductPriceHour> ProductPriceHours { get; set; }

        public DbSet<MerchantFreeCharge> MerchantFreeCharges { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<ProductImageList> ProductImageLists { get; set; }

        public DbSet<Warehouse> Warehouses { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<OrderDelivery> OrderDeliveries { get; set; }
        public DbSet<OrderDeliveryDetail> OrderDeliveryDetails { get; set; }

        public DbSet<OrderPriceDetail> OrderPriceDetails { get; set; }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public DbSet<PurchaseReturnOrder> PurchaseReturnOrders { get; set; }
        public DbSet<PurchaseReturnOrderDetail> PurchaseReturnOrderDetails { get; set; }

        public DbSet<RelocationOrder> RelocationOrders { get; set; }
        public DbSet<RelocationOrderDetail> RelocationOrderDetails { get; set; }

        public DbSet<SalesReturnOrder> SalesReturnOrders { get; set; }
        public DbSet<SalesReturnOrderDetail> SalesReturnOrderDetails { get; set; }

        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OrderDiscount> OrderDiscounts { get; set; }

        public DbSet<Customization> Customizations { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public DbSet<ExpressDiscount> ExpressDiscounts { get; set; }
        public DbSet<ExpressPrice> ExpressPrices { get; set; }
        public DbSet<ExpressRule> ExpressRules { get; set; }
        public DbSet<ExpressZone> ExpressZones { get; set; }

        public DbSet<ExpressCountry> ExpressCountries { get; set; }
        public DbSet<ShoppingCartItemDetail> ShoppingCartItemDetails { get; set; }

        public DbSet<ExpressZoneCountry> ExpressZoneCountries { get; set; }
        public DbSet<ExpressZoneProvince> ExpressZoneProvinces { get; set; }
        public DbSet<InventoryHold> InventoryHolds { get; set; }

        public DbSet<City> Cities { get; set; }
        public DbSet<DeliveryAddress> DeliveryAddresses { get; set; }

        public DbSet<Coupon> Coupons { get; set; }
        public DbSet<PromotionRule> PromotionRules { get; set;}
        public  DbSet<PromotionRuleProduct> PromotionRuleProducts { get; set; }

        public DbSet<MemberGroupDiscount> MemberGroupDiscounts { get; set; }

        public DbSet<MemberGroupDiscountItem> MemberGroupDiscountItems { get; set; }    

        public DbSet<FunHistory> FunHistories { get; set; }

        public DbSet<OrderStatusHistory> OrderStatusHistories { get; set; } 

        public DbSet<SubOrderStatusHistory> SubOrderStatusHistories { get; set; }

        public DbSet<MerchantSalesStatistic> MerchantSalesStatistics { get; set; }
    }
}