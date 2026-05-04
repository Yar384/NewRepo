using System;

namespace MoviesApp
{
    public class Title
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Duration { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
