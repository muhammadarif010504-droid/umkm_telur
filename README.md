# Aplikasi UMKM Telur

## Deskripsi
Aplikasi desktop berbasis C# WinForms untuk membantu UMKM dalam mencatat stok telur di gudang dan transaksi penjualan di toko.

## Fitur
- Login pengguna (admin/user)
- Dashboard stok dan penjualan
- Input stok gudang
- Input transaksi toko
- Laporan penjualan dan hapus data
- Logout dengan dialog konfirmasi

## Teknologi
- C# WinForms
- SQLite
- Visual Studio Code
- Git & GitHub

## Cara Menjalankan
1. Pastikan .NET SDK sudah terinstall di komputer
2. Buka terminal di folder project
3. Jalankan perintah berikut: "dotnet run"
4. Login menggunakan akun yang sudah terdaftar di tabel `users`

## Struktur Database
- **users**: menyimpan data login pengguna
- `username` (TEXT)
- `password_hash` (TEXT)
- `role` (TEXT)

- **stok**: mencatat stok masuk ke gudang
- `tanggal` (TEXT)
- `jumlah_masuk` (INTEGER)
- `jumlah_keluar` (INTEGER)
- `sisa` (INTEGER)

- **transaksi**: mencatat penjualan ke pembeli
- `nama_pembeli` (TEXT)
- `jumlah` (INTEGER)
- `tanggal` (TEXT)

## Kontributor
- Muhammad Arif (3202416126)