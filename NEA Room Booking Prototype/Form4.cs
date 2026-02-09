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

		// initialise variables to store the booking ID of the booking being transferred, the current user, the teacher the booking is currently booked for and a list of all teacher initials for filling the dropdown list
		string bookingID_to_transfer;
		string CurrentUser;
		string currently_booked_for;
		List<string> teacher_initials;

		public TransferBookingScreen(String BookingID, String TeacherInitials)
		{
			InitializeComponent();
			// get the connection string from the BookingScreen and create a new SQL connection using it, then store the booking ID of the booking being transferred and the current user's initials in variables for use in other methods, finally call the methods to show the transfer message and fill the teacher initials dropdown list on opening
			CONNECT = BookingScreen.getconnectionstring();
			sqlConnection3 = new SqlConnection(CONNECT);
			bookingID_to_transfer = BookingID;
			CurrentUser = TeacherInitials.ToUpper();
			show_message();
			fill_teacher_initials();
		}

		private void TransferBookingScreen_Load(object sender, EventArgs e)
		{
		}

		// Method to show the transfer message on the form, which includes the name of the teacher the booking is currently booked for, the date of the booking and the period of the booking
		private void show_message()
		{
			if (sqlConnection3.State != ConnectionState.Open)
			{
				sqlConnection3.Open();
			}

			// SQL command to get the details of the booking being transferred, using the booking ID stored in the variable bookingID_to_transfer, and store the name of the teacher the booking is currently booked for in a variable for use in other methods
			SqlCommand command = new SqlCommand("SELECT * FROM Bookings WHERE BookingID = @bookingID;", sqlConnection3);
			command.Parameters.AddWithValue("@bookingID", bookingID_to_transfer);

			using (SqlDataReader reader = command.ExecuteReader())
			{
				if (reader.Read()) // should only read one record as booking IDs are unique, so no need for a while loop
				{// store the name of the teacher the booking is currently booked for in a variable for use in fill_techer_initials method and show the transfer message on the form using the details of the booking
					currently_booked_for = reader["BookedFor"].ToString();
					TransferMsg.Text = $"Transfer the booking for {currently_booked_for} on {Convert.ToDateTime(reader["DateOfBooking"]).ToString("dd/MM/yyyy")} during period {reader["BookedPeriod"].ToString()} to:";
				}
			}

			if (sqlConnection3.State == ConnectionState.Open)
			{
				sqlConnection3.Close();
			}
		}

		// Method to fill the teacher initials dropdown list with the initials of all teachers except the teacher the booking is currently booked for, which is stored in the variable currently_booked_for, and also indicate which teacher is the current user by adding "(You)" after their initials in the dropdown list
		private void fill_teacher_initials()
		{
			if (sqlConnection3.State != ConnectionState.Open)
			{
				sqlConnection3.Open();
			}

			SqlCommand command = new SqlCommand("SELECT TeacherInitials FROM Teachers WHERE TeacherInitials != @currentlyBookedFor;", sqlConnection3);

			command.Parameters.AddWithValue("@currentlyBookedFor", currently_booked_for);

			// Temporary variable to store the initials of each teacher as they are read from the database, which is then added to the list of teacher initials and used to fill the dropdown list
			string tempInitials;
			teacher_initials = new List<string>();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					// store the initials of each teacher as they are read from the database in a temporary variable, which is then added to the list of teacher initials and used to fill the dropdown list, if the initials match the current user's initials then add "(You)" after their initials in the dropdown list to indicate that they are the current user
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

		// Event handler for the Confirm button, which checks if a teacher has been selected from the dropdown list and if so updates the booking in the database to be booked for the teacher selected in the dropdown list, using the booking ID stored in the variable bookingID_to_transfer to identify which booking to update, and then shows a message indicating whether the transfer was successful or if there was an error, before closing the form
		private void Confirm_Click(object sender, EventArgs e)
		{
			if (TeacherList.SelectedItem == null) // shouldn't be possible as the Confirm button is disabled until a teacher is selected, but just in case, if no teacher is selected show a message asking the user to select a teacher to transfer the booking to or to cancel the booking if they are trying to cancel the booking instead of transferring it
			{
				MessageBox.Show("Select a teacher to transfer this booking to. \nIf you are trying to cancel a booking please close this menu and select the cancel booking button.");
			}
			else // if a teacher has been selected from the dropdown list
			{
				if (TeacherList.SelectedIndex < 0) // just in case, check if the selected index of the dropdown list is valid, if not show a message asking the user to select a teacher to transfer the booking to or to cancel the booking if they are trying to cancel the booking instead of transferring it
				{
					MessageBox.Show("Please enter the initials of the person you want to transfer the booking to.");
					return;
				}
				if (sqlConnection3.State != ConnectionState.Open)
				{
					sqlConnection3.Open();
				}

				// SQL command to update the booking in the database to be booked for the teacher selected in the dropdown list, using the booking ID stored in the variable bookingID_to_transfer
				SqlCommand command = new SqlCommand("UPDATE Bookings SET BookedFor = @newBookedFor WHERE BookingID = @bookingID;", sqlConnection3);
				command.Parameters.AddWithValue("@newBookedFor", teacher_initials[TeacherList.SelectedIndex]);
				command.Parameters.AddWithValue("@bookingID", bookingID_to_transfer);

				int rowsAffected = command.ExecuteNonQuery();

				if (rowsAffected > 0) // if the number of rows affected by the SQL command is greater than 0, then the update was successful and show a message indicating that the booking was transferred successfully, otherwise show a message indicating that there was an error transferring the booking, return to view bookings form and close the transfer booking form
				{
					MessageBox.Show($"Booking transferred to {teacher_initials[TeacherList.SelectedIndex]} successfully.");
					DialogResult = DialogResult.OK;
					this.Close();
				}
				else // if the number of rows affected by the SQL command is not greater than 0, then there was an error updating the booking and show a message indicating that there was an error transferring the booking, and then close the transfer booking form to return to the view bookings form, where the user can try transferring the booking again
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

		// Event handler for the dropdown list of teacher initials, which checks if a teacher has been selected from the dropdown list and if so enables the Confirm button, otherwise disables the Confirm button to prevent the user from trying to confirm a transfer without selecting a teacher to transfer the booking to
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
