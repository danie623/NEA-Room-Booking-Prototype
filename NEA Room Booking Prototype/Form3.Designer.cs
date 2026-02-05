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
			this.Back = new System.Windows.Forms.Button();
			this.Cancel_Booking = new System.Windows.Forms.Button();
			this.Transfer_Booking = new System.Windows.Forms.Button();
			this.Bookings_For_others_check = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// BookingsList
			// 
			this.BookingsList.FormattingEnabled = true;
			this.BookingsList.Location = new System.Drawing.Point(17, 49);
			this.BookingsList.Name = "BookingsList";
			this.BookingsList.Size = new System.Drawing.Size(347, 381);
			this.BookingsList.TabIndex = 0;
			this.BookingsList.SelectedIndexChanged += new System.EventHandler(this.BookingsList_SelectedIndexChanged);
			// 
			// Back
			// 
			this.Back.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Back.Location = new System.Drawing.Point(645, 396);
			this.Back.Name = "Back";
			this.Back.Size = new System.Drawing.Size(154, 55);
			this.Back.TabIndex = 1;
			this.Back.Text = "Back to bookings screen";
			this.Back.UseVisualStyleBackColor = true;
			this.Back.Click += new System.EventHandler(this.Back_Click);
			// 
			// Cancel_Booking
			// 
			this.Cancel_Booking.Location = new System.Drawing.Point(545, 49);
			this.Cancel_Booking.Name = "Cancel_Booking";
			this.Cancel_Booking.Size = new System.Drawing.Size(196, 56);
			this.Cancel_Booking.TabIndex = 2;
			this.Cancel_Booking.Text = "Cancel Booking";
			this.Cancel_Booking.UseVisualStyleBackColor = true;
			this.Cancel_Booking.Click += new System.EventHandler(this.Cancel_Booking_Click);
			// 
			// Transfer_Booking
			// 
			this.Transfer_Booking.Location = new System.Drawing.Point(545, 123);
			this.Transfer_Booking.Name = "Transfer_Booking";
			this.Transfer_Booking.Size = new System.Drawing.Size(196, 56);
			this.Transfer_Booking.TabIndex = 3;
			this.Transfer_Booking.Text = "Transfer Booking";
			this.Transfer_Booking.UseVisualStyleBackColor = true;
			this.Transfer_Booking.Click += new System.EventHandler(this.Transfer_Booking_Click);
			// 
			// Bookings_For_others_check
			// 
			this.Bookings_For_others_check.AutoSize = true;
			this.Bookings_For_others_check.Location = new System.Drawing.Point(17, 26);
			this.Bookings_For_others_check.Name = "Bookings_For_others_check";
			this.Bookings_For_others_check.Size = new System.Drawing.Size(210, 17);
			this.Bookings_For_others_check.TabIndex = 5;
			this.Bookings_For_others_check.Text = "Show Bookings you\'ve made for others";
			this.Bookings_For_others_check.UseVisualStyleBackColor = true;
			this.Bookings_For_others_check.CheckedChanged += new System.EventHandler(this.Bookings_For_others_check_CheckedChanged);
			// 
			// View_Bookings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Back;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.ControlBox = false;
			this.Controls.Add(this.Bookings_For_others_check);
			this.Controls.Add(this.Transfer_Booking);
			this.Controls.Add(this.Cancel_Booking);
			this.Controls.Add(this.Back);
			this.Controls.Add(this.BookingsList);
			this.Name = "View_Bookings";
			this.Text = "My Bookings";
			this.Load += new System.EventHandler(this.View_Bookings_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox BookingsList;
        private System.Windows.Forms.Button Back;
        private System.Windows.Forms.Button Cancel_Booking;
        private System.Windows.Forms.Button Transfer_Booking;
        private System.Windows.Forms.CheckBox Bookings_For_others_check;
    }
}