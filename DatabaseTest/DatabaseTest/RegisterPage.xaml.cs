using DatabaseTest.DataHelper;
using DatabaseTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DatabaseTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        Database db;
        public Page1()
        {
            InitializeComponent();
        }

        private void Register_User(object sender, EventArgs e)
        {
            db = new Database();
            db.createDatabase();

            User user = new User()
            {
                Username = reg_username.Text,
                Password = reg_password.Text,
            };

            if (db.InsertIntoTableUser(user))
            {
                DisplayAlert("Succes", "You successfully created your account!", "Okay");
                this.Navigation.PopAsync();
            }
            else
                DisplayAlert("Warning", "That username is already in use!", "Retry");
        }
    }
}