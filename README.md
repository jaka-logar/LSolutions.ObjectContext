# LSolutions.ObjectContext

ObjectContext implementation for EntityFrameworkCore based on BaseEntity and IRepository.\
Implementation taken from NopCommerce (https://github.com/nopSolutions/nopCommerce).

[![Build status](https://ci.appveyor.com/api/projects/status/elxa4rs56rjsmgbt?svg=true)](https://ci.appveyor.com/project/jaka-logar/lsolutions-objectcontext)
[![contributions welcome](https://img.shields.io/badge/contributions-welcome-brightgreen.svg?style=flat)](https://github.com/jaka-logar/LSolutions.ObjectContext/issues)

Packed as two NuGet packages in case you have database related code in separate project:
- LSolutions.EntityRepository [![NuGet](https://img.shields.io/nuget/v/LSolutions.EntityRepository.svg)](https://www.nuget.org/packages/LSolutions.EntityRepository/)
- LSolutions.EfObjectContext [![NuGet](https://img.shields.io/nuget/v/LSolutions.EfObjectContext.svg)](https://www.nuget.org/packages/LSolutions.EfObjectContext/)


## Usage

 1. Every entity class must extend BaseEntity.
```csharp
    public class Product : BaseEntity
    {
        public int Id { get; set; }
        public string Sku { get; set; }
	public string Ean { get; set; }
    }
```
 2. Your ObjectContext implementation must extend BaseObjectContext.
 3. Register ObjectContext and EfRepository.
```csharp 
    services.AddDbContext<IDbContext, ObjectContext>();
    services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
```
 4. Create entities database mapping using fluent API.
```csharp
    internal class ProductMap : CustomEntityTypeConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Sku)
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(x => x.Ean)
                .HasMaxLength(15)
                .IsRequired();

            base.Configure(builder);
        }
    }
```
5. Implement database queries
```csharp
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public virtual Product GetProductById(int id)
        {
            IQueryable<Product> query =
                from p in _productRepository.Table
                where p.Id == id
                select p;

            return query.FirstOrDefault();
        }

        public virtual void InsertProduct(Product product)
        {
            _productRepository.Insert(product);
        }
    } 
```
