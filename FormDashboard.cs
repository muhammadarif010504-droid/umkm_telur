using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

namespace umkm_telur
{
    public class FormDashboard : Form
    {
        private Label lblHeader;
        private Label lblGudang;
        private Label lblToko;
        private TextBox txtStok;
        private TextBox txtTerjual;
        private Button btnUpdate;
        private Button btnGudang;
        private Button btnToko;
        private Button btnLaporan;
        private Button btnLogout;

        public FormDashboard(string user, string role)
        {
            Text = "Dashboard UMKM Telur";
            Width = 600;
            Height = 400;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            lblHeader = new Label()
            {
                Text = $"Selamat datang, {user} ({role})",
                Left = 20,
                Top = 20,
                Width = 500,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };

            lblGudang = new Label()
            {
                Text = "Total Stok: -",
                Left = 20,
                Top = 80,
                Width = 400,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            lblToko = new Label()
            {
                Text = "Total Terjual: -",
                Left = 20,
                Top = 120,
                Width = 400,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            txtStok = new TextBox() { Left = 200, Top = 80, Width = 100 };
            txtTerjual = new TextBox() { Left = 200, Top = 120, Width = 100 };

            btnUpdate = new Button()
            {
                Text = "Update",
                Left = 320,
                Top = 100,
                Width = 80
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnGudang = new Button() { Text = "📦 Form Gudang", Left = 20, Top = 200, Width = 160, Height = 40 };
            btnGudang.Click += (s, e) => new FormGudang().Show();

            btnToko = new Button() { Text = "🛒 Form Toko", Left = 200, Top = 200, Width = 160, Height = 40 };
            btnToko.Click += (s, e) => new FormToko().Show();

            btnLaporan = new Button() { Text = "📊 Form Laporan", Left = 380, Top = 200, Width = 160, Height = 40 };
            btnLaporan.Click += (s, e) => new FormLaporan().Show();

            btnLogout = new Button() { Text = "Logout", Left = 20, Top = 260, Width = 160, Height = 40 };
            btnLogout.Click += BtnLogout_Click;

            Controls.AddRange(new Control[] { lblHeader, lblGudang, lblToko, txtStok, txtTerjual, btnUpdate, btnGudang, btnToko, btnLaporan, btnLogout });

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string dbPath = Application.StartupPath + "\\umkm_telur.db";
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                using var cmd1 = new SqliteCommand("SELECT SUM(sisa) FROM stok", conn);
                object? totalStok = cmd1.ExecuteScalar();
                lblGudang.Text = "Total Stok: " + (totalStok?.ToString() ?? "0") + " kilo";

                using var cmd2 = new SqliteCommand("SELECT SUM(jumlah) FROM transaksi", conn);
                object? totalTerjual = cmd2.ExecuteScalar();
                lblToko.Text = "Total Terjual: " + (totalTerjual?.ToString() ?? "0") + " kilo";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error load dashboard: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            try
            {
                string dbPath = Application.StartupPath + "\\umkm_telur.db";
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                if (!string.IsNullOrWhiteSpace(txtStok.Text))
                {
                    string qStok = "INSERT INTO stok (tanggal, jumlah_masuk, jumlah_keluar, sisa) VALUES (@t, @jm, 0, @s)";
                    using var cmdStok = new SqliteCommand(qStok, conn);
                    cmdStok.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdStok.Parameters.AddWithValue("@jm", int.Parse(txtStok.Text.Trim()));
                    cmdStok.Parameters.AddWithValue("@s", int.Parse(txtStok.Text.Trim()));
                    cmdStok.ExecuteNonQuery();
                }

                if (!string.IsNullOrWhiteSpace(txtTerjual.Text))
                {
                    string qTrans = "INSERT INTO transaksi (nama_pembeli, jumlah, tanggal) VALUES ('Manual', @j, @t)";
                    using var cmdTrans = new SqliteCommand(qTrans, conn);
                    cmdTrans.Parameters.AddWithValue("@j", int.Parse(txtTerjual.Text.Trim()));
                    cmdTrans.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmdTrans.ExecuteNonQuery();
                }

                MessageBox.Show("Data berhasil disimpan.");
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error update: " + ex.Message);
            }
        }

        private void BtnLogout_Click(object? sender, EventArgs e)
        {
            var result = MessageBox.Show("Apakah kamu yakin ingin logout?",
                                         "Konfirmasi Logout",
                                         MessageBoxButtons.YesNo,
                                         MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
                FormLogin login = new FormLogin();
                login.Show();
            }
        }
    }
}