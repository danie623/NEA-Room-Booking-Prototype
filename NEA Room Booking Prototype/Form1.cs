using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace NEA_Room_Booking_Prototype
{
	public partial class BookingScreen : Form
	{
		// Database connection setup
		SqlConnection sqlConnection;
		private const string CONNECT = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"M:\\Visual Studio 2022\\projects\\New NEA\\NEA Room Booking\\NEA Room Booking\\resourses\\RoomBookingDatabase.mdf\";Integrated Security = True; Connect Timeout = 30;";

		//variable initialisation for use in multiple functions
		bool loggedIn = false;
		string currentUser = null;
		List<String> idOfRoomsList;
		List<string> transferBookings;
		bool booking = true;

		public BookingScreen()
		{
			InitializeComponent();
			sqlConnection = new SqlConnection(CONNECT);
			GetTags();
			Adddates();
			this.Enabled = true;
			GetRooms.PerformClick();

		}



		// Add dates to date selection box function
		private void Adddates()
		{
			DateTime today = DateTime.Now;
			string addedDays = null;
			for ( int i = 0; i <= 10;  i++)
			{
				if (!((today.AddDays(i)).DayOfWeek == DayOfWeek.Sunday || (today.AddDays(i)).DayOfWeek == DayOfWeek.Saturday))
				{
					addedDays = Convert.ToString(today.AddDays(i));
					DateBox.Items.Add($"{(today.AddDays(i)).DayOfWeek}, {addedDays.Substring(0,10)}");
				}
				
			}
		}

		// Get tags from database function
		private void GetTags()
		{
			if (sqlConnection.State != ConnectionState.Open)
			{
				sqlConnection.ConnectionString = CONNECT;
				sqlConnection.Open();
			}

			SqlCommand command = new SqlCommand("SELECT Tag FROM Tags ORDER BY Tag", sqlConnection);

			using (SqlDataReader Reader = command.ExecuteReader())
			{
				while (Reader.Read())
				{
					TagsList.Items.Add(Reader["Tag"], false);
				}
			}
			if (sqlConnection.State == ConnectionState.Open)
			{
				sqlConnection.Close();
			}
		}

		private void getBookedForTeachers()
		{
			if (currentUser != null)
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.ConnectionString = CONNECT;
					sqlConnection.Open();
				}
				teacherBookingFor.Items.Clear();
				teacherBookingFor.Items.Add("Myself");
				teacherBookingFor.SelectedIndex = 0;
				SqlCommand command = new SqlCommand("SELECT TeacherInitials FROM Teachers ORDER BY TeacherInitials", sqlConnection);
				using (SqlDataReader Reader = command.ExecuteReader())
				{
					while (Reader.Read())
					{
						if (!($"{Reader["TeacherInitials"]}".Equals(currentUser)))
						{
							teacherBookingFor.Items.Add($"{Reader["TeacherInitials"]}");
						}
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
			}
        }
		private void BookingScreen_Load(object sender, EventArgs e)
		{

		}

		// Login/Logout button
		private void Login_Click(object sender, EventArgs e)
		{

			if (loggedIn)
			{
				Login.Text = "Login";
				InitialsBox.Enabled = true;
				PasswordBox.Enabled = true;
				InitialsLabel.Enabled = true;
				PasswordLabel.Enabled = true;
				ViewBookings.Enabled = false;
				ViewBookings.Visible = false;
                Book_Room_Button.Enabled = false;
                Book_Room_Button.Visible = false;
                loggedIn = false;
				ShowBookingbutton(false);
				LogInStatus.Text = "You are not logged in. \nYou need to log in to book a room.";
			}

			else
			{
                Book_Room_Button.Enabled = false;
                Book_Room_Button.Visible = false;
                ViewBookings.Enabled = false;
                ViewBookings.Visible = false;

                //open connection
                if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.ConnectionString = CONNECT;
					sqlConnection.Open();
				}

				//setup query
				SqlCommand command = new SqlCommand("SELECT * FROM Teachers WHERE TeacherInitials=@initials AND Password=@pass", sqlConnection);

				//parameters
				SqlParameter paramUser = new SqlParameter();
				paramUser.ParameterName = "@initials";
				paramUser.Value = InitialsBox.Text.ToUpper();
				SqlParameter paramPass = new SqlParameter();
				paramPass.ParameterName = "@pass";
				paramPass.Value = PasswordBox.Text;



				//add params to the query
				command.Parameters.Add(paramUser);
				command.Parameters.Add(paramPass);

				using (SqlDataReader reader = command.ExecuteReader())
				{
					if (reader.Read())
					{
						currentUser = InitialsBox.Text;
						InitialsBox.Text = "";
						PasswordBox.Text = "";
						loggedIn = true;
						Login.Text = "Logout";
						InitialsBox.Enabled = false;
						PasswordBox.Enabled = false;
						InitialsLabel.Enabled = false;
						PasswordLabel.Enabled = false;
						ViewBookings.Enabled = true;
						ViewBookings.Visible = true;
                        LogInStatus.Text = "Welcome " + currentUser.ToUpper();
						RoomsList_SelectedIndexChanged(sender, EventArgs.Empty);
                    }
					else
					{
						loggedIn = false;
						currentUser = null;
						LogInStatus.Text = "Incorrect initials or password!";
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
				getBookedForTeachers();

            }
		}


		// Get chosen tags function

		private List<string> GetChosenTags()
		{
			List<string> tags = new List<string>();

			foreach (string item in TagsList.CheckedItems)
			{
				tags.Add(item);
			}
			return tags;
		}


		// Get rooms button
		private void GetRooms_Click_1(object sender, EventArgs e)
		{
			if (sqlConnection.State != ConnectionState.Open)
			{
				sqlConnection.ConnectionString = CONNECT;
				sqlConnection.Open();
			}

			RoomsList.Items.Clear();
			idOfRoomsList = new List<string> ();
			transferBookings = new List<string>();
			int countIndex = 0;
			SqlCommand command;

			if (ShowAlreadyBookedRooms.CheckState != CheckState.Checked)
			{
				if (GetChosenTags().Count == 0)
				{
					command = new SqlCommand("SELECT r.RoomID, r.Seats, r.Department FROM Rooms r, bookings b where r.RoomID = b.RoomID AND (b.DateOfBooking != @dateBookingFor AND b.BookedPeriod != @periodBookingFor);", sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", (DateBox.SelectedIndex == -1 ? (DateTime.MaxValue) : DateTime.Now.Date.AddDays(DateBox.SelectedIndex)));
					command.Parameters.AddWithValue("@periodBookingFor", (PeriodSelect.SelectedIndex == -1 ? 0 : PeriodSelect.SelectedItem));

					using (SqlDataReader Reader = command.ExecuteReader())
					{
						while (Reader.Read())
						{
							idOfRoomsList.Add($"{Reader["RoomID"]}");
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\n Capacity: {Reader["Seats"]}\n Department:{Reader["Department"]}");
							countIndex++;
						}
					}
				}
				else if (GetChosenTags().Count != 0)
				{
					command = new SqlCommand($"SELECT * FROM Rooms r, Tags t, TagAssign ta, Bookings b WHERE (r.RoomID = b.RoomID AND (b.DateOfBooking != @dateBookingFor AND b.BookedPeriod != @periodBookingFor)) AND r.RoomID = ta.RoomID AND t.TagID = ta.TagID AND t.Tag IN ('{String.Join("','", GetChosenTags().ToArray())}') --ORDER BY RoomID", sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", (DateBox.SelectedIndex == -1 ? (DateTime.MaxValue) : DateTime.Now.Date.AddDays(DateBox.SelectedIndex)));
					command.Parameters.AddWithValue("@periodBookingFor", (PeriodSelect.SelectedIndex == -1 ? 0 : PeriodSelect.SelectedItem));

					using (SqlDataReader Reader = command.ExecuteReader())
					{
						while (Reader.Read())
						{
							idOfRoomsList.Add($"{Reader["RoomID"]}");
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\n Capacity: {Reader["Seats"]}\n Department:{Reader["Department"]}");
							countIndex++;
						}
					}
				}
			}
			else
			{
				
				if (GetChosenTags().Count == 0)
				{
					command = new SqlCommand("SELECT r.RoomID, r.Seats, r.Department, r.Available, b.TeacherInitials FROM Rooms r, Tags t, TagAssign ta LEFT JOIN Bookings b ON b.RoomID = r.RoomID AND b.DateOfBooking =  @dateBookingFor  AND b.BookedPeriod = @periodBookingFor;", sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", (DateBox.SelectedIndex == -1 ? (DateTime.MaxValue) : DateTime.Now.Date.AddDays(DateBox.SelectedIndex)));
					command.Parameters.AddWithValue("@periodBookingFor", (PeriodSelect.SelectedIndex == -1 ? 0 : PeriodSelect.SelectedItem));

					using (SqlDataReader Reader = command.ExecuteReader())
					{
						while (Reader.Read())
						{
							idOfRoomsList.Add($"{Reader["RoomID"]}");
							if ($"{Reader["TeacherInitials"]}" != "Null")
							{
								transferBookings.Add($"{Reader["RoomID"]}");
								RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]} Currently Booked by: {Reader["TeacherInitials"]}");
							}
							else
							{
								RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]}");
							}
							countIndex++;
						}
					}
				}
				else if (GetChosenTags().Count != 0)
				{
					command = new SqlCommand($"SELECT r.RoomID, r.Seats, r.Department, r.Available, b.TeacherInitials FROM Rooms r, Tags t, TagAssign ta LEFT JOIN Bookings b ON b.RoomID = r.RoomID    AND b.DateOfBooking =  @dateBookingFor  AND b.BookedPeriod = @periodBookingFor WHERE (r.RoomID = b.RoomID AND (b.DateOfBooking != @dateBookingFor AND b.BookedPeriod != @periodBookingFor)) AND r.RoomID = ta.RoomID AND t.TagID = ta.TagID AND t.Tag IN ('{String.Join("','", GetChosenTags().ToArray())}') --ORDER BY RoomID", sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", (DateBox.SelectedIndex == -1 ? (DateTime.MaxValue) : DateTime.Now.Date.AddDays(DateBox.SelectedIndex)));
					command.Parameters.AddWithValue("@periodBookingFor", (PeriodSelect.SelectedIndex == -1 ? 0 : PeriodSelect.SelectedItem));

					using (SqlDataReader Reader = command.ExecuteReader())
					{
						while (Reader.Read())
						{
							idOfRoomsList.Add($"{Reader["RoomID"]}");
							if ($"{Reader["TeacherInitials"]}" != "Null")
							{
								transferBookings.Add($"{Reader["RoomID"]}");
								RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]} Currently Booked by: {Reader["TeacherInitials"]}"); 
							}
							else
							{
								RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]}");
							}
							countIndex++;
						}
					}
				}
			}



			if (sqlConnection.State == ConnectionState.Open)
			{
				sqlConnection.Close();
			}
		}





		// Book room button
		private void Book_Room_Button_Click(object sender, EventArgs e)
		{
			Booking_Confirm popup = new Booking_Confirm();

			String selectedRoom = idOfRoomsList[RoomsList.SelectedIndex];
			int selectedPeriod = int.Parse($"{PeriodSelect.SelectedItem}");
			DateTime selectedDate =  DateTime.Now.Date.AddDays(DateBox.SelectedIndex);
			String teacherBoooking = currentUser;
			String teacherBookedFor = ((teacherBookingFor.SelectedIndex != 0) ? $"{teacherBookingFor.SelectedItem}" : currentUser);
      
            popup.showMessage(selectedRoom, selectedPeriod, selectedDate, teacherBookedFor);
			


			if (popup.ShowDialog() == DialogResult.OK)
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.ConnectionString = CONNECT;
					sqlConnection.Open();
				}

				

                SqlCommand command = new SqlCommand("INSERT INTO Bookings (BookingID, TeacherInitials, RoomID, DateOfBooking, BookedPeriod, BookedFor) VALUES ( (SELECT ISNULL(MAX(BookingID) + 1, 0) FROM Bookings) , @initials, @RoomID, @Date , @period, @bookedFor)");

				command.Parameters.AddWithValue("@initials", teacherBoooking);
				command.Parameters.AddWithValue("@RoomID", selectedRoom);
				command.Parameters.AddWithValue("@Date", selectedDate.Date);
				command.Parameters.AddWithValue("@period", selectedPeriod);
				command.Parameters.AddWithValue("@bookedFor", teacherBookedFor);


                using (var connection1 = sqlConnection)
				using (var cmd = new SqlDataAdapter())
				using (command)
				{
					command.Connection = connection1;
					cmd.InsertCommand = command;


					int rowsAffected = command.ExecuteNonQuery();

					if (rowsAffected > 0)
					{
						MessageBox.Show("Booking saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
					}
					else
					{
						MessageBox.Show("Booking was not saved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
			}
		}


		
		private void ViewBookings_Click(object sender, EventArgs e)
		{
			View_Bookings popup = new View_Bookings();
			popup.Get_Teacher(currentUser);
			popup.Show();
		}


		#region show/hide buttons

		// Room selection changed
		private void RoomsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (RoomsList.SelectedIndex != -1)
			{
				ShowBookingbutton(true);
			}
			else
			{
				ShowBookingbutton(false);
			}
		}

		// Show or hide book room button function
		private void ShowBookingbutton(bool show)
		{
			if (loggedIn && PeriodSelect.SelectedIndex != -1 && DateBox.SelectedIndex != -1)
			{
				Book_Room_Button.Enabled = show;
				Book_Room_Button.Visible = show;
				BookingForLabel.Enabled = show;
				BookingForLabel.Visible = show;
				teacherBookingFor.Enabled = show;
				teacherBookingFor.Visible = show;
			}
			else
			{
				Book_Room_Button.Enabled = false;
				Book_Room_Button.Visible = false;
				BookingForLabel.Enabled = false;
				BookingForLabel.Visible = false;
				teacherBookingFor.Enabled = false;
				teacherBookingFor.Visible = false;
			}
		}
		#endregion

		// refreshes etc.
		#region key and button pushes
		private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { Login.PerformClick(); }
		}

		private void InitialsBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { Login.PerformClick(); }
		}

        private void PeriodSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
			GetRooms.PerformClick();
        }
		private void ShowAlreadyBookedRooms_CheckedChanged(object sender, EventArgs e)
		{
			GetRooms.PerformClick();
		}

		private void DateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRooms.PerformClick();
        }
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetRooms.PerformClick();
        }
		#endregion
	}
}

