using FLOSSLOOP.Repositories;
using FLOSSLOOP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLOSSLOOP.Controllers
{
    public class LoginController
    {
        private readonly UserRepository _userRepo = new UserRepository();

        public User? Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email dan password wajib diisi!");
            }

            return _userRepo.GetByEmailAndPassword(email.Trim(), password);
        }
    }
}
