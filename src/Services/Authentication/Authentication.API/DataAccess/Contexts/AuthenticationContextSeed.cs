using Authentication.API.Entities;
using Shared.Enums;

namespace Authentication.API.DataAccess.Contexts
{
    public class AuthenticationContextSeed
    {
        public static async Task SeedAsync(AuthenticationContext authenticationContext)
        {
            if (!authenticationContext.Users.Any())
            {
                await authenticationContext.Users.AddRangeAsync(AddUsers());
                await authenticationContext.SaveChangesAsync();
            }
        }

        private static List<User> AddUsers()
        {
            return
            [
                new User{ Name = "firat", Surname = "ortaç", Mail = "furat@gmail.com", Phone = "537536525", Password = "lRhUNqp28FWSplbWlkOnMmIT8T+crvG4knRutYdIRAM=", PasswordSalt = "4FdIJ0kqSFE3vkLL2z4zzg==", Role = Enums.Role.User,
                    CreatedDate = DateTime.Now, CreatedBy = "firat",IsDeleted = false, LastModifiedBy = "firat", LastModifiedDate = DateTime.Now,
                    Address = new Address
                    {
                        AddressLine = "home",
                        Country = "turkey",
                        State = "izmir",
                        ZipCode = "35",
                        CreatedDate = DateTime.Now,
                        CreatedBy = "firat",
                        IsDeleted = false,
                        LastModifiedBy = "firat",
                        LastModifiedDate = DateTime.Now
                    },
                    PaymentCard = new PaymentCard
                    {
                        CardName = "visa",
                        CVV = "399",
                        CardNumber = "3254123532521",
                        Expiration = DateTime.Now.ToString(),
                        PaymentMethod = PaymentMethod.DebitCard,
                        CreatedDate = DateTime.Now,
                        CreatedBy = "firat",
                        IsDeleted = false,
                        LastModifiedBy = "firat",
                        LastModifiedDate = DateTime.Now
                    }
                }
            ];
        }
    }
}
