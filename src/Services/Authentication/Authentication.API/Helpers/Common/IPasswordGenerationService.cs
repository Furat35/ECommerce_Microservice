namespace Authentication.API.Helpers.Common
{
    public interface IPasswordGenerationService
    {
        bool VerifyPassword(string storedSalt, string storedHashedPassword, string inputPassword);
        byte[] GenerateSalt();
        byte[] Combine(byte[] passwordBytes, byte[] salt);
        byte[] HashBytes(byte[] inputBytes);
    }
}
