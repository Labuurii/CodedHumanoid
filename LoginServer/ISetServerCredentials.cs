using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginServer
{
    internal interface ISetServerCredentials
    {
        void SetServerCredentials(List<InstanceServerDecl> credentials);
    }
}
