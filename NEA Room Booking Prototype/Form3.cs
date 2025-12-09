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
		SqlConnection sqlConnection2;
		private const string CONNECT = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"M:\\Visual Studio 2022\\projects\\New NEA\\NEA Room Booking\\NEA Room Booking\\resourses\\RoomBookingDatabase.mdf\";Integrated Security = True; Connect Timeout = 30;";

		string currentUser;
		List<String> bookingIDs;
		String selectedBookingID;

        public View_Bookings()
		{
			InitializeComponent();
			sqlConnection2 = new SqlConnection(CONNECT);
            selectedBookingID = null;
            showBookingButtons(false);
        }


		
		private void View_Bookings_Load(object sender, EventArgs e)
		{
			 
		}
		
		public void Get_Teacher(string teacher)
		{
			currentUser = teacher;
			Get_Bookings(currentUser);
        }
		private void Get_Bookings(string teacher)
		{
			bookingIDs = new List<String>();
			DateTime dateOfBooking;
			//connect to database
			if (sqlConnection2.State != ConnectionState.Open)
			{
				sqlConnection2.Open();
			}

			//get bookings for teacher
			SqlCommand command = new SqlCommand("SELECT * FROM Bookings WHERE TeacherInitials = @teacher ORDER BY DateOfBooking ASC, BookedPeriod ASC; --AND DateOfBooking >= @date ORDER BY DateOfBooking ASC, BookedPeriod ASC;", sqlConnection2 );
			command.Parameters.AddWithValue("@teacher", teacher);
			command.Parameters.AddWithValue("@date", DateTime.Now.AddDays(-1));
			using (SqlDataReader reader = command.ExecuteReader())
			{
				while (reader.Read())
				{
					//display bookings in listbox
					bookingIDs.Add($"{reader["bookingID"]}");
					dateOfBooking = (DateTime) reader["DateOfBooking"];
					BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.Day} {($"{dateOfBooking}").Substring(0,10)} period: {reader["BookedPeriod"]}");
				}
			}

			
			if (sqlConnection2.State == ConnectionState.Open)
			{
				sqlConnection2.Close();
			}
		}

        private void Back_Click(object sender, EventArgs e)
        {
			this.DialogResult = DialogResult.Cancel;
			this.Close();
        }

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

        private void BookingsList_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (BookingsList.SelectedIndex != -1)
			{
				selectedBookingID = bookingIDs[BookingsList.SelectedIndex];
				showBookingButtons(true);
            }
			else
			{
				selectedBookingID = null;
                showBookingButtons(false);
            }
        }

        private void Cancel_Booking_Click(object sender, EventArgs e)
        {
			if (sqlConnection2.State != ConnectionState.Open)
			{
				sqlConnection2.Open();
            }

            SqlCommand command = new SqlCommand("DELETE FROM Bookings WHERE bookingID = @bookingID;", sqlConnection2);
			command.Parameters.AddWithValue("@bookingID", selectedBookingID);

			if (command.ExecuteNonQuery() > 0)
			{
				MessageBox.Show("Booking cancelled successfully.");
				BookingsList.Items.Clear();
				Get_Bookings(currentUser);
			}
			else
			{
				MessageBox.Show("Error cancelling booking. Please try again.");
			}
            
			if (sqlConnection2.State == ConnectionState.Open)
			{
				sqlConnection2.Close();
            }
        }
    }
}
