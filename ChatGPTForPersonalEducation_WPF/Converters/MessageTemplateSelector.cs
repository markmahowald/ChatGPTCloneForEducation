using SharedClassesAndUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ChatGPTForPersonalEducation_WPF.Converters
{
    public class MessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate UserTemplate { get; set; }
        public DataTemplate AssistantTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var message = item as Message;
            if (message != null)
            {
                if (message.Role == "user")
                {
                    return UserTemplate;
                }
                else
                {
                    return AssistantTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}
