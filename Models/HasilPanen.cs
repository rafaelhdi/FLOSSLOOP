namespace FLOSSLOOP.Models
{
    public class HasilPanen
    {
        public int Id { get; set; }
        public DateTime Tanggal { get; set; }
        public string Komoditas { get; set; } = "";
        public string Lokasi { get; set; } = "";
        public decimal BeratPanen { get; set; }
        public decimal FoodLossKg { get; set; }   // diinput pengguna, disimpan di database
        public int? CreatedBy { get; set; }

        // Dihitung otomatis, tidak disimpan di database
        public decimal BeratDistribusi => BeratPanen - FoodLossKg;
    }
}
