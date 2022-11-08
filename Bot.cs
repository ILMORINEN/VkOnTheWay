using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;

namespace VkOnTheWay
{
    public class Bot
    {
        private readonly VkApi vkApi;
        public Bot(VkApi vkApi)
        {
            this.vkApi = vkApi;
        }
        private void AggregateUserMessages(KeyValuePair<long, long?> conversation, Dictionary<long, User> users)
        {
            var messages = VkRequestHandler.GetUserUnreadMessages(vkApi, conversation);
        }
        public void Receive()
        {
            var usersUnreadMessagesInfo = VkRequestHandler.GetConversationsUnreadMessgesCount(vkApi);
            var usersInfo = VkRequestHandler.GetUsersById(vkApi, usersUnreadMessagesInfo.Keys);
            Parallel.ForEach(usersUnreadMessagesInfo, userMessages => AggregateUserMessages(userMessages, usersInfo));
        }
    }
}
