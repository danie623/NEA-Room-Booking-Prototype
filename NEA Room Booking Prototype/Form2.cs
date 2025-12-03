using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Room_Booking_Prototype
{
	public partial class Booking_Confirm : Form
	{
		public Booking_Confirm()
		{
			InitializeComponent();
		}

		private void Booking_Confirm_Load(object sender, EventArgs e)
		{
			
		}

		public void showMessage(string roomID, int period, DateTime dateOfBooking, string teacherInitials)
		{
			Booking_msg.Text = $"Confirm booking for {roomID} \nperiod {period} on {dateOfBooking} \nfor {teacherInitials}";
		}

		private void QuitButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		private void Confirm_Click(object sender, EventArgs e)
		{
		}

		private void Confirm_Click_1(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}
	}
}
