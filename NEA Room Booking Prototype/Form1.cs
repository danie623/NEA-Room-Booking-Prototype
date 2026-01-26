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
		private const String CONNECT = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"M:\\Visual Studio 2022\\projects\\New NEA\\NEA Room Booking\\NEA Room Booking\\resourses\\RoomBookingDatabase.mdf\";Integrated Security = True; Connect Timeout = 30;";

		//variable initialisation for use in multiple functions
		bool loggedIn = false;
		String currentUser = null;
		List<String> idOfRoomsList;
		List<String> transferBookings;
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
			String addedDays = null;
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
				SqlCommand command = new SqlCommand("SELECT Password FROM Teachers WHERE TeacherInitials=@initials", sqlConnection);

				//parameters
				SqlParameter paramUser = new SqlParameter();
				paramUser.ParameterName = "@initials";
				paramUser.Value = InitialsBox.Text.ToUpper();
				

				String PasswordInput = PasswordBox.Text;



				//add params to the query
				command.Parameters.Add(paramUser);

				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						if (PasswordInput == $"{reader["Password"]}")
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
					
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
				getBookedForTeachers();

            }
		}


		// Get chosen tags function

		private List<String> GetChosenTags()
		{
			List<String> tags = new List<String>();

			foreach (String item in TagsList.CheckedItems)
			{
				tags.Add(item);
			}
			return tags;
		}

		private void GetRooms_Click_1(object sender, EventArgs e)
		{
			if (sqlConnection.State != ConnectionState.Open)
			{
				sqlConnection.ConnectionString = CONNECT;
				sqlConnection.Open();
			}

			RoomsList.Items.Clear();
			idOfRoomsList = new List<String>();
			transferBookings = new List<String>();
			int countIndex = 0;

			DateTime dateParam = (DateBox.SelectedIndex == -1) ? DateTime.MaxValue : DateTime.Now.Date.AddDays(DateBox.SelectedIndex);
			int periodParam;
			if (PeriodSelect.SelectedIndex == -1 || !int.TryParse(Convert.ToString(PeriodSelect.SelectedItem), out periodParam))
				periodParam = 0;

			List<String> chosenTags = GetChosenTags();
			SqlCommand command;

			// Show only rooms NOT booked for date/period
			if (ShowAlreadyBookedRooms.CheckState != CheckState.Checked)
			{
				if (chosenTags.Count == 0)
				{
					// No tags: return rooms that do NOT have a booking for the specified date/period
					String sql = @"SELECT r.RoomID, r.Seats, r.Department FROM Rooms r WHERE NOT EXISTS (SELECT 1 FROM Bookings b WHERE b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor) ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
				}
				else
				{
					// With tags: rooms that have at least one chosen tag and are NOT booked for the date/period
					var tagParamNames = new List<String>();
					for (int i = 0; i < chosenTags.Count; i++)
					{
						tagParamNames.Add("@tag" + i);
					}
					String listOfTagsIncmd = String.Join(",", tagParamNames);
					String sql = $@"SELECT DISTINCT r.RoomID, r.Seats, r.Department FROM Rooms r INNER JOIN TagAssign ta ON ta.RoomID = r.RoomID INNER JOIN Tags t ON t.TagID = ta.TagID WHERE t.Tag IN ({listOfTagsIncmd}) AND NOT EXISTS ( SELECT 1 FROM Bookings b WHERE b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor) ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
					for (int i = 0; i < chosenTags.Count; i++)
					{
						command.Parameters.AddWithValue("@tag" + i, chosenTags[i]);
					}
				}

				command.Parameters.AddWithValue("@dateBookingFor", dateParam);
				command.Parameters.AddWithValue("@periodBookingFor", periodParam);

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
			else // Show already-booked (show all rooms and booking info for the selected date/period)
			{
				if (chosenTags.Count == 0)
				{
					String sql = @"SELECT r.RoomID, r.Seats, r.Department, r.Available, b.BookedFor FROM Rooms r LEFT JOIN Bookings b  ON b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", dateParam);
					command.Parameters.AddWithValue("@periodBookingFor", periodParam);
				}
				else
				{
					var tagParamNames = new List<String>();
					for (int i = 0; i < chosenTags.Count; i++)
					{
						tagParamNames.Add("@tag" + i);
					}
					String listOfTagsIncmd = String.Join(",", tagParamNames);
					String sql = $@"SELECT DISTINCT r.RoomID, r.Seats, r.Department, r.Available, b.BookedFor FROM Rooms r INNER JOIN TagAssign ta ON ta.RoomID = r.RoomID INNER JOIN Tags t ON t.TagID = ta.TagID LEFT JOIN Bookings b  ON b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor WHERE t.Tag IN ({listOfTagsIncmd}) ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
					for (int i = 0; i < chosenTags.Count; i++)
					{
						command.Parameters.AddWithValue("@tag" + i, chosenTags[i]);
					}
					command.Parameters.AddWithValue("@dateBookingFor", dateParam);
					command.Parameters.AddWithValue("@periodBookingFor", periodParam);
				}

				using (SqlDataReader Reader = command.ExecuteReader())
				{
					while (Reader.Read())
					{
						idOfRoomsList.Add($"{Reader["RoomID"]}");
						if (Reader["BookedFor"] != DBNull.Value)
						{
							transferBookings.Add($"{Reader["RoomID"]}");
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]} Currently Booked by: {Reader["BookedFor"]}");
						}
						else
						{
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\nCapacity: {Reader["Seats"]}\nDepartment:{Reader["Department"]}");
						}
						countIndex++;
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

		private void TransferRequests_Click(object sender, EventArgs e)
		{

		}


		#region show/hide buttons

		// Room selection changed
		private void RoomsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (RoomsList.SelectedIndex != -1)
			{
				if (transferBookings.Contains(idOfRoomsList[RoomsList.SelectedIndex]))
				{
					booking = false;
					Book_Room_Button.Text = "Request Transfer";
					ShowBookingbutton(true);
				}
				else
				{
					booking = true;
					Book_Room_Button.Text = "Book Room";
					ShowBookingbutton(true);
				}
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

