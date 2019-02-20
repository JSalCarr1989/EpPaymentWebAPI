using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPWebAPI.Interfaces
{
    interface IEnvironmentSettingsRepository
    {
        string GetConnectionStringSetting();
        string GetMpSK();
        string GetUrlSuccess();
        string GetUrlFailure();
    }
}
