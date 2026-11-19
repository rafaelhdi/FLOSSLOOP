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
        // Buat objek DatabaseConnection dengan konfigurasi PostgreSQL kamu
        private readonly DatabaseConnection _db = new DatabaseConnection(
            host: "localhost",
            port: 5432,
            database: "FLOSSLOOP_db",
            username: "postgres",
            password: "passwordmu_rek"
        );

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
