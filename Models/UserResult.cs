using System;

namespace Models
{
    public class UserResult : IModel
    {
        public int Id { get; set; }
        public int UserId {get; set;} 
        public DateTime Date { get; set; }
        public int R { get; set; }
        public int I { get; set; }
        public int A { get; set; }
        public int S { get; set; }
        public int E { get; set; }
        public int C { get; set; }

        public User User { get; set; }
    }
}
