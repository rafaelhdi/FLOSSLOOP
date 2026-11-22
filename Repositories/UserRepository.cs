using FLOSSLOOP.Database;
using FLOSSLOOP.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace FLOSSLOOP.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseConnection _db;

        public UserRepository()
        {
            // Load konfigurasi database dari environment variables (.env)
            var dbSettings = DatabaseSettings.LoadFromEnvironment();          // izin ubah ya kell ngga enak kan kalo PW database ditaruh sini hehe - alif
            _db = new DatabaseConnection(
                host: dbSettings.Host,
                port: dbSettings.Port,
                database: dbSettings.Database,
                username: dbSettings.Username,
                password: dbSettings.Password
            );
        }

        public User? GetByEmailAndPassword(string email, string password)
        {
            // Panggil CreateConnection() dari objek _db
            using var conn = _db.CreateConnection();
            conn.Open();

            string query = "SELECT id, nama, email, password, role FROM users WHERE email = @e AND password = @p";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("e", email);
            cmd.Parameters.AddWithValue("p", password);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader.GetString(4);
                User user = role switch
                {
                    "Pemilik" => new PemilikUser(),
                    "Pegawai" => new PegawaiUser(),
                    _ => throw new Exception($"Role '{role}' tidak dikenali.")
                };

                user.Id = reader.GetInt32(0);
                user.Nama = reader.GetString(1);
                user.Email = reader.GetString(2);
                user.Password = reader.GetString(3);

                return user;
            }

            return null;
        }
    }
}
