
using Dapper;
using FluentAssertions.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.Entity;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using Configuration = System.Configuration.Configuration;

namespace RepositoryLayer.Services
{
    public class UserService : IUser
    {
        private readonly UserDapperContext _context;
        private readonly IConfiguration _configuration;
        public UserService(UserDapperContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        //signup
        public async Task<string> Register(UserEntity userEntity)
        {
            try
            {
                string insertQuery = @"
                    INSERT INTO UserEntity (FirstName, LastName, Email, Password, UserRole)
                    VALUES ( @FirstName, @LastName, @Email, @Password, @UserRole);";

                string encryptedPassword = EncryptPassword(userEntity.Password);

                var parameters = new DynamicParameters();
                parameters.Add("FirstName", userEntity.FirstName, DbType.String);
                parameters.Add("LastName", userEntity.LastName, DbType.String);
                parameters.Add("Email", userEntity.Email, DbType.String);
                parameters.Add("Password", encryptedPassword, DbType.String); 
                parameters.Add("UserRole", userEntity.UserRole, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    await connection.ExecuteAsync(insertQuery, parameters);
                    return "User registered successfully.";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return "Error: " + ex.Message;
            }
        }

        // Encryption
        private string EncryptPassword(string password)
        {
            byte[] plaintext = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(plaintext);
        }

        //decryption
        public string DecryptPassword(string encryptedPassword)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            return Encoding.UTF8.GetString(encryptedBytes);
        }

       
    

        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                var query = "SELECT * FROM UserEntity";
                using (var connection = _context.CreateConnection())
                {
                    var users = await connection.QueryAsync<UserEntity>(query);
                    return users.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred in GetAllUsers: {ex}");
                throw;
            }
        }


        public async Task<IEnumerable<UserEntity>> Login(UserLoginEntity userLoginEntity)
        {
            try
            {
                var query = "SELECT * FROM userentity WHERE Email = @Email";

                using (var connection = _context.CreateConnection())
                {
                    var users = await connection.QueryAsync<UserEntity>(query, new { Email = userLoginEntity.Email });
                    foreach (var user in users)
                    {
                        string storedPassword = DecryptPassword(user.Password);
                        if (userLoginEntity.Password == storedPassword)
                        {
                            return new List<UserEntity> { user }; 
                        }
                    }
                    return Enumerable.Empty<UserEntity>(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while logging in: {ex}");
                throw;
            }
        }





        public async Task<IEnumerable<UserEntity>> GetUserByUserId(int userId)
        {
            var query = "SELECT * FROM UserEntity WHERE UserId = @UserId";

            using (var connection = _context.CreateConnection())
            {
                var employees = await connection.QueryAsync<UserEntity>(query, new { UserId = userId });
                return employees.ToList();
            }
        }





        private string TokenGeneration(UserEntity ref_var)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddHours(1);
            var claims = new List<Claim>
            {
                   new Claim(ClaimTypes.Email, ref_var.Email),
                   new Claim(ClaimTypes.Role,ref_var.UserRole)
                 
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<List<DoctorInfo>> GetAllUserDoctor()
        {
            try
            {
                var query = "SELECT * FROM UserEntity WHERE UserRole = 'Doctor'";

                using (var connection = _context.CreateConnection())
                {
                    var doctors = await connection.QueryAsync<DoctorInfo>(query);
                    return doctors.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching all user doctors: {ex}");
                throw;
            }
        }


    }
}



