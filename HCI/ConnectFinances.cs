using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;
using System.IO;

namespace HCI
{
    class ConnectFinances
    {

        string connectionString;
        int Balance;
        int TypedBalance;
        List<Finances> AllFinances;

        public ConnectFinances() // konstruktor: üres tábla létrehozása, elemek inicializálása
        {

            connectionString = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            Balance = 0;
            TypedBalance = 0;
            AllFinances = new List<Finances>();

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.CreateTable<Finances>();
            }

        }

        /*public void InsertRecord(DateTime date, int amount, string type) // hozzáad gombra hívódik
        {
            Type tempType = new Type() { Type = type }; 
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Insert(tempType);
                    });
            }

            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(()=>
                    {
                        conn.Insert(new Finances() {Date=date, Amount=amount, TypeID=tempType.Id});
                    });
            }
        }*/

        public void InsertRecord(Finances f) // hozzáad gombra hívódik
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(new Finances() { Date = f.Date, Amount = f.Amount, Type = f.Type });
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

        public void UpdateRecord(Finances f) // valamilyen szerkesztő felület bejön ha duplázunk egy tételre, ahol majd lehet módosításokat menteni, ekkor híívódik ez a metódus
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

        public void RefreshTypedBalance(string type) // visszaadja az egyenleget a megadott típusra bontva
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        // TypedBalance = conn.Query<Finances>("SELECT SUM(Amount) FROM Finances WHERE TypeID = ?", t.Id).ElementAt(0).Amount;
                        // TypedBalance = conn.Table<Finances>().Where(v1=>v1.TypeID == t.Id).Sum(v2 => v2.Amount); // Ehhez (Type t) az argumentum
                        TypedBalance = conn.Table<Finances>().Where(v1 => v1.Type == type).Sum(v2 => v2.Amount);
                    });
            }
        }

        public void ResfreshAllFinances() // az összes tételt tároló listát firssíti (azaz a db tartalmát kiolvassuk)
        {
            using (var conn = new SQLiteConnection(connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        // AllFinances = conn.Query<Finances>("SELECT * FROM Finances");
                        AllFinances = conn.Table<Finances>().ToList();
                    });
            }
        }

    }
}
