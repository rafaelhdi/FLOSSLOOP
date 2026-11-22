using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using FLOSSLOOP.Controllers;
using FLOSSLOOP.Models;

namespace FLOSSLOOP.Views
{
    public partial class LoginForm : Form
    {
        private readonly LoginController _loginController = new LoginController();

        public LoginForm()
        {
            InitializeComponent();
            this.AcceptButton = buttonLogin;
        }

        // 2. Fungsi ini otomatis jalan saat tombol hijau di desain diklik
        private void buttonLogin_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Mengambil teks dari desain (txtEmail & txtPassword) lalu dikirim ke database via Controller
                User? user = _loginController.Login(textBoxEmail.Text, textBoxPassword.Text);

                if (user != null)
                {
                    MessageBox.Show($"Selamat datang, {user.Nama} ({user.Role})!", "Login Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Pindah ke form Pencatatan Hasil Panen, bawa data user (untuk pengecekan role)
                    FormPencatatan mainForm = new FormPencatatan(user);
                    mainForm.FormClosed += (s, args) => Application.Exit();
                    mainForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Email atau password salah!", "Login Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxPassword.Clear();
                    textBoxPassword.Focus();
                }
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Peringatan", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal terhubung ke database: {ex.Message}", "Error Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
