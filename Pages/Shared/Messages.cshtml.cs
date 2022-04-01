using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class MessagesModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}


public class MessageInfo
{
	public string Message { get; set; }
	DateTime StartTime { get; set; }
	TimeSpan timeSpan { get; set; }
	public string MessageFor { get; set; }
	public string messageType { get; set; } // basic, rate, yn, user
	public string messageSubType { get; set; } //invite

	public MessageInfo(string message, string For, ref int tempID, string type, string subType )
	{
		Message = message;
		//StartTime = startTime;
		//timeSpan = timeSpan;
		messageType = type;
		messageSubType = subType;
		MessageFor = For;
		tempID = 1;
	}
}

public class MessageList
{
	public List<MessageInfo> messages = new List<MessageInfo>();
}