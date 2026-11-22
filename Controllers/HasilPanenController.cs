using FLOSSLOOP.Models;
using FLOSSLOOP.Repositories;

namespace FLOSSLOOP.Controllers
{
    public class HasilPanenController
    {
        private readonly HasilPanenRepository _repo = new HasilPanenRepository();

        public List<HasilPanen> GetAll(User user)
        {
            if (!user.CanAccess("View_Panen"))
                throw new UnauthorizedAccessException("Anda tidak berhak melihat data hasil panen.");

            return _repo.GetAll();
        }

        public void Simpan(User user, HasilPanen h)
        {
            // Pemilik hanya View_, Pegawai boleh CRUD_ (lihat Models/Users.cs)
            if (!user.CanAccess("CRUD_Panen"))
                throw new UnauthorizedAccessException("Role " + user.Role + " tidak diizinkan menyimpan data panen.");

            if (string.IsNullOrWhiteSpace(h.Komoditas))
                throw new ArgumentException("Pilih jenis komoditas terlebih dahulu.");
            if (string.IsNullOrWhiteSpace(h.Lokasi))
                throw new ArgumentException("Lokasi/Blok lahan wajib diisi.");
            if (h.BeratPanen <= 0)
                throw new ArgumentException("Berat panen harus lebih dari 0.");
            if (h.FoodLossKg < 0)
                throw new ArgumentException("Food loss tidak boleh negatif.");
            if (h.FoodLossKg > h.BeratPanen)
                throw new ArgumentException("Food loss tidak boleh melebihi berat panen.");

            h.Lokasi = h.Lokasi.Trim();
            h.CreatedBy = user.Id;
            _repo.Insert(h);
        }
    }
}
