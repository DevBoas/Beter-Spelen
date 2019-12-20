using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseTest.Models
{
    public class Vraag
    {
        [PrimaryKey, AutoIncrement]
        public int vraagID
        {
            get;
            set;
        }

        public string DeVraag
        {
            get;
            set;
        }
        public string Antwoorden
        {
            get;
            set;
        }

        public int Antwoord
        {
            get;
            set;
        }

        public Vraag(int id, string vraag, string antwoorden, int antwoord )
        {
            this.vraagID = id;
            this.Antwoorden = antwoorden;
            this.DeVraag = vraag;
            this.Antwoord = antwoord;
        }

        public Vraag()
        { }
    }
}
