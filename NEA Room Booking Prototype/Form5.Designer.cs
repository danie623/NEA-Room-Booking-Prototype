namespace NEA_Room_Booking_Prototype
{
	partial class ViewTransferRequests
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
			this.IncomingRequestsList = new System.Windows.Forms.ListBox();
			this.OutgoingRequestsList = new System.Windows.Forms.ListBox();
			this.Back = new System.Windows.Forms.Button();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.ApproveRequestButton = new System.Windows.Forms.Button();
			this.DenyRequestButton = new System.Windows.Forms.Button();
			this.CancelRequestButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// IncomingRequestsList
			// 
			this.IncomingRequestsList.FormattingEnabled = true;
			this.IncomingRequestsList.Location = new System.Drawing.Point(12, 50);
			this.IncomingRequestsList.Name = "IncomingRequestsList";
			this.IncomingRequestsList.Size = new System.Drawing.Size(404, 316);
			this.IncomingRequestsList.TabIndex = 0;
			this.IncomingRequestsList.SelectedIndexChanged += new System.EventHandler(this.IncomingRequestsList_SelectedIndexChanged);
			// 
			// OutgoingRequestsList
			// 
			this.OutgoingRequestsList.FormattingEnabled = true;
			this.OutgoingRequestsList.Location = new System.Drawing.Point(422, 50);
			this.OutgoingRequestsList.Name = "OutgoingRequestsList";
			this.OutgoingRequestsList.Size = new System.Drawing.Size(366, 316);
			this.OutgoingRequestsList.TabIndex = 1;
			this.OutgoingRequestsList.SelectedIndexChanged += new System.EventHandler(this.OutgoingRequestsList_SelectedIndexChanged);
			// 
			// Back
			// 
			this.Back.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.Back.Location = new System.Drawing.Point(700, 396);
			this.Back.Name = "Back";
			this.Back.Size = new System.Drawing.Size(100, 55);
			this.Back.TabIndex = 2;
			this.Back.Text = "Back to bookings screen";
			this.Back.UseVisualStyleBackColor = true;
			this.Back.Click += new System.EventHandler(this.Back_Click);
			// 
			// richTextBox1
			// 
			this.richTextBox1.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.richTextBox1.Enabled = false;
			this.richTextBox1.Location = new System.Drawing.Point(413, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new System.Drawing.Size(10, 451);
			this.richTextBox1.TabIndex = 3;
			this.richTextBox1.Text = "";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(14, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(273, 31);
			this.label1.TabIndex = 4;
			this.label1.Text = "Incoming Requests:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(429, 9);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(273, 31);
			this.label2.TabIndex = 5;
			this.label2.Text = "Outgoing Requests:";
			// 
			// ApproveRequestButton
			// 
			this.ApproveRequestButton.Enabled = false;
			this.ApproveRequestButton.Location = new System.Drawing.Point(34, 384);
			this.ApproveRequestButton.Name = "ApproveRequestButton";
			this.ApproveRequestButton.Size = new System.Drawing.Size(133, 43);
			this.ApproveRequestButton.TabIndex = 6;
			this.ApproveRequestButton.Text = "Approve Request";
			this.ApproveRequestButton.UseVisualStyleBackColor = true;
			this.ApproveRequestButton.Visible = false;
			this.ApproveRequestButton.Click += new System.EventHandler(this.ApproveRequestButton_Click);
			// 
			// DenyRequestButton
			// 
			this.DenyRequestButton.Enabled = false;
			this.DenyRequestButton.Location = new System.Drawing.Point(231, 384);
			this.DenyRequestButton.Name = "DenyRequestButton";
			this.DenyRequestButton.Size = new System.Drawing.Size(133, 43);
			this.DenyRequestButton.TabIndex = 7;
			this.DenyRequestButton.Text = "Deny Request";
			this.DenyRequestButton.UseVisualStyleBackColor = true;
			this.DenyRequestButton.Visible = false;
			this.DenyRequestButton.Click += new System.EventHandler(this.DenyRequestButton_Click);
			// 
			// CancelRequestButton
			// 
			this.CancelRequestButton.Enabled = false;
			this.CancelRequestButton.Location = new System.Drawing.Point(461, 384);
			this.CancelRequestButton.Name = "CancelRequestButton";
			this.CancelRequestButton.Size = new System.Drawing.Size(133, 43);
			this.CancelRequestButton.TabIndex = 8;
			this.CancelRequestButton.Text = "Cancel Request";
			this.CancelRequestButton.UseVisualStyleBackColor = true;
			this.CancelRequestButton.Visible = false;
			this.CancelRequestButton.Click += new System.EventHandler(this.CancelRequestButton_Click);
			// 
			// ViewTransferRequests
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.CancelRequestButton);
			this.Controls.Add(this.DenyRequestButton);
			this.Controls.Add(this.ApproveRequestButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.Back);
			this.Controls.Add(this.OutgoingRequestsList);
			this.Controls.Add(this.IncomingRequestsList);
			this.MaximumSize = new System.Drawing.Size(816, 489);
			this.MinimumSize = new System.Drawing.Size(816, 489);
			this.Name = "ViewTransferRequests";
			this.Text = "Transfer Requests";
			this.Load += new System.EventHandler(this.ViewTransferRequests_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox IncomingRequestsList;
		private System.Windows.Forms.ListBox OutgoingRequestsList;
		private System.Windows.Forms.Button Back;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button ApproveRequestButton;
		private System.Windows.Forms.Button DenyRequestButton;
		private System.Windows.Forms.Button CancelRequestButton;
	}
}