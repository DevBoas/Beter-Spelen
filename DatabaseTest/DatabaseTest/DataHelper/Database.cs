using DatabaseTest.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DatabaseTest.DataHelper
{
    public class Database
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public bool createDatabase()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.CreateTable<User>();
                    connection.CreateTable<Vraag>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }
    
        public bool InsertIntoTableUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    var query = connection.Query<User>("SELECT * FROM User Where Username=?", user.Username);
                    foreach (var item in query)
                        return false;
                    connection.Insert(user);
                    return true;
                }
            }
            catch(SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public List<User> selectTableUser()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    return connection.Table<User>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return null;
            }
        }

        public List<Vraag> selectTableVraag()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    return connection.Table<Vraag>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return null;
            }
        }

        public bool deleteTableUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Delete(user);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public bool selectQueryTableUser(int RowID)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Query<User>("SELECT * FROM User Where RowID=?", RowID);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }
        
        public int loginUser(string username, string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    var query = connection.Query<User>("SELECT * FROM User WHERE Username = ? AND Password = ?", username, password);
                    foreach (var item in query)
                    {
                        User db_user = new User(item.RowID, item.Username, item.Password, item.WaarBenik);
                        return db_user.RowID;
                    }
                    return 0;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return 0;
            }
        }
        
        public bool purgeTableUser()
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Query<User>("Delete FROM User");
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public bool addVraag(Vraag vraag)
        {
            try
            {
                using (var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")))
                {
                    connection.Insert(vraag);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Debug.Print(ex.Message);
                return false;
            }
        }

        public bool updateVraag()
        {
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            connection.Query<Vraag>("SELECT * FROM Vraag WHERE vraagID=?", "1");
            return true;
        }
    }
}
