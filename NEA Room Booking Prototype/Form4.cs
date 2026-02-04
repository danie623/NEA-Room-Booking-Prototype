using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Room_Booking_Prototype
{
	public partial class TransferBookingScreen : Form
	{
		SqlConnection sqlConnection3;
		string CONNECT;

		string bookingID_to_transfer;
		string CurrentUser;
		string currently_booked_for;
		List<string> teacher_initials;

		public TransferBookingScreen()
		{
			InitializeComponent();
			CONNECT = BookingScreen.getconnectionstring();
			sqlConnection3 = new SqlConnection(CONNECT);
		}

		private void TransferBookingScreen_Load(object sender, EventArgs e)
		{

		}

		public void Get_Booking_ID(string bookingID)
		{
			bookingID_to_transfer = bookingID;
			show_message();
		}

		private void show_message()
		{
			if (sqlConnection3.State != ConnectionState.Open)
			{
				sqlConnection3.Open();
			}

			SqlCommand command = new SqlCommand("SELECT * FROM Bookings WHERE BookingID = @bookingID;", sqlConnection3);
			command.Parameters.AddWithValue("@bookingID", bookingID_to_transfer);

			using (SqlDataReader reader = command.ExecuteReader())
			{
				if (reader.Read())
				{
					currently_booked_for = reader["BookedFor"].ToString();
					TransferMsg.Text = $"Transfer the booking for {currently_booked_for} on {Convert.ToDateTime(reader["DateOfBooking"]).ToString("dd/MM/yyyy")} during period {reader["BookedPeriod"].ToString()} to";
				}
			}

			if (sqlConnection3.State == ConnectionState.Open)
			{
				sqlConnection3.Close();
			}
		}

		public void Get_Current_User(string currentUser)
		{
			CurrentUser = currentUser.ToUpper();
			fill_teacher_initials();
		}

		private void fill_teacher_initials()
		{
			if (sqlConnection3.State != ConnectionState.Open)
			{
				sqlConnection3.Open();
			}

			SqlCommand command = new SqlCommand("SELECT TeacherInitials FROM Teachers WHERE TeacherInitials != @currentlyBookedFor;", sqlConnection3);

			command.Parameters.AddWithValue("@currentlyBookedFor", currently_booked_for);

			string tempInitials = "";
			teacher_initials = new List<string>();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					tempInitials = reader["TeacherInitials"].ToString().ToUpper();
					teacher_initials.Add(tempInitials);
					if (tempInitials != CurrentUser)
					{
						TeacherList.Items.Add(tempInitials);
					}
					else
					{
						TeacherList.Items.Add($"{tempInitials} (You)");
					}
				}
			}


			if (sqlConnection3.State == ConnectionState.Open)
			{
				sqlConnection3.Close();
			}
		}

		private void Confirm_Click(object sender, EventArgs e)
		{
			if (TeacherList.SelectedItem == null)
			{
				MessageBox.Show("Select a teacher to transfer this booking to. \nIf you are trying to cancel a booking please close this menu and select the cancel booking button.");
			}
			else
			{
				if (TeacherList.SelectedIndex < 0)
				{
					MessageBox.Show("Please enter the initials of the person you want to transfer the booking to.");
					return;
				}
				if (sqlConnection3.State != ConnectionState.Open)
				{
					sqlConnection3.Open();
				}

				SqlCommand command = new SqlCommand("UPDATE Bookings SET BookedFor = @newBookedFor WHERE BookingID = @bookingID;", sqlConnection3);
				command.Parameters.AddWithValue("@newBookedFor", teacher_initials[TeacherList.SelectedIndex]);
				command.Parameters.AddWithValue("@bookingID", bookingID_to_transfer);

				int rowsAffected = command.ExecuteNonQuery();
				
				if (rowsAffected > 0)
				{
					MessageBox.Show($"Booking transferred to {teacher_initials[TeacherList.SelectedIndex]} successfully.");
					DialogResult = DialogResult.OK;
					this.Close();
				}
				else
				{
					MessageBox.Show("Error transferring booking. Please try again.");
					this.Close();
				}
				
				if (sqlConnection3.State == ConnectionState.Open)
				{
					sqlConnection3.Close();
				}
			}




			
		}

		private void TeacherList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (TeacherList.SelectedIndex != -1)
			{
				Confirm.Enabled = true;
			}
			else
			{
				Confirm.Enabled = false;
			}
		}

		
	}
}
