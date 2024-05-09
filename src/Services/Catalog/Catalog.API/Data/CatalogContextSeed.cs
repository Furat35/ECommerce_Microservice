using Catalog.API.Entities;
using MongoDB.Driver;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> productCollection, IMongoCollection<Category> categoryCollection)
        {
            bool existProduct = productCollection.Find(p => true).Any();
            bool existCategory = categoryCollection.Find(c => true).Any();
            if (!existProduct || !existCategory)
            {
                productCollection.InsertManyAsync(GetPreConfiguredProducts());
                categoryCollection.InsertManyAsync(GetPreConfiguredCategories());
            }
        }

        private static IEnumerable<Product> GetPreConfiguredProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-1.png",
                    Price = 950.00M,
                    CategoryId = "202d2149e773f2a3990b47f6",
                    FavoriCount = 54,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung 10",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-2.png",
                    Price = 840.00M,
                    CategoryId = "502d2149e773f2a3990b47f7",
                    FavoriCount = 350,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Bileklik",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-3.png",
                    Price = 650.00M,
                    CategoryId = "302d2149e773f2a3990b47f4",
                    FavoriCount = 189,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"

                },
                new Product()
                {
                    Id = "102d2149e773f2a3990b47f8",
                    Name = "Nike Ayakkabı",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-4.png",
                    Price = 470.00M,
                    CategoryId = "302d2149e773f2a3990b47f4",
                    FavoriCount = 250,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"
                },
                new Product()
                {
                    Id = "202d2149e773f2a3990b47f9",
                    Name = "Sandalye",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-5.png",
                    Price = 380.00M,
                    CategoryId = "502d2149e773f2a3990b47f7",
                    FavoriCount = 11,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"
                },
                new Product()
                {
                    Id = "302d2149e773f2a3990b47fa",
                    Name = "TShirt",
                    Summary = "This phone is the company's biggest change to its flagship smartphone in years. It includes a borderless.",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    ImageFile = "product-6.png",
                    Price = 240.00M,
                    CategoryId = "302d2149e773f2a3990b47f4",
                    FavoriCount = 90,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "F29D024F-D477-4629-F04B-08DC4B5A982E"
                }
            };


        }

        private static IEnumerable<Category> GetPreConfiguredCategories()
        {
            return new List<Category>()
            {
                new Category
                {
                    CategoryId = "202d2149e773f2a3990b47f6",
                    Name = "Elektronik"
                },
                new Category
                {
                    CategoryId = "502d2149e773f2a3990b47f7",
                    Name = "Mobilya"
                },
                new Category
                {
                    CategoryId = "302d2149e773f2a3990b47f4",
                    Name = "Giyim"
                }
            };
        }
    }
}
