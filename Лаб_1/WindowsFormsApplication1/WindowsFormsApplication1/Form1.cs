using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        static public double alpha = 0.5;
        static public double treshold = 0;
        private Point start;
        private bool drawing = false;
        private Bitmap bm = new Bitmap(100, 100);
        private Bitmap bm2 = new Bitmap(100, 100);
        private Neuron[] n = new Neuron[10];




        public Form1()
        {
            InitializeComponent();
            n[0] = new Neuron('0');
            n[1] = new Neuron('1');
            n[2] = new Neuron('2');
            n[3] = new Neuron('3');
            n[4] = new Neuron('4');
            n[5] = new Neuron('5');
            n[6] = new Neuron('6');
            n[7] = new Neuron('7');
            n[8] = new Neuron('8');
            n[9] = new Neuron('9');


        }

        private void Form1_Load(object sender, EventArgs e)
        {

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

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            start = new Point(e.X, e.Y);
            Bitmap orig = bm;
            drawing = true;
        }

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

        private void button6_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                n[i].save();
            MessageBox.Show("Успешно!");

        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                n[i].load();
            MessageBox.Show("Успешно!");

        }

        private void button5_Click(object sender, EventArgs e)
        {
           
            Bitmap bmp = new Bitmap(pictureBox1.Image);

            int[] Y = new int[10];
            Y[0] = n[0].analyz(bmp); label5.Text = "0: " + Convert.ToString(Y[0]);
            Y[1] = n[1].analyz(bmp); label6.Text = "1: " + Convert.ToString(Y[1]);
            Y[2] = n[2].analyz(bmp); label7.Text = "2: " + Convert.ToString(Y[2]);
            Y[3] = n[3].analyz(bmp); label8.Text = "3: " + Convert.ToString(Y[3]);
            Y[4] = n[4].analyz(bmp); label9.Text = "4: " + Convert.ToString(Y[4]);
            Y[5] = n[5].analyz(bmp); label10.Text = "5: " + Convert.ToString(Y[5]);
            Y[6] = n[6].analyz(bmp); label11.Text = "6: " + Convert.ToString(Y[6]);
            Y[7] = n[7].analyz(bmp); label12.Text = "7: " + Convert.ToString(Y[7]);
            Y[8] = n[8].analyz(bmp); label13.Text = "8: " + Convert.ToString(Y[8]);
            Y[9] = n[9].analyz(bmp); label14.Text = "9: " + Convert.ToString(Y[9]);
            

            int S = 0;
            for (int i = 0; i < 10; i++)
                S = S + Y[i];
            if (S == 1)
            {
                if (Y[0] == 1) textBox2.Text = "Это цифра 0";
                if (Y[1] == 1) textBox2.Text = "Это цифра 1";
                if (Y[2] == 1) textBox2.Text = "Это цифра 2";
                if (Y[3] == 1) textBox2.Text = "Это цифра 3";
                if (Y[4] == 1) textBox2.Text = "Это цифра 4";
                if (Y[5] == 1) textBox2.Text = "Это цифра 5";
                if (Y[6] == 1) textBox2.Text = "Это цифра 6";
                if (Y[7] == 1) textBox2.Text = "Это цифра 7";
                if (Y[8] == 1) textBox2.Text = "Это цифра 8";
                if (Y[9] == 1) textBox2.Text = "Это цифра 9";
            }
            else textBox2.Text = "Я не знаю, что это";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap bb = new Bitmap(100, 100);
            pictureBox1.Image = bb;
            bm = bb; bm2 = bb;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            int max;
            max = 0;
            for (int i = 0; i < 10; i++)
            {
                n[i].train_sample();
                if (n[i].epohs > max) max = n[i].epohs;
            }
            MessageBox.Show("Успешно!");
            textBox3.Text = "Количество эпох " + max;

        }

        private void button9_Click(object sender, EventArgs e)
        {
            int index = 0;
            if (radioButton1.Checked) index = 1;
            else if (radioButton2.Checked) index = 2;
            else if (radioButton3.Checked) index = 3;
            else if (radioButton4.Checked) index = 4;
            else if (radioButton5.Checked) index = 5;
            else if (radioButton6.Checked) index = 6;
            else if (radioButton7.Checked) index = 7;
            else if (radioButton8.Checked) index = 8;
            else if (radioButton9.Checked) index = 9;
            else if (radioButton10.Checked) index = 0;
            for (int i = 0; i < 10; i++)
            {
                if (n[index].Y == 0) n[index].hand_train();
                else if (n[i].Y == 1 && i != index)
                {
                    n[index].hand_train();
                    n[i].hand_train();
                }
            }
        }

      
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                treshold = Convert.ToDouble(textBox4.Text);
                alpha = Convert.ToDouble(textBox1.Text);
                if (alpha > 1)
                {
                    alpha = 0.5; MessageBox.Show("Недопустимое значение альфа");
                    textBox1.Text = " ";
                }
            }
            catch
            {
                MessageBox.Show("Недопустимое значение порога или альфа");
            }
        }

 
    }




    public class Neuron
    {
        public double[] W = new double[10000];
        public int Y;
        public int epohs = 0;
        public char number;

        private int[] X = new int[10000];
        private Random rand = new Random();
        private readonly int N = 10;
        private int sigma = 0;
        private bool truth = true;

        public Neuron(char number)
        {
            new_W();
            this.number = number;
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
        }

        private void new_W()
        {
            for (int k = 0; k < 10000; k++)
                W[k] = (rand.Next(7) - 3) / 10.0;
        }

        private void vhodX(Bitmap b)
        {
            Color black = Color.FromArgb(255, 0, 0, 0);
            int i = 0;
            for (int x = 0; x < b.Height; x++)
                for (int y = 0; y < b.Width; y++)
                {
                    if (b.GetPixel(x, y) == black) X[i] = 1; else X[i] = 0;
                    i++;
                 
                }
        }

        private double summ()
        {
            double S = 0;
            for (int k = 0; k < 10000; k++)
                if (X[k] != 0)
                    S = S + W[k] * X[k];
            return S;
        }

        private void obuch()
        {
            for (int k = 0; k < 10000; k++)
                if (X[k] != 0) 
               W[k] = W[k] + Form1.alpha * sigma * X[k];
        }

        public void save()
        {
            try
            {
                StreamWriter sr = new StreamWriter("file" + number + ".txt");
                for (int i = 0; i < 10000; i++)
                    sr.Write(W[i] + " ");
                sr.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить выгрузку в файл");
            }
        }

        public void load()
        {
            try
            {
                StreamReader sr = new StreamReader("file" + number + ".txt");
                String[] mas = sr.ReadToEnd().Split(' ');
                for (int i = 0; i < 10000; i++)
                    W[i] = Convert.ToDouble(mas[i]);
                sr.Close();
            }
            catch
            {
                MessageBox.Show("Не удалось выполнить загрузку");
            }
        }

        public void train_sample()
        {
            epohs = 0;
            StreamWriter wr = new StreamWriter("num.txt", true);
            wr.Write(number + " ");
            int h = 0;
            new_W();
            int[] numbers = new int[10];
            numbers[0] = 0;
            numbers[1] = 1;
            numbers[2] = 2;
            numbers[3] = 3;
            numbers[4] = 4;
            numbers[5] = 5;
            numbers[6] = 6;
            numbers[7] = 7;
            numbers[8] = 8;
            numbers[9] = 9;
            string[] trSet = new string[10 * N];
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < N; y++)
                    trSet[x * N + y] = numbers[x] + Convert.ToString(y);
          //  trSet = trSet.OrderBy(x => rand.Next()).ToArray();

            try
            {
                do
                {
                    epohs++;
                    h = 0;
                    truth = true;
                    trSet = trSet.OrderBy(x => rand.Next()).ToArray();
                    for (int t = 0; t < 10 * N; t++)
                    {
                        Bitmap bmp = new Bitmap(Image.FromFile(@"Pictures\" + trSet[t] + ".png"));
                        bmp = Cut(bmp);
                        int Yk;
                        if (number == trSet[t][0]) Yk = 1; else Yk = 0;
                        vhodX(bmp);
                        if (summ() >= Form1.treshold) Y = 1; else Y = 0;
                        sigma = Yk - Y;
                        if (sigma != 0)
                        {
                            truth = false;
                            obuch();
                            h++;
                        }
                    }
                    wr.Write(epohs + "-" + h + " ");
                  //  MessageBox.Show("Работай!!!!!!" + number+" " + epohs + " " + h);
                }
                while (!truth);
            }
            catch { }
            wr.WriteLine();
            wr.Close();
           // MessageBox.Show("Работай");
        }


        public int analyz(Bitmap bmp)
        {
            try
            {
                bmp = Cut(bmp);
                vhodX(bmp);
            }
            catch
            {
                MessageBox.Show("Отсутствует рисунок");
            }
              if (summ() >= Form1.treshold) Y = 1; else Y = 0;
            return Y;
        }

        public void hand_train()
        {
            if (Y == 1) sigma = -1; else sigma = 1;
            obuch();
        }
    }
}
    


