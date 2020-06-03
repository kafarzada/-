using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Hopfild
{
    public partial class Form1 : Form
    {
        int N = 4;
        protected int n = 400;  //размер входного вектора
        protected double[] X;   //входной вектор
        protected double[,] W;   //веса 
        public double[] Y;  //вектор выхода 
        double teta = 0;
        double epsylon = 0.05;
        protected Random rand = new Random();
        char[] numbers;
        double[] sum;
        public Form1()
        {
            InitializeComponent();
            X = new double[n];
            W = new double[n, n];
            Y = new double[n];
            numbers = new char[N];
            numbers[0] = '1';
            numbers[1] = '2';
            numbers[2] = '6';
            numbers[3] = '7';
            sum = new double[n];
            textBox2.Text = teta.ToString();
            textBox3.Text = epsylon.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Bitmap bmp2 = new Bitmap(pictureBox1.Image);
            Bitmap bmp3 = new Bitmap(20, 20);
            enterX(bmp2);
            WtatIsThis(X);
            int i = 0;
            for (int x = 0; x < bmp3.Height; x++)
                for (int y = 0; y < bmp3.Width; y++)
                {
                    if (Y[i] == 1)
                    bmp3.SetPixel(x, y, Color.Black);
                    i++;
                }
            pictureBox2.Image = bmp3;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\1.png")); break; }
                case 1: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\2.png")); break; }
                case 2: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\6.png")); break; }
                case 3: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\7.png")); break; }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\1.png")); break; }
                case 1: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\2.png")); break; }
                case 2: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\6.png")); break; }
                case 3: { pictureBox1.Image = new Bitmap(Image.FromFile(@"Pictures\7.png")); break; }
            }

            Bitmap bmp1 = new Bitmap(pictureBox1.Image);
            Bitmap bmp = new Bitmap(bmp1);

            double percent = 30;
            try
            {
                percent = Convert.ToDouble(textBox1.Text);
            }
            catch
            {
                MessageBox.Show("Неверно задан процент");
            }


            int i = 0;
            for (int x = 0; x < bmp.Height; x++)
                for (int y = 0; y < bmp.Width; y++)
                {
                    i = rand.Next(100);
                    if (i < percent)
                        bmp.SetPixel(x, y, Color.Black);
                }

            pictureBox1.Image = bmp;
        }
        private void enterX(Bitmap b)
        {
            Color black = Color.FromArgb(255, 0, 0, 0);
            int i = 0;
            for (int x = 0; x < b.Height; x++)
                for (int y = 0; y < b.Width; y++)
                {
                    if (b.GetPixel(x, y) == black) X[i] = 1; else X[i] = -1;
                    i++;
                }
        }   //формирование входного вектора Х

        public void Initial()
        {
            for (int i = 0; i < n; i++)                 //матрица W 
                for (int j = 0; j < n; j++)
                    W[i, j] = 0;
        }  //инициализация матрицы весов 

        public void WtatIsThis(double[] Xr)
        {
            double eps = 0;
            for (int i = 0; i < n; i++)
            {
                double sumLast = sum[i];
                sum[i] = 0;
                for (int j = 0; j < n; j++)
                    sum[i] = sum[i] + Xr[j] * W[j, i];
                if (sum[i] < teta) Y[i] = -1;
                else if (sum[i] > teta) Y[i] = 1;
                eps += Math.Abs(sumLast - sum[i]);
            }
            if (eps > 0) WtatIsThis(Y);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter srW = new StreamWriter("fileW.txt");
                for (int i = 0; i < n; i++)                 //матрица W 
                    for (int j = 0; j < n; j++)
                        srW.Write(W[i, j] + " ");
                srW.Close();
                MessageBox.Show("Успешно");
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить запись в файл");
            };
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader srW = new StreamReader("fileW.txt");
                String[] masW = srW.ReadToEnd().Split(' ');
                for (int i = 0; i < n; i++)                 //матрица W 
                    for (int j = 0; j < n; j++)
                        W[i, j] = Convert.ToDouble(masW[i * n + j]);

                srW.Close();
                MessageBox.Show("Успешно");
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить загрузку");
            };
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                teta = Convert.ToDouble(textBox2.Text);
                epsylon = Convert.ToDouble(textBox3.Text);
            }
            catch
            {
                MessageBox.Show("Неверное значениe параметрa");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Initial();

            char[] trSet = new char[N];  //массив имен образов обучающей выборки
            for (int x = 0; x < N; x++)
                trSet[x] = numbers[x];

            for (int t = 0; t < N; t++)
            {
                Bitmap bmp = new Bitmap(Image.FromFile(@"Pictures\" + trSet[t] + ".png"));  //образ из папки
                enterX(bmp);

                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                    {
                        if (i != j)
                            W[i, j] += X[i] * X[j];
                    }
            }
            MessageBox.Show("Готово");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox2.Image);
            enterX(bmp);

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    W[i, j] -= epsylon * X[i] * X[j];
                }
        }
    }
}
