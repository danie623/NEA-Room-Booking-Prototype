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

		public View_Bookings()
		{
			InitializeComponent();
			sqlConnection2 = new SqlConnection(CONNECT);
		}


		
		private void View_Bookings_Load(object sender, EventArgs e)
		{
			 
		}
		/*
		public void get_Teacher(string teacher)
		{
			currentUser = teacher;
		}
		private void get_Bookings(string teacher)
		{
			//connect to database
			sqlConnection.Open();

			//get bookings for teacher
			



			//display bookings in listbox
			sqlConnection.Close();
		} */
	}
}
