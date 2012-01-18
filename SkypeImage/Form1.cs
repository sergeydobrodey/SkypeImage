using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SkypeImage
{
    public partial class Form1 : Form
    {
        byte[] result = new byte[10000];
        byte[] source = new byte[10000];
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox2.Image = Image.FromFile(openFileDialog1.FileName);          
            ToGrayScale();
            button3.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
        }      
        private void ToGrayScale()
        {
            MemoryStream ms = new MemoryStream();
            Bitmap image = new Bitmap(pictureBox2.Image, pictureBox1.Width, pictureBox1.Height);
            image.RotateFlip(RotateFlipType.Rotate180FlipX);
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] temp = ms.GetBuffer();

            for (int p = 0; p < 4 * pictureBox1.Width * pictureBox1.Height; p++)
                source[p] = temp[p + 54];
            for (int i = 54; i < temp.Count(); i += 4)
            {
                temp[i] = Convert.ToByte((temp[i] + temp[i + 1] + temp[i + 2]) / 3);
                result[(i - 54) / 4] = temp[i];
                temp[i + 1] = temp[i];
                temp[i + 2] = temp[i];

            }
            ms = new MemoryStream(temp);
            pictureBox1.Image = new Bitmap(ms);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string text = "<font size=\"1\"><u>";
            int row = 1;
            string hole = "";
            string color = "";
            for (int j = 0; j < pictureBox1.Height; j++)
            {
                for (int i = 0; i < pictureBox1.Width; i++)
                    if ((Math.Abs(result[i] - result[i + 1 - row]) < 10) && (i != pictureBox1.Width - 1) && (Math.Abs(result[i + pictureBox1.Width * j] - result[i + 1 + pictureBox1.Width * j]) < 5)) row++;
                    else
                    {
                        for (int k = 1; k <= row; k++)
                            hole = hole + "███";
                        Color pointc = Color.FromArgb(255, source[4 * i + 4 * pictureBox1.Width * j], source[4 * i + 4 * pictureBox1.Width * j + 1], source[4 * i + 4 * pictureBox1.Width * j + 2]);
                        color = "\"" + System.Drawing.ColorTranslator.ToHtml(pointc) + "\">";
                        text = text + "<font color=" + color + hole + "</font>";
                        hole = "";
                        row = 1;
                    };
                text = text + "\n";
            }
            text = text + "</u></font>";
            label1.Text = text.Count().ToString();
            richTextBox1.Text = text;
            
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Height = 40;
            pictureBox1.Width = 40;            
            ToGrayScale();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Height = 35;
            pictureBox1.Width = 35;
            ToGrayScale();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Height = 30;
            pictureBox1.Width = 30;            
            ToGrayScale();
        }
    }
}
