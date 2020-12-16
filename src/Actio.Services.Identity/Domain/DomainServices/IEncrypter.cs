using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Domain.DomainServices
{
    public interface IEncrypter
    {
        string GetSalt();
        string GetHash(string value, string salt);
    }
}
