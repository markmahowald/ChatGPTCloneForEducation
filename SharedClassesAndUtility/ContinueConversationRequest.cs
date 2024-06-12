using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassesAndUtility
{
    public class ContinueConversationRequest
    {
        public Guid ConversationId { get; set; }
        public string Message { get; set; }
    }
}
