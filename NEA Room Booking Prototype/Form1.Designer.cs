namespace NEA_Room_Booking_Prototype
{
	partial class BookingScreen
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
			this.TagsList = new System.Windows.Forms.CheckedListBox();
			this.RoomsList = new System.Windows.Forms.ListBox();
			this.PeriodSelect = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.LogInStatus = new System.Windows.Forms.Label();
			this.PasswordBox = new System.Windows.Forms.TextBox();
			this.InitialsBox = new System.Windows.Forms.TextBox();
			this.Login = new System.Windows.Forms.Button();
			this.Period = new System.Windows.Forms.Label();
			this.InitialsLabel = new System.Windows.Forms.Label();
			this.PasswordLabel = new System.Windows.Forms.Label();
			this.DateBox = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.GetRooms = new System.Windows.Forms.Button();
			this.Book_Room_Button = new System.Windows.Forms.Button();
			this.ShowAlreadyBookedRooms = new System.Windows.Forms.CheckBox();
			this.ViewBookings = new System.Windows.Forms.Button();
			this.teacherBookingFor = new System.Windows.Forms.ComboBox();
			this.BookingForLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// TagsList
			// 
			this.TagsList.FormattingEnabled = true;
			this.TagsList.Location = new System.Drawing.Point(20, 29);
			this.TagsList.Margin = new System.Windows.Forms.Padding(2);
			this.TagsList.Name = "TagsList";
			this.TagsList.Size = new System.Drawing.Size(153, 119);
			this.TagsList.TabIndex = 0;
			this.TagsList.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
			// 
			// RoomsList
			// 
			this.RoomsList.FormattingEnabled = true;
			this.RoomsList.ItemHeight = 20;
			this.RoomsList.Location = new System.Drawing.Point(193, 82);
			this.RoomsList.Margin = new System.Windows.Forms.Padding(2);
			this.RoomsList.Name = "RoomsList";
			this.RoomsList.Size = new System.Drawing.Size(501, 524);
			this.RoomsList.TabIndex = 1;
			this.RoomsList.SelectedIndexChanged += new System.EventHandler(this.RoomsList_SelectedIndexChanged);
			// 
			// PeriodSelect
			// 
			this.PeriodSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PeriodSelect.FormattingEnabled = true;
			this.PeriodSelect.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
			this.PeriodSelect.Location = new System.Drawing.Point(273, 46);
			this.PeriodSelect.Margin = new System.Windows.Forms.Padding(2);
			this.PeriodSelect.Name = "PeriodSelect";
			this.PeriodSelect.Size = new System.Drawing.Size(35, 45);
			this.PeriodSelect.Sorted = true;
			this.PeriodSelect.TabIndex = 2;
			this.PeriodSelect.SelectedIndexChanged += new System.EventHandler(this.PeriodSelect_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 14);
			this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(166, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "Filter By: (Equiptment)";
			// 
			// LogInStatus
			// 
			this.LogInStatus.Location = new System.Drawing.Point(1135, 116);
			this.LogInStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LogInStatus.Name = "LogInStatus";
			this.LogInStatus.Size = new System.Drawing.Size(113, 72);
			this.LogInStatus.TabIndex = 17;
			this.LogInStatus.Text = "You are not logged in.\r\n\r\nYou need to log in to book a room.";
			// 
			// PasswordBox
			// 
			this.PasswordBox.Location = new System.Drawing.Point(1131, 94);
			this.PasswordBox.Margin = new System.Windows.Forms.Padding(2);
			this.PasswordBox.Name = "PasswordBox";
			this.PasswordBox.PasswordChar = '*';
			this.PasswordBox.Size = new System.Drawing.Size(119, 26);
			this.PasswordBox.TabIndex = 16;
			this.PasswordBox.UseSystemPasswordChar = true;
			this.PasswordBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasswordBox_KeyDown);
			// 
			// InitialsBox
			// 
			this.InitialsBox.Location = new System.Drawing.Point(1131, 59);
			this.InitialsBox.Margin = new System.Windows.Forms.Padding(2);
			this.InitialsBox.MaxLength = 3;
			this.InitialsBox.Name = "InitialsBox";
			this.InitialsBox.Size = new System.Drawing.Size(119, 26);
			this.InitialsBox.TabIndex = 15;
			this.InitialsBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InitialsBox_KeyDown);
			// 
			// Login
			// 
			this.Login.Location = new System.Drawing.Point(1131, 14);
			this.Login.Margin = new System.Windows.Forms.Padding(2);
			this.Login.Name = "Login";
			this.Login.Size = new System.Drawing.Size(118, 31);
			this.Login.TabIndex = 14;
			this.Login.Text = "Log In";
			this.Login.UseVisualStyleBackColor = true;
			this.Login.Click += new System.EventHandler(this.Login_Click);
			// 
			// Period
			// 
			this.Period.AutoSize = true;
			this.Period.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Period.Location = new System.Drawing.Point(190, 51);
			this.Period.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.Period.Name = "Period";
			this.Period.Size = new System.Drawing.Size(118, 37);
			this.Period.TabIndex = 18;
			this.Period.Text = "Period:";
			// 
			// InitialsLabel
			// 
			this.InitialsLabel.AutoSize = true;
			this.InitialsLabel.Location = new System.Drawing.Point(1135, 46);
			this.InitialsLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.InitialsLabel.Name = "InitialsLabel";
			this.InitialsLabel.Size = new System.Drawing.Size(58, 20);
			this.InitialsLabel.TabIndex = 19;
			this.InitialsLabel.Text = "Initials:";
			// 
			// PasswordLabel
			// 
			this.PasswordLabel.AutoSize = true;
			this.PasswordLabel.Location = new System.Drawing.Point(1135, 79);
			this.PasswordLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.PasswordLabel.Name = "PasswordLabel";
			this.PasswordLabel.Size = new System.Drawing.Size(82, 20);
			this.PasswordLabel.TabIndex = 20;
			this.PasswordLabel.Text = "Password:";
			// 
			// DateBox
			// 
			this.DateBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.DateBox.FormattingEnabled = true;
			this.DateBox.Location = new System.Drawing.Point(465, 48);
			this.DateBox.Margin = new System.Windows.Forms.Padding(2);
			this.DateBox.Name = "DateBox";
			this.DateBox.Size = new System.Drawing.Size(229, 37);
			this.DateBox.TabIndex = 21;
			this.DateBox.SelectedIndexChanged += new System.EventHandler(this.DateBox_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(383, 48);
			this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 37);
			this.label2.TabIndex = 22;
			this.label2.Text = "Date:";
			// 
			// GetRooms
			// 
			this.GetRooms.Location = new System.Drawing.Point(19, 216);
			this.GetRooms.Margin = new System.Windows.Forms.Padding(2);
			this.GetRooms.Name = "GetRooms";
			this.GetRooms.Size = new System.Drawing.Size(158, 36);
			this.GetRooms.TabIndex = 23;
			this.GetRooms.Text = "GetRooms";
			this.GetRooms.UseVisualStyleBackColor = true;
			this.GetRooms.Click += new System.EventHandler(this.GetRooms_Click_1);
			// 
			// Book_Room_Button
			// 
			this.Book_Room_Button.Enabled = false;
			this.Book_Room_Button.Location = new System.Drawing.Point(719, 51);
			this.Book_Room_Button.Margin = new System.Windows.Forms.Padding(2);
			this.Book_Room_Button.Name = "Book_Room_Button";
			this.Book_Room_Button.Size = new System.Drawing.Size(222, 66);
			this.Book_Room_Button.TabIndex = 24;
			this.Book_Room_Button.Text = "Book Room";
			this.Book_Room_Button.UseVisualStyleBackColor = true;
			this.Book_Room_Button.Visible = false;
			this.Book_Room_Button.Click += new System.EventHandler(this.Book_Room_Button_Click);
			// 
			// ShowAlreadyBookedRooms
			// 
			this.ShowAlreadyBookedRooms.AutoSize = true;
			this.ShowAlreadyBookedRooms.Location = new System.Drawing.Point(20, 257);
			this.ShowAlreadyBookedRooms.Name = "ShowAlreadyBookedRooms";
			this.ShowAlreadyBookedRooms.Size = new System.Drawing.Size(189, 24);
			this.ShowAlreadyBookedRooms.TabIndex = 25;
			this.ShowAlreadyBookedRooms.Text = "Show Booked Rooms";
			this.ShowAlreadyBookedRooms.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.ShowAlreadyBookedRooms.UseVisualStyleBackColor = true;
			// 
			// ViewBookings
			// 
			this.ViewBookings.Enabled = false;
			this.ViewBookings.Location = new System.Drawing.Point(1059, 591);
			this.ViewBookings.Name = "ViewBookings";
			this.ViewBookings.Size = new System.Drawing.Size(197, 63);
			this.ViewBookings.TabIndex = 26;
			this.ViewBookings.Text = "My Bookings";
			this.ViewBookings.UseVisualStyleBackColor = true;
			this.ViewBookings.Visible = false;
			this.ViewBookings.Click += new System.EventHandler(this.ViewBookings_Click);
			// 
			// teacherBookingFor
			// 
			this.teacherBookingFor.Enabled = false;
			this.teacherBookingFor.FormattingEnabled = true;
			this.teacherBookingFor.Location = new System.Drawing.Point(12, 343);
			this.teacherBookingFor.MaxDropDownItems = 10;
			this.teacherBookingFor.Name = "teacherBookingFor";
			this.teacherBookingFor.Size = new System.Drawing.Size(157, 28);
			this.teacherBookingFor.TabIndex = 27;
			this.teacherBookingFor.Visible = false;
			// 
			// BookingForLabel
			// 
			this.BookingForLabel.AutoSize = true;
			this.BookingForLabel.Enabled = false;
			this.BookingForLabel.Location = new System.Drawing.Point(17, 327);
			this.BookingForLabel.Name = "BookingForLabel";
			this.BookingForLabel.Size = new System.Drawing.Size(127, 20);
			this.BookingForLabel.TabIndex = 28;
			this.BookingForLabel.Text = "I am booking for:";
			this.BookingForLabel.Visible = false;
			// 
			// BookingScreen
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(1263, 649);
			this.Controls.Add(this.BookingForLabel);
			this.Controls.Add(this.teacherBookingFor);
			this.Controls.Add(this.ViewBookings);
			this.Controls.Add(this.ShowAlreadyBookedRooms);
			this.Controls.Add(this.Book_Room_Button);
			this.Controls.Add(this.GetRooms);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.DateBox);
			this.Controls.Add(this.PasswordLabel);
			this.Controls.Add(this.InitialsLabel);
			this.Controls.Add(this.Period);
			this.Controls.Add(this.LogInStatus);
			this.Controls.Add(this.PasswordBox);
			this.Controls.Add(this.InitialsBox);
			this.Controls.Add(this.Login);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.PeriodSelect);
			this.Controls.Add(this.RoomsList);
			this.Controls.Add(this.TagsList);
			this.Enabled = false;
			this.Margin = new System.Windows.Forms.Padding(2);
			this.MaximumSize = new System.Drawing.Size(1285, 705);
			this.MinimumSize = new System.Drawing.Size(1285, 705);
			this.Name = "BookingScreen";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.BookingScreen_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox TagsList;
		private System.Windows.Forms.ListBox RoomsList;
		private System.Windows.Forms.ComboBox PeriodSelect;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label LogInStatus;
		private System.Windows.Forms.TextBox PasswordBox;
		private System.Windows.Forms.TextBox InitialsBox;
		private System.Windows.Forms.Button Login;
		private System.Windows.Forms.Label Period;
		private System.Windows.Forms.Label InitialsLabel;
		private System.Windows.Forms.Label PasswordLabel;
		private System.Windows.Forms.ComboBox DateBox;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button GetRooms;
		private System.Windows.Forms.Button Book_Room_Button;
		private System.Windows.Forms.CheckBox ShowAlreadyBookedRooms;
		private System.Windows.Forms.Button ViewBookings;
        private System.Windows.Forms.ComboBox teacherBookingFor;
        private System.Windows.Forms.Label BookingForLabel;
    }
}

