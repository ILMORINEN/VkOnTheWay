using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using System.Threading;
using System.Linq;

namespace VkOnTheWay
{
    internal class Program
    {
        static public VkApi vkApi = new VkApi();
        static string[] Commands = { "Hello" };
        const string key = "vk1.a.mkY2jhTapCACplg87Iw5ESce5FBx1dIuPtIfoTu87f6Wijd23nQ9WDvZwrSXQ6zT9nRC747g5aBAd-eEoCUnZiOob3DLV1W3eZv6W3rj2u4K6vRbq3SURAWShU9-ZXHibv1Tb5rAGr4995e7rpfnZ0u1QZwu3QTRmTVNhbTui8lMuyGnH6cbi5zLvFYRYXfyWFcNsGyO0d4eEb6sGkMtEA";

        static void Main(string[] args)
        {
            try
            {
                vkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = key
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Авторизация пройдена успешно");
            while (true)
            {
                Thread.Sleep(50);
                Receive();
            }
        }

        public static void Receive()
        {
            var message = GetMessage();
            if (message == null)
                return;
            
            Console.WriteLine(message.Text);
            
            var user = vkApi.Users.Get(new long[] { (long)message.PeerId },
                                       ProfileFields.Sex);

            string userName = user.Select(x => $"{x.FirstName} {x.LastName}")
                                  .FirstOrDefault();
            
            Console.WriteLine(userName);
            vkApi.Messages.MarkAsRead(message.PeerId.ToString());
            vkApi.Messages.Send(new MessagesSendParams
            {
                PeerId = message.PeerId,
                RandomId = generateId(),
                Message = "еблан?"
            });
        }
        public static Message GetMessage()
        {
            var message = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Count = 1,
                Filter = GetConversationFilter.Unread
            });
            if (message.Items.Count != 0)
                return message.Items[0].LastMessage;
            return null;
        }

        public static Int32 generateId()
        {
            return DateTime.Now.Millisecond;
        }
    }
}
