using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedClassesAndUtility
{

        public class Message
        {
            public string Role { get; set; }
            public string Content { get; set; }

        public string FormattedRole
        {
            get
            {
                if (string.IsNullOrEmpty(Role))
                {
                    return string.Empty;
                }
                return char.ToUpper(Role[0]) + Role.Substring(1).ToLower() + ":";
            }
        }
    }
    
}
