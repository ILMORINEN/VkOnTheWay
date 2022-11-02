using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkOnTheWay
{
    internal class MessageHandler
    {
        public static Dictionary<long, long?> GetConversationsUnreadMessages(VkApi vkApi)
        {
            var conversationsResult = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Filter = GetConversationFilter.Unread
            });
            return conversationsResult.Items.ToDictionary(x => x.Conversation.Peer.Id, x => x.Conversation.UnreadCount);
        }
        public static Dictionary<long, User> GetUserByIds(VkApi vkApi, IEnumerable<long> ids)
        {
            return vkApi.Users.Get(ids).ToDictionary(x => x.Id, x => x);
        }
    }
}
