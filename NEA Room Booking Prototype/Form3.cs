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

		public View_Bookings()
		{
			InitializeComponent();
			sqlConnection2 = new SqlConnection(CONNECT);
		}


		
		private void View_Bookings_Load(object sender, EventArgs e)
		{
			 
		}
		
		public void get_Teacher(string teacher)
		{
			currentUser = teacher;
		}
		private void get_Bookings(string teacher)
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
					bookingIDs.Add($"{reader["bookingID"]}");
					dateOfBooking = (DateTime) reader["DateOfBooking"];
					BookingsList.Items.Add($"{reader["RoomID"]} booked for {dateOfBooking.Day} {($"{dateOfBooking}").Substring(0,10)} period: {reader["BookedPeriod"]}");
				}
			}

			//display bookings in listbox
			if (sqlConnection2.State == ConnectionState.Open)
			{
				sqlConnection2.Close();
			}
		} 
	}
}
