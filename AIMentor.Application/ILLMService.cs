using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIMentor.Application
{
    public interface ILLMService
    {
        Task<string> GetChatResponseAsync(string userMessage);
        IAsyncEnumerable<string> StreamChatResponseAsync(string userMessage);
    }
}
