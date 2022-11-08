using System.Collections.Generic;
using System.Linq;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;

namespace VkOnTheWay
{
    public class VkRequestHandler
    {
        public static Dictionary<long, long?> GetUsersUnreadMessagesInfo(VkApi vkApi)
        {
            var conversationsResult = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Filter = GetConversationFilter.Unread
            });
            return conversationsResult.Items.ToDictionary(x => x.Conversation.Peer.Id,
                                                          x => x.Conversation.UnreadCount);
        }
        public static Dictionary<long, User> GetUsersById(VkApi vkApi, IEnumerable<long> ids)
        {
            return vkApi.Users.Get(ids,ProfileFields.Sex)
                              .ToDictionary(x => x.Id, x => x);
        }
        public static List<string> GetUserUnreadMessages(VkApi vkApi, KeyValuePair<long, long?> conversation)
        {
            var messagesHistory = vkApi.Messages.GetHistory(new MessagesGetHistoryParams
            {
                PeerId = conversation.Key,
                Count = conversation.Value,
            });
            return messagesHistory.Messages
                                  .Select(x => x.Text)
                                  .ToList();
        }
        public static void MarkAsRead(VkApi vkApi, long user)
        {
            vkApi.Messages.MarkAsRead(user.ToString());
        }
        public static void SendMessageToUser(VkApi vkApi, long user, string text)
        {
            vkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = user,
                Message = text,
            });
        }
        public static void SendMessageToUser(VkApi vkApi, long user, string text, MessageKeyboard keyboard)
        {
            vkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = user,
                Message = text,
                Keyboard = keyboard
            });
        }
    }
}
