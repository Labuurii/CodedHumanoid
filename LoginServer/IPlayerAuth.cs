using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    internal interface IPlayerAuth
    {
        Task<bool> GetPermanentId(string name, string pw, out Guid permanent_id);
    }
}
