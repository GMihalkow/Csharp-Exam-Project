using Forum.Services.Interfaces.Message;
using Forum.ViewModels.Message;
using Forum.Web.Services.Account.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;

namespace Forum.Web.Controllers.Message
{
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMessageService messageService;

        public MessageController(IAccountService accountService, IMessageService messageService) : base(accountService)
        {
            this.messageService = messageService;
        }

        [HttpPost]
        public void Send([FromBody] SendMessageInputModel model)
        {
            var author = this.accountService.GetUser(this.User);

            this.messageService.SendMessage(model, author.Id);

            var reciever = this.accountService.GetUserById(model.RecieverId);

            var viewModel = new SendMessageInputModel
            {
                Messages = this.messageService.GetConversationMessages(author.UserName, reciever.UserName),
                RecieverId = reciever.Id,
                RecieverName = reciever.UserName
            };
        }
        
        public void UpdateChat()
        {
            var context = this.HttpContext;

            WebSocket webSocket = null;
            if (context.WebSockets.IsWebSocketRequest)
            {
                webSocket = context.WebSockets.AcceptWebSocketAsync().GetAwaiter().GetResult();

                while (true)
                {
                    var byteArr = new byte[4096];

                    webSocket.ReceiveAsync(byteArr, CancellationToken.None).GetAwaiter().GetResult();

                    var result = Encoding.UTF8.GetString(byteArr);

                    var splittedResult = result.Split(" - ");

                    string date = splittedResult[0];

                    string otherUserId = splittedResult[1];

                    if (result == "END")
                    {
                        //TODO: break loop here
                    }

                    var messages = this.messageService.GetLatestMessages(date, this.User.Identity.Name, otherUserId);

                    var jsonStr = JsonConvert.SerializeObject(messages);

                    var testArr = Encoding.UTF8.GetBytes(jsonStr);

                    webSocket.SendAsync(
                   buffer: new ArraySegment<byte>(
                       array: testArr,
                       offset: 0,
                       count: testArr.Length),
                   messageType: WebSocketMessageType.Text,
                   endOfMessage: true,
                   cancellationToken: CancellationToken.None).GetAwaiter().GetResult();
                }
            }
        }
    }
}