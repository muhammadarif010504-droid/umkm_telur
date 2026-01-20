using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

public class FormGudang : Form
{
    private TextBox txtJumlahMasuk;
    private DateTimePicker dtTanggal;
    private Button btnSimpan;
    private Label lblStatus;

    public FormGudang()
    {
        Text = "Form Gudang - Input Stok Masuk";
        Width = 400;
        Height = 300;
        StartPosition = FormStartPosition.CenterScreen;

        Label lblJumlah = new Label() { Text = "Jumlah Masuk (kilo)", Left = 20, Top = 20, Width = 120 };
        txtJumlahMasuk = new TextBox() { Left = 160, Top = 20, Width = 180 };

        Label lblTanggal = new Label() { Text = "Tanggal", Left = 20, Top = 60, Width = 120 };
        dtTanggal = new DateTimePicker() { Left = 160, Top = 60, Width = 180 };

        btnSimpan = new Button() { Text = "Simpan", Left = 160, Top = 100, Width = 100 };
        btnSimpan.Click += BtnSimpan_Click;

        lblStatus = new Label() { Left = 20, Top = 150, Width = 300, ForeColor = Color.Green };

        Controls.AddRange(new Control[] { lblJumlah, txtJumlahMasuk, lblTanggal, dtTanggal, btnSimpan, lblStatus });
    }

    private void BtnSimpan_Click(object? sender, EventArgs e)
    {
        try
        {
            int masuk = int.Parse(txtJumlahMasuk.Text.Trim());
            string tanggal = dtTanggal.Value.ToString("yyyy-MM-dd");

            string dbPath = System.IO.Path.Combine(Application.StartupPath, "umkm_telur.db");
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            string query = "INSERT INTO stok (tanggal, jumlah_masuk, jumlah_keluar, sisa) VALUES (@t, @jm, 0, @s)";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@t", tanggal);
            cmd.Parameters.AddWithValue("@jm", masuk);
            cmd.Parameters.AddWithValue("@s", masuk); // awalnya sisa = jumlah masuk

            cmd.ExecuteNonQuery();
            lblStatus.Text = "Stok berhasil disimpan.";
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + ex.Message;
        }
    }
}