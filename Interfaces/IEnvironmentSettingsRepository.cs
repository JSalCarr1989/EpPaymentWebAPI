using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWebAPI.Interfaces
{
    public interface IEnvironmentSettingsRepository
    {
        string GetConnectionString();
        string GetMpSK();
        string GetUrlSuccess();
        string GetUrlFailure();
    }
}
