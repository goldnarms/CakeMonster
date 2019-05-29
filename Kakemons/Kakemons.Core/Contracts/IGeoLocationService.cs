using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Common.Responses;
using Kakemons.Core.ViewModels.Register;

namespace Kakemons.Core.Contracts
{
    public interface IGeoLocationService
    {
        Task<GeoCodeResult> Search(string addressSearchText);
    }
}
