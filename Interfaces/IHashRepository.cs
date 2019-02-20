using EPWebAPI.Models;

namespace EPWebAPI.Interfaces
{

public interface IHashRepository 
{

    Hash CreateRequestHash(HashDTO hash);
    string GetHashStatus(MultiPagosResponsePaymentDTO multipagosResponse);

    }

}