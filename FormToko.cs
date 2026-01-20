using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

public class FormToko : Form
{
    private TextBox txtNama;
    private TextBox txtJumlah;
    private DateTimePicker dtTanggal;
    private Button btnSimpan;
    private Label lblStatus;

    public FormToko()
    {
        Text = "Form Toko - Input Transaksi";
        Width = 400;
        Height = 300;
        StartPosition = FormStartPosition.CenterScreen;

        Label lblNama = new Label() { Text = "Nama Pembeli", Left = 20, Top = 20, Width = 100 };
        txtNama = new TextBox() { Left = 140, Top = 20, Width = 200 };

        Label lblJumlah = new Label() { Text = "Jumlah (kilo)", Left = 20, Top = 60, Width = 100 };
        txtJumlah = new TextBox() { Left = 140, Top = 60, Width = 200 };

        Label lblTanggal = new Label() { Text = "Tanggal", Left = 20, Top = 100, Width = 100 };
        dtTanggal = new DateTimePicker() { Left = 140, Top = 100, Width = 200 };

        btnSimpan = new Button() { Text = "Simpan", Left = 140, Top = 150, Width = 100 };
        btnSimpan.Click += BtnSimpan_Click;

        lblStatus = new Label() { Left = 20, Top = 200, Width = 300, ForeColor = Color.Green };

        Controls.AddRange(new Control[] { lblNama, txtNama, lblJumlah, txtJumlah, lblTanggal, dtTanggal, btnSimpan, lblStatus });
    }

    private void BtnSimpan_Click(object? sender, EventArgs e)
    {
        try
        {
            string dbPath = System.IO.Path.Combine(Application.StartupPath, "umkm_telur.db");
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            string query = "INSERT INTO transaksi (nama_pembeli, jumlah, tanggal) VALUES (@n, @j, @t)";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@n", txtNama.Text.Trim());
            cmd.Parameters.AddWithValue("@j", int.Parse(txtJumlah.Text.Trim())); // jumlah dalam kilo
            cmd.Parameters.AddWithValue("@t", dtTanggal.Value.ToString("yyyy-MM-dd"));

            cmd.ExecuteNonQuery();
            lblStatus.Text = "Transaksi berhasil disimpan.";
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + ex.Message;
        }
    }
}