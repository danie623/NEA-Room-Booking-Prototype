namespace NEA_Room_Booking_Prototype
{
	partial class View_Bookings
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
			this.BookingsList = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// BookingsList
			// 
			this.BookingsList.FormattingEnabled = true;
			this.BookingsList.ItemHeight = 20;
			this.BookingsList.Location = new System.Drawing.Point(26, 75);
			this.BookingsList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.BookingsList.Name = "BookingsList";
			this.BookingsList.Size = new System.Drawing.Size(518, 584);
			this.BookingsList.TabIndex = 0;
			// 
			// View_Bookings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1200, 692);
			this.Controls.Add(this.BookingsList);
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "View_Bookings";
			this.Text = "Form3";
			this.Load += new System.EventHandler(this.View_Bookings_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox BookingsList;
	}
}