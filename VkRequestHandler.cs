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
        /// <summary>
        /// Возвращает словарь, где ключом является id пользователя, а значением количество непрочитанных сообщений.
        /// </summary>
        public static Dictionary<long, long?> GetConversationsUnreadMessgesCount(VkApi vkApi)
        {
            var conversationsResult = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Filter = GetConversationFilter.Unread
            });
            return conversationsResult.Items.ToDictionary(x => x.Conversation.Peer.Id,
                                                          x => x.Conversation.UnreadCount);
        }
        /// <summary>
        /// Возвращает словарь, где ключом является id пользователя, а значением объект пользователя <see cref="VkNet"/>.
        /// </summary>
        public static Dictionary<long, User> GetUsersById(VkApi vkApi, IEnumerable<long> ids)
        {
            return vkApi.Users.Get(ids,ProfileFields.Sex)
                              .ToDictionary(x => x.Id, x => x);
        }
        /// <summary>
        /// Возвращает список непрочитанных сообщений беседы.
        /// </summary>
        public static List<string> GetUserUnreadMessages(VkApi vkApi, KeyValuePair<long, long?> conversation)
        {
            var messagesHistory = vkApi.Messages.GetHistory(new MessagesGetHistoryParams
            {
                PeerId = conversation.Key,
                Count = conversation.Value,
            });
            return messagesHistory.Messages
                                  .Select(x => x.Text)
                                  .Reverse()
                                  .ToList();
        }
        /// <summary>
        /// Помечает сообщения как прочитанные.
        /// </summary>
        public static void MarkAsRead(VkApi vkApi, long user)
        {
            vkApi.Messages.MarkAsRead(user.ToString());
        }
        /// <summary>
        /// Отправляет пользователю сообщение.
        /// </summary>
        public static void SendMessageToUser(VkApi vkApi, long user, string text)
        {
            vkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = user,
                RandomId = generateId(),
                Message = text,
            });
        }
        /// <summary>
        /// Отправляет пользователю сообщение.
        /// </summary>
        public static void SendMessageToUser(VkApi vkApi, long user, string text, string[] buttonsTitles)
        {
            KeyboardBuilder keyboardBuilder = new KeyboardBuilder();
            foreach (var buttonTitle in buttonsTitles)
                keyboardBuilder.AddButton(new AddButtonParams
                { Label = buttonTitle }).AddLine();
            vkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = user,
                RandomId = generateId(),
                Message = text,
                Keyboard = keyboardBuilder.SetOneTime().Build()
            });
        }

        private static long generateId()
        {
            return System.DateTime.Now.Ticks;
        }
    }
}
