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
		string CurrentUser;
		List<int> OutRequestIDs;
		List<int> IncRequestIDs;


		public ViewTransferRequests()
		{
			InitializeComponent();
		}

		private void ViewTransferRequests_Load(object sender, EventArgs e)
		{
			CurrentUser = BookingScreen.getcurrentuser();
			CONNECT = BookingScreen.getconnectionstring();
			sqlConnection4 = new SqlConnection(CONNECT);
			Get_Out_Transfer_Requests(CurrentUser);
			Get_Inc_Transfer_Requests(CurrentUser);
		}

		public void Get_Out_Transfer_Requests(String User)
		{
			if(sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}
			OutgoingRequestsList.Items.Clear();

			SqlCommand command = new SqlCommand("SELECT * FROM TransferRequests t LEFT JOIN Bookings b ON t.BookingID = b.BookingID WHERE t.MadeRequest = @currentUser;", sqlConnection4);
			command.Parameters.AddWithValue("@currentUser", User);

			OutRequestIDs = new List<int>();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					DateTime dateOfBooking = Convert.ToDateTime(reader["DateOfBooking"]).Date;
					if (dateOfBooking >= DateTime.Now.Date)
					{
						OutRequestIDs.Add(int.Parse($"{reader["RequestID"]}"));
						OutgoingRequestsList.Items.Add($"{reader["BookedFor"]}'s booking for {reader["RoomID"]} \non {dateOfBooking.DayOfWeek} {($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
					}
					
				}
			}

			if (OutRequestIDs.Count == 0)
			{
				OutgoingRequestsList.Items.Add("No outgoing transfer requests");
				OutRequestIDs.Clear();
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}
		}

		public void Get_Inc_Transfer_Requests(String User)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}
			IncomingRequestsList.Items.Clear();

			SqlCommand command = new SqlCommand("SELECT * FROM TransferRequests t LEFT JOIN Bookings b ON t.BookingID = b.BookingID WHERE b.TeacherInitials = @currentUser OR b.BookedFor = @currentuser;", sqlConnection4);
			command.Parameters.AddWithValue("@currentUser", User);

			IncRequestIDs = new List<int>();

			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					DateTime dateOfBooking = Convert.ToDateTime(reader["DateOfBooking"]).Date;
					if (dateOfBooking >= DateTime.Now.Date)
					{
						IncRequestIDs.Add(int.Parse($"{reader["RequestID"]}"));
						if (reader["BookedFor"].ToString().ToUpper() == User.ToUpper())
						{
							IncomingRequestsList.Items.Add($"{reader["MadeRequest"]} wants to transfer your booking for {reader["RoomID"]} \non {dateOfBooking.DayOfWeek} {($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
						}
						else
						{
							IncomingRequestsList.Items.Add($"{reader["MadeRequest"]} wants to transfer the booking for {reader["BookedFor"]} \nfor {reader["RoomID"]} on {dateOfBooking.DayOfWeek}{($"{dateOfBooking.Date}").Substring(0, 7)} during period {reader["BookedPeriod"]}");
						}
					}

				}
			}

			if (IncomingRequestsList.Items.Count == 0)
			{
				IncomingRequestsList.Items.Add("No incoming transfer requests");
				IncRequestIDs.Clear();
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}
		}

		private void Back_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			this.Close();
		}

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

		private void CancelRequestButton_Click(object sender, EventArgs e)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			SqlCommand command = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
			command.Parameters.AddWithValue("@requestID", OutRequestIDs[OutgoingRequestsList.SelectedIndex]);

			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0)
			{
				MessageBox.Show("Transfer request cancelled successfully.");
			}
			else
			{
				MessageBox.Show("Error cancelling transfer request. Please try again.");
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			Get_Out_Transfer_Requests(CurrentUser);
		}

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

		private void DenyRequestButton_Click(object sender, EventArgs e)
		{
			if (sqlConnection4.State != ConnectionState.Open)
			{
				sqlConnection4.Open();
			}

			SqlCommand command = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
			command.Parameters.AddWithValue("@requestID", IncRequestIDs[IncomingRequestsList.SelectedIndex]);

			int rowsaffected = command.ExecuteNonQuery();

			if (rowsaffected > 0)
			{
				MessageBox.Show("Transfer request denied successfully.");
			}
			else
			{
				MessageBox.Show("Error cancelling transfer request. Please try again.");
			}

			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			Get_Inc_Transfer_Requests(CurrentUser);
		}

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
			if (rowsaffected > 0)
			{
				MessageBox.Show("Transfer request approved successfully.");

				SqlCommand delCommand = new SqlCommand("DELETE FROM TransferRequests WHERE RequestID = @requestID;", sqlConnection4);
				delCommand.Parameters.AddWithValue("@requestID", IncRequestIDs[IncomingRequestsList.SelectedIndex]);
				delCommand.ExecuteNonQuery();
			}
			else
			{
				MessageBox.Show("Error approving transfer request. Please try again.");
			}



			if (sqlConnection4.State == ConnectionState.Open)
			{
				sqlConnection4.Close();
			}

			Get_Inc_Transfer_Requests(CurrentUser);
		}
	}
}
