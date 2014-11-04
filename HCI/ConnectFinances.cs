using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI
{
    class ConnectFinances
    {
        SQLiteConnection conn;
        string connectionString;

        public ConnectFinances()
        {
            connectionString = @" Data Source = ..."; // ide kell majd a DB elérése
            conn = new SQLiteConnection(connectionString);
            
        }

        public void InsertRecord(Finances f) // hozzáad gombra hívódik
        {

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(()=>
                    {
                        conn.Insert(new Finances() {Id=f.Id, Date=f.Date, Amount=f.Amount, Type=f.Type});
                    });
            }
        }

        public void DeleteRecord(int ID) // kukába húzásra hívódik
        {

        }

        public int MaxId() // visszaadja a max Id-t a táblában
        {
            return 0;
        }

        public int Total() // visszaadja az egyenleget
        {
            return 0;
        }

    }
}
