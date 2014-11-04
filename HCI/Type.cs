using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI
{
    class Type
    {
        private int id;
        private string type;

        [SQLite.AutoIncrement, SQLite.PrimaryKey]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [SQLite.Unique]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }
    }
}
