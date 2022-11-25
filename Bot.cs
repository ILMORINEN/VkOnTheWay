using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;

namespace VkOnTheWay
{
    public enum ActionParameter
    {
        OutDateTime,
        StartPoint,
        EndPoint,
        UserType,
        CarPlaces,
        Cost
    }

    public class Bot
    {
        private readonly VkApi vkApi;
        private BotActionDelegate actionDelegate;
        public delegate void BotActionDelegate(params string[] args);
        public Bot(VkApi vkApi)
        {
            this.vkApi = vkApi;
        }
        public void Receive()
        {
            var usersUnreadMessagesInfo = VkRequestHandler.GetConversationsUnreadMessgesCount(vkApi);
            var usersInfo = VkRequestHandler.GetUsersById(vkApi, usersUnreadMessagesInfo.Keys);
            Parallel.ForEach(usersUnreadMessagesInfo, userMessages => AggregateUserMessages(userMessages, usersInfo));
        }
        private void AggregateUserMessages(KeyValuePair<long, long?> conversation, Dictionary<long, User> users)
        {
            var userID = conversation.Key;
            var messages = VkRequestHandler.GetUserUnreadMessages(vkApi, conversation);
            foreach (var message in messages)
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.WrongMessage);
                    continue;
                }
                if (actionDelegate == null)
                { 
                    SelectAction(message, userID);
                    return; 
                }
                if (args)
            }
        }
        private void SelectAction(string message, long userID)
        {
            VkRequestHandler.MarkAsRead(vkApi, userID);
            if (message == BotAnswersResource.Start)
            {
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.Greeting, new[]
                {
                    BotAnswersResource.CreateTripRequest,
                    BotAnswersResource.ViewTrips,
                    BotAnswersResource.ViewPassengers
                });
            }
            else if (message == BotAnswersResource.CreateTripRequest)
            {
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.SelectUserType, new[]
                {
                    BotAnswersResource.UserDriver,
                    BotAnswersResource.UserPassager
                });
                actionDelegate = CreateTripRequest;
                //TODO: Придумать как контролировать ввод параметров
                actionParams = new[] {}
            }
            else if (message == BotAnswersResource.ViewTrips)
            {
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.SelectOutDateTime);
                actionDelegate = ViewTrips;
            }
            else if (message == BotAnswersResource.ViewPassengers)
            {
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.SelectOutDateTime);
                actionDelegate = ViewPassengers;
            }
            else
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.WrongMessage);
        }
        private string ReadParameter(string message, long userID)
        {
            if (true)
            {

            }
            else
                VkRequestHandler.SendMessageToUser(vkApi, userID, BotAnswersResource.WrongMessage);
        }
        private void CreateTripRequest(params string[] args)
        {
        }
        private void ViewTrips(params string[] args)
        {
        }
        private void ViewPassengers(params string[] args)
        {
        }
    }
}
