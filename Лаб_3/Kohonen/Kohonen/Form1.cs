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

namespace Kohonen
{
    public partial class Form1 : Form
    {

        int N = 20;
        double D;
        int epoch = 0;
        protected int m = 10;  //количество нейронов 
        protected int n = 10000;  //размер входного вектора
        protected double[] X;   //входной вектор
        protected double[,] W;   //веса 
        public double[] Y;  //вектор выхода 
        static public double a = 0.95d;  //альфа
        double akoef = 0.994;
        double Dkoef = 0.749;
        protected Random rand = new Random();
        char[] numbers;
        Label[] Label;
       // TextBox[] ttt;
        private Point start;
        private bool drawing = false;
        private Bitmap bm = new Bitmap(100, 100);
        private Bitmap bm2 = new Bitmap(100, 100);


        public Form1()
        {
            InitializeComponent();
            X = new double[n];
            W = new double[n, m];
            Y = new double[m];
            numbers = new char[m];
            numbers[0] = '0';
            numbers[1] = '1';
            numbers[2] = '2';
            numbers[3] = '3';
            numbers[4] = '4';
            numbers[5] = '5';
            numbers[6] = '6';
            numbers[7] = '7';
            numbers[8] = '8';
            numbers[9] = '9';
            Label = new Label[10];
            Label[0] = label1;
            Label[1] = label2;
            Label[2] = label3;
            Label[3] = label4;
            Label[4] = label5;
            Label[5] = label6;
            Label[6] = label7;
            Label[7] = label8;
            Label[8] = label9;
            Label[9] = label10;
            textBox1.Text = a.ToString();
            textBox2.Text = akoef.ToString();
            textBox3.Text = Dkoef.ToString();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private Bitmap Cut(Bitmap b)
        {
            Color black = Color.FromArgb(255, 0, 0, 0);
            bool ok = false;
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            for (int x = 0; x < b.Height; x++)
            {
                for (int y = 0; y < b.Width; y++)
                {
                    if (b.GetPixel(x, y) == black)
                    {
                        x1 = x;
                        ok = true;
                        break;
                    }
                }
                if (ok) break;
            }
            ok = false;

            for (int x = x1 + 1; x < b.Height; x++)
            {
                ok = false;
                for (int y = 0; y < b.Width; y++)
                    if (b.GetPixel(x, y) == black) { ok = true; break; }
                if (!ok)
                {
                    x2 = x;
                    break;
                }
            }
            ok = false;

            for (int y = 0; y < b.Width; y++)
            {
                for (int x = 0; x < b.Height; x++)
                {
                    if (b.GetPixel(x, y) == black)
                    {
                        y1 = y;
                        ok = true;
                        break;
                    }
                }
                if (ok) break;
            }
            ok = false;

            for (int y = y1 + 1; y < b.Width; y++)
            {
                ok = false;
                for (int x = 0; x < b.Height; x++)
                    if (b.GetPixel(x, y) == black) { ok = true; break; }
                if (!ok)
                {
                    y2 = y;
                    break;
                }
            }

            Rectangle cropRect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            Bitmap src = b;
            Bitmap target = new Bitmap(100, 100);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(src, new Rectangle(0, 0, 100, 100), cropRect, GraphicsUnit.Pixel);
            }

            return target;
        }   //обрезка рисунка

        private void enterX(Bitmap b)
        {
            Color black = Color.FromArgb(255, 0, 0, 0);
            int i = 0;
            for (int x = 0; x < b.Height; x++)
                for (int y = 0; y < b.Width; y++)
                {
                    if (b.GetPixel(x, y) == black) X[i] = 1; else X[i] = 0;
                    i++;
                }
        }   //формирование входного вектора Х

        public void Randomization()
        {
            for (int i = 0; i < n; i++)                 //матрица W 
                for (int j = 0; j < m; j++)
                    W[i, j] = (rand.Next(7) - 3) / 10.0;
        }  //инициализация матрицы весов случайными числами


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!drawing) return;
            start = new Point(e.X, e.Y);
            var finish = new Point(e.X, e.Y);
            Bitmap bm2 = new Bitmap(bm);
            pictureBox1.Image = bm2;
            var g = Graphics.FromImage(bm);
            var pen = new Pen(Color.Black, 2f);
            g.DrawEllipse(pen, finish.X, finish.Y, 2, 2);
            g.Dispose();
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            Bitmap orig = bm;
            drawing = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var finish = new Point(e.X, e.Y);
            var g = Graphics.FromImage(bm);
            var pen = new Pen(Color.Black, 1f);
            g.DrawLine(pen, start, finish);
            g.Save();
            drawing = false;
            g.Dispose();
            pictureBox1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bb = new Bitmap(100, 100);
            pictureBox1.Image = bb;
            bm = bb; bm2 = bb;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                StreamWriter srW = new StreamWriter("fileW.txt");
                for (int i = 0; i < n; i++)                 //матрица W 
                    for (int j = 0; j < m; j++)
                        srW.Write(W[i, j] + " ");
                srW.Close();
                MessageBox.Show("Успешно");
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить запись в файл");
            };
        }

        public void Raspozn()
        {
            epoch = 0;
            double eps = 0;
            double eps1 = -1000;
            Randomization();

            double[] sum = new double[m * (m - 1)];
            int c = 0; D = -1000;

            for (int j = 0; j < m; j++)
            {
                for (int k = j + 1; k < m; k++)
                {
                    sum[c] = 0;
                    for (int i = 0; i < n; i++)
                    {
                        sum[c] += Math.Pow(W[i, j] - W[i, k], 2);
                    }
                    if (sum[c] > D) D = sum[c];
                    c++;
                }
            }

            string[] trSet = new string[m * N];  //массив имен образов обучающей выборки
            for (int x = 0; x < m; x++)
                for (int y = 0; y < N; y++)
                    trSet[(x * N) + y] = numbers[x] + Convert.ToString(y);

            do   //начало новой эпохи
            {
                trSet = trSet.OrderBy(x => rand.Next()).ToArray();  //перемешивание массива в случайном порядке

                epoch++;
                eps1 = -1000;
                eps = 0;
                Label[0].Text = "";
                Label[1].Text = "";
                Label[2].Text = "";
                Label[3].Text = "";
                Label[4].Text = "";
                Label[4].Text = "";
                Label[5].Text = "";
                Label[6].Text = "";
                Label[7].Text = "";
                Label[8].Text = "";
              


                for (int t = 0; t < m * N; t++)  //подача обучающей выборки
                {
                    Bitmap bmp = new Bitmap(Image.FromFile(@"Pictures\" + trSet[t] + ".png"));  //образ из папки
                    enterX(bmp);
                    bmp = Cut(bmp);

                    double[] d = new double[m];
                    int minj = 0;
                    double min = 100000;
                    for (int j = 0; j < m; j++)
                    {
                        d[j] = 0;
                        for (int i = 0; i < n; i++)
                        {
                            d[j] += Math.Pow((X[i] - W[i, j]), 2);
                        }
                        if (d[j] < min)
                        {
                            minj = j;
                            min = d[j];
                        }
                    }

                    Label[minj].Text = Label[minj].Text + trSet[t][0];

                    for (int j = 0; j < m; j++)
                    {
                        d[j] = 0;
                        if (j != minj) for (int i = 0; i < n; i++)
                            {
                                d[j] += Math.Pow((W[i, minj] - W[i, j]), 2);
                            }
                        if (d[j] <= D || j == minj)
                        {
                            for (int i = 0; i < n; i++)
                            {
                                W[i, j] += a * (X[i] - W[i, j]);
                                eps += Math.Abs(a * (X[i] - W[i, j]));
                                if (Math.Abs(a * (X[i] - W[i, j])) > eps1)
                                {

                                    eps1 = Math.Abs(a * (X[i] - W[i, j]));
                                }
                            }
                        }
                    }
                    a = a * akoef;
                    D = D * Dkoef;
                }


                /*if (epoch % 20 == 0) MessageBox.Show("" + eps + "  " + eps1);*/
            }
            while (eps > 0.01);

            int[] sum1 = new int[m]; int maxsum = -100; string max = "";
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    sum1[j] = 0;
                    for (int k = 0; k < Label[i].Text.Length; k++)
                    {
                        if (Label[i].Text[k] == numbers[j])
                            sum1[j]++;
                    }
                    if (sum1[j] > maxsum)
                    {
                        maxsum = sum1[j];
                        max = "" + numbers[j];
                    }
                    else if (sum1[j] == maxsum)
                        max = max + "" + numbers[j];
                }
                maxsum = -100;
                Label[i].Text = max;
                max = "";
            }
        }

      /* public double Bt2()
        {
            double h = 0;

            string[] trSet = new string[m * 6];  //массив имен образов
            for (int x = 0; x < m; x++)
                for (int y = 0; y < 6; y++)
                    trSet[(x * 6) + y] = numbers[x] + Convert.ToString(y);

            for (int t = 0; t < m * 6; t++)  //подача  выборки
            {
                Bitmap bmp = new Bitmap(Image.FromFile(@"test\" + trSet[t] + ".png"));  //образ из папки
                enterX(bmp);

                double[] d = new double[m];
                int maxj = 0;
                double max = -100000;
                for (int j = 0; j < m; j++)
                {
                    d[j] = 0;
                    for (int i = 0; i < n; i++)
                    {
                        d[j] += X[i] * W[i, j];
                    }
                    if (d[j] > max) { maxj = j; max = d[j]; }
                }
                if (Label[maxj].Text.IndexOf(trSet[t][0]) == -1) h++;
            }
           return h / (m * 6d);
        }*/

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                StreamReader srW = new StreamReader("fileW.txt");
                String[] masW = srW.ReadToEnd().Split(' ');
                for (int i = 0; i < n; i++)                 //матрица W 
                    for (int j = 0; j < m; j++)
                        W[i, j] = Convert.ToDouble(masW[i * m + j]);

                srW.Close();
                MessageBox.Show("Успешно");
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить загрузку");
            };
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                a = Convert.ToDouble(textBox1.Text);
                akoef = Convert.ToDouble(textBox2.Text);
                Dkoef = Convert.ToDouble(textBox3.Text);
            }
            catch
            {
                MessageBox.Show("Неверные значения параметров");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Raspozn();
            MessageBox.Show("готово " + epoch);
            label25.Text = "Количество эпох: "+epoch;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);  //образ из папки
            Cut(bmp);
            enterX(bmp);

            double[] d = new double[m];
            int maxj = 0;
            double max = -100000;
            for (int j = 0; j < m; j++)
            {
                d[j] = 0;
                for (int i = 0; i < n; i++)
                {
                    d[j] += X[i] * W[i, j];
                }
                if (d[j] > max) { maxj = j; max = d[j]; }
            }
            textBox4.Text = "Класс " + (maxj + 1 );
        }

        private void button7_Click(object sender, EventArgs e)
        {
         
        }
    }
}
