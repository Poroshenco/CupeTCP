using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels;

namespace CubeTCP_Client
{
    public partial class Form1 : Form
    {
        const int CELL = 40;
        int Width, Height;

        int color = 4;

        int[,] arr;

        List<Point> cubes;

        IPAddress ipAddr;
        int port = 11000;
        IPEndPoint ipEnd;

        Socket sender;

        public Form1()
        {
            InitializeComponent();

            ipAddr = IPAddress.Parse("127.0.0.1");

            ipEnd = new IPEndPoint(ipAddr, port);
            sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                sender.Connect(ipEnd);
                MessageBox.Show("Подключился");
            }
            catch (Exception)
            {

            }


            Width = pictureBox1.Width;
            Height = pictureBox1.Height;

            arr = new int[Convert.ToInt32(Width / CELL), Convert.ToInt32(Height / CELL)];

            Draw();
        }

        public void Draw()
        {
            Bitmap bmp = new Bitmap(Width, Height);

            Graphics graph = Graphics.FromImage(bmp);

            for (int i = 0; i < Width / CELL; i++)
            {
                for (int j = 0; j < Height / CELL; j++)
                {
                    if (arr[i, j] == 1)
                        graph.FillRectangle(new SolidBrush(Color.Red), i * CELL, j * CELL, CELL, CELL);
                    if (arr[i, j] == 2)
                        graph.FillRectangle(new SolidBrush(Color.Yellow), i * CELL, j * CELL, CELL, CELL);
                    if (arr[i, j] == 3)
                        graph.FillRectangle(new SolidBrush(Color.Black), i * CELL, j * CELL, CELL, CELL);
                    if (arr[i, j] == 4)
                        graph.FillRectangle(new SolidBrush(Color.Purple), i * CELL, j * CELL, CELL, CELL);
                }
            }

            for (int i = 0; i < Width / CELL + 1; i++)
            {
                graph.DrawLine(new Pen(Color.Black), i * CELL, 0, i * CELL, Height);
            }
            for (int i = 0; i < Height / CELL + 1; i++)
            {
                graph.DrawLine(new Pen(Color.Black), 0, i * CELL, Width, i * CELL);
            }

            pictureBox1.Image = bmp;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            char temp = e.KeyData.ToString().ToLower()[0];
            if ("rpby".Contains(temp))
            {
                if (temp == 'r')
                    color = 1;
                if (temp == 'y')
                    color = 2;
                if (temp == 'b')
                    color = 3;
                if (temp == 'p')
                    color = 4;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int X = Convert.ToInt32(e.X / CELL);
            int Y = Convert.ToInt32(e.Y / CELL);

            if (e.Button == MouseButtons.Left)
            {
                arr[X, Y] = color;
                Send(X, Y, color);

            }

            if (e.Button == MouseButtons.Right)
            {
                arr[X, Y] = 0;
                Send(X, Y, 0);
            }
            Draw();
        }

        public void Send(int X, int Y, int COLOR)
        {
            try
            {
                sender.Send(Encoding.UTF8.GetBytes(X + " " + Y + " " + COLOR));
            }
            catch (Exception) { }
        }
    }
}
