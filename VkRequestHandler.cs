using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace VkOnTheWay
{
    internal class VkRequestHandler
    {
        public static Dictionary<long, long?> GetUsersUnreadMessagesCount(VkApi vkApi)
        {
            var conversationsResult = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Filter = GetConversationFilter.Unread
            });
            return conversationsResult.Items.ToDictionary(x => x.Conversation.Peer.Id, x => x.Conversation.UnreadCount);
        }
        public static Dictionary<long, User> GetUserByIds(VkApi vkApi, IEnumerable<long> ids)
        {
            return vkApi.Users.Get(ids,ProfileFields.Sex).ToDictionary(x => x.Id, x => x);
        }
    }
}
