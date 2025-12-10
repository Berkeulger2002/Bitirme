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
    public partial class Form1 : Form
    {
        // SENİN VERDİĞİN DOĞRU DOSYA YOLU (Bunu tepeye sabitliyoruz)
        string dosyaYolu = @"Data Source=okul.db.db;Version=3;Pooling=False;";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOgretmenGiris_Click(object sender, EventArgs e)
        {
            if (textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Lütfen alanları doldurun.");
                return;
            }

            // DEĞİŞEN KISIM: Uzun SQL yerine DatabaseLayer'a soruyoruz
            string girisYapan = DatabaseLayer.OgretmenGirisYap(textBox3.Text, textBox4.Text);

            if (girisYapan != null)
            {
                MessageBox.Show("Giriş Başarılı!");
                Form2 yonetim = new Form2(girisYapan); // İsmi gönderiyoruz
                yonetim.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Giriş!");
            }
        }

        private void btnOgrenciGiris_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen alanları doldurun.");
                return;
            }

            // DEĞİŞEN KISIM: SQL yok, sadece fonksiyonu çağırıyoruz
            string ogrenciIsmi = DatabaseLayer.OgrenciGirisYap(textBox1.Text, textBox2.Text);

            if (ogrenciIsmi != null)
            {
                MessageBox.Show("Hoşgeldin " + ogrenciIsmi);
                Form3 anaSayfa = new Form3(textBox1.Text); // Numarayı gönderiyoruz
                anaSayfa.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Hatalı Numara veya Şifre!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
