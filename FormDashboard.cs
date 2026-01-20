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
        private Label lblSisa;
        private TextBox txtStok;
        private TextBox txtTerjual;
        private Button btnUpdate;
        private Button btnGudang;
        private Button btnToko;
        private Button btnLaporan;
        private Button btnLogout;
        private DateTimePicker datePicker; // pilih tanggal

        public FormDashboard(string user, string role)
        {
            Text = "Dashboard UMKM Telur";
            Width = 650;
            Height = 500;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.White;

            lblHeader = new Label()
            {
                Text = "🕊 Selamat datang Ariff, semangat ya buat hari ini! 🕊",
                Left = 20,
                Top = 20,
                Width = 600,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DarkGreen
            };

            // DateTimePicker
            datePicker = new DateTimePicker()
            {
                Format = DateTimePickerFormat.Short,
                Left = 400,
                Top = 60,
                Width = 120
            };
            datePicker.ValueChanged += (s, e) => LoadData();

            lblGudang = new Label()
            {
                Text = "Total Stok: -",
                Left = 20,
                Top = 100,
                Width = 400,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.ForestGreen
            };

            lblToko = new Label()
            {
                Text = "Total Terjual: -",
                Left = 20,
                Top = 140,
                Width = 400,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.RoyalBlue
            };

            lblSisa = new Label()
            {
                Text = "Total Sisa: -",
                Left = 20,
                Top = 180,
                Width = 400,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.DarkOrange
            };

            txtStok = new TextBox() { Left = 200, Top = 100, Width = 100 };
            txtTerjual = new TextBox() { Left = 200, Top = 140, Width = 100 };

            btnUpdate = new Button()
            {
                Text = "Update",
                Left = 320,
                Top = 120,
                Width = 80
            };
            btnUpdate.Click += BtnUpdate_Click;

            btnGudang = new Button() { Text = "📄 Form Gudang", Left = 20, Top = 240, Width = 160, Height = 40 };
            btnGudang.Click += (s, e) => new FormGudang().Show();

            btnToko = new Button() { Text = "🛒 Form Toko", Left = 200, Top = 240, Width = 160, Height = 40 };
            btnToko.Click += (s, e) => new FormToko().Show();

            btnLaporan = new Button() { Text = "📊 Form Laporan", Left = 380, Top = 240, Width = 160, Height = 40 };
            btnLaporan.Click += (s, e) => new FormLaporan().Show();

            btnLogout = new Button() { Text = "Logout", Left = 20, Top = 300, Width = 160, Height = 40 };
            btnLogout.Click += BtnLogout_Click;

            Controls.AddRange(new Control[] {
                lblHeader, datePicker, lblGudang, lblToko, lblSisa,
                txtStok, txtTerjual, btnUpdate,
                btnGudang, btnToko, btnLaporan, btnLogout
            });

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string dbPath = Application.StartupPath + "\\umkm_telur.db";
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                string tanggalDipilih = datePicker.Value.ToString("yyyy-MM-dd");

                // Total stok masuk
                using var cmd1 = new SqliteCommand("SELECT SUM(jumlah_masuk) FROM stok WHERE tanggal = @tgl", conn);
                cmd1.Parameters.AddWithValue("@tgl", tanggalDipilih);
                object? totalStok = cmd1.ExecuteScalar();

                // Total terjual
                using var cmd2 = new SqliteCommand("SELECT SUM(jumlah) FROM transaksi WHERE tanggal = @tgl", conn);
                cmd2.Parameters.AddWithValue("@tgl", tanggalDipilih);
                object? totalTerjual = cmd2.ExecuteScalar();

                // ✅ cek null/DBNull
                int stok = (totalStok != null && totalStok != DBNull.Value) ? Convert.ToInt32(totalStok) : 0;
                int terjual = (totalTerjual != null && totalTerjual != DBNull.Value) ? Convert.ToInt32(totalTerjual) : 0;
                int sisa = stok - terjual;

                lblGudang.Text = "Total Stok (" + tanggalDipilih + "): " + stok + " kilo";
                lblToko.Text = "Total Terjual (" + tanggalDipilih + "): " + terjual + " kilo";
                lblSisa.Text = "Total Sisa (" + tanggalDipilih + "): " + sisa + " kilo";
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

                string tanggalDipilih = datePicker.Value.ToString("yyyy-MM-dd");

                if (!string.IsNullOrWhiteSpace(txtStok.Text))
                {
                    int jumlahMasuk = int.Parse(txtStok.Text.Trim());
                    string qStok = "INSERT INTO stok (tanggal, jumlah_masuk, jumlah_keluar, sisa) VALUES (@t, @jm, 0, @s)";
                    using var cmdStok = new SqliteCommand(qStok, conn);
                    cmdStok.Parameters.AddWithValue("@t", tanggalDipilih);
                    cmdStok.Parameters.AddWithValue("@jm", jumlahMasuk);
                    cmdStok.Parameters.AddWithValue("@s", jumlahMasuk);
                    cmdStok.ExecuteNonQuery();
                }

                if (!string.IsNullOrWhiteSpace(txtTerjual.Text))
                {
                    int jumlahTerjual = int.Parse(txtTerjual.Text.Trim());
                    string qTrans = "INSERT INTO transaksi (nama_pembeli, jumlah, tanggal) VALUES ('Manual', @j, @t)";
                    using var cmdTrans = new SqliteCommand(qTrans, conn);
                    cmdTrans.Parameters.AddWithValue("@j", jumlahTerjual);
                    cmdTrans.Parameters.AddWithValue("@t", tanggalDipilih);
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