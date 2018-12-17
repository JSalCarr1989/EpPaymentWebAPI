using System.Data;

namespace EPWebAPI.Interfaces
{
   public interface IDbConnectionRepository
    {
        IDbConnection CreateDbConnection();
        string GetEpPaymentConnectionString();
    }
}
