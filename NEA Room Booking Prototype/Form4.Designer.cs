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
			this.TeacherList.Location = new System.Drawing.Point(259, 224);
			this.TeacherList.Name = "TeacherList";
			this.TeacherList.Size = new System.Drawing.Size(236, 28);
			this.TeacherList.TabIndex = 0;
			// 
			// TransferMsg
			// 
			this.TransferMsg.AutoSize = true;
			this.TransferMsg.Location = new System.Drawing.Point(357, 84);
			this.TransferMsg.Name = "TransferMsg";
			this.TransferMsg.Size = new System.Drawing.Size(51, 20);
			this.TransferMsg.TabIndex = 1;
			this.TransferMsg.Text = "label1";
			// 
			// Confirm
			// 
			this.Confirm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Confirm.Enabled = false;
			this.Confirm.Location = new System.Drawing.Point(480, 327);
			this.Confirm.Name = "Confirm";
			this.Confirm.Size = new System.Drawing.Size(229, 75);
			this.Confirm.TabIndex = 2;
			this.Confirm.Text = "Confirm Transfer";
			this.Confirm.UseVisualStyleBackColor = true;
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(87, 327);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(229, 75);
			this.Cancel.TabIndex = 3;
			this.Cancel.Text = "Cancel Transfer";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// TransferBookingScreen
			// 
			this.AcceptButton = this.Confirm;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Confirm);
			this.Controls.Add(this.TransferMsg);
			this.Controls.Add(this.TeacherList);
			this.Name = "TransferBookingScreen";
			this.Text = "Form4";
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