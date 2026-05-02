using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace UserAppWinForms
{
    public partial class Form1 : Form
    {
        static string connectionString = "Data Source=users.db;Version=3;";

        public Form1()
        {
            InitializeComponent();
            CreateTable();
            ShowAll();
        }

        void CreateTable()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "CREATE TABLE IF NOT EXISTS Users (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Username TEXT," +
                "Email TEXT," +
                "BirthDate TEXT)", con);

            cmd.ExecuteNonQuery();
            con.Close();
        }
        
        void ShowAll()
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Users", con);
            SQLiteDataReader reader = cmd.ExecuteReader();

            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add("Id", "ID");
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("BirthDate", "Дата рождения");

            while (reader.Read())
            {
                dgvUsers.Rows.Add(reader["Id"], reader["Username"], reader["Email"], reader["BirthDate"]);
            }

            reader.Close();
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text == "" || txtEmail.Text == "" || txtBirthDate.Text == "")
            {
                MessageBox.Show("не все поля заполнены");
                return;
            }

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "INSERT INTO Users (Username, Email, BirthDate) VALUES (@u, @e, @b)", con);

            cmd.Parameters.AddWithValue("@u", txtUsername.Text);
            cmd.Parameters.AddWithValue("@e", txtEmail.Text);
            cmd.Parameters.AddWithValue("@b", txtBirthDate.Text);

            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("добавлен");
            txtUsername.Text = "";
            txtEmail.Text = "";
            txtBirthDate.Text = "";
            ShowAll();
        }
        
        private void btnShowAll_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        private void btnSearchUsername_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "SELECT * FROM Users WHERE Username = @u", con);

            cmd.Parameters.AddWithValue("@u", txtUsername.Text);
            SQLiteDataReader reader = cmd.ExecuteReader();

            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add("Id", "ID");
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("BirthDate", "Дата рождения");

            bool found = false;

            while (reader.Read())
            {
                found = true;
                dgvUsers.Rows.Add(reader["Id"], reader["Username"], reader["Email"], reader["BirthDate"]);
            }

            if (!found) MessageBox.Show("не найден");

            reader.Close();
            con.Close();
        }

        private void btnSearchEmail_Click(object sender, EventArgs e)
        {
            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "SELECT * FROM Users WHERE Email = @e", con);

            cmd.Parameters.AddWithValue("@e", txtEmail.Text);
            SQLiteDataReader reader = cmd.ExecuteReader();

            dgvUsers.Rows.Clear();
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add("Id", "ID");
            dgvUsers.Columns.Add("Username", "Username");
            dgvUsers.Columns.Add("Email", "Email");
            dgvUsers.Columns.Add("BirthDate", "Дата рождения");

            bool found = false;

            while (reader.Read())
            {
                found = true;
                dgvUsers.Rows.Add(reader["Id"], reader["Username"], reader["Email"], reader["BirthDate"]);
            }

            if (!found) MessageBox.Show("не найден");

            reader.Close();
            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("необходимо выбрать пользователя из таблицы");
                return;
            }

            string id = dgvUsers.SelectedRows[0].Cells["Id"].Value.ToString();

            SQLiteConnection con = new SQLiteConnection(connectionString);
            con.Open();

            SQLiteCommand cmd = new SQLiteCommand(
                "DELETE FROM Users WHERE Id = @id", con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Пользователь удалён!");
            ShowAll();
        }
    }
}
