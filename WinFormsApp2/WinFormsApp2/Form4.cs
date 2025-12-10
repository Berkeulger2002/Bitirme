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
    public partial class Form4 : Form
    {
        string dosyaYolu = @"Data Source=okul.db.db;Version=3;Pooling=False;";

        string ogrenciNo;   // Giriş yapanın numarası
        private Button button2;
        string ogrenciAdi;  // Veritabanından çekeceğimiz ismi
        private Panel panel1;
        string bagliOlduguOgretmen; // Bu öğrenci hangi hocanın grubunda?
        public Form4(string gelenNo)
        {
            InitializeComponent();
            ogrenciNo = gelenNo;
        }

        private void InitializeComponent()
        {
            components = new Container();
            richTextBox1 = new RichTextBox();
            textBox1 = new TextBox();
            button1 = new Button();
            timer1 = new Timer(components);
            button2 = new Button();
            panel1 = new Panel();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // richTextBox1
            // 
            richTextBox1.Location = new Point(3, 0);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(374, 458);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(3, 454);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(374, 23);
            textBox1.TabIndex = 1;
            // 
            // button1
            // 
            button1.Location = new Point(302, 481);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "==>";
            button1.UseVisualStyleBackColor = true;
            button1.Click += btnMesajGonder_Click;
            // 
            // timer1
            // 
            timer1.Interval = 2000;
            timer1.Tick += timer1_Tick;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(192, 0, 0);
            button2.FlatStyle = FlatStyle.Flat;
            button2.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);
            button2.Location = new Point(291, 510);
            button2.Name = "button2";
            button2.Size = new Size(89, 37);
            button2.TabIndex = 3;
            button2.Text = "Ana menü";
            button2.UseVisualStyleBackColor = false;
            button2.Click += btnAnaMenu_Click;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.None;
            panel1.Controls.Add(richTextBox1);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(383, 550);
            panel1.TabIndex = 4;
            // 
            // Form4
            // 
            BackColor = Color.FromArgb(244, 247, 249);
            ClientSize = new Size(410, 572);
            Controls.Add(panel1);
            Name = "Form4";
            Load += Form4_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        private RichTextBox richTextBox1;
        private TextBox textBox1;
        private Button button1;
        private Timer timer1;
        private IContainer components;

        private void Form4_Load(object sender, EventArgs e)
        {
            IsmiBul(); // Önce numaranın kime ait olduğunu bulalım
            SohbetiGetir(); // Eski mesajları yükleyelim
            timer1.Start(); // Otomatik yenilemeyi başlatalım
        }
        private void IsmiBul()
        {
            // DatabaseLayer'dan tek bir satır bilgi çekiyoruz (İsim ve Öğretmen)
            DataRow satir = DatabaseLayer.OgrenciBilgisiGetir(ogrenciNo);

            if (satir != null)
            {
                ogrenciAdi = satir["OgrenciIsmi"].ToString();
                bagliOlduguOgretmen = satir["Ogretmen"].ToString();
            }
            else
            {
                ogrenciAdi = "Bilinmeyen";
                bagliOlduguOgretmen = "Yok";
            }
        }
        private void SohbetiGetir()
        {
            // Veritabanı kodları gitti, yerine bu geldi:
            DataTable tablo = DatabaseLayer.SohbetMesajlariniGetir(bagliOlduguOgretmen);

            if (tablo != null)
            {
                string tumMesajlar = "";

                // Tablodaki her bir satırı okuyup yazıya çeviriyoruz
                foreach (DataRow satir in tablo.Rows)
                {
                    string saat = satir["Saat"].ToString();
                    string isim = satir["GonderenIsmi"].ToString();
                    string mesaj = satir["Mesaj"].ToString();

                    tumMesajlar += $"[{saat}] {isim}: {mesaj}\n";
                }

                // Ekran güncelleme (Titreme olmasın diye kontrol ediyoruz)
                if (richTextBox1.Text != tumMesajlar)
                {
                    richTextBox1.Text = tumMesajlar;
                    richTextBox1.SelectionStart = richTextBox1.Text.Length;
                    richTextBox1.ScrollToCaret();
                }
            }
        }

        private void btnMesajGonder_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "") return;

            timer1.Stop(); // Yazarken çakışma olmasın diye durdur

            // DatabaseLayer'a mesajı gönderiyoruz
            bool sonuc = DatabaseLayer.SohbetMesajiEkle(ogrenciAdi, textBox1.Text, bagliOlduguOgretmen);

            if (sonuc)
            {
                textBox1.Text = ""; // Başarılıysa kutuyu temizle
                SohbetiGetir();     // Listeyi hemen güncelle
            }
            else
            {
                MessageBox.Show("Mesaj gönderilemedi (Veritabanı hatası).");
            }

            timer1.Start(); // Sayacı tekrar başlat
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.IsDisposed || this.Disposing)
            {
                timer1.Stop();
                return;
            }

            SohbetiGetir();
        }

        private void btnAnaMenu_Click(object sender, EventArgs e)
        {
            // Form3 bizden bir numara bekliyor. 
            Form3 ogrenciSayfasi = new Form3(ogrenciNo);

            // Sayfayı göster
            ogrenciSayfasi.Show();

            // İsterseniz şu anki sayfayı gizleyebilirsiniz
            // this.Hide();
        }
        // Pencere kapanırken çalışacak özel metod
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            timer1.Stop(); // Sayacı durdur
            timer1.Dispose(); // Sayacı hafızadan sil
        }

    }

}
