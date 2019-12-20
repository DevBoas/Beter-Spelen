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
    public partial class Beter_Spellen : ContentPage
    {
        string folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        int user_ID = 0;
        int now_question = 0;
        int answer1 = -1;
        int answer2 = -1;
        private void laadVraag(int vraag)
        {
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            var table = connection.Table<Vraag>();
            string antwoordenConcat = "";
            int count = 0;
            List<Label> labels1 = new List<Label>
            {
                antwoord1,
                antwoord2,
                antwoord3,
                antwoord4,
            };
            Label_Vraag1.Text = "";
            Boolean found = false;
            foreach (var item in table)
            {
                if (item.vraagID == vraag)
                {
                    antwoordenConcat = item.Antwoorden;
                    Label_Vraag1.Text = item.DeVraag;
                    found = true;
                    break;
                }
            }
            if (!found)
                return;
            while (antwoordenConcat.IndexOf('~') != -1)
            {
                int length = antwoordenConcat.IndexOf('~');
                String substring = antwoordenConcat.Substring(0, length);
                labels1[count].Text = substring;
                antwoordenConcat = antwoordenConcat.Substring(length+1);
                count++;
            }
            labels1[count].Text = antwoordenConcat;
            List<Label> labels2 = new List<Label>
            {
                antwoord5,
                antwoord6,
                antwoord7,
                antwoord8,
            };
            vraag++;
            found = false;
            count = 0;
            foreach (var item in table)
            {
                if (item.vraagID == vraag)
                {
                    antwoordenConcat = item.Antwoorden;
                    Label_Vraag2.Text = item.DeVraag;
                    found = true;
                    break;
                }
            }
            if (!found)
                return;

            while (antwoordenConcat.IndexOf('~') != -1)
            {
                int length = antwoordenConcat.IndexOf('~');
                String substring = antwoordenConcat.Substring(0, length);
                labels2[count].Text = substring;
                antwoordenConcat = antwoordenConcat.Substring(length + 1);
                count++;
            }
            labels2[count].Text = antwoordenConcat;

        }
        public Beter_Spellen(int ID)
        {
            user_ID = ID;
            InitializeComponent();
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            var table = connection.Table<User>();
            foreach (var item in table)
            {
                if(item.RowID == ID)
                {
                    now_question = item.WaarBenik;
                    break;
                }
            }
            var table2 = connection.Table<Vraag>();
            if (now_question < table2.Count())
            {
                laadVraag(now_question);
            }
            else
            {
                DisplayAlert("Game", "There are no more questions for you", "Okay");
            }

        }

        public Boolean debounce1 = false;
        public Boolean debounce2 = false;
        private void handleChoice1(object sender, CheckedChangedEventArgs e)
        {
            if (debounce1)
                return;
            debounce1 = true;
            CheckBox check = sender as CheckBox;
            var classId = check.ClassId;
            List<CheckBox> checks = new List<CheckBox>
            {
                CheckBox1,
                CheckBox2,
                CheckBox3,
                CheckBox4,
            };

            for (int i = 0; i < checks.Count(); i++)
                if (checks[i] != check)
                    checks[i].IsChecked = false;
                else if (checks[i] == check)
                    answer1 = i;

            debounce1 = false;
        }
        private void handleChoice2(object sender, CheckedChangedEventArgs e)
        {
            if (debounce2)
                return;
            debounce2 = true;
            CheckBox check = sender as CheckBox;
            var classId = check.ClassId;
            List<CheckBox> checks = new List<CheckBox>
            {
                CheckBox5,
                CheckBox6,
                CheckBox7,
                CheckBox8,
            };
            for (int i = 0; i < checks.Count(); i++)
            {
                if (checks[i] != check)
                    checks[i].IsChecked = false;
                else if (checks[i] == check)
                    answer2 = i;
            }
            debounce2 = false;
        }

        private void CheckQuestions(int question)
        {
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db"));
            
            var table = connection.Table<Vraag>();
            int antwoord1 = -2;
            int antwoord2 = -2;
            Debug.Print("Question = " + question);
            string output = "";
            foreach (var item in table)
            {
                Debug.Print(item.vraagID.ToString() + " - " + (question+1));
                if (item.vraagID == (question + 1))
                {
                    if (antwoord1 == -2)
                    {
                        Debug.Print("Antwoord1 set");
                        antwoord1 = item.Antwoord;
                        question++;
                    }
                    else
                    {
                        Debug.Print("Antwoord2 set");
                        antwoord2 = item.Antwoord;
                        break;
                    }
                }
            }
            if (antwoord1 == -2 || antwoord2 == -2)
                return;
            answer1++;
            answer2++;
            Debug.Print("Correct Answers are:  " + antwoord1 + " - " + antwoord2);
            Debug.Print("Given Answers are: " + answer1 + " - " + answer2);
            if (antwoord1 == answer1)
                output += "Antwoord1 is correct, ";
            else
                output += "Antwoord1 is fout, ";
            if (antwoord2 == answer2)
                output += "antwoord2 is correct.";
            else
                output += "antwoord2 is fout.";
            DisplayAlert("Scoreboard", output, "Okay");

        }
        
        private void ClearCheckBoxes()
        {
            List<CheckBox> checks = new List<CheckBox>
            {
                CheckBox1,
                CheckBox2,
                CheckBox3,
                CheckBox4,
                CheckBox5,
                CheckBox6,
                CheckBox7,
                CheckBox8,
            };
            for (int i = 0; i < checks.Count(); i++)
            {
                if (checks[i].IsChecked)
                    checks[i].IsChecked = false;
            }
        }

        private void CheckResult(object sender, EventArgs e)
        {
            if (user_ID == 0)
                return;
            CheckQuestions(now_question);
            ClearCheckBoxes();
            answer1 = -1;
            answer2 = -1;
            now_question += 1;
            var connection = new SQLiteConnection(System.IO.Path.Combine(folder, "Users.db")); 
            connection.Query<User>("UPDATE User SET WaarBenik = ? WHERE RowID = ?", now_question, user_ID);//now_question
            var table = connection.Table<Vraag>();
            if (now_question < table.Count())
                laadVraag(now_question);
            else
                DisplayAlert("Game", "There are no more questions for you", "Okay");
        }
    }
}