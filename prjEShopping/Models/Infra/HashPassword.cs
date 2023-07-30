using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using prjEShopping.Models.EFModels;
using System.Text;

namespace prjEShopping.Models.Infra
{
    public class HashPassword
    {
        private readonly AppDbContext _db;
        public HashPassword(AppDbContext db)
        {
            _db = db;
        }
        private string CreatSalt()
        {
            byte[] salt = new byte[128 / 8]; // 16 bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string CreatHashPassword(string password, out string salt)
        {
            salt = CreatSalt();
            using (var hashAlgorithm = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password + salt);
                var hash = hashAlgorithm.ComputeHash(passwordBytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        public static bool VerifyPassword(string password, string storedSalt, string storedHash)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password + storedSalt);
                var hash = hashAlgorithm.ComputeHash(passwordBytes);
                var hashString = Convert.ToBase64String(hash);
                return hashString == storedHash;
            }
        }
    }
}