using MainServerV2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Scripts
{
    /// <summary>
    /// Exists because <see cref="BasicPlayerInfo"/> is generated code and I only want to compare basic player info by <see cref="BasicPlayerInfo.PermanentId"/>.
    /// </summary>
    public static class BasicPlayerInfoOps
    {
        public static bool Contains(List<BasicPlayerInfo> l, BasicPlayerInfo player)
        {
            if (l == null)
                return false;
            var permanent_id = player.PermanentId;
            var count = l.Count;
            for(var i = 0; i < count; ++i)
            {
                var item = l[i];
                if (item.PermanentId == permanent_id)
                    return true;
            }

            return false;
        }

        public static bool RemoveOne(List<BasicPlayerInfo> l, BasicPlayerInfo player)
        {
            if (l == null)
                return false;
            var permanent_id = player.PermanentId;
            var count = l.Count;
            for(var i = 0; i < count; ++i)
            {
                var item = l[i];
                if(item.PermanentId == permanent_id)
                {
                    l.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        internal static BasicPlayerInfo GetInList(List<BasicPlayerInfo> l, long permanent_player_id)
        {
            for(var i = 0; i < l.Count; ++i)
            {
                var item = l[i];
                if (item.PermanentId == permanent_player_id)
                    return item;
            }

            return null;
        }
    }
}
