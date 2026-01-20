using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

public class FormToko : Form
{
    private Label lblJudul;
    private TextBox txtNamaPembeli;
    private TextBox txtJumlah;
    private Button btnSimpan;

    public FormToko()
    {
        Text = "Form Toko";
        Width = 400;
        Height = 300;
        StartPosition = FormStartPosition.CenterScreen;

        lblJudul = new Label()
        {
            Text = "Input Transaksi Toko",
            Left = 20,
            Top = 20,
            Width = 300,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };

        txtNamaPembeli = new TextBox() { Left = 20, Top = 60, Width = 200, PlaceholderText = "Nama Pembeli" };
        txtJumlah = new TextBox() { Left = 20, Top = 100, Width = 200, PlaceholderText = "Jumlah (kilo)" };

        btnSimpan = new Button()
        {
            Text = "Simpan",
            Left = 240,
            Top = 80,
            Width = 100
        };
        btnSimpan.Click += BtnSimpan_Click;

        Controls.Add(lblJudul);
        Controls.Add(txtNamaPembeli);
        Controls.Add(txtJumlah);
        Controls.Add(btnSimpan);
    }

    // Fungsi simpan transaksi toko ke database
    private void BtnSimpan_Click(object? sender, EventArgs e)
    {
        try
        {
            string dbPath = System.IO.Path.Combine(Application.StartupPath, "umkm_telur.db");
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            string query = "INSERT INTO transaksi (nama_pembeli, jumlah, tanggal) VALUES (@n, @j, @t)";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@n", txtNamaPembeli.Text.Trim());
            cmd.Parameters.AddWithValue("@j", int.Parse(txtJumlah.Text.Trim()));
            cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();

            MessageBox.Show("Transaksi toko berhasil ditambahkan!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtNamaPembeli.Clear();
            txtJumlah.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error simpan transaksi: " + ex.Message);
        }
    }
}