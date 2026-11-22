# FLOSSLOOP

FLOSSLOOP (Food Loss Optimization and Sustainability Loop) merupakan aplikasi manajemen hasil panen berbasis desktop yang dikembangkan menggunakan C# dan WinForms. Aplikasi ini dirancang untuk membantu industri agroindustri skala kecil hingga menengah dalam mencatat hasil panen buah dan sayuran, mengelompokkan kualitas produk, mengelola distribusi, serta memanfaatkan sisa produksi menjadi pupuk kompos atau bahan pakan ternak.

---

## Persyaratan

- **Visual Studio 2026** dengan workload .NET Desktop Development
- **.NET 10 SDK**
- **PostgreSQL 12+** (sudah terinstall dan berjalan)

---

## Setup Development Environment

### Setup Database

Jalankan **1 command** ini untuk setup otomatis:

```bash
psql -U postgres -f Database/setup-database.sql
```

**Apa yang terjadi:**
-  Database `FLOSSLOOP_db` dibuat
-  Tabel `users` dibuat + 2 data dummy
-  Tabel `hasil_panen` dibuat + 4 data contoh
-  Selesai!

**Jika PostgreSQL tidak di-PATH:**

Buka "SQL Shell (psql)" dari Start Menu PostgreSQL, lalu copy-paste seluruh isi file `Database/setup-database.sql` ke terminal.

---

### Konfigurasi File `.env`

Copy file `.env.example` menjadi `.env` di root project:

```bash
# Windows PowerShell
Copy-Item .env.example .env
```

Buka file `.env` dan sesuaikan konfigurasi PostgreSQL:

```
DB_HOST=localhost
DB_PORT=5432
DB_NAME=FLOSSLOOP_db
DB_USERNAME=postgres
DB_PASSWORD=password_postgresql_anda
```

**Catatan:** 
- Ganti `password_postgresql_anda` dengan password PostgreSQL yang sebenarnya
- File `.env` **TIDAK** akan di-commit ke Git (sudah di `.gitignore`)
- Setiap developer harus membuat `.env` pribadi mereka

### Jalankan App

Di Visual Studio:

1. **Build project:** `Ctrl+Shift+B`
2. **Jalankan aplikasi:** `F5`

Atau dari PowerShell:

```powershell
cd C:\Users\alif\source\repos\FLOSSLOOP
dotnet build
dotnet run
```

---

## Login Credentials (untuk testing aja)

Gunakan data dummy yang sudah diinsert:

| Email | Password | Role |
|-------|----------|------|
| pemilik@gmail.com | 123456 | Pemilik |
| pegawai@gmail.com | 123456 | Pegawai |

---

## Struktur Project

```
FLOSSLOOP/
├── Controllers/        # Business logic untuk login
├── Database/          # Konfigurasi database
├── Models/            # Model User dan role (Pemilik, Pegawai)
├── Repositories/      # Data access layer
├── Views/             # UI forms (LoginForm, FormPencatatan, Form1)
├── Program.cs         # Entry point aplikasi
├── .env.example       # Template konfigurasi (untuk di-share)
├── .env              # Konfigurasi lokal (private, not in Git)
└── FLOSSLOOP.csproj  # Project file
```

---





## Troubleshooting

### Error: "Email atau password salah" saat login

**Penyebab kemungkinan:**
1. Data user belum ada di tabel `users`
2. Email atau password yang diinput tidak sesuai dengan database
3. Aplikasi tidak konek ke database

**Solusi:**
1. Verifikasi data ada di database:
```sql
SELECT * FROM public.users;
```

2. Jika tidak ada data, insert data dummy:
```sql
INSERT INTO public.users (nama, email, password, role)
VALUES 
    ('Admin Pemilik', 'pemilik@gmail.com', '123456', 'Pemilik'),
    ('Karyawan', 'pegawai@gmail.com', '123456', 'Pegawai');
```

3. Verifikasi file `.env` sudah ada dan konfigurasi PostgreSQL benar:
- `DB_HOST=localhost`
- `DB_PORT=5432`
- `DB_PASSWORD=` password PostgreSQL yang benar

4. Restart aplikasi setelah update `.env`

### Error: "relation 'users' does not exist"

**Solusi:** Pastikan tabel `users` sudah dibuat di database `FLOSSLOOP_db`.

```sql
SELECT * FROM information_schema.tables WHERE table_name = 'users';
```

### Error: "Gagal terhubung ke database"

**Kemungkinan penyebab:**
1. PostgreSQL tidak berjalan → Jalankan PostgreSQL service
2. Database `FLOSSLOOP_db` tidak ada → Buat database baru (lihat langkah 1)
3. Kredensial salah di `.env` → Verifikasi `DB_USERNAME` dan `DB_PASSWORD`
4. Port PostgreSQL berbeda → Default port adalah `5432`, ubah `DB_PORT` jika berbeda

### Error: Build Failed

**Solusi:**
```powershell
dotnet clean
dotnet build
```

