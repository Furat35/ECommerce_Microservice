using Shared.Exceptions;

namespace Shared.Extensions
{
    public static class ModelValidations
    {
        public static void ThrowBadRequestIfIdIsNotValidGuid(params string[] ids)
        {
            foreach (var id in ids)
                if (!Guid.TryParse(id, out Guid validId))
                    throw new BadRequestException("Geçersiz id bilgisi! Tekrar deneyiniz.");

        }
    }
}
