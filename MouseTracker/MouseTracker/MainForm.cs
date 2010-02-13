using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Gma.UserActivityMonitor;

namespace MouseTracker
{
	public partial class MainForm : Form
	{
		private Bitmap bitmap;
		private Point previousPoint;
		private Point previousCharacterPoint;

		private int characterCounter;
		private int line;

		private float doubleClickTime;
		private DateTime lastClick;
		private DateTime lastMove;

		private Rectangle totalScreen;

		public MainForm()
		{
			InitializeComponent();
			previousPoint = new Point(-1, -1);
			previousCharacterPoint = new Point(-1, -1);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			lastClick = DateTime.Now;
			lastMove = DateTime.Now;
			var rect = new Rectangle();

			var screens = new List<Rectangle>();

			foreach (var screen in Screen.AllScreens)
			{
				rect.Width += screen.Bounds.Width;
				rect.Height = Math.Max(rect.Height, screen.Bounds.Height);
				screens.Add(screen.Bounds);
			}

			totalScreen = rect;

			Size = new Size(rect.Width / 4, rect.Height / 4);

			bitmap = new Bitmap(rect.Width, rect.Height, PixelFormat.Format24bppRgb);
			var graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.White);

			foreach (var screen in screens)
			{
				graphics.DrawRectangle(new Pen(Color.Black, 3), screen);
			}
			graphics.Dispose();

			drawTimer.Enabled = true;
			drawTimer.Start();

			HookManager.MouseMove += MouseMoved;
			HookManager.MouseDown += MouseClicked;
			HookManager.KeyPress += KeyPressed;

			doubleClickTime = HookManager.GetDoubleClickTime();
		}

		private void KeyPressed(object sender, KeyPressEventArgs e)
		{
			var graphics = Graphics.FromImage(bitmap);

			if (previousCharacterPoint == previousPoint)
				characterCounter++;
			else
			{
				characterCounter = 0;
				line = 0;
			}

			if (e.KeyChar == (char)Keys.Delete || e.KeyChar == (char)Keys.Back)
				characterCounter -= 2;

			if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.Enter)
			{
				line++;
				characterCounter = 0;
			}

			var p = new PointF(previousPoint.X + (10 * characterCounter), previousPoint.Y - 5 + (10 * line));

			if ((p.X + 10) > totalScreen.Width)
			{
				characterCounter = 0;
				line++;
			}

			graphics.DrawString(e.KeyChar.ToString(), new Font("Courier New", 12), new SolidBrush(Color.FromArgb(128, Color.Black)), p);

			previousCharacterPoint = previousPoint;
		}

		private void MouseClicked(object sender, MouseEventArgs e)
		{
			var graphics = Graphics.FromImage(bitmap);
			var milli = DateTime.Now.Subtract(lastClick).TotalMilliseconds;

			if (milli <= doubleClickTime)
			{
				graphics.DrawEllipse(new Pen(Color.FromArgb(128, Color.Black), 2), e.X - 8, e.Y - 8, 16, 16);
			}
			else
			{
				graphics.DrawEllipse(new Pen(Color.FromArgb(128, Color.Black), 2), e.X - 4, e.Y - 4, 8, 8);
			}

			lastClick = DateTime.Now;
		}

		private void MouseMoved(object sender, MouseEventArgs e)
		{
			var point = e.Location;

			if (previousPoint.X == -1)
			{
				previousPoint = point;
				return;
			}

			var graphics = Graphics.FromImage(bitmap);
			var difference = DateTime.Now.Subtract(lastMove);

			if (difference.TotalSeconds >= 1)
			{
				var size = (float) difference.TotalSeconds;
				graphics.FillEllipse(new SolidBrush(Color.FromArgb(128, Color.Black)), point.X - size / 2, point.Y - size / 2, size, size);
			}
			else
			{
				var distance = (float)Math.Sqrt(Math.Pow(point.X - previousPoint.X, 2) + Math.Pow(point.Y - previousPoint.Y, 2));
				var alpha = (int)Math.Max(180 - (distance * 4), 50);
				var size = e.Button == MouseButtons.Left ? 2 : 1;
				graphics.DrawLine(new Pen(Color.FromArgb(alpha, Color.Black), size), previousPoint, point);
			}
			graphics.Dispose();

			previousPoint = point;
			lastMove = DateTime.Now;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e)
		{
			var graphics = e.Graphics;

			graphics.DrawImage(bitmap, 0, 0, Width, Height);
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			bitmap.Save("image.jpg");
		}

		private void drawTimer_Tick(object sender, EventArgs e)
		{
			Invalidate();
		}
	}
}
