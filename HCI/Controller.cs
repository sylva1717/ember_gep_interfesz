using System;
using System.Collections.Generic;

namespace HCI
{
    class Controller
    {
        ConnectFinances m_model;

        Controller()
        {
            m_model = new ConnectFinances();
        }

        public List<string> GetCategories()
        {
            return m_model.Types;
        }

        public List<Finances> GetAllRecords()
        {
            m_model.RefreshAllFinances();
            return m_model.AllFinances;
        }

        public void InsertRecord(DateTime Date, int Amount, string Category)
        {
            Finances Record = new Finances();
            Record.Date = Date;
            Record.Amount = Amount;
            Record.Type = Category;

            m_model.InsertRecord(Record);
        }

        public void ModifyRecord(Finances Record)
        {
            m_model.UpdateRecord(Record);
        }

        public void RemoveRecord(Finances Record)
        {
            m_model.DeleteRecord(Record);
        }

        public int QueryBalance(string Category = null)
        {
            if (Category != null)
            {
                m_model.RefreshTypedBalance(Category);
                return m_model.TypedBalance;
            }
            else
            {
                m_model.RefreshBalance();
                return m_model.Balance;
            }
        }
    }
}
