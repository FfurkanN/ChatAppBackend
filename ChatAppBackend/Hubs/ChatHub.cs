using ChatAppBackend.Models;
using ChatAppBackend.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace ChatAppBackend.Hubs
{
    public sealed class ChatHub(IUserRepository userRepository) : Hub
    {
        public static Dictionary<string, Guid> ConnectedUsers = new();

        public async Task Connect(Guid userId)
        {
            if (ConnectedUsers.ContainsKey(Context.ConnectionId))
            {
                return;
            }
            ConnectedUsers.Add(Context.ConnectionId, userId);

            AppUser? user = await userRepository.GetUserByIdAsync(userId);
            if (user is not null)
            {
                user = await userRepository.ChangeUserStatus(user, true);
                await Clients.All.SendAsync("UserConnected", userId);
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (ConnectedUsers.TryGetValue(Context.ConnectionId, out Guid userId))
            {
                AppUser? user = await userRepository.GetUserByIdAsync(userId);
                if (user is not null)
                {
                    user = await userRepository.ChangeUserStatus(user, false);
                    await Clients.All.SendAsync("UserDisconnected", userId);
                }
                ConnectedUsers.Remove(Context.ConnectionId);
            }
        }

        // 📌 WebRTC Offer Gönderme
        public async Task SendOffer(string offer, string toUserId)
        {
            await Clients.User(toUserId).SendAsync("ReceiveOffer", offer, ConnectedUsers[Context.ConnectionId]);
        }

        // 📌 WebRTC Answer Gönderme
        public async Task SendAnswer(string answer, string toUserId)
        {
            await Clients.User(toUserId).SendAsync("ReceiveAnswer", answer, ConnectedUsers[Context.ConnectionId]);
        }

        // 📌 ICE Candidate Gönderme
        public async Task SendIceCandidate(string candidate, string toUserId)
        {
            await Clients.User(toUserId).SendAsync("ReceiveIceCandidate", candidate, ConnectedUsers[Context.ConnectionId]);
        }
    }
}
