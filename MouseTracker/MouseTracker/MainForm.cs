using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MouseTracker
{
	public partial class MainForm : Form
	{
		private Bitmap bitmap;
		private Point previousPoint;
		private float counter;

		public MainForm()
		{
			InitializeComponent();
			previousPoint = new Point(-1, -1);
		}

		[DllImport("user32.dll")]
		private static extern bool GetCursorPos(ref Point lpPoint);

		private void updateTimer_Tick(object sender, System.EventArgs e)
		{
			var point = new Point();

			GetCursorPos(ref point);

			point = new Point(point.X / 2, point.Y / 2);

			Text = "Mouse Tracker: (" + point.X + ", " + point.Y + ")";

			if (previousPoint.X == -1)
			{
				previousPoint = point;
				return;
			}

			var graphics = Graphics.FromImage(bitmap);

			if (previousPoint == point)
			{
				counter += 0.01f;

				graphics.FillEllipse(new SolidBrush(Color.Black), point.X - counter / 2, point.Y - counter / 2, counter, counter);
			}
			else
			{
				counter = 0;
				var distance = (float)Math.Sqrt(Math.Pow(point.X - previousPoint.X, 2) + Math.Pow(point.Y - previousPoint.Y, 2));
				var size = 15 / distance;
				graphics.DrawLine(new Pen(Color.Black, size), previousPoint, point);
			}
			graphics.Dispose();

			previousPoint = point;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			var rect = new Rectangle();

			foreach (var screen in Screen.AllScreens)
			{
				rect.Width += screen.Bounds.Width;
				rect.Height = Math.Max(rect.Height, screen.Bounds.Height);
			}

			Size = new Size(rect.Width / 4, rect.Height / 4);

			bitmap = new Bitmap(rect.Width / 2, rect.Height / 2, PixelFormat.Format24bppRgb);
			var graphics = Graphics.FromImage(bitmap);
			graphics.Clear(Color.White);
			graphics.Dispose();

			updateTimer.Enabled = true;
			updateTimer.Start();

			drawTimer.Enabled = true;
			drawTimer.Start();
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

		private void drawTimer_Tick(object sender, System.EventArgs e)
		{
			Invalidate();
		}
	}
}
