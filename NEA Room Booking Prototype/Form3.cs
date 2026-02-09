using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NEA_Room_Booking_Prototype
{
	public partial class View_Bookings : Form
	{
		//This form allows teachers to view their current and upcoming bookings, as well as bookings made for them by other teachers. Teachers can also choose to cancel or transfer their bookings to another teacher.
		SqlConnection sqlConnection2;
		string CONNECT;

		//initialising variables to store current user, booking IDs and selected booking ID for cancelling or transfering bookings
		string currentUser;
		List<String> bookingIDs;
		String selectedBookingID;

		//constructor to initialize the form, set up the database connection, and hide the cancel and transfer buttons until a booking is selected
		public View_Bookings()
		{
			InitializeComponent();
			CONNECT = BookingScreen.getconnectionstring();
			sqlConnection2 = new SqlConnection(CONNECT);
            selectedBookingID = null;
            showBookingButtons(false);
			// get current user from booking screen and display their bookings
			currentUser = BookingScreen.getcurrentuser();
			Get_Bookings(currentUser);
		}

		private void View_Bookings_Load(object sender, EventArgs e)
		{	 
		}

		// method to retrieve bookings for the current user from the database and display them in a listbox, with different formatting depending on whether the booking was made by the user or for the user by another teacher. The method also populates a list of booking IDs to keep track of which booking is selected for cancelling or transfering.
		private void Get_Bookings(string teacher)
		{
			//connect to database
			if (sqlConnection2.State != ConnectionState.Open)
			{
				sqlConnection2.Open();
			}

			// clear listbox and bookingIDs list before populating with current bookings
			BookingsList.Items.Clear();
			bookingIDs = new List<String>();
			// create SQL command to retrieve bookings for the current user, with an option to include bookings made for others if the corresponding checkbox is checked. The bookings are ordered by date and period, and only upcoming bookings (from the current date onwards) are retrieved.
			SqlCommand command;
            DateTime dateOfBooking;
			string whoBooked = null;
			string bookedFor = null;

			//get bookings for teacher
			if (Bookings_For_others_check.Checked) // if the checkbox to show bookings for others is checked, retrieve and display all bookings that were made by the current user for themselves, as well as bookings that were made for the current user by other teachers, with additional information about who made the booking and who it was booked for. The same formatting is used for the date and period of the booking, but additional information is included in the listbox item to indicate whether the booking was made by the current user or for the current user by another teacher.
			{ 
                command = new SqlCommand("SELECT * FROM Bookings WHERE (TeacherInitials = @teacher OR BookedFor = @teacher) AND DateOfBooking >= @date ORDER BY DateOfBooking ASC, BookedPeriod ASC;", sqlConnection2);
				command.Parameters.AddWithValue("@teacher", teacher);
				command.Parameters.AddWithValue("@date", DateTime.Now.Date);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						//display bookings in listbox
						bookingIDs.Add($"{reader["bookingID"]}");
						whoBooked = ($"{reader["TeacherInitials"]}").ToUpper();
						bookedFor = ($"{reader["BookedFor"]}").ToUpper();
						dateOfBooking = (DateTime)reader["DateOfBooking"];
						if (whoBooked == currentUser.ToUpper() && bookedFor == currentUser.ToUpper()) // if the booking was made by the current user for themselves, display it without any additional information. If the booking was made by the current user for another teacher, display the name of the teacher it was booked for. If the booking was made for the current user by another teacher, display the name of the teacher who made the booking.
						{
							BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.DayOfWeek} {dateOfBooking.Date.ToString("dd/MM/yyyy")} period: {reader["BookedPeriod"]}");
						}
						else if (whoBooked == currentUser.ToUpper() && bookedFor != currentUser.ToUpper()) // if the booking was made by the current user for another teacher, display the name of the teacher it was booked for.
						{
							BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.DayOfWeek} {dateOfBooking.Date.ToString("dd/MM/yyyy")} period: {reader["BookedPeriod"]} \nBooked for {bookedFor} by you.");
						}
						else // if the booking was made for the current user by another teacher, display the name of the teacher who made the booking.
						{
							BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.DayOfWeek} {dateOfBooking.Date.ToString("dd/MM/yyyy")} period: {reader["BookedPeriod"]} \nBooked for you by {whoBooked}.");
						}
					}
				}
			}
			else // if the checkbox to show bookings for others is not checked, only retrieve and display bookings that were made by the current user for themselves, with the same formatting as before but without any additional information about who made the booking or who it was booked for.
			{ 
				command = new SqlCommand("SELECT * FROM Bookings WHERE BookedFor = @teacher AND DateOfBooking >= @date ORDER BY DateOfBooking ASC, BookedPeriod ASC;", sqlConnection2);
				command.Parameters.AddWithValue("@teacher", teacher);
				command.Parameters.AddWithValue("@date", DateTime.Now.Date);
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						//display bookings in listbox
						bookingIDs.Add($"{reader["bookingID"]}");
						whoBooked = ($"{reader["TeacherInitials"]}").ToUpper();
						dateOfBooking = (DateTime)reader["DateOfBooking"];
						if (whoBooked == currentUser.ToUpper()) // if the booking was made by the current user for themselves, display it without any additional information. If the booking was made for the current user by another teacher, display the name of the teacher who made the booking.
						{
							BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.DayOfWeek} {dateOfBooking.Date.ToString("dd/MM/yyyy")} period: {reader["BookedPeriod"]}");
						}
						else // if the booking was made for the current user by another teacher, display the name of the teacher who made the booking.
						{
							BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.DayOfWeek} {dateOfBooking.Date.ToString("dd/MM/yyyy")} period: {reader["BookedPeriod"]} \nBooked for you by {whoBooked}.");
						}
					}
				}
			}


			// close database connection
			if (sqlConnection2.State == ConnectionState.Open)
			{
				sqlConnection2.Close();
			}
		}

		// event handler for the Back button to close the form and return to the previous screen without making any changes to the bookings
		private void Back_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
        }

		// method to show or hide the cancel and transfer buttons based on whether a booking is selected in the listbox. If a booking is selected, the buttons are enabled and made visible, allowing the user to cancel or transfer the selected booking. If no booking is selected, the buttons are disabled and hidden to prevent any actions from being taken without a valid selection.
		private void showBookingButtons(bool show)
		{
			if (show)
			{
				Cancel_Booking.Enabled = true;
				Cancel_Booking.Visible = true;
				Transfer_Booking.Enabled = true;
				Transfer_Booking.Visible = true;
            }
			else
			{
				Cancel_Booking.Enabled = false;
				Cancel_Booking.Visible = false;
				Transfer_Booking.Enabled = false;
				Transfer_Booking.Visible = false;
            }
        }

		// event handler for when the selected index of the bookings listbox changes. If a booking is selected, the corresponding booking ID is stored in the selectedBookingID variable and the cancel and transfer buttons are shown. If no booking is selected, the selectedBookingID variable is set to null and the buttons are hidden.
		private void BookingsList_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (BookingsList.SelectedIndex != -1) // no booking is selected -> buttons arent shown
			{
				selectedBookingID = bookingIDs[BookingsList.SelectedIndex];
				showBookingButtons(true);
            }
			else // booking is selected -> buttons are shown
			{
				selectedBookingID = null;
                showBookingButtons(false);
            }
        }

		// event handler for the Cancel Booking button to cancel the selected booking. The method first checks if there is an active transfer request for the selected booking. If there is, the booking is transferred to the requester instead of being cancelled. If there is no active transfer request, the booking is deleted from the database. After the cancellation or transfer is processed, a message box is displayed to inform the user of the outcome, and the bookings list is refreshed to reflect any changes.
		private void Cancel_Booking_Click(object sender, EventArgs e)
        {
			if (sqlConnection2.State != ConnectionState.Open)
			{
				sqlConnection2.Open();
            }

			// check if there is an active transfer request for the selected booking. If there is, transfer the booking to the requester instead of cancelling it. If there isn't, cancel the booking as normal by deleting it from the database.
			SqlCommand checkForTransferRequestCommand = new SqlCommand("SELECT TOP 1 * FROM TransferRequests WHERE BookingID= @bookingID;", sqlConnection2);
			checkForTransferRequestCommand.Parameters.AddWithValue("@bookingID", selectedBookingID);


			// initialize a SqlCommand variable to hold a command for either transferring the booking to the requester or cancelling the booking.
			SqlCommand command;

			// boolean variable to keep track of whether the booking was transfered to the requester instead of being cancelled
			bool transferedInstead = false;

			using (SqlDataReader reader = checkForTransferRequestCommand.ExecuteReader())
			{
				if (reader.Read()) // if there is an active transfer request for the selected booking, transfer the booking to the requester instead of cancelling it. This is done by updating the BookedFor field of the booking to the MadeRequest value from the TransferRequests table, and then deleting the corresponding transfer request from the TransferRequests table. If there is no active transfer request, cancel the booking as normal by deleting it from the Bookings table.
				{
					command = new SqlCommand("UPDATE Bookings SET BookedFor = (SELECT TOP 1 MadeRequest FROM TransferRequests where BookingID= @bookingID ORDER BY RequestID ASC) WHERE BookingID = @bookingID;   DELETE FROM TransferRequests WHERE RequestID = (SELECT TOP 1 RequestID FROM TransferRequests where BookingID= @bookingID ORDER BY RequestID ASC);", sqlConnection2);
					command.Parameters.AddWithValue("@bookingID", selectedBookingID);
					transferedInstead = true;
				}
				else // if there is no active transfer request for the selected booking, cancel the booking as normal by deleting it from the Bookings table.
				{
					command = new SqlCommand("DELETE FROM Bookings WHERE bookingID = @bookingID;", sqlConnection2);
					command.Parameters.AddWithValue("@bookingID", selectedBookingID);
				}
			}

			// execute the appropriate command to either transfer the booking to the requester or cancel the booking
			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0 && transferedInstead) // if the booking was transfered to the requester instead of being cancelled, display a message box to inform the user that the booking was transfered, and refresh the bookings list
			{
				MessageBox.Show("There was an active transfer request for this booking so it was automatically transfered to the requester.");
				BookingsList.Items.Clear();
				Get_Bookings(currentUser);
			}
			else if (rowsaffected > 0) // if the booking was cancelled successfully, display a message box to inform the user that the booking was cancelled, and refresh the bookings list
			{
				MessageBox.Show("Booking cancelled successfully.");
				BookingsList.Items.Clear();
				Get_Bookings(currentUser);
			}
			else // if there was an error cancelling the booking, display a message box showing that there was an error and to try again
			{
				MessageBox.Show("Error cancelling booking. Please try again.");
			}

			// close database connection
			if (sqlConnection2.State == ConnectionState.Open)
			{
				sqlConnection2.Close();
            }
        }

		// event handler for the checkbox to show bookings for others. When the checkbox is checked or unchecked, the Get_Bookings method is called to refresh the bookings list and display the appropriate bookings based on the state of the checkbox.
		private void Bookings_For_others_check_CheckedChanged(object sender, EventArgs e)
        {
			Get_Bookings(currentUser);
        }

		// event handler for the Transfer Booking button to transfer the selected booking to another teacher
		private void Transfer_Booking_Click(object sender, EventArgs e)
		{
			// create a new instance of the TransferBookingScreen form and show it as a dialog. Pass the selected booking ID and current user to the transfer form using constructor
			TransferBookingScreen transferForm = new TransferBookingScreen(selectedBookingID, currentUser);

			if (transferForm.ShowDialog() == DialogResult.OK) // if the transfer form returns a DialogResult of OK, indicating that the transfer was successful, refresh the bookings list to reflect any changes.
			{
				BookingsList.Items.Clear();
				Get_Bookings(currentUser);
			}
		}
	}
}
