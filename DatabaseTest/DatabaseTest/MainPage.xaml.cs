using DatabaseTest.DataHelper;
using DatabaseTest.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DatabaseTest
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)] 
    public partial class MainPage : ContentPage
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        Database db;
        public MainPage()
        {
            InitializeComponent();
        }

        //debug
        private void DisplayDB(object sender, EventArgs e)
        {
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            var table = connection.Table<User>();
            Output.Text = "";
            foreach (var item in table)
            {
                User db_user = new User(item.RowID, item.Username, item.Password, item.WaarBenik);
                Output.Text += db_user.RowID.ToString() + ":" + db_user.Username + " - " + db_user.Password + " - " + item.WaarBenik  + "\n";
            }
        }

        public ICommand TapCommand => new Command<string>((url) =>
        {
            Navigation.PushAsync(new Page1());
        });

        private void RegisterUser(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page1());
        }

        private void LoginUser(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(form_username.Text) && (!String.IsNullOrEmpty(form_password.Text)))
            {
                db = new Database();
                db.createDatabase();
                int ID = db.loginUser(form_username.Text, form_password.Text);
                //Debug.Print("ID = " + ID.ToString());
                if (ID != 0)
                {
                    DisplayAlert("Login", "Succesful", "Continue");
                    if (form_username.Text != "root")
                        Navigation.PushAsync(new Beter_Spellen(ID));
                    else
                        Navigation.PushAsync(new UploadVragen());
                    this.Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
                }
                else
                    DisplayAlert("Login", "Failed", "Retry");
            }
        }
    }
}
