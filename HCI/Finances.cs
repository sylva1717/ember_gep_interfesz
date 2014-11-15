using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI
{
    class Finances
    {
        private int id;
        private string title;
        private DateTime date;
        private int amount;
        private string type;

        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
    }

}
