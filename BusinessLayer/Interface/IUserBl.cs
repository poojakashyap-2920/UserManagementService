using ModelLayer.Entity;
using RepositoryLayer.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface IUserBl
    {
        // Sign up
        Task<string> Register(UserEntity userEntity);

        // Get all users
        Task<List<UserEntity>> GetAllUsers();

        // Login
        Task<IEnumerable<UserEntity>> Login(UserLoginEntity userLoginEntity);

        // Get user by ID
        Task<IEnumerable<UserEntity>> GetUserByUserId(int userId);

        // Get all user doctors
        Task<List<DoctorInfo>> GetAllUserDoctor();
    }
}
