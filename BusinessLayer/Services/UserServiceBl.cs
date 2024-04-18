using BusinessLayer.Interface;
using ModelLayer.Entity;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UserServiceBl : IUserBl
    {
        private readonly IUser _user;

        public UserServiceBl(IUser user)
        {
            _user = user;
        }


        public Task<string> Register(UserEntity userEntity)
        {

            return _user.Register(userEntity);
        }

        //getall
        public async Task<List<UserEntity>> GetAllUsers()
        {
            return await _user.GetAllUsers();
        }

        public string EncyptionPassword(string password)
        {
            throw new NotImplementedException();
        }

        //login
        public async Task<IEnumerable<UserEntity>> Login(UserLoginEntity userLoginEntity)
        {
           
            return await _user.Login(userLoginEntity);
        }

        public async Task<IEnumerable<UserEntity>> GetUserByUserId(int userid)
        {
            return await _user.GetUserByUserId(userid);
        }

        public async Task<List<DoctorInfo>> GetAllUserDoctor()
        { 
            return await _user.GetAllUserDoctor
                ();
        }
    }
}
