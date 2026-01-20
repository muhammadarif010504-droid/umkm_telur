using System;
using System.Data;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

public class FormLaporan : Form
{
    private DataGridView dgvTransaksi;
    private Label lblTotal;

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

        Controls.Add(dgvTransaksi);
        Controls.Add(lblTotal);

        LoadData();
    }

    private void LoadData()
    {
        try
        {
            string dbPath = System.IO.Path.Combine(Application.StartupPath, "umkm_telur.db");
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
}