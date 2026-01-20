using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

namespace umkm_telur
{
    public class FormLaporan : Form
    {
        private DataGridView dgvTransaksi;
        private Label lblTotal;
        private Button btnDelete;

        public FormLaporan()
        {
            Text = "Form Laporan";
            Width = 600;
            Height = 400;
            StartPosition = FormStartPosition.CenterScreen;

            dgvTransaksi = new DataGridView()
            {
                Left = 20,
                Top = 20,
                Width = 540,
                Height = 280,
                ReadOnly = true,
                AllowUserToAddRows = false
            };

            lblTotal = new Label()
            {
                Left = 20,
                Top = 320,
                Width = 400,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            btnDelete = new Button()
            {
                Text = "Hapus Data",
                Left = 460,
                Top = 320,
                Width = 100,
                Height = 30
            };
            btnDelete.Click += BtnDelete_Click;

            Controls.Add(dgvTransaksi);
            Controls.Add(lblTotal);
            Controls.Add(btnDelete);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                string dbPath = Application.StartupPath + "\\umkm_telur.db";
                using var conn = new SqliteConnection($"Data Source={dbPath}");
                conn.Open();

                string query = "SELECT nama_pembeli, jumlah AS 'Jumlah (kilo)', tanggal FROM transaksi ORDER BY tanggal DESC";
                using var cmd = new SqliteCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                DataTable table = new DataTable();
                table.Load(reader);
                dgvTransaksi.DataSource = table;

                using var cmdTotal = new SqliteCommand("SELECT SUM(jumlah) FROM transaksi", conn);
                object? result = cmdTotal.ExecuteScalar();
                lblTotal.Text = "Total Penjualan: " + (result?.ToString() ?? "0") + " kilo";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error load laporan: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvTransaksi.CurrentRow != null)
            {
                var result = MessageBox.Show("Apakah kamu yakin ingin menghapus data ini?",
                                             "Konfirmasi Hapus",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    try
                    {
                        // Ambil nilai nama_pembeli dengan aman
                        object? cellValue = dgvTransaksi.CurrentRow.Cells["nama_pembeli"].Value;
                        string namaPembeli = cellValue?.ToString() ?? "";

                        if (string.IsNullOrWhiteSpace(namaPembeli))
                        {
                            MessageBox.Show("Data tidak valid untuk dihapus.");
                            return;
                        }

                        string dbPath = Application.StartupPath + "\\umkm_telur.db";
                        using var conn = new SqliteConnection($"Data Source={dbPath}");
                        conn.Open();

                        string query = "DELETE FROM transaksi WHERE nama_pembeli = @nama";
                        using var cmd = new SqliteCommand(query, conn);
                        cmd.Parameters.AddWithValue("@nama", namaPembeli);
                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Data berhasil dihapus!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error hapus data: " + ex.Message);
                    }
                }
            }
        }
    }
}