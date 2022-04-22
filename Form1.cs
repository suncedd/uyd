using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace uyd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            for (int j = 0; j < bl.Length; j++)
            {
                //создаем потоковые объекты
                bl[j] = new Ball(j, j * 10, 10, 10, j + 1, j + 1, Pens.Magenta, true);
                //подписываемся на событие
                bl[j].dl += new Ball.DlTp(Invalidate);
            }
        }
        public class Ball
        {
            public Pen color;
            public bool vis;
            int x, y; // координаты
            int dx, dy; //приращение координат-определяет скорость
            int w, h; //ширина высота шарика
            public bool live; // признак жизни
            public delegate void DlTp();// Объявление типа (делегат) и
                                        //создание пока что пустой ссылки для организации в последующем
                                        // с помощью ее вызова функции Invalidate()для главного потока
            public DlTp dl;
            public Thread thr; //Создание ссылки на потоковый объект
                               // потоковая функция
            void FnThr()
            {
                while (live)
                { //здесь отражемся от границ области
                    if (x < 0 || x > 200) dx = -dx;
                    if (y < 0 || y > 200) dy = -dy;
                    //здесь пересчитываем координаты
                    x += dx;
                    y += dy;
                    Thread.Sleep(100);//спим
                    dl(); //вызываем с помощью делегата Invalidate()
                }
                w = h = 0; //схлопываем шарик
                dl(); //вызываем с помощью делегата Invalidate()
            }//функция рисования шарика
            public void DrawBall(Graphics dc)
            {
                    dc.DrawEllipse(color, x, y, w, h);
                    
                    
                
            }
            //конструктор класса
            public Ball(int xn, int yn, int wn, int hn, int dxn, int dyn, Pen colorPen, bool v)
            {
                vis = v;
                color = colorPen;
                x = xn; y = yn; w = wn; h = hn; dx = dxn; dy = dyn;//инициализируем
                thr = new Thread(new ThreadStart(FnThr)); //создаем потоковый объект
                live = true; //устанавливаем признак жизни
                thr.Start(); //запускаем поток
            }
        }
        Ball[] bl = new Ball[10];//массив пустых ссылок типа Ball
        
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int j = 0; j < bl.Length; j++)
            {
                if(bl[j].vis)
                bl[j].DrawBall(e.Graphics);//рисуем
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < bl.Length; j++)
            {
                bl[j].live = false;// Уничтожаем потоки
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int j = 0; j < bl.Length; j++)
            {
                bl[j].live = false;//уничтожаем потоки
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox1.Text);
            bl[i].thr.Suspend();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox1.Text);
            bl[i].thr.Resume();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox2.Text);
            bl[i].color = Pens.White;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox2.Text);
            bl[i].color = Pens.Magenta;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox3.Text);
            bl[i].vis = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(textBox3.Text);
            bl[i].vis = true;
        }
    }

}
