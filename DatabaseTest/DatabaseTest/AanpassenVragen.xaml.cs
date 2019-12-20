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
    public partial class AanpassenVragen : ContentPage
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

        public AanpassenVragen()
        {
            InitializeComponent();
        }

        private void LoadQuestion(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(up_vraag_id.Text))
                return;
            int QuestionID = Int32.Parse(up_vraag_id.Text);
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            var table = connection.Table<Vraag>();
            String Antwoorden = "";
            int count = 0;
            List<Entry> entries = new List<Entry>
            {
                up_antwoord1,
                up_antwoord2,
                up_antwoord3,
                up_antwoord4,
            };
            foreach (var item in table)
            {
                if (item.vraagID == QuestionID)
                {
                    up_vraag.Text = item.DeVraag;
                    up_antwoord_nummer.Text = item.Antwoord.ToString();
                    Debug.Print(item.Antwoorden);
                    Antwoorden = item.Antwoorden;
                    break;
                }
            }
            while (Antwoorden.IndexOf('~') != -1)
            {
                int length = Antwoorden.IndexOf('~');
                String substring = Antwoorden.Substring(0, length);
                entries[count].Text = substring;
                Antwoorden = Antwoorden.Substring(length + 1);
                count++;
            }
            entries[count].Text = Antwoorden;
        }
        private Boolean validateInput()
        {
            List<Entry> entries = new List<Entry>
            {
                up_antwoord1,
                up_antwoord1,
                up_antwoord1,
                up_antwoord1,
            };
            if (String.IsNullOrEmpty(up_vraag_id.Text) || String.IsNullOrEmpty(up_vraag.Text) || String.IsNullOrEmpty(up_antwoord_nummer.Text)) 
                return false;
            for (int i = 0; i < entries.Count(); i++)
                if (String.IsNullOrEmpty(entries[i].Text))
                    return false;
            return true;
        }

        private void UpdateQuestion(object sender, EventArgs e)
        {
            if (!validateInput())
            {
                DisplayAlert("Error", "Fill in all forms", "Back");
                return;
            }
            List<Entry> entries = new List<Entry>
            {
                up_antwoord1,
                up_antwoord2,
                up_antwoord3,
                up_antwoord4,
            };
            string deAntwoorden = "";
            for (int i = 0; i < entries.Count(); i++)
            {
                deAntwoorden += entries[i].Text;
                if (i != (entries.Count() - 1))
                    deAntwoorden += "~";
                entries[i].Text = "";
            }
            int antwoordNummer = Int32.Parse(up_antwoord_nummer.Text);
            int vraag_id = Int32.Parse(up_vraag_id.Text);
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            connection.Query<Vraag>("UPDATE Vraag SET DeVraag=?, Antwoorden=?, Antwoord=? WHERE vraagID=?", up_vraag.Text, deAntwoorden, antwoordNummer, vraag_id);
            up_vraag.Text = "";
            up_vraag_id.Text = "";
            up_antwoord_nummer.Text = "";
        }
    }
}