using System;
using System.Collections.Generic;
using System.Text;

namespace FLOSSLOOP.Models
{
    public abstract class User
    {
        public int Id { get; set; }
        public string Nama { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public abstract string Role { get; }
        public abstract bool CanAccess(string aksi);
    }

    public class PemilikUser : User
    {
        public override string Role => "Pemilik";

        // Pemilik bisa melihat semua data (termasuk Laporan) secara read-only
        public override bool CanAccess(string aksi)
        {
            return aksi.StartsWith("View_");
        }
    }

    public class PegawaiUser : User
    {
        public override string Role => "Pegawai";

        // Pegawai bisa View & CRUD fitur operasional, tapi dilarang akses Laporan
        public override bool CanAccess(string aksi)
        {
            if (aksi == "View_Laporan") return false;
            return aksi.StartsWith("View_") || aksi.StartsWith("CRUD_");
        }
    }
}