using System;
using Microsoft.Data.Sqlite;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

public class FormLogin : Form
{
    private Label lblUsername;
    private TextBox txtUsername;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnLogin;
    private Label lblStatus;

    public FormLogin()
    {
        this.Text = "Login UMKM Telur";
        this.Width = 320;
        this.Height = 240;
        this.StartPosition = FormStartPosition.CenterScreen;

        lblUsername = new Label() { Text = "Username", Left = 20, Top = 20, Width = 100 };
        txtUsername = new TextBox() { Left = 130, Top = 20, Width = 150 };

        lblPassword = new Label() { Text = "Password", Left = 20, Top = 60, Width = 100 };
        txtPassword = new TextBox() { Left = 130, Top = 60, Width = 150, PasswordChar = '*' };

        btnLogin = new Button() { Text = "Login", Left = 130, Top = 100, Width = 80 };
        btnLogin.Click += BtnLogin_Click;

        lblStatus = new Label() { Left = 20, Top = 140, Width = 260, ForeColor = Color.Red };

        Controls.AddRange(new Control[] { lblUsername, txtUsername, lblPassword, txtPassword, btnLogin, lblStatus });
    }

    private void BtnLogin_Click(object? sender, EventArgs e)
    {
        try
        {
            string dbPath = Path.Combine(Application.StartupPath, "umkm_telur.db");
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            string query = "SELECT * FROM users WHERE username=@u AND password_hash=@p";
            using var cmd = new SqliteCommand(query, conn);

            cmd.Parameters.AddWithValue("@u", txtUsername.Text.Trim());
            cmd.Parameters.AddWithValue("@p", txtPassword.Text.Trim());

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                string role = reader["role"]?.ToString() ?? "user";
                Hide();
                new FormDashboard(txtUsername.Text, role).Show();
            }
            else
            {
                lblStatus.Text = "Login gagal, periksa username/password.";
            }
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Error: " + ex.Message;
        }
    }
}