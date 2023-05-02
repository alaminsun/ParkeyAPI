using ParkeyWeb.Models;
using System.Threading.Tasks;

namespace ParkeyWeb.Repository.IRepository
{
    public interface IAccountRepository : IRepository<User>
    {
        Task<User> LoginAsync(string url, User objToCreate);
        Task<bool> RegisterAsync(string url, User objToCreate);
    }
}
