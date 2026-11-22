using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FLOSSLOOP.Controllers;
using FLOSSLOOP.Models;

namespace FLOSSLOOP.Views
{
    /// <summary>
    /// Form pencatatan hasil panen. UI dibuat lewat kode (tanpa Designer).
    /// Dibuka dari LoginForm: new FormPencatatan(user)
    /// </summary>
    public partial class FormPencatatan : Form
    {
        private readonly User _user;
        private readonly HasilPanenController _controller = new HasilPanenController();
        private readonly bool _bisaEdit;

        // ====== Kontrol input ======
        private DateTimePicker dtpTanggal = null!;
        private ComboBox cmbKomoditas = null!;
        private TextBox txtLokasi = null!;
        private TextBox txtBeratPanen = null!;
        private TextBox txtFoodLossKg = null!;
        private TextBox txtBeratDistribusi = null!;
        private Button btnSimpan = null!;
        private Button btnReset = null!;

        // ====== Tabel ======
        private DataGridView dgvPanen = null!;

        // ====== Warna ======
        private readonly Color hijau = Color.FromArgb(46, 125, 50);
        private readonly Color hijauMuda = Color.FromArgb(232, 245, 233);
        private readonly Color latar = Color.FromArgb(240, 240, 240);

        public FormPencatatan(User user)
        {
            _user = user;
            // Pegawai: CRUD_ -> boleh simpan. Pemilik: hanya View_ -> read-only.
            _bisaEdit = user.CanAccess("CRUD_Panen");

            Text = "FLOSSLOOP - Pencatatan Hasil Panen";
            ClientSize = new Size(1060, 640);
            MinimumSize = new Size(900, 560);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Segoe UI", 10F);
            BackColor = latar;

            BuatSidebar();
            BuatKonten();

            Load += FormPencatatan_Load;
        }

        private void FormPencatatan_Load(object? sender, EventArgs e)
        {
            MuatData();
        }

        // =====================================================
        //  SIDEBAR
        // =====================================================
        private void BuatSidebar()
        {
            Panel sidebar = new Panel
            {
                Dock = DockStyle.Left,
                Width = 220,
                BackColor = Color.White
            };

            Label lblKeluar = BuatMenu("Keluar", Color.Black, false);
            lblKeluar.Dock = DockStyle.Bottom;
            lblKeluar.Click += (s, e) => Close();

            // Info user yang sedang login
            Label lblUser = new Label
            {
                Text = $"{_user.Nama}\n({_user.Role})",
                ForeColor = Color.Gray,
                Dock = DockStyle.Bottom,
                Height = 52,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Font = new Font("Segoe UI", 9F)
            };

            // Manajemen User belum dibuat -> selalu terkunci
            Label menuManajemen = BuatMenu("Manajemen User (terkunci)", Color.Gray, false);
            menuManajemen.Click += (s, e) => MessageBox.Show("Menu ini terkunci.", "Informasi",
                MessageBoxButtons.OK, MessageBoxIcon.Information);

            // Laporan: terkunci untuk Pegawai (lihat User.CanAccess("View_Laporan"))
            bool laporanBuka = _user.CanAccess("View_Laporan");
            Label menuLaporan = BuatMenu(
                laporanBuka ? "Laporan Hasil Panen" : "Laporan Hasil Panen (terkunci)",
                laporanBuka ? Color.Black : Color.Gray, false);
            menuLaporan.Click += (s, e) =>
            {
                string pesan = laporanBuka
                    ? "Halaman laporan belum tersedia."
                    : "Menu ini terkunci untuk role " + _user.Role + ".";
                MessageBox.Show(pesan, "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            Label menuPencatatan = BuatMenu("Pencatatan Hasil Panen", hijau, true);

            Label logo = new Label
            {
                Text = "FLOSSLOOP",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = hijau,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0)
            };

            // Urutan Add: yang terakhir ditambah akan berada paling atas (Dock.Top)
            sidebar.Controls.Add(lblKeluar);
            sidebar.Controls.Add(lblUser);
            sidebar.Controls.Add(menuManajemen);
            sidebar.Controls.Add(menuLaporan);
            sidebar.Controls.Add(menuPencatatan);
            sidebar.Controls.Add(logo);

            // Garis pemisah tipis di sisi kanan sidebar
            Panel garis = new Panel { Dock = DockStyle.Right, Width = 1, BackColor = Color.Silver };
            sidebar.Controls.Add(garis);

            Controls.Add(sidebar);
        }

        private Label BuatMenu(string teks, Color warna, bool aktif)
        {
            return new Label
            {
                Text = teks,
                ForeColor = warna,
                BackColor = aktif ? hijauMuda : Color.White,
                Dock = DockStyle.Top,
                Height = 48,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(12, 0, 0, 0),
                Cursor = Cursors.Hand
            };
        }

        // =====================================================
        //  KONTEN UTAMA
        // =====================================================
        private void BuatKonten()
        {
            Panel konten = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(24, 20, 24, 20),
                BackColor = latar
            };

            // ---- Judul halaman ----
            Label judul = new Label
            {
                Text = "Pencatatan Hasil Panen",
                Font = new Font("Segoe UI", 16F),
                Dock = DockStyle.Top,
                Height = 44,
                TextAlign = ContentAlignment.MiddleLeft
            };

            // ---- GroupBox input ----
            GroupBox grpInput = new GroupBox
            {
                Text = _bisaEdit ? "Input Hasil Panen" : "Input Hasil Panen (hanya lihat - role " + _user.Role + ")",
                Dock = DockStyle.Top,
                Height = 170,
                Padding = new Padding(10, 8, 10, 8)
            };

            TableLayoutPanel tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 2
            };
            for (int i = 0; i < 4; i++)
                tbl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            // Baris 1
            dtpTanggal = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "dd/MM/yyyy"
            };

            cmbKomoditas = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
            cmbKomoditas.Items.AddRange(new object[]
            {
                "Bayam", "Kangkung", "Tomat", "Cabai", "Wortel", "Sawi"
            });
            cmbKomoditas.SelectedIndex = 0;

            txtLokasi = new TextBox();
            txtBeratPanen = new TextBox();

            tbl.Controls.Add(BuatField("Tanggal Panen", dtpTanggal), 0, 0);
            tbl.Controls.Add(BuatField("Jenis Komoditas", cmbKomoditas), 1, 0);
            tbl.Controls.Add(BuatField("Lokasi/Blok Lahan", txtLokasi), 2, 0);
            tbl.Controls.Add(BuatField("Berat Panen (kg)", txtBeratPanen), 3, 0);

            // Baris 2
            txtFoodLossKg = new TextBox();   // input
            txtBeratDistribusi = new TextBox { ReadOnly = true, BackColor = Color.Gainsboro, Text = "0.0" };   // otomatis

            tbl.Controls.Add(BuatField("Food Loss (kg)", txtFoodLossKg), 0, 1);
            tbl.Controls.Add(BuatField("Distribusi (kg), otomatis", txtBeratDistribusi), 1, 1);

            // Tombol
            btnSimpan = new Button
            {
                Text = "Simpan",
                Size = new Size(100, 36),
                BackColor = hijau,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSimpan.FlatAppearance.BorderSize = 0;

            btnReset = new Button
            {
                Text = "Reset",
                Size = new Size(100, 36),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.Gainsboro
            };

            FlowLayoutPanel panelTombol = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 22, 0, 0)
            };
            panelTombol.Controls.Add(btnSimpan);
            panelTombol.Controls.Add(btnReset);
            tbl.Controls.Add(panelTombol, 3, 1);

            grpInput.Controls.Add(tbl);

            // Pemilik tidak boleh menambah data: kunci semua input
            if (!_bisaEdit)
            {
                dtpTanggal.Enabled = false;
                cmbKomoditas.Enabled = false;
                txtLokasi.ReadOnly = true;
                txtBeratPanen.ReadOnly = true;
                txtFoodLossKg.ReadOnly = true;
                btnSimpan.Enabled = false;
                btnReset.Enabled = false;
            }

            // ---- Label "Data Hasil Panen" ----
            Label lblData = new Label
            {
                Text = "Data Hasil Panen",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 40,
                TextAlign = ContentAlignment.BottomLeft,
                Padding = new Padding(0, 0, 0, 6)
            };

            // ---- DataGridView ----
            BuatGrid();

            // Urutan Add: Fill dulu, lalu Top dari bawah ke atas
            konten.Controls.Add(dgvPanen);
            konten.Controls.Add(lblData);
            konten.Controls.Add(grpInput);
            konten.Controls.Add(judul);

            Controls.Add(konten);
            konten.BringToFront();

            // ---- Event ----
            txtBeratPanen.TextChanged += HitungDistribusi;
            txtFoodLossKg.TextChanged += HitungDistribusi;
            btnSimpan.Click += BtnSimpan_Click;
            btnReset.Click += (s, e) => ResetForm();
        }

        /// <summary>Membuat satu "field": label di atas, input di bawah.</summary>
        private Panel BuatField(string teksLabel, Control input)
        {
            Panel p = new Panel { Dock = DockStyle.Fill, Padding = new Padding(0, 0, 14, 0) };

            input.Dock = DockStyle.Top;

            Label lbl = new Label
            {
                Text = teksLabel,
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.BottomLeft
            };

            p.Controls.Add(input); // ditambah dulu agar berada di bawah label
            p.Controls.Add(lbl);
            return p;
        }

        private void BuatGrid()
        {
            dgvPanen = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                EnableHeadersVisualStyles = false,
                ColumnHeadersHeight = 36,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                RowTemplate = { Height = 36 }
            };

            dgvPanen.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvPanen.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvPanen.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
            dgvPanen.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;
            dgvPanen.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 247, 247);
            dgvPanen.DefaultCellStyle.SelectionBackColor = Color.FromArgb(204, 232, 255);
            dgvPanen.DefaultCellStyle.SelectionForeColor = Color.Black;

            TambahKolom("No", "No", 40, false);
            TambahKolom("Tanggal", "Tanggal", 100, false);
            TambahKolom("Komoditas", "Komoditas", 120, false);
            TambahKolom("Lokasi", "Lokasi", 100, false);
            TambahKolom("Panen", "Panen (kg)", 90, true);
            TambahKolom("Distribusi", "Distribusi (kg)", 110, true);
            TambahKolom("FoodLossKg", "Food Loss (kg)", 110, true);
        }

        private void TambahKolom(string nama, string header, int lebarMin, bool kanan)
        {
            DataGridViewTextBoxColumn kol = new DataGridViewTextBoxColumn
            {
                Name = nama,
                HeaderText = header,
                MinimumWidth = lebarMin
            };
            if (kanan)
            {
                kol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                kol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            dgvPanen.Columns.Add(kol);
        }

        // =====================================================
        //  LOGIKA
        // =====================================================

        /// <summary>Ambil data dari database lalu tampilkan di tabel.</summary>
        private void MuatData()
        {
            try
            {
                var data = _controller.GetAll(_user);

                dgvPanen.Rows.Clear();
                int no = 1;
                foreach (HasilPanen h in data)
                {
                    dgvPanen.Rows.Add(
                        no++,
                        h.Tanggal.ToString("dd/MM/yyyy"),
                        h.Komoditas,
                        h.Lokasi,
                        Fmt(h.BeratPanen),
                        Fmt(h.BeratDistribusi),
                        Fmt(h.FoodLossKg));
                }
                dgvPanen.ClearSelection();
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Akses ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal memuat data dari database: {ex.Message}", "Error Database",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>Distribusi (kg) = Berat Panen - Food Loss. Dihitung otomatis.</summary>
        private void HitungDistribusi(object? sender, EventArgs e)
        {
            if (TryParseAngka(txtBeratPanen.Text, out decimal panen) &&
                TryParseAngka(txtFoodLossKg.Text, out decimal loss) &&
                loss >= 0 && loss <= panen)
            {
                txtBeratDistribusi.Text = Fmt(panen - loss);
            }
            else
            {
                txtBeratDistribusi.Text = "0.0";
            }
        }

        private void BtnSimpan_Click(object? sender, EventArgs e)
        {
            if (!TryParseAngka(txtBeratPanen.Text, out decimal panen))
            {
                Peringatan("Berat panen harus berupa angka.", txtBeratPanen);
                return;
            }
            if (!TryParseAngka(txtFoodLossKg.Text, out decimal foodLoss))
            {
                Peringatan("Food loss harus berupa angka.", txtFoodLossKg);
                return;
            }

            var h = new HasilPanen
            {
                Tanggal = dtpTanggal.Value.Date,
                Komoditas = cmbKomoditas.Text,
                Lokasi = txtLokasi.Text,
                BeratPanen = panen,
                FoodLossKg = foodLoss
            };

            try
            {
                _controller.Simpan(_user, h);   // validasi bisnis + insert ke PostgreSQL
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(ex.Message, "Akses ditolak", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal menyimpan ke database: {ex.Message}", "Error Database",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MuatData();

            // Pilih baris terbaru
            if (dgvPanen.Rows.Count > 0)
            {
                int terakhir = dgvPanen.Rows.Count - 1;
                dgvPanen.Rows[terakhir].Selected = true;
                dgvPanen.FirstDisplayedScrollingRowIndex = terakhir;
            }

            ResetForm();
        }

        private void ResetForm()
        {
            dtpTanggal.Value = DateTime.Today;
            cmbKomoditas.SelectedIndex = 0;
            txtLokasi.Clear();
            txtBeratPanen.Clear();
            txtFoodLossKg.Clear();
            txtBeratDistribusi.Text = "0.0";
            dtpTanggal.Focus();
        }

        private void Peringatan(string pesan, Control fokus)
        {
            MessageBox.Show(pesan, "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            fokus.Focus();
        }

        /// <summary>Format 1 angka desimal dengan titik, mis. 10.0 (sama seperti desain).</summary>
        private static string Fmt(decimal nilai)
        {
            return nilai.ToString("F1", CultureInfo.InvariantCulture);
        }

        /// <summary>Menerima "60", "60.5" maupun "60,5".</summary>
        private static bool TryParseAngka(string teks, out decimal hasil)
        {
            teks = (teks ?? "").Trim().Replace(',', '.');
            return decimal.TryParse(teks, NumberStyles.Number, CultureInfo.InvariantCulture, out hasil);
        }
    }
}
