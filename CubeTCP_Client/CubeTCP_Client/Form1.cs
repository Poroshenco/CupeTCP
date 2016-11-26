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
        public const int CELL = 40;
        public int Width, Height;

        private int[,] arr;

        public List<Point> cubes;

        private IPAddress ipAddr;
        private int port = 11000;
        private IPEndPoint ipEnd;

        private Socket sender;

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
                        graph.FillRectangle(new SolidBrush(Color.Yellow), i * CELL, j * CELL, CELL, CELL);
                }
            }

            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int X = Convert.ToInt32(e.X / CELL);
            int Y = Convert.ToInt32(e.Y / CELL);

            if (e.Button == MouseButtons.Left)
            {
                arr[X, Y] = 1;
                this.sender.Send(Encoding.UTF8.GetBytes(X + " " + Y));
            }

            if (e.Button == MouseButtons.Right)
            {
                arr[X, Y] = 0;
                this.sender.Send(Encoding.UTF8.GetBytes(X + " " + Y));
            }
            Draw();
        }
    }
}
