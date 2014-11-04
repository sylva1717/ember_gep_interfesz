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

        string connectionString;

        public ConnectFinances()
        {
            connectionString = @" Data Source = ..."; // ide kell majd a DB elérése
            
        }

        public void InsertRecord(Finances f) // hozzáad gombra hívódik
        {

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(()=>
                    {
                        conn.Insert(new Finances() {Date=f.Date, Amount=f.Amount, Type=f.Type});
                    });
            }
        }

        public void DeleteRecord(Finances f) // kukába húzásra hívódik
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Delete(f);
                    });
            }
        }

        public void UpdateRecord(Finances f)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Update(f);
                    });
            }
        }

        public int Total() // visszaadja az egyenleget
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Query();
                    });
            }
        }

    }
}
