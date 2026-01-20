using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.Windows.Forms;

public class FormGudang : Form
{
    private Label lblJudul;
    private TextBox txtJumlahMasuk;
    private Button btnSimpan;

    public FormGudang()
    {
        Text = "Form Gudang";
        Width = 400;
        Height = 250;
        StartPosition = FormStartPosition.CenterScreen;

        lblJudul = new Label()
        {
            Text = "Input Stok Gudang (kilo)",
            Left = 20,
            Top = 20,
            Width = 300,
            Font = new Font("Segoe UI", 12, FontStyle.Bold)
        };

        txtJumlahMasuk = new TextBox() { Left = 20, Top = 60, Width = 200 };

        btnSimpan = new Button()
        {
            Text = "Simpan",
            Left = 240,
            Top = 60,
            Width = 100
        };
        btnSimpan.Click += BtnSimpan_Click;

        Controls.Add(lblJudul);
        Controls.Add(txtJumlahMasuk);
        Controls.Add(btnSimpan);
    }

    // Fungsi simpan stok gudang ke database
    private void BtnSimpan_Click(object? sender, EventArgs e)
    {
        try
        {
            string dbPath = System.IO.Path.Combine(Application.StartupPath, "umkm_telur.db");
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            string query = "INSERT INTO stok (tanggal, jumlah_masuk, jumlah_keluar, sisa) VALUES (@t, @jm, 0, @s)";
            using var cmd = new SqliteCommand(query, conn);
            cmd.Parameters.AddWithValue("@t", DateTime.Now.ToString("yyyy-MM-dd"));
            cmd.Parameters.AddWithValue("@jm", int.Parse(txtJumlahMasuk.Text.Trim()));
            cmd.Parameters.AddWithValue("@s", int.Parse(txtJumlahMasuk.Text.Trim()));
            cmd.ExecuteNonQuery();

            MessageBox.Show("Stok gudang berhasil ditambahkan!", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtJumlahMasuk.Clear();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error simpan gudang: " + ex.Message);
        }
    }
}