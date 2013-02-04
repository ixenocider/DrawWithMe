﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace DrawWithMe
{
    public partial class FormDrawWithMe : Form
    {
        public Bitmap image;
        Color color1, color2;
        int color;

        public Point OldPoint, NewPoint;

        public FormDrawWithMe(string file)
        {
            InitializeComponent();
            if (file != "")
                LoadImage(file);

            color = 0;
            color1 = Color.Black;
            color2 = Color.White;

            image = new Bitmap(Canvas.Width, Canvas.Height);
            Clear(Color.White);
            Canvas.BackgroundImage = image;
            this.DoubleBuffered = true;
        }

        #region Events

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear(Color.White);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            OldPoint = NewPoint;
            NewPoint = new Point(e.X, e.Y);
            SetStatus(NewPoint.X + ", " + NewPoint.Y);

            if (e.Button == MouseButtons.Left)
                if (NewPoint.X >= 0 && NewPoint.X < image.Width && NewPoint.Y >= 0 && NewPoint.Y < image.Height)
                    DrawLine(OldPoint, NewPoint);
        }

        private void Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                Paste(LoadImage(@"C:\Users\Taylor\Pictures\RandomNiggahShit.PNG"), e.X, e.Y);
            else if (e.Button == MouseButtons.Left)
            {
                if (NewPoint.X >= 0 && NewPoint.X < image.Width && NewPoint.Y >= 0 && NewPoint.Y < image.Height)
                {
                    image = new Bitmap(image, Canvas.Size);
                    image.SetPixel(NewPoint.X, NewPoint.Y, color1);
                    Canvas.BackgroundImage = image;
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            new FormSave(image, "C:\\", null).ShowDialog();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            var bmp = LoadImage(@"C:\Users\Taylor\Pictures\RandomNiggahShit.PNG");
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            color1 = colorDialog.Color;
        }

        new private void Resize(object sender, EventArgs e)
        {
            new FormResize(this).ShowDialog();
        }
        #endregion

        public Bitmap LoadImage(string location)
        {
            var bmp = new Bitmap(location);
            return bmp;
        }

        public void SetStatus(string text)
        {
            Status.Text = text;
        }

        public void Clear(Color color)
        {
            for (int col = 0; col < image.Width; col++)
                for (int row = 0; row < image.Height; row++)
                    image.SetPixel(col, row, color);
        }

        public void Paste(Bitmap toPaste, int x, int y)
        {
            Bitmap bmp = new Bitmap(toPaste);
            image = new Bitmap(image, Canvas.Size);

            for (int col = 0; col < bmp.Width; col++)
                for (int row = 0; row < bmp.Height; row++)
                    if (col + x >= 0 && col + x < image.Width && row + y >= 0 && row + y < image.Height)
                        image.SetPixel(col + x, row + y, bmp.GetPixel(col, row));

            Canvas.BackgroundImage = image;
        }

        public void DrawLine(Point p1, Point p2)
        {
            image = new Bitmap(image, Canvas.Size);

            var g = Graphics.FromImage(image);
            g.DrawLine(new Pen(Color.Red), p1, p2);

            Canvas.BackgroundImage = image;
        }
    }
}
