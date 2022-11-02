using System;
using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using System.Threading;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;


namespace VkOnTheWay
{
    internal class Program
    {
        static public VkApi vkApi = new VkApi();
        static string[] Commands = { "Hello" };
        private static string key = ConfigurationManager.AppSettings["apiKey"];
        
        static void Main(string[] args)
        {
            try
            {
                vkApi.Authorize(new ApiAuthParams
                {
                    AccessToken = key,
                    
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            Console.WriteLine("Авторизация пройдена успешно");
            var startTime = System.Diagnostics.Stopwatch.StartNew();
            while (true)
            {
                Thread.Sleep(250);
                Receive();
                startTime.Stop();
                Console.WriteLine(startTime.ElapsedMilliseconds);
                startTime.Restart();
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
            Console.WriteLine("Я получаю сообщение");
            var message = vkApi.Messages.GetConversations(new GetConversationsParams
            {
                Count = 1,
                Filter = GetConversationFilter.Unread
            });
            if (message.Items.Count != 0)
                return message.Items[0].LastMessage;
            return null;
        }

        public static long generateId()
        {
            return DateTime.Now.Ticks;
        }
    }
}
