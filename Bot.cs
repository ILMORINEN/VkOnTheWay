using System;
using System.Collections.Generic;
using System.Text;

namespace VkOnTheWay
{
    public class Bot
    {
        private VkApi vkApi;
        public Bot(VkApi vkApi)
        {
            this.vkApi = vkApi;
        }

        public void Reiceive()
        {
            var usersUnreadMessagesInfo = VkRequestHandler.GetUsersUnreadMessagesInfo(vkApi);
            var usersInfo = VkRequestHandler.GetUsersById(vkApi, usersUnreadMessagesInfo.Keys);
            Parallel.ForEach(usersUnreadMessagesInfo, userMessages => AggregateUserMessages(userMessages, usersInfo))
        }
    }
}
