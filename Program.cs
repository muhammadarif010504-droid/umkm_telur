using System;
using System.Windows.Forms;
using umkm_telur; // pastikan namespace sama dengan file FormLogin.cs

namespace umkm_telur
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Jalankan FormLogin sebagai form utama
            Application.Run(new FormLogin());
        }
    }
}