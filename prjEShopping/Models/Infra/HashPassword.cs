using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using prjEShopping.Models.EFModels;
using System.Text;

namespace prjEShopping.Models.Infra
{
    //雜湊密碼的方法 跟解碼方法
    public class HashPassword
    {
        private readonly AppDbContext _db;
        public HashPassword(AppDbContext db)
        {
            _db = db;
        }

        //生成隨機鹽~!
        private string CreatSalt()
        {
            byte[] salt = new byte[128 / 8]; // 16 bytes
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }


        //密碼+鹽 組成雜湊密碼
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

        //解碼用
        public static bool VerifyPassword(string password, string storedSalt, string storedHash)
        {
            using (var hashAlgorithm = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password + storedSalt);
                var hash = hashAlgorithm.ComputeHash(passwordBytes);
                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                return hashString == storedHash;
            }
        }
    }
}