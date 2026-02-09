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
	public partial class ViewTransferRequests : Form
	{
		SqlConnection sqlConnection4;
		string CONNECT;

		// initialise variables to store the current user and lists of the booking IDs of the transfer requests to fill the list boxes and use in the approve, deny and cancel request methods
		string CurrentUser;
		List<int> OutRequestIDs;
		List<int> IncRequestIDs;

		public ViewTransferRequests()
		{
			InitializeComponent();
		}

		private void ViewTransferRequests_Load(object sender, EventArgs e)
		{
			// get the current user's initials and the connection string from the BookingScreen, then create a new SQL connection using the connection string and call the methods to fill the list boxes with the incoming and outgoing transfer requests for the current user
			CurrentUser = BookingScreen.getcurrentuser();
			CONNECT = BookingScreen.getconnectionstring();
			sqlConnection4 = new SqlConnection(CONNECT);
			Get_Out_Transfer_Requests(CurrentUser);
			Get_Inc_Transfer_Requests(CurrentUser);
		}

		// Method to fill the outgoing requests list box with the outgoing transfer requests for the current user
		public void Get_Out_Transfer_Requests(String User)
		{
			if(sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}
			// clear the list box and the list of booking IDs to ensure only current transfer requests are shown
			OutgoingRequestsList.Items.Clear();
			OutRequestIDs = new List<int>();

			SqlCommand command = new SqlCommand("SELECT * FROM TransferRequests t LEFT JOIN Bookings b ON t.BookingID = b.BookingID WHERE t.MadeRequest = @currentUser;", sqlConnection4);
			command.Parameters.AddWithValue("@currentUser", User);

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read()) // loop through all transfer requests made by the current user and add the details of the booking being transferred to the list box, as well as storing the booking ID in a list for use in the cancel request method, only showing transfer requests for bookings that have not yet passed
				{
					DateTime dateOfBooking = Convert.ToDateTime(reader["DateOfBooking"]).Date;
					if (dateOfBooking >= DateTime.Now.Date)
					{
						OutRequestIDs.Add(int.Parse($"{reader["RequestID"]}"));
						OutgoingRequestsList.Items.Add($"{reader["BookedFor"]}'s booking for {reader["RoomID"]} \non {dateOfBooking.DayOfWeek} {($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
					}
					
				}
			}

			if (OutRequestIDs.Count == 0) // if there are no outgoing transfer requests, show a message in the list box and clear the list of booking IDs to ensure the approve and deny buttons are not shown
			{
				OutgoingRequestsList.Items.Add("No outgoing transfer requests");
				OutRequestIDs.Clear();
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}
		}

		// Method to fill the incoming requests list box with the incoming transfer requests for the current user, showing different messages depending on whether the current user is the teacher the booking is currently booked for or the teacher who made the booking, as both of these could be transfer requests for the current user
		public void Get_Inc_Transfer_Requests(String User)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			// clear the list box and the list of booking IDs to ensure only current transfer requests are shown
			IncomingRequestsList.Items.Clear();
			IncRequestIDs = new List<int>();

			SqlCommand command = new SqlCommand("SELECT * FROM TransferRequests t LEFT JOIN Bookings b ON t.BookingID = b.BookingID WHERE b.TeacherInitials = @currentUser OR b.BookedFor = @currentuser;", sqlConnection4);
			command.Parameters.AddWithValue("@currentUser", User);

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					DateTime dateOfBooking = Convert.ToDateTime(reader["DateOfBooking"]).Date;
					if (dateOfBooking >= DateTime.Now.Date)
					{
						IncRequestIDs.Add(int.Parse($"{reader["RequestID"]}"));
						if (reader["BookedFor"].ToString().ToUpper() == User.ToUpper()) // if the current user is the teacher the booking is currently booked for, show a message indicating that another teacher wants to transfer the booking to them
						{
							IncomingRequestsList.Items.Add($"{reader["MadeRequest"]} wants to transfer your booking for {reader["RoomID"]} \non {dateOfBooking.DayOfWeek} {($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
						}
						else // if the current user is the teacher who made the booking but not who the booking is for, show a message indicating that another teacher wants to transfer the booking for them, as they are the one who made the booking but it is currently booked for another teacher, so it could be a transfer request for either of them
						{
							IncomingRequestsList.Items.Add($"{reader["MadeRequest"]} wants to transfer the booking for {reader["BookedFor"]} that you made \nfor {reader["RoomID"]} on {dateOfBooking.DayOfWeek}{($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
						}
					}

				}
			}

			if (IncomingRequestsList.Items.Count == 0) // if there are no incoming transfer requests, show a message in the list box and clear the list of booking IDs to ensure the approve and deny buttons are not shown
			{
				IncomingRequestsList.Items.Add("No incoming transfer requests");
				IncRequestIDs.Clear();
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}
		}

		// Event handler for the Back button to set the dialog result to Cancel and close the form, returning the user to the booking screen
		private void Back_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

		// Event handler for the SelectedIndexChanged event of the outgoing requests list box to show the cancel request button when a transfer request is selected, and hide it when no transfer request is selected, using the list of booking IDs to ensure the button is only shown when a valid transfer request is selected
		private void OutgoingRequestsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (OutgoingRequestsList.SelectedIndex != -1 && OutRequestIDs.Count != 0)
			{
				CancelRequestButton.Enabled = true;
				CancelRequestButton.Visible = true;
			}
            else
            {
				CancelRequestButton.Enabled = false;
				CancelRequestButton.Visible = false;

			}
        }

		// Event handler for the Cancel Request button to delete the selected transfer request from the database using the booking ID stored in the list of booking IDs, and then show a message indicating whether the cancellation was successful or if there was an error, before refreshing the list of outgoing transfer requests to show the updated list
		private void CancelRequestButton_Click(object sender, EventArgs e)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			SqlCommand command = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
			command.Parameters.AddWithValue("@requestID", OutRequestIDs[OutgoingRequestsList.SelectedIndex]);

			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0) // if the number of rows affected by the SQL command is greater than 0, then the deletion was successful
			{
				MessageBox.Show("Transfer request cancelled successfully.");
			}
			else // if the number of rows affected by the SQL command is not greater than 0, then there was an error deleting the transfer request
			{
				MessageBox.Show("Error cancelling transfer request. Please try again.");
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			// refresh the list of outgoing transfer requests to show the updated list after the cancellation
			Get_Out_Transfer_Requests(CurrentUser);
		}

		// Event handler for the SelectedIndexChanged event of the incoming requests list box to show the approve and deny buttons when a transfer request is selected, and hide them when no transfer request is selected, using the list of booking IDs to ensure the buttons are only shown when a valid transfer request is selected
		private void IncomingRequestsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (IncomingRequestsList.SelectedIndex != -1 && IncRequestIDs.Count != 0)
			{
				ApproveRequestButton.Enabled = true;
				ApproveRequestButton.Visible = true;
				DenyRequestButton.Enabled = true;
				DenyRequestButton.Visible = true;
			}
			else
			{
				ApproveRequestButton.Enabled = false;
				ApproveRequestButton.Visible = false;
				DenyRequestButton.Enabled = false;
				DenyRequestButton.Visible = false;

			}
		}

		// Event handler for the Deny Request button to delete the selected transfer request from the database using the booking ID stored in the list of booking IDs, and then show a message indicating whether the denial was successful or if there was an error, before refreshing the list of incoming transfer requests to show the updated list
		private void DenyRequestButton_Click(object sender, EventArgs e)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			SqlCommand command = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
			command.Parameters.AddWithValue("@requestID", IncRequestIDs[IncomingRequestsList.SelectedIndex]);

			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0) // if the number of rows affected by the SQL command is greater than 0, then the deletion was successful
			{
				MessageBox.Show("Transfer request denied successfully.");
			}
			else // if the number of rows affected by the SQL command is not greater than 0, then there was an error deleting the transfer request
			{
				MessageBox.Show("Error cancelling transfer request. Please try again.");
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			// refresh the list of incoming transfer requests to show the updated list after the denial
			Get_Inc_Transfer_Requests(CurrentUser);
		}

		//	Event handler for the Approve Request button to update the booking in the database 
		private void ApproveRequestButton_Click(object sender, EventArgs e)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			SqlCommand command = new SqlCommand("UPDATE Bookings SET BookedFor = @user WHERE BookingID = (SELECT BookingID FROM TransferRequests WHERE RequestID = @requestid);", sqlConnection4);
			command.Parameters.AddWithValue("@user", CurrentUser);
			command.Parameters.AddWithValue("@requestid", IncRequestIDs[IncomingRequestsList.SelectedIndex]);

			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0) // if the number of rows affected by the SQL command is greater than 0, then the transfer was successful
			{
				MessageBox.Show("Transfer request approved successfully.");

				// deletes transfer request
				SqlCommand delCommand = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
				delCommand.Parameters.AddWithValue("@requestID", IncRequestIDs[IncomingRequestsList.SelectedIndex]);
				delCommand.ExecuteNonQuery();
			}
			else // if the number of rows affected by the SQL command is not greater than 0, then there was an error transfering the booking
			{
				MessageBox.Show("Error approving transfer request. Please try again.");
			}



			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			// refresh the list of incoming transfer requests to show the updated list after the approval
			Get_Inc_Transfer_Requests(CurrentUser);
		}
	}
}
