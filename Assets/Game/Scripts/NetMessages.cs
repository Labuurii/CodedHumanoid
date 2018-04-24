using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace NetMessages
{
    public enum RequestIds : short
    {
        PlayerData = 888
    }

    public class PlayerDataAttempt : MessageBase
    {
        public NetworkIdentity net_id;
    }
}
