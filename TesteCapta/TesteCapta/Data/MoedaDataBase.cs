using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TesteCapta.Models;

namespace TesteCapta.Data
{
    public class MoedaDataBase
    {
        readonly SQLiteAsyncConnection _database;

        public MoedaDataBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<MoedaDataBaseModel>().Wait();
        }

        public Task<List<MoedaDataBaseModel>> GetMoedaAsync()
        {
            return _database.Table<MoedaDataBaseModel>().ToListAsync();
        }

        public Task<MoedaDataBaseModel> GetMoedaAsync(string simbolo)
        {
            return _database.Table<MoedaDataBaseModel>()
                            .Where(i => i.Simbolo == simbolo)
                            .FirstOrDefaultAsync();
        }

        public Task<int> SaveMoedaAsync(MoedaDataBaseModel moeda)
        {
            if (moeda.ID != 0)
            {
                return _database.UpdateAsync(moeda);
            }
            else
            {
                return _database.InsertAsync(moeda);
            }
        }

        public Task<int> DeleteMoedaAsync(MoedaDataBaseModel moeda)
        {
            return _database.DeleteAsync(moeda);
        }

    }
}
