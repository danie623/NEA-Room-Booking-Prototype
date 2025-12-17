namespace NEA_Room_Booking_Prototype
{
	partial class TransferBookingScreen
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
			this.TeacherList = new System.Windows.Forms.ComboBox();
			this.TransferMsg = new System.Windows.Forms.Label();
			this.Confirm = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// TeacherList
			// 
			this.TeacherList.FormattingEnabled = true;
			this.TeacherList.Location = new System.Drawing.Point(173, 146);
			this.TeacherList.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.TeacherList.Name = "TeacherList";
			this.TeacherList.Size = new System.Drawing.Size(159, 21);
			this.TeacherList.TabIndex = 0;
			this.TeacherList.SelectedIndexChanged += new System.EventHandler(this.TeacherList_SelectedIndexChanged);
			// 
			// TransferMsg
			// 
			this.TransferMsg.AutoSize = true;
			this.TransferMsg.Location = new System.Drawing.Point(238, 55);
			this.TransferMsg.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.TransferMsg.Name = "TransferMsg";
			this.TransferMsg.Size = new System.Drawing.Size(35, 13);
			this.TransferMsg.TabIndex = 1;
			this.TransferMsg.Text = "label1";
			// 
			// Confirm
			// 
			this.Confirm.Enabled = false;
			this.Confirm.Location = new System.Drawing.Point(320, 213);
			this.Confirm.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Confirm.Name = "Confirm";
			this.Confirm.Size = new System.Drawing.Size(153, 49);
			this.Confirm.TabIndex = 2;
			this.Confirm.Text = "Confirm Transfer";
			this.Confirm.UseVisualStyleBackColor = true;
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(58, 213);
			this.Cancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(153, 49);
			this.Cancel.TabIndex = 3;
			this.Cancel.Text = "Cancel Transfer";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// TransferBookingScreen
			// 
			this.AcceptButton = this.Confirm;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(533, 292);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Confirm);
			this.Controls.Add(this.TransferMsg);
			this.Controls.Add(this.TeacherList);
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.Name = "TransferBookingScreen";
			this.Text = "Form4";
			this.Load += new System.EventHandler(this.TransferBookingScreen_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox TeacherList;
		private System.Windows.Forms.Label TransferMsg;
		private System.Windows.Forms.Button Confirm;
		private System.Windows.Forms.Button Cancel;
	}
}