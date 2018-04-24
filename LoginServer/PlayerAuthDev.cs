using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    internal class PlayerAuthDev : IPlayerAuth
    {
        public Task<bool> GetPermanentId(string name, string pw, out Guid permanent_id)
        {
            permanent_id = Guid.NewGuid();
            return Task.FromResult(true);
        }
    }
}
