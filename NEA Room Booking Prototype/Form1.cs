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
		
		//college connection string
		//private const String CONNECT = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"M:\\Visual Studio 2022\\projects\\New NEA\\NEA Room Booking\\NEA Room Booking\\resourses\\RoomBookingDatabase.mdf\";Integrated Security = True; Connect Timeout = 30;";

		//home connection string
		private const String CONNECT = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=\"C:\\Users\\danie\\source\\repos\\danie623\\NEA-Room-Booking-Prototype\\NEA Room Booking Prototype\\Database\\RoomBookingDatabase.mdf\";Integrated Security=True;Connect Timeout=30";

		//variable initialisation for use in multiple functions
		bool loggedIn = false;
		static String currentUser = null;
		List<String> idOfRoomsList;
		List<String> transferBookings;
		Dictionary<string, int> bookingIDs;
		bool booking = true;
		List<DateTime> listOfDates;

		public BookingScreen()
		{
			InitializeComponent();
			//initialize connection to database
			sqlConnection = new SqlConnection(CONNECT);
			//gets tags and dates and inserts into appropriate selection boxes
			GetTags();
			getDates();
			this.Enabled = true;
			//adds rooms to the listbox on opening form
			GetRooms.PerformClick();

		}

		// Get connection string function (for use in other forms)
		public static string getconnectionstring()
		{
			return CONNECT;
		}

		// Get current user function (for use in other forms)
		public static String getcurrentuser()
		{
			return currentUser;
		}

		// Add dates to date selection box function
		private void getDates()
		{
			listOfDates = new List<DateTime>();
			DateTime today = DateTime.Now;
			DateTime addedDays;
			for (int i = 0; i <= 10; i++)
			{
				if (!((today.AddDays(i)).DayOfWeek == DayOfWeek.Sunday || (today.AddDays(i)).DayOfWeek == DayOfWeek.Saturday))
				{
					addedDays = (today.AddDays(i));
					listOfDates.Add(addedDays);
				}

			}
			//adds dates to the date selection box
			AddDates();
		}


		// Add dates to date selection box function
		private void AddDates()
		{
			foreach (DateTime date in listOfDates)
			{
				DateBox.Items.Add($"{date.DayOfWeek} {date.Date.ToString("dd/MM/yyyy")}");
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


		// Get teachers and adds to 'booking for' selection box
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
			//if already logged in, logs out & hides buttons
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
				TransferRequests.Enabled = false;
				TransferRequests.Visible = false;
				loggedIn = false;
				ShowBookingbutton(false);
				LogInStatus.Text = "You are not logged in. \nYou need to log in to book a room.";
			}

			else //if not logged in, checks credentials and if correct, logs in & shows buttons
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
							TransferRequests.Enabled = true;
							TransferRequests.Visible = true;
							LogInStatus.Text = "Welcome " + currentUser.ToUpper();
							RoomsList_SelectedIndexChanged(sender, EventArgs.Empty);
						}
						else
						{
							loggedIn = false;
							currentUser = null;
							PasswordBox.Text = "";
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


		// Get chosen tags for use in SQL query
		private List<String> GetChosenTags()
		{
			List<String> tags = new List<String>();

			foreach (String item in TagsList.CheckedItems)
			{
				tags.Add(item);
			}
			return tags;
		}


		// Get rooms button - shows rooms based on tags and whether to show already booked rooms -> shows booking info if showing already booked rooms
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
			bookingIDs = new Dictionary<string, int>();
			int countIndex = 0;

			DateTime dateParam = (DateBox.SelectedIndex == -1) ? DateTime.MaxValue : listOfDates[DateBox.SelectedIndex].Date;
			int periodParam;
			if (PeriodSelect.SelectedIndex == -1 || !int.TryParse(Convert.ToString(PeriodSelect.SelectedItem), out periodParam))
				periodParam = 0;

			List<String> chosenTags = GetChosenTags();
			SqlCommand command;

			// Show only rooms NOT booked for date/period
			if (ShowAlreadyBookedRooms.CheckState != CheckState.Checked)
			{
				if (chosenTags.Count == 0) // If no tags are chosen, return all rooms that are not booked for the specified date/period
				{
					// No tags: return rooms that do NOT have a booking for the specified date/period
					String sql = @"SELECT r.RoomID, r.Seats, r.Department FROM Rooms r WHERE NOT EXISTS (SELECT 1 FROM Bookings b WHERE b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor) ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
				}
				else // If tags are chosen, return rooms that have at least one of the chosen tags and do NOT have a booking for the specified date/period
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

				using (SqlDataReader Reader = command.ExecuteReader()) // execute reader and add rooms to listbox (only rooms that are not booked for the date/period, so no booking info needed)
				{
					while (Reader.Read())
					{
						idOfRoomsList.Add($"{Reader["RoomID"]}");
						RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\n Capacity: {Reader["Seats"]}\n Department: {Reader["Department"]}");
						countIndex++;
					}
				}
			}
			else // Show already-booked (show all rooms and booking info for the selected date/period)
			{
				if (chosenTags.Count == 0) // No tags: return all rooms with booking info for the specified date/period (if a room is not booked for the date/period, booking info will be null)
				{
					String sql = @"SELECT r.RoomID, r.Seats, r.Department, r.Available, b.BookedFor, b.BookingID FROM Rooms r LEFT JOIN Bookings b  ON b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
					command.Parameters.AddWithValue("@dateBookingFor", dateParam);
					command.Parameters.AddWithValue("@periodBookingFor", periodParam);
				}
				else // With tags: return rooms that have at least one chosen tag with booking info for the date/period (if a room is not booked for the date/period, booking info will be null)
				{
					var tagParamNames = new List<String>();
					for (int i = 0; i < chosenTags.Count; i++)
					{
						tagParamNames.Add("@tag" + i);
					}
					String listOfTagsIncmd = String.Join(",", tagParamNames);
					String sql = $@"SELECT DISTINCT r.RoomID, r.Seats, r.Department, r.Available, b.BookedFor, b.BookingID FROM Rooms r INNER JOIN TagAssign ta ON ta.RoomID = r.RoomID INNER JOIN Tags t ON t.TagID = ta.TagID LEFT JOIN Bookings b  ON b.RoomID = r.RoomID AND b.DateOfBooking = @dateBookingFor AND b.BookedPeriod = @periodBookingFor WHERE t.Tag IN ({listOfTagsIncmd}) ORDER BY r.RoomID;";
					command = new SqlCommand(sql, sqlConnection);
					for (int i = 0; i < chosenTags.Count; i++)
					{
						command.Parameters.AddWithValue("@tag" + i, chosenTags[i]);
					}
					command.Parameters.AddWithValue("@dateBookingFor", dateParam);
					command.Parameters.AddWithValue("@periodBookingFor", periodParam);
				}

				using (SqlDataReader Reader = command.ExecuteReader()) // execute reader and add rooms to listbox with booking info (if a room is not booked for the date/period, booking info will be null so just show room info)
				{
					while (Reader.Read())
					{
						idOfRoomsList.Add($"{Reader["RoomID"]}");
						bookingIDs.Add($"{Reader["RoomID"]}", (Reader["BookingID"] != DBNull.Value) ? int.Parse($"{Reader["BookingID"]}") : -1);
						if (Reader["BookedFor"] != DBNull.Value)
						{
							transferBookings.Add($"{Reader["RoomID"]}");
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\n Capacity: {Reader["Seats"]}\n Department: {Reader["Department"]} Currently Booked by: {Reader["BookedFor"]}");
						}
						else
						{
							RoomsList.Items.Add($"{idOfRoomsList[countIndex]}\n Capacity: {Reader["Seats"]}\n Department: {Reader["Department"]}");
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
			if (!booking) // if booking is false, it means the button is in "Request Transfer" mode, so make transfer request instead of booking the room
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.ConnectionString = CONNECT;
					sqlConnection.Open();
				}

				if (bookingIDs.ContainsKey(idOfRoomsList[RoomsList.SelectedIndex])) // check if the selected room has a booking ID (it should if it's in the transferBookings list, but just to be safe before trying to access it)
				{
					int existingBookingID = bookingIDs[idOfRoomsList[RoomsList.SelectedIndex]];


					if (existingBookingID < 0) // if there is no valid booking ID for the selected room (shouldn't happen for rooms in the transferBookings list), show error message
					{
						MessageBox.Show("Error. This booking does not exist.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						return;
					}
					else 
					{
						bool alreadyRequested = false;


						SqlCommand command = new SqlCommand("SELECT * FROM TransferRequests WHERE BookingID = @idOfBooking AND MadeRequest = @user", sqlConnection);
						command.Parameters.AddWithValue("@idOfBooking", existingBookingID);
						command.Parameters.AddWithValue("@user", currentUser);

						using (SqlDataReader reader = command.ExecuteReader()) // check if the user has already made a transfer request for this booking
						{
							if (reader.Read())
							{
								alreadyRequested = true;
								MessageBox.Show("You have already made a transfer request for this booking.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
							}
						}

						if (!alreadyRequested) // if the user has not already made a transfer request for this booking, check if the booking is already booked for them or made by them before allowing to make transfer request
						{
							bool alreadyBookedForYou = false;

							command = new SqlCommand("SELECT * FROM Bookings WHERE BookingID = @idOfBooking AND BookedFor = @user", sqlConnection);
							command.Parameters.AddWithValue("@idOfBooking", existingBookingID);
							command.Parameters.AddWithValue("@user", currentUser);

							using (SqlDataReader reader = command.ExecuteReader())
							{
								if (reader.Read())
								{
									alreadyBookedForYou = true;
									MessageBox.Show("This booking is already yours", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								}
							}

							if (!alreadyBookedForYou) // if the booking is not already booked for the user, check if the user made the booking before allowing to make transfer request (users should not be able to make transfer requests for their own bookings, they should just transfer it back to themselves in the My Bookings menu if they want to change it)
							{
								bool alreadyBookedByYou = false;

								command = new SqlCommand("SELECT * FROM Bookings WHERE BookingID = @idOfBooking AND TeacherInitials = @user", sqlConnection);
								command.Parameters.AddWithValue("@idOfBooking", existingBookingID);
								command.Parameters.AddWithValue("@user", currentUser);

								using (SqlDataReader reader = command.ExecuteReader())
								{
									if (reader.Read())
									{
										alreadyBookedByYou = true;
										MessageBox.Show("You made this booking. Transfer it back to yourself in the MyBookings menu.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									}
								}

								if (!alreadyBookedByYou) // if the user did not make the booking, make a transfer request for it
								{
									command = new SqlCommand("INSERT INTO TransferRequests (BookingID, MadeRequest) VALUES ( @idOfBooking, @madeBy );", sqlConnection);
									command.Parameters.AddWithValue("@idOfBooking", existingBookingID);
									command.Parameters.AddWithValue("@madeBy", currentUser);

									int rowsAffected = command.ExecuteNonQuery();

									if (rowsAffected > 0) // if the insert was successful, show success message, otherwise show error message
									{
										MessageBox.Show("Transfer request made.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
									}
									else
									{
										MessageBox.Show("Error. Transfer request was not made.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
									}
								}
							}
						}
					}
				}

				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}
			}
			else // if booking is true, it means the button is in "Book Room" mode, so proceed with normal booking process
			{
				if (sqlConnection.State != ConnectionState.Open)
				{
					sqlConnection.ConnectionString = CONNECT;
					sqlConnection.Open();
				}

				// show booking confirmation popup and if user confirms, insert booking into database with the selected room/date/period and current user as the teacher who made the booking (and also the teacher booked for if "Myself" is selected in the "Booking for" selection box, otherwise use the selected teacher in the "Booking for" selection box as the teacher booked for)
				Booking_Confirm popup = new Booking_Confirm();

				String selectedRoom = idOfRoomsList[RoomsList.SelectedIndex];
				int selectedPeriod = int.Parse($"{PeriodSelect.SelectedItem}");
				DateTime selectedDate = listOfDates[DateBox.SelectedIndex];
				String teacherBoooking = currentUser;
				String teacherBookedFor = ((teacherBookingFor.SelectedIndex != 0) ? $"{teacherBookingFor.SelectedItem}" : currentUser);
      
				popup.showMessage(selectedRoom, selectedPeriod, selectedDate, teacherBookedFor);
			


				if (popup.ShowDialog() == DialogResult.OK) // if the user confirms the booking in the popup, insert the booking into the database
				{ 	
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

						if (rowsAffected > 0) // if the insert was successful, show success message, otherwise show error message
						{
							MessageBox.Show("Booking saved.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
						}
						else
						{
							MessageBox.Show("Booking was not saved.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						}
					}
				}
				if (sqlConnection.State == ConnectionState.Open)
				{
					sqlConnection.Close();
				}

				// refresh rooms list to show the newly booked room as booked
				GetRooms.PerformClick();
			}
		}


		// View Bookings button - opens a new form to show the user's bookings
		private void ViewBookings_Click(object sender, EventArgs e)
		{
			View_Bookings popup = new View_Bookings();
			popup.Get_Teacher(currentUser);
			if (popup.ShowDialog() == DialogResult.Cancel)
			{
				GetRooms.PerformClick();
			}
		}

		// View Transfer Requests button - opens a new form to show the user's transfer requests
		private void TransferRequests_Click(object sender, EventArgs e)
		{
			ViewTransferRequests popup = new ViewTransferRequests();
			if (popup.ShowDialog() == DialogResult.Cancel)
			{
				GetRooms.PerformClick();
			}

		}


		#region show/hide buttons

		// when a room is selected in the rooms list, check if it's already booked for the selected date/period (by checking if it's in the transferBookings list which is populated when showing already booked rooms) and if it is, change the book room button to a request transfer button, otherwise show the normal book room button. If no room is selected, hide the book room button.
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

		// refreshes
		// QOL features such as allowing to press enter to log in and automatically refreshing the rooms list when changing the date/period/tags selection so the user doesn't have to manually click the Get Rooms button every time they change a selection criteria
		#region key and button pushes
		private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter) { Login.PerformClick(); }
		}

		private void tagslist_SelectedIndexChanged(object sender, EventArgs e)
		{
			GetRooms.PerformClick();
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

