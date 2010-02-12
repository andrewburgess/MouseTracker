namespace MouseTracker
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.updateTimer = new System.Windows.Forms.Timer(this.components);
			this.drawTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// updateTimer
			// 
			this.updateTimer.Interval = 50;
			this.updateTimer.Tick += new System.EventHandler(this.updateTimer_Tick);
			// 
			// drawTimer
			// 
			this.drawTimer.Interval = 1000;
			this.drawTimer.Tick += new System.EventHandler(this.drawTimer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(491, 276);
			this.Name = "MainForm";
			this.Text = "Mouse Tracker";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.MainForm_Paint);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer updateTimer;
		private System.Windows.Forms.Timer drawTimer;
	}
}

