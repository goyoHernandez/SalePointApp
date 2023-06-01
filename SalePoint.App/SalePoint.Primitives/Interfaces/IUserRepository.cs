using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalePoint.Primitives.Interfaces
{
    public interface IUserRepository
    {
        Task<StoreUser?> Login(Access access);

        Task<List<Rol>?> GetRols();

        Task<int> CreateUser(StoreUser storeUser);

        Task<IEnumerable<StoreUser>?> GetAllUsers();

        Task<StoreUser?> GetUserById(int userId);

        Task<int> UpdateUser(StoreUser storeUser);

        Task<int> DeleteUserById(int userId);
    }
}
