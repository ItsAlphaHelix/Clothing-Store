using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clothing_Store.Core.Contracts
{
    public interface IPaymentsService
    {
        Task<string> CreateCheckoutSessionAsync(string userId);
    }
}
