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

                    // Pindah ke Form utama (sesuaikan dengan nama form utama dari temanmu, misal Form1)
                    Form1 mainForm = new Form1();
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
    }
}
