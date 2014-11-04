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
        int Balance;
        int TypedBalance;
        IEnumerable<Finances> AllFinances;

        public ConnectFinances()
        {
            connectionString = @" Data Source = ..."; // ide kell majd a DB elérése
            Balance = 0;
            TypedBalance = 0;
        }

        public void InsertRecord(Finances f) // hozzáad gombra hívódik
        {

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(()=>
                    {
                        conn.Insert(new Finances() {Date=f.Date, Amount=f.Amount, TypeID=f.TypeID});
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

        public void RefreshBalance() // visszaadja az egyenleget
        {
            
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        // Balance = conn.Query<Finances>("SELECT SUM(Amount) FROM Finances").ElementAt(0).Amount;
                        Balance = conn.Table<Finances>().Sum(v => v.Amount);
                    });
            }

        }

        public void RefreshTypedBalance(string Type)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        // TypedBalance = conn.Query<Finances>("SELECT SUM(Amount) FROM Finances WHERE Type = ?", Type).ElementAt(0).Amount;
                        TypedBalance = conn.Table<Finances>().Where(v1=>v1.Type == Type).Sum(v2 => v2.Amount);
                    });
            }
        }

        public void GetAllRecords()
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        // AllFinances = conn.Query<Finances>("SELECT * FROM Finances");
                        AllFinances = conn.Table<Finances>();
                    });
            }
        }

    }
}
