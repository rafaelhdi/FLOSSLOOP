using FLOSSLOOP.Database;
using FLOSSLOOP.Models;
using Npgsql;
using NpgsqlTypes;

namespace FLOSSLOOP.Repositories
{
    public class HasilPanenRepository
    {
        private readonly DatabaseConnection _db;

        public HasilPanenRepository()
        {
            // Konfigurasi database dibaca dari .env (sama seperti UserRepository)
            var s = DatabaseSettings.LoadFromEnvironment();
            _db = new DatabaseConnection(s.Host, s.Port, s.Database, s.Username, s.Password);
        }

        public List<HasilPanen> GetAll()
        {
            var hasil = new List<HasilPanen>();

            using var conn = _db.CreateConnection();
            conn.Open();

            const string query = @"SELECT id, tanggal, komoditas, lokasi, berat_panen, food_loss_kg, created_by
                                   FROM hasil_panen
                                   ORDER BY tanggal, id";
            using var cmd = new NpgsqlCommand(query, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                hasil.Add(new HasilPanen
                {
                    Id = reader.GetInt32(0),
                    Tanggal = reader.GetDateTime(1),
                    Komoditas = reader.GetString(2),
                    Lokasi = reader.GetString(3),
                    BeratPanen = reader.GetDecimal(4),
                    FoodLossKg = reader.GetDecimal(5),
                    CreatedBy = reader.IsDBNull(6) ? null : reader.GetInt32(6)
                });
            }

            return hasil;
        }

        public void Insert(HasilPanen h)
        {
            using var conn = _db.CreateConnection();
            conn.Open();

            const string query = @"INSERT INTO hasil_panen
                                       (tanggal, komoditas, lokasi, berat_panen, food_loss_kg, created_by)
                                   VALUES (@tgl, @kom, @lok, @panen, @loss, @by)";
            using var cmd = new NpgsqlCommand(query, conn);
            cmd.Parameters.AddWithValue("tgl", NpgsqlDbType.Date, h.Tanggal.Date);
            cmd.Parameters.AddWithValue("kom", h.Komoditas);
            cmd.Parameters.AddWithValue("lok", h.Lokasi);
            cmd.Parameters.AddWithValue("panen", h.BeratPanen);
            cmd.Parameters.AddWithValue("loss", h.FoodLossKg);
            cmd.Parameters.AddWithValue("by", (object?)h.CreatedBy ?? DBNull.Value);
            cmd.ExecuteNonQuery();
        }
    }
}
