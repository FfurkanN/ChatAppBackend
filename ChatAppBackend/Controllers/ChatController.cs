using ChatAppBackend.Dtos;
using ChatAppBackend.Hubs;
using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackend.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ChatController(IHubContext<ChatHub> hubContext, IChatRepository chatRepository, IUserRepository userRepository, IUserChatRepository userChatRepository) : ControllerBase
    {

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Create(CreateChatDto createChatDto,CancellationToken cancellationToken)
        {
            AppChat chat = new AppChat
            {
                Name = createChatDto.ChatName,
                Creator_Id = createChatDto.CreatorId,
                isPublic = createChatDto.isPublic
            };

            AppChat createdChat = await chatRepository.CreateChatAsync(chat);

            foreach (var member in createChatDto.members)
            {
                await userChatRepository.AddUserChat(member, createdChat.Id);
            }

            return Ok(createdChat);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetChats(CancellationToken cancellationToken)
        {
            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            IEnumerable<AppChat> chats = new List<AppChat>();

            chats = await userChatRepository.GetUserChatsAsync(userId);

            return Ok(chats.OrderBy(chat=>chat.Name).ToList());
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUsersFromChatAsync(Guid chatId, CancellationToken cancellationToken)
        {
            IEnumerable<AppUser> users = new List<AppUser>();

            users = await userChatRepository.GetUsersFromChatAsync(chatId);

            return Ok(users);
        }

        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteChat(DeleteChatDto deleteChatDto, CancellationToken cancellationToken)
        {
            var chat = await chatRepository.GetChatAsync(deleteChatDto.chatId);
            var userId = new Guid(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            if(chat == null)
            {
                return BadRequest(new {Message="Chat not found!"});
            }

            if (userId != chat.Creator_Id)
            {
                return BadRequest(new { Message = "You are not the creator of this chat!" });
            }

            var deletedChat = await chatRepository.DeleteChatAsync(deleteChatDto.chatId);

            await userChatRepository.RemoveUserChat(userId, deleteChatDto.chatId);

            return Ok(deletedChat);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendMessage(SendMessageDto sendMessageDto, CancellationToken cancellationToken)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            AppMessage message = new()
            {
                Chat_Id = sendMessageDto.ChatId,
                Sender_Id = new Guid(userId),
                Content = sendMessageDto.Content,
                Send_Date =  DateTime.Now
            };

            AppMessage createdMessage = await chatRepository.CreateMessageAsync(message);

            IEnumerable<AppUser> users = await userChatRepository.GetUsersFromChatAsync(createdMessage.Chat_Id);

            foreach(var user in users)
            {
                if(user.Id == new Guid(userId))
                {
                    continue;
                }
                string memberConnectionId = ChatHub.ConnectedUsers.FirstOrDefault(x => x.Value == user.Id).Key;
                if(memberConnectionId != null)
                {
                    await hubContext.Clients.Client(memberConnectionId).SendAsync("ReceiveMessage", createdMessage);
                }
            }

            return Ok(createdMessage);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMessages(Guid chatId, CancellationToken cancellationToken)
        {
            IEnumerable<AppMessage> messages = await chatRepository.GetMessagesByChatIdAsync(chatId);
            return Ok(messages.OrderBy(message => message.Send_Date).ToList());
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUnreadMessageCount(UpdateUnreadMessageDto unreadMessageDto, CancellationToken cancellationToken)
        {
            Console.WriteLine("CHATID" + unreadMessageDto.chatId);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            AppUserChat userChat = await userChatRepository.UpdateUnreadMessageCountAsync(new Guid(userId), unreadMessageDto.chatId,unreadMessageDto.count);

            return Ok(userChat);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUnreadMessageCount(Guid chatId, CancellationToken cancellationToken)
        {
            Console.WriteLine("CHATID" + chatId);
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            int unreadMessageCount = await userChatRepository.GetUnreadMessageCountAsync(new Guid(userId), chatId);

            return Ok(unreadMessageCount);
        }
    }
}
