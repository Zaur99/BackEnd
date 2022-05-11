using Comm.DataAccess.IdentityModel;
using Comm.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comm.DataAccess.Concrete
{
    public static class SeedDb
    {

        public static void Seed()
        {
            var contextOptions = new DbContextOptionsBuilder<CommerceContext>()
     .UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=ECommerceProjectDb; Trusted_Connection=True;")
     .Options;

            using var context = new CommerceContext(contextOptions);
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                }

                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategories);
                }

                context.SaveChanges();

            }


        }


        public static Category[] Categories =
        {
            new Category{ Name = "Telefon",Url="telefon"},
            new Category{ Name = "Komputer",Url="komputer"},
            new Category{ Name = "Elektronik",Url="elektronik"},
            new Category{ Name="Samsung",Url="samsungs", ParentId=1},
            new Category{ Name="IPhone",Url="iphones", ParentId=1},
            new Category{ Name="Huawei",Url="huawei", ParentId=1},

        };

        public static Product[] Products =
       {
            new Product{ Name = "Samsung",Url="samsung", Price= 1000, Description="Smartphone" },
            new Product{ Name = "Apple", Url="apple",Price= 3000, Description="Smartphone2" },
            new Product{ Name = "Huawei1", Url="huawei1",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei2", Url="huawei2",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei3", Url="huawei3",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei4", Url="huawei4",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei5", Url="huawei5",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei6", Url="huawei6",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei7", Url="huawei7",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei8", Url="huawei8",Price= 2000, Description="Smartphone3" },
            new Product{ Name = "Huawei9", Url="huawei9",Price= 2000, Description="Smartphone3" },


        };

        private static ProductCategory[] ProductCategories =
        {

            new ProductCategory{Product= Products[0], Category=Categories[3]},
            new ProductCategory{Product= Products[1], Category=Categories[4]},
            new ProductCategory{Product= Products[2], Category=Categories[5]},
            new ProductCategory{Product= Products[3], Category=Categories[5]},
            new ProductCategory{Product= Products[4], Category=Categories[5]},

        };
    }
}
