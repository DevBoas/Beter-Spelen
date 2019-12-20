using DatabaseTest.DataHelper;
using DatabaseTest.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DatabaseTest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadVragen : ContentPage
    {
        Database db;
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public UploadVragen()
        {
            InitializeComponent();
        }

        private Boolean validateInput()
        {
            List<Entry> entries = new List<Entry>
            {
                reg_antwoord1,
                reg_antwoord2,
                reg_antwoord3,
                reg_antwoord4,
            };
            if (String.IsNullOrEmpty(reg_vraag.Text) || String.IsNullOrEmpty(reg_antwoord_nummer.Text))
                return false;
            for (int i = 0; i < entries.Count(); i++)
                if (String.IsNullOrEmpty(entries[i].Text))
                    return false;
            return true;
        }

        private void AddVraag(object sender, EventArgs e)
        {
            if (!validateInput())
            {
                DisplayAlert("Error", "Fill in all forms", "Back");
                return;
            }

            db = new Database();
            db.createDatabase();
            string deVraag = reg_vraag.Text;
            string deAntwoorden = "";
            List<Entry> entries = new List<Entry>
            {
                reg_antwoord1,
                reg_antwoord2,
                reg_antwoord3,
                reg_antwoord4,
            };
            for (int i = 0; i < entries.Count(); i++)
            {
                deAntwoorden += entries[i].Text;
                if (i != (entries.Count()-1))
                    deAntwoorden += "~";
                entries[i].Text = "";
            }
            int antwoordNummer = Int32.Parse(reg_antwoord_nummer.Text);
            Vraag vraag = new Vraag()
            {
                DeVraag = deVraag,
                Antwoorden = deAntwoorden,
                Antwoord = antwoordNummer,
            };
            db.addVraag(vraag);
            reg_vraag.Text = "";
            reg_antwoord_nummer.Text = "";
        }

        //debug
        private void DisplayDB(object sender, EventArgs e)
        {
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            var table = connection.Table<Vraag>();
            Output.Text = "";
            foreach (var item in table)
            {
                Vraag db_user = new Vraag(item.vraagID,item.DeVraag, item.Antwoorden, item.Antwoord);
                Output.Text += db_user.vraagID.ToString() + ":" + db_user.DeVraag + ":" + db_user.Antwoorden + ":" + db_user.Antwoord.ToString()+ "\n";
            }
        }

        private void NavigateEditing(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AanpassenVragen());
        }
    }
}