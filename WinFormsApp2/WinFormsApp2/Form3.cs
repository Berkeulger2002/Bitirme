using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form3 : Form
    {
        string dosyaYolu = @"Data Source=okul.db.db;Version=3;Pooling=False;";
        string gelenOgrenciNo;
        public Form3(string no)
        {
            InitializeComponent();
            gelenOgrenciNo = no;
        }
        private void Form3_Load(object sender, EventArgs e)
        {
            MesajlariGetir();
        }

        private void MesajlariGetir()
        {
            // DatabaseLayer'a "Bu numaranın özel mesajlarını getir" diyoruz.
            DataTable tablo = DatabaseLayer.OgrenciOzelMesajlariGetir(gelenOgrenciNo);

            if (tablo != null)
            {
                dataGridView1.DataSource = tablo;
            }
            else
            {
                MessageBox.Show("Mesajlar yüklenirken bir hata oluştu.");
            }
        }
        private void btnCikis_Click(object sender, EventArgs e)
        {
            // 1. Yeni bir giriş ekranı (Form1) oluştur
            Form1 girisEkrani = new Form1();

            // 2. Giriş ekranını göster
            girisEkrani.Show();

            // 3. Şu anki ana ekranı (Form3) kapat
            this.Close();
        }

        private void Form3_Load_1(object sender, EventArgs e)
        {
            MesajlariGetir();

        }

        private void btnSohbetAc_Click(object sender, EventArgs e)
        {
            // Form3'teki öğrenci numarasını (gelenOgrenciNo) Form4'e gönderiyoruz
            Form4 sohbetEkrani = new Form4(gelenOgrenciNo);
            sohbetEkrani.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
