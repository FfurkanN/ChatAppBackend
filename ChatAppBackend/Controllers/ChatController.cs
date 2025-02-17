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
    public class ChatController(IHubContext<ChatHub> hubContext, IChatRepository chatRepository, IUserRepository userRepository) : ControllerBase
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
            AppUser? user = await userRepository.GetUserByIdAsync(createChatDto.CreatorId);
            if(user == null)
            {
                return BadRequest(new { Message = "User not found!" });
            }
            chat.Members.Add(createChatDto.CreatorId);
            foreach (var member in createChatDto.members)
            {
                chat.Members.Add(member);
            }
            AppChat createdChat = await chatRepository.CreateChatAsync(chat);

            foreach (var member in createdChat.Members)
            {
                await userRepository.AddChatToUserAsync(member, createdChat.Id);
            }

            return Ok(createdChat);
        }



        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetChats(Guid userId, CancellationToken cancellationToken)
        {
            List<AppChat> Chats = new List<AppChat>();
            AppUser? user = await userRepository.GetUserByIdAsync(userId);
            if(user == null)
            {
                return BadRequest(new {Message="User not found!"});
            }

            foreach(var id in user.Chats)
            {
                AppChat chat = await chatRepository.GetChatAsync(id);
                Chats.Add(chat);
            }

            return Ok(Chats.OrderBy(chat=>chat.Name).ToList());
        }
        [HttpDelete]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteChat(DeleteChatDto deleteChatDto, CancellationToken cancellationToken)
        {
            var chat = await chatRepository.GetChatAsync(deleteChatDto.chatId);
            if (deleteChatDto.userId != chat.Creator_Id)
            {
                return BadRequest(new { Message = "You are not the creator of this chat!" });
            }
            AppUser user = await userRepository.RemoveChatFromUserAsync(deleteChatDto.userId, deleteChatDto.chatId);
            if(user == null)
            {
                return BadRequest(new { Message="User not found" });
            }
            var deletedChat = await chatRepository.DeleteChatAsync(deleteChatDto.chatId);

            return Ok(deletedChat);
        }
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SendMessage(SendMessageDto sendMessageDto, CancellationToken cancellationToken)
        {
            AppMessage message = new()
            {
                Chat_Id = sendMessageDto.ChatId,
                Sender_Id = sendMessageDto.SenderId,
                Content = sendMessageDto.Content,
                Send_Date = sendMessageDto.SendDate,
            };

            AppMessage createdMessage = await chatRepository.CreateMessageAsync(message);
            AppChat chat = await chatRepository.GetChatAsync(sendMessageDto.ChatId);

            foreach(var member in chat.Members)
            {
                if(member == sendMessageDto.SenderId)
                {
                    continue;
                }
                string memberConnectionId = ChatHub.ConnectedUsers.FirstOrDefault(x => x.Value == member).Key;
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
    }
}
