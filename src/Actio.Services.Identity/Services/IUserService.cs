using System.Threading.Tasks;

namespace Actio.Services.Identity.Services
{
    public interface IUserService
    {
        Task RegisterAsync(string name, string email, string password);
        Task LoginAsync(string email, string password);
    }
}
