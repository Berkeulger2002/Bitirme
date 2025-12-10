using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
namespace WinFormsApp2
{
    public partial class Form2 : Form
    {
        string _girisYapanOgretmen;
        public Form2(string ogretmenAdi)
        {
            InitializeComponent();
            _girisYapanOgretmen = ogretmenAdi;

            this.Text = "Hoşgeldin: " + _girisYapanOgretmen; // Başlığa yazalım
            Listele(); // Program açılınca çalışır
        }
        private void Listele()
        {
            // DatabaseLayer'dan veriyi çekiyoruz
            DataTable tablo = DatabaseLayer.OgrencileriGetir(_girisYapanOgretmen);

            if (tablo != null)
            {
                dataGridView1.DataSource = tablo;
            }
            else
            {
                MessageBox.Show("Veriler yüklenirken bir sorun oluştu.");
            }
        }
        private void btnOgrenciKaydet_Click(object sender, EventArgs e)
        {
            // DatabaseLayer'a gönderiyoruz
            bool sonuc = DatabaseLayer.OgrenciEkle(textBox1.Text, textBox2.Text, textBox3.Text, _girisYapanOgretmen);

            if (sonuc == true)
            {
                MessageBox.Show("Kayıt Başarılı! Kaydeden: " + _girisYapanOgretmen);
                Listele(); // Listeyi yenile
            }
            else
            {
                MessageBox.Show("Kayıt sırasında hata oluştu!");
            }
        }

        private void dgvOgrenciler_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string baglantiYolu = @"Data Source=okul.db.db;Version=3;Pooling=False;";

            using (SQLiteConnection baglanti = new SQLiteConnection(baglantiYolu))
            {
                try
                {
                    baglanti.Open();

                    // 1. Verileri çekme sorgusu (SELECT)
                    string sorgu = "SELECT * FROM Ogrenciler";

                    // 2. Verileri tabloya doldurmak için bir 'DataAdapter' kullanıyoruz
                    SQLiteDataAdapter adaptor = new SQLiteDataAdapter(sorgu, baglanti);

                    // 3. Hafızada sanal bir tablo oluşturuyoruz
                    DataTable tablo = new DataTable();

                    // 4. Verileri bu sanal tabloya döküyoruz
                    adaptor.Fill(tablo);

                    // 5. DataGridView aracına bu tabloyu bağlıyoruz
                    dataGridView1.DataSource = tablo;
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata: " + hata.Message);
                }
            }
        }

        private void btnCikis_Click(object sender, EventArgs e)
        {
            // 1. Giriş ekranını (Form1) tekrar oluşturuyoruz
            Form1 girisEkrani = new Form1();

            // 2. Giriş ekranını kullanıcıya gösteriyoruz
            girisEkrani.Show();

            // 3. Şu anki yönetim panelini (Form2) kapatıyoruz
            this.Close();
        }

        private void btnMesajGonder_Click(object sender, EventArgs e)
        {
            if (textBox4.Text == "" || textBox5.Text == "")
            {
                MessageBox.Show("Lütfen öğrenci numarasını ve mesajı giriniz.");
                return;
            }

            // Gönderen kısmına 'Öğretmen' veya kendi adını (_girisYapanOgretmen) yazabilirsin.
            // Ben buraya _girisYapanOgretmen yazdım ki mesajı kimin attığı belli olsun.
            bool sonuc = DatabaseLayer.MesajGonder(_girisYapanOgretmen, textBox4.Text, textBox5.Text);

            if (sonuc == true)
            {
                MessageBox.Show("Mesaj Başarıyla Gönderildi!");
                textBox5.Text = ""; // Mesaj kutusunu temizle
            }
            else
            {
                MessageBox.Show("Mesaj gönderilemedi.");
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
    

