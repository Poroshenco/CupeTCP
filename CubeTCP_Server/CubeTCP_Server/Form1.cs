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

namespace CubeTCP_Server
{
    public partial class Form1 : Form
    {
        private int[,] arr;

        const int CELL = 40;

        public Form1()
        {
            InitializeComponent();

            IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
            int host = 11000;
            IPEndPoint ipEnd = new IPEndPoint(ipAddr, host);

            Width = pictureBox1.Width;
            Height = pictureBox1.Height;

            arr = new int[Convert.ToInt32(Width / CELL), Convert.ToInt32(Height / CELL)];

            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            sListener.Bind(ipEnd);
            sListener.Listen(10);

            Task.Factory.StartNew((() =>
            {
                while (true)
                {
                    Socket accept = sListener.Accept();

                    Task.Factory.StartNew((() =>
                    {
                        try
                        {
                            while (true)
                            {
                                Socket reciever = accept;

                                byte[] bytes = new byte[256];
                                int recieve = reciever.Receive(bytes);

                                string str = Encoding.UTF8.GetString(bytes, 0, recieve);

                                int k1 = str.IndexOf(" ");
                                int k2 = str.IndexOf(" ", k1 + 1);

                                int X = int.Parse(str.Substring(0, k1));
                                int Y = int.Parse(str.Substring(k1, k2 - k1));
                                int color = int.Parse(str.Substring(k2));


                                arr[X, Y] = color;
                                Draw();
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }));
                }
            }));
        }

        public void Draw()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            Graphics graph = Graphics.FromImage(bmp);

            for (int i = 0; i < Width / CELL; i++)
            {
                for (int j = 0; j < Height / CELL; j++)
                {
                    if (arr[i, j] == 1)
                        graph.FillRectangle(new SolidBrush(Color.Red), i * CELL, j * CELL, CELL, CELL);
                }
            }

            Action action = () =>
            {
                pictureBox1.Image = bmp;
            };

            if (pictureBox1.InvokeRequired)
                pictureBox1.Invoke(action);
            else
            {
                action();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
