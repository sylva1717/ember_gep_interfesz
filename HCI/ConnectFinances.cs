using SQLite;
using System.Collections.Generic;
using System.Linq;

namespace HCI
{
    class ConnectFinances
    {

        string m_connectionString;

        int m_Balance;

        public int Balance
        {
            get { return m_Balance; }
            set { m_Balance = value; }
        }
        
        int m_TypedBalance;

        public int TypedBalance
        {
            get { return m_TypedBalance; }
            set { m_TypedBalance = value; }
        }
        
        List<Finances> m_AllFinances;

        internal List<Finances> AllFinances
        {
            get { return m_AllFinances; }
            set { m_AllFinances = value; }
        }

        List<Finances> m_TypedFinances;

        internal List<Finances> TypedFinances
        {
            get { return m_TypedFinances; }
            set { m_TypedFinances = value; }
        }

        List<string> m_Types;

        public List<string> Types
        {
            get { return m_Types; }
        }

        public ConnectFinances() // konstruktor: üres tábla létrehozása, elemek inicializálása
        {

            m_connectionString = System.IO.Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            m_Balance = 0;
            m_TypedBalance = 0;
            m_AllFinances = new List<Finances>();

            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.CreateTable<Finances>();
            }

            m_Types = new List<string>();
            m_Types.Add("Ajándék");
            m_Types.Add("Barkácsolás");
            m_Types.Add("Egészségügy");
            m_Types.Add("Élelmiszer");
            m_Types.Add("Irodaszerek");
            m_Types.Add("Kölcsön");
            m_Types.Add("Kultúra");
            m_Types.Add("Szállás, albérlet");
            m_Types.Add("Szórakozás");
            m_Types.Add("Tanulmányok");
            m_Types.Add("Túra, sport");
            m_Types.Add("Utazás");
            m_Types.Add("Fizetés");
            m_Types.Add("Ösztöndíj");
        }

        public void InsertRecord(Finances f) // hozzáad gombra hívódik
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                {
                    conn.Insert(new Finances() {Title = f.Title, Date = f.Date, Amount = f.Amount, Type = f.Type });
                });
            }
        }

        public void DeleteRecord(Finances f) // kukába húzásra hívódik
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Delete(f);
                    });
            }
        }

        // valamilyen szerkesztő felület bejön ha duplázunk egy tételre, ahol majd lehet módosításokat menteni, ekkor hívódik ez a metódus
        public void UpdateRecord(Finances f) 
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        conn.Update(f);
                    });
            }
        }

        public void RefreshBalance() // visszaadja az egyenleget
        {
            
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        m_Balance = conn.Table<Finances>().Sum(v => v.Amount);
                    });
            }

        }

        public void RefreshTypedBalance(string type) // visszaadja az egyenleget a megadott típusra bontva
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        m_TypedBalance = conn.Table<Finances>().Where(v1 => v1.Type == type).Sum(v2 => v2.Amount);
                    });
            }
        }

        public void RefreshAllFinances() // az összes tételt tároló listát frissíti (azaz a db tartalmát kiolvassuk)
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                    {
                        m_AllFinances = conn.Table<Finances>().OrderByDescending(v1 => v1.Id).ToList();
                    });
            }
        }

        public void RefreshTypedFinances(string type) // az összes tételt frissíti egy adott kategóriára
        {
            using (var conn = new SQLiteConnection(m_connectionString))
            {
                conn.RunInTransaction(() =>
                {
                    m_TypedFinances = conn.Table<Finances>().Where(v1 => v1.Type == type).OrderByDescending(v1 => v1.Id).ToList();
                });
            }
        }

    }
}
