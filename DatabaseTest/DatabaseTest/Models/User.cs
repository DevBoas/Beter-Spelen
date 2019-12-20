using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseTest.Models
{
    public class User
    {
        [PrimaryKey,AutoIncrement]
        public int RowID
        {
            get;
            set;
        }
        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public int WaarBenik
        {
            get;
            set;
        }

        public User(int rowid, string username, string password, int waarikben)
        {
            this.RowID = rowid;
            this.WaarBenik = waarikben;
            this.Username = username;
            this.Password = password;
        }
        public User()
        { }
    }
}
