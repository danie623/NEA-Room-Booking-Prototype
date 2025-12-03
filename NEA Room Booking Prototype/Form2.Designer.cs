namespace NEA_Room_Booking_Prototype
{
	partial class Booking_Confirm
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
			this.Confirm = new System.Windows.Forms.Button();
			this.Cancel = new System.Windows.Forms.Button();
			this.Booking_msg = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// Confirm
			// 
			this.Confirm.Location = new System.Drawing.Point(112, 315);
			this.Confirm.Name = "Confirm";
			this.Confirm.Size = new System.Drawing.Size(185, 69);
			this.Confirm.TabIndex = 0;
			this.Confirm.Text = "Confirm Booking";
			this.Confirm.UseVisualStyleBackColor = true;
			this.Confirm.Click += new System.EventHandler(this.Confirm_Click_1);
			// 
			// Cancel
			// 
			this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Cancel.Location = new System.Drawing.Point(473, 315);
			this.Cancel.Name = "Cancel";
			this.Cancel.Size = new System.Drawing.Size(185, 69);
			this.Cancel.TabIndex = 1;
			this.Cancel.Text = "Cancel";
			this.Cancel.UseVisualStyleBackColor = true;
			// 
			// Booking_msg
			// 
			this.Booking_msg.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.Booking_msg.AutoSize = true;
			this.Booking_msg.Location = new System.Drawing.Point(359, 72);
			this.Booking_msg.Name = "Booking_msg";
			this.Booking_msg.Size = new System.Drawing.Size(51, 20);
			this.Booking_msg.TabIndex = 2;
			this.Booking_msg.Text = "label1";
			// 
			// Booking_Confirm
			// 
			this.AcceptButton = this.Confirm;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.Cancel;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.ControlBox = false;
			this.Controls.Add(this.Booking_msg);
			this.Controls.Add(this.Cancel);
			this.Controls.Add(this.Confirm);
			this.Name = "Booking_Confirm";
			this.Text = "Confirm Booking";
			this.Load += new System.EventHandler(this.Booking_Confirm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button Confirm;
		private System.Windows.Forms.Button Cancel;
		private System.Windows.Forms.Label Booking_msg;
	}
}