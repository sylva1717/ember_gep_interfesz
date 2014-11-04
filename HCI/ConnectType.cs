using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCI
{
    class ConnectType
    {

        string connectionString;

        public ConnectType()
        {
            connectionString = @" Data Source = ..."; // ide kell majd a DB elérése
            
        }

        public void InsertRecord(Type f) // hozzáad gombra hívódik
        {

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(()=>
                    {
                        conn.Insert(new Type() {Type=f.Type});
                    });
            }
        }

        public void DeleteRecord(Type f) // kukába húzásra hívódik
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Delete(f);
                    });
            }
        }
    }
}
