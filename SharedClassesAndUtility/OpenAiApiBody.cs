using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassesAndUtility
{
     public class OpenAiApiBody
    {
        public string Model { get; set; }
        public int MaxTokens { get; set; }
        public List<Message> Messages { get; set; }
        //    max_tokens = 150,
        //    messages = _conversationHistory
    }
}
