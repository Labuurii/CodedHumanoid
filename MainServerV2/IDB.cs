using MainServerV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArenaHost
{
    internal struct PlayerPasswordAndInfo
    {
        internal string password_hash;
        internal PlayerInfo player_info;
    }

    internal interface IDB
    {
        Task<PlayerPasswordAndInfo?> GetPlayerInfoAndPassword(string username);
        Task SavePlayerInfo(PlayerInfo player_info);
        Task<bool> AddPendingFriendRequest(long permanent_id, long target_player_permanent_id);
        Task<BasicPlayerInfo> GetBasicInfoOfPlayer(long permanent_id);
        Task<bool> AnswerFriendRequest(long answerer_permanent_id, long from_permanent_id, bool accepted);
    }
}
