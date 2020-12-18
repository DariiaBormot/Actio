namespace Actio.Services.Identity.Domain.DomainServices
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
