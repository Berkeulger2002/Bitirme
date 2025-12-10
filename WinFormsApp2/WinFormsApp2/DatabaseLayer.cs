using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite; // SQLite Kütüphanesi
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp2
{
    public static class DatabaseLayer
    {
        // MERKEZİ BAĞLANTI YOLU (Pooling=False ile kilitlenmeyi önledik)
        static string baglantiYolu = @"Data Source=okul.db.db;Version=3;Pooling=False;";

        // =============================================================
        // BÖLÜM 1: GİRİŞ İŞLEMLERİ (FORM 1)
        // =============================================================

        public static string OgretmenGirisYap(string kadi, string sifre)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT KullaniciAdi FROM Ogretmenler WHERE KullaniciAdi=@kadi AND Sifre=@sifre";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@kadi", kadi);
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        object sonuc = komut.ExecuteScalar();
                        return (sonuc != null) ? sonuc.ToString() : null;
                    }
                }
                catch { return null; }
            }
        }

        public static string OgrenciGirisYap(string numara, string sifre)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT OgrenciIsmi FROM Ogrenciler WHERE Numara=@no AND Sifre=@sifre";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@no", numara);
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        object sonuc = komut.ExecuteScalar();
                        return (sonuc != null) ? sonuc.ToString() : null;
                    }
                }
                catch { return null; }
            }
        }

        // =============================================================
        // BÖLÜM 2: ÖĞRETMEN YÖNETİMİ (FORM 2)
        // =============================================================

        public static DataTable OgrencileriGetir(string ogretmenAdi)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT OgrenciIsmi 'Öğrenci İsmi', Numara, Sifre 'Şifre', Ogretmen FROM Ogrenciler WHERE Ogretmen=@ogr";

                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@ogr", ogretmenAdi);
                        using (SQLiteDataAdapter adaptor = new SQLiteDataAdapter(komut))
                        {
                            DataTable tablo = new DataTable();
                            adaptor.Fill(tablo);
                            return tablo;
                        }
                    }
                }
                catch { return null; }
            }
        }

        public static bool OgrenciEkle(string isim, string numara, string sifre, string ogretmenAdi)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "INSERT INTO Ogrenciler (OgrenciIsmi, Numara, Sifre, Ogretmen) VALUES (@isim, @no, @sifre, @ogr)";

                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@isim", isim);
                        komut.Parameters.AddWithValue("@no", numara);
                        komut.Parameters.AddWithValue("@sifre", sifre);
                        komut.Parameters.AddWithValue("@ogr", ogretmenAdi);
                        komut.ExecuteNonQuery();
                        return true;
                    }
                }
                catch { return false; }
            }
        }

        // Öğretmenin öğrenciye attığı özel mesajları kaydeder
        public static bool MesajGonder(string gonderen, string aliciNo, string mesaj)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "INSERT INTO Mesajlar (Gonderen, AlanOgrenciNo, MesajIcerigi) VALUES (@gonderen, @aliciNo, @mesaj)";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@gonderen", gonderen);
                        komut.Parameters.AddWithValue("@aliciNo", aliciNo);
                        komut.Parameters.AddWithValue("@mesaj", mesaj);
                        komut.ExecuteNonQuery();
                        return true;
                    }
                }
                catch { return false; }
            }
        }

        // =============================================================
        // BÖLÜM 3: ÖĞRENCİ ANA SAYFASI (FORM 3) - YENİ EKLENEN
        // =============================================================

        // Öğrenciye gelen özel mesajları getirir
        public static DataTable OgrenciOzelMesajlariGetir(string ogrenciNo)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT Gonderen, MesajIcerigi FROM Mesajlar WHERE AlanOgrenciNo = @numara";

                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@numara", ogrenciNo);
                        using (SQLiteDataAdapter adaptor = new SQLiteDataAdapter(komut))
                        {
                            DataTable tablo = new DataTable();
                            adaptor.Fill(tablo);
                            return tablo;
                        }
                    }
                }
                catch { return null; }
            }
        }

        #region Sohbet Modülü
        // =============================================================
        // BÖLÜM 4: SOHBET MODÜLÜ (FORM 4)
        // =============================================================

        public static DataRow OgrenciBilgisiGetir(string numara)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT OgrenciIsmi, Ogretmen FROM Ogrenciler WHERE Numara=@no";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@no", numara);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0) return dt.Rows[0];
                            else return null;
                        }
                    }
                }
                catch { return null; }
            }
        }

        public static DataTable SohbetMesajlariniGetir(string ogretmenGrubu)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "SELECT * FROM GrupSohbeti WHERE OgretmenGrubu=@grup ORDER BY Id ASC";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@grup", ogretmenGrubu);
                        using (SQLiteDataAdapter da = new SQLiteDataAdapter(komut))
                        {
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
                catch { return null; }
            }
        }

        public static bool SohbetMesajiEkle(string isim, string mesaj, string ogretmenGrubu)
        {
            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();
                    string sql = "INSERT INTO GrupSohbeti (GonderenIsmi, Mesaj, Saat, OgretmenGrubu) VALUES (@isim, @mesaj, @saat, @grup)";
                    using (SQLiteCommand komut = new SQLiteCommand(sql, baglanti))
                    {
                        komut.Parameters.AddWithValue("@isim", isim);
                        komut.Parameters.AddWithValue("@mesaj", mesaj);
                        komut.Parameters.AddWithValue("@saat", DateTime.Now.ToString("HH:mm"));
                        komut.Parameters.AddWithValue("@grup", ogretmenGrubu);
                        komut.ExecuteNonQuery();
                        return true;
                    }
                }
                catch { return false; }
            }
        }
        #endregion
    }
}