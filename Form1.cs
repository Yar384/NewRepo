using System;
using System.Windows.Forms;
using Dapper;
using Microsoft.Data.Sqlite;

namespace LibraryWinForms
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
    }

    public class Form1 : Form
    {
        static string connectionString = "Data Source=library.db";

        TextBox txtTitle;
        TextBox txtAuthor;
        TextBox txtSearch;
        TextBox txtUpdateId;
        TextBox txtUpdateTitle;
        TextBox txtUpdateAuthor;
        DataGridView dgvBooks;
        Button btnAdd;
        Button btnShowAll;
        Button btnSearchTitle;
        Button btnSearchAuthor;
        Button btnSearchId;
        Button btnDelete;
        Button btnUpdate;
        Label lblTitle;
        Label lblAuthor;
        Label lblSearch;
        Label lblUpdateId;
        Label lblUpdateTitle;
        Label lblUpdateAuthor;

        public Form1()
        {
            InitDB();
            BuildUI();
            ShowAll();
        }

        void InitDB()
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            con.Execute(
                "CREATE TABLE IF NOT EXISTS Books (" +
                "Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Title TEXT," +
                "Author TEXT)"
            );

            con.Close();
        }

        void BuildUI()
        {
            this.Text = "Бібліотека";
            this.Width = 800;
            this.Height = 600;

            lblTitle = new Label();
            lblTitle.Text = "Назва:";
            lblTitle.Left = 10;
            lblTitle.Top = 10;
            lblTitle.Width = 60;

            txtTitle = new TextBox();
            txtTitle.Left = 80;
            txtTitle.Top = 10;
            txtTitle.Width = 150;

            lblAuthor = new Label();
            lblAuthor.Text = "Автор:";
            lblAuthor.Left = 10;
            lblAuthor.Top = 40;
            lblAuthor.Width = 60;

            txtAuthor = new TextBox();
            txtAuthor.Left = 80;
            txtAuthor.Top = 40;
            txtAuthor.Width = 150;

            btnAdd = new Button();
            btnAdd.Text = "Додати";
            btnAdd.Left = 240;
            btnAdd.Top = 10;
            btnAdd.Width = 80;
            btnAdd.Click += BtnAdd_Click;

            btnShowAll = new Button();
            btnShowAll.Text = "Всі книги";
            btnShowAll.Left = 240;
            btnShowAll.Top = 40;
            btnShowAll.Width = 80;
            btnShowAll.Click += BtnShowAll_Click;

            lblSearch = new Label();
            lblSearch.Text = "Пошук:";
            lblSearch.Left = 340;
            lblSearch.Top = 10;
            lblSearch.Width = 60;

            txtSearch = new TextBox();
            txtSearch.Left = 410;
            txtSearch.Top = 10;
            txtSearch.Width = 150;

            btnSearchTitle = new Button();
            btnSearchTitle.Text = "За назвою";
            btnSearchTitle.Left = 570;
            btnSearchTitle.Top = 10;
            btnSearchTitle.Width = 90;
            btnSearchTitle.Click += BtnSearchTitle_Click;

            btnSearchAuthor = new Button();
            btnSearchAuthor.Text = "За автором";
            btnSearchAuthor.Left = 570;
            btnSearchAuthor.Top = 40;
            btnSearchAuthor.Width = 90;
            btnSearchAuthor.Click += BtnSearchAuthor_Click;

            btnSearchId = new Button();
            btnSearchId.Text = "За ID";
            btnSearchId.Left = 670;
            btnSearchId.Top = 10;
            btnSearchId.Width = 70;
            btnSearchId.Click += BtnSearchId_Click;

            lblUpdateId = new Label();
            lblUpdateId.Text = "ID:";
            lblUpdateId.Left = 10;
            lblUpdateId.Top = 80;
            lblUpdateId.Width = 30;

            txtUpdateId = new TextBox();
            txtUpdateId.Left = 50;
            txtUpdateId.Top = 80;
            txtUpdateId.Width = 50;

            lblUpdateTitle = new Label();
            lblUpdateTitle.Text = "Нова назва:";
            lblUpdateTitle.Left = 110;
            lblUpdateTitle.Top = 80;
            lblUpdateTitle.Width = 80;

            txtUpdateTitle = new TextBox();
            txtUpdateTitle.Left = 200;
            txtUpdateTitle.Top = 80;
            txtUpdateTitle.Width = 120;

            lblUpdateAuthor = new Label();
            lblUpdateAuthor.Text = "Новий автор:";
            lblUpdateAuthor.Left = 330;
            lblUpdateAuthor.Top = 80;
            lblUpdateAuthor.Width = 85;

            txtUpdateAuthor = new TextBox();
            txtUpdateAuthor.Left = 425;
            txtUpdateAuthor.Top = 80;
            txtUpdateAuthor.Width = 120;

            btnUpdate = new Button();
            btnUpdate.Text = "Оновити";
            btnUpdate.Left = 555;
            btnUpdate.Top = 80;
            btnUpdate.Width = 80;
            btnUpdate.Click += BtnUpdate_Click;

            btnDelete = new Button();
            btnDelete.Text = "Видалити";
            btnDelete.Left = 645;
            btnDelete.Top = 80;
            btnDelete.Width = 80;
            btnDelete.Click += BtnDelete_Click;

            dgvBooks = new DataGridView();
            dgvBooks.Left = 10;
            dgvBooks.Top = 120;
            dgvBooks.Width = 760;
            dgvBooks.Height = 420;
            dgvBooks.ReadOnly = true;
            dgvBooks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            this.Controls.Add(lblTitle);
            this.Controls.Add(txtTitle);
            this.Controls.Add(lblAuthor);
            this.Controls.Add(txtAuthor);
            this.Controls.Add(btnAdd);
            this.Controls.Add(btnShowAll);
            this.Controls.Add(lblSearch);
            this.Controls.Add(txtSearch);
            this.Controls.Add(btnSearchTitle);
            this.Controls.Add(btnSearchAuthor);
            this.Controls.Add(btnSearchId);
            this.Controls.Add(lblUpdateId);
            this.Controls.Add(txtUpdateId);
            this.Controls.Add(lblUpdateTitle);
            this.Controls.Add(txtUpdateTitle);
            this.Controls.Add(lblUpdateAuthor);
            this.Controls.Add(txtUpdateAuthor);
            this.Controls.Add(btnUpdate);
            this.Controls.Add(btnDelete);
            this.Controls.Add(dgvBooks);
        }

        void ShowAll()
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            var books = con.Query<Book>("SELECT * FROM Books").AsList();
            con.Close();

            FillGrid(books);
        }

        void FillGrid(System.Collections.Generic.List<Book> books)
        {
            dgvBooks.Rows.Clear();
            dgvBooks.Columns.Clear();

            dgvBooks.Columns.Add("Id", "ID");
            dgvBooks.Columns.Add("Title", "Назва");
            dgvBooks.Columns.Add("Author", "Автор");

            foreach (var b in books)
            {
                dgvBooks.Rows.Add(b.Id, b.Title, b.Author);
            }
        }

        void BtnAdd_Click(object sender, EventArgs e)
        {
            if (txtTitle.Text == "" || txtAuthor.Text == "")
            {
                MessageBox.Show("потрібно заповнити усі поля");
                return;
            }

            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            con.Execute(
                "INSERT INTO Books (Title, Author) VALUES (@Title, @Author)",
                new { Title = txtTitle.Text, Author = txtAuthor.Text }
            );

            con.Close();

            txtTitle.Text = "";
            txtAuthor.Text = "";
            MessageBox.Show("додано");
            ShowAll();
        }

        void BtnShowAll_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        void BtnSearchTitle_Click(object sender, EventArgs e)
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            var books = con.Query<Book>(
                "SELECT * FROM Books WHERE Title = @Title",
                new { Title = txtSearch.Text }
            ).AsList();

            con.Close();
            FillGrid(books);
        }

        void BtnSearchAuthor_Click(object sender, EventArgs e)
        {
            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            var books = con.Query<Book>(
                "SELECT * FROM Books WHERE Author = @Author",
                new { Author = txtSearch.Text }
            ).AsList();

            con.Close();
            FillGrid(books);
        }

        void BtnSearchId_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtSearch.Text);

            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            var books = con.Query<Book>(
                "SELECT * FROM Books WHERE Id = @Id",
                new { Id = id }
            ).AsList();

            con.Close();
            FillGrid(books);
        }

        void BtnDelete_Click(object sender, EventArgs e)
        {
            if (txtUpdateId.Text == "")
            {
                MessageBox.Show("потрібен ID");
                return;
            }

            int id = int.Parse(txtUpdateId.Text);

            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            int rows = con.Execute(
                "DELETE FROM Books WHERE Id = @Id",
                new { Id = id }
            );

            con.Close();

            if (rows == 0) MessageBox.Show("не знайдено");
            else MessageBox.Show("видалено");

            ShowAll();
        }

        void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUpdateId.Text == "")
            {
                MessageBox.Show("потрібен ID");
                return;
            }

            int id = int.Parse(txtUpdateId.Text);

            SqliteConnection con = new SqliteConnection(connectionString);
            con.Open();

            var book = con.QueryFirstOrDefault<Book>(
                "SELECT * FROM Books WHERE Id = @Id",
                new { Id = id }
            );

            con.Close();

            if (book == null)
            {
                MessageBox.Show("не знайдено");
                return;
            }

            if (txtUpdateTitle.Text != "") book.Title = txtUpdateTitle.Text;
            if (txtUpdateAuthor.Text != "") book.Author = txtUpdateAuthor.Text;

            con = new SqliteConnection(connectionString);
            con.Open();

            con.Execute(
                "UPDATE Books SET Title = @Title, Author = @Author WHERE Id = @Id",
                new { Title = book.Title, Author = book.Author, Id = id }
            );

            con.Close();

            MessageBox.Show("оновлено");
            ShowAll();
        }

        [STAThread]
        static void Main()
        {
            Application.Run(new Form1());
        }
    }
}
