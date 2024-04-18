using ModelLayer.Entity;
using RepositoryLayer.Entity;
namespace RepositoryLayer.Interface
{
    public interface IUser
    {
        //sign up

        public Task<string> Register(UserEntity userEntity);

        //login
        public Task<IEnumerable<UserEntity>> Login(UserLoginEntity userLoginEntity);

        //getall
        public  Task<List<UserEntity>> GetAllUsers();
        public Task<List<DoctorInfo>> GetAllUserDoctor();

        //getby id
        public Task<IEnumerable<UserEntity>> GetUserByUserId(int userid);

    }
}
