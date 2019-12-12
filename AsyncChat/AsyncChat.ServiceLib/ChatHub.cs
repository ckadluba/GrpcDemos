using AsyncChat.SharedLib.Generated;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncChat.ServiceLib
{
    public class ChatHub
    {
        private readonly ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>> _joinedUsers;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ILogger<ChatHub> logger)
        {
            _joinedUsers = new ConcurrentDictionary<string, IServerStreamWriter<ChatMessage>>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public Task HandleIncomingMessage(ChatMessage message, IServerStreamWriter<ChatMessage> responseStream)
        {
            JoinUser(message.User, responseStream);
            return DistributeMessage(message);
        }

        private void JoinUser(string user, IServerStreamWriter<ChatMessage> responseStream)
        {
            lock (_joinedUsers)
            {
                IServerStreamWriter<ChatMessage> storedResponseStream;
                if (_joinedUsers.TryGetValue(user, out storedResponseStream))
                {
                    if (storedResponseStream != responseStream)
                    {
                        throw new InvalidOperationException($"A user {user} is already in the chat!");
                    }
                }
                else
                {
                    if (_joinedUsers.TryAdd(user, responseStream))
                    {
                        _logger.LogInformation($"User {user} joined the chat.");
                    }
                }
            }
        }

        private async Task DistributeMessage(ChatMessage message)
        {
            if (message.Text == string.Empty)
            {
                return; // Suppress the init-messages
            }

            foreach (var reveiver in _joinedUsers.Where(u => u.Key != message.User))
            {
                await reveiver.Value.WriteAsync(message);
            }
        }
    }
}
