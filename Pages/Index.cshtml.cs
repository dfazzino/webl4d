using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System.Web;
using Microsoft.Extensions.Caching.Memory;

namespace WebApplication1.Pages
{
	public class IndexModel : PageModel
	{
		public AllGroups AllGroups = new AllGroups();
		public List<Group> CurrentGroups = new List<Group>();
		public CurrentPairs currentPairs = new CurrentPairs();
		public AllUsers allUsers = new AllUsers();
		public MessageList messageList = new MessageList();

		private readonly IMemoryCache _memoryCache;

		public IndexModel(IMemoryCache memoryCache) => _memoryCache = memoryCache;


		public PartialViewResult OnPost()
		{
			LoadFromMemory();
			AllGroups = (AllGroups) _memoryCache.Get("AllGroups");
			currentPairs = (CurrentPairs) _memoryCache.Get("CurrentPairs");
			//currentPairs.Pairs[0].AddNext(AllGroups[2]);
			var TempViewData = new ViewDataDictionary<CurrentPairs>(ViewData, currentPairs);
			return new PartialViewResult()
			{
				ViewName = "_Pairs",
				ViewData = TempViewData,

			};

		}

		public PartialViewResult OnPostNewGroup(string Name)
		{
			LoadFromMemory();
			AllGroups = (AllGroups) _memoryCache.Get("AllGroups");
			Group tempGroup = new Group();
			tempGroup.GroupUsers.Add(new MyUser() { Name = Name });
			AllGroups.AddGroup(tempGroup);
			var TempViewData = new ViewDataDictionary<AllGroups>(ViewData, AllGroups);
			return new PartialViewResult()
			{
				ViewName = "_Groups",
				ViewData = TempViewData,

			};
		}
		public PartialViewResult OnPostJoinGroup(string GroupId)
		{
			var tempUser = (MyUser)AllGroups.Groups.SelectMany(a => a.GroupUsers).Select(b => b).Where(c => c.Id == "QQQQ").FirstOrDefault();
			if (tempUser == null)
			{
				LoadFromMemory();
				GroupId = GroupId.Replace("group_", "");
				Group tempGroup = AllGroups.Groups.Where(g => g.ID == Int32.Parse(GroupId) && (g.Invitations.Where(i => i.InviteTo == "QQQQ").FirstOrDefault() != null || g.Public == true)).FirstOrDefault();
				if (tempGroup != null)
				{
					tempUser = allUsers.Users.Where(u => u.Id == "QQQQ").FirstOrDefault();
					tempUser.Status = "ingroup";
					tempGroup.GroupUsers.Add(tempUser);

					Invite tempInvite = tempGroup.Invitations.Where(i => i.InviteTo == "QQQQ").FirstOrDefault();
					if (tempGroup != null)
					{
						tempGroup.Invitations.Remove(tempInvite);
					}
				}
				//AllGroups.AddGroup(tempGroup);

			}
			var TempViewData = new ViewDataDictionary<AllGroups>(ViewData, AllGroups);
			return new PartialViewResult()
			{
				ViewName = "_Groups",
				ViewData = TempViewData,

			};
		}
		public PartialViewResult OnPostInvite(string ID)
		{
			LoadFromMemory();
			var info = new Object();
			Group tempGroup = AllGroups.Groups[4];
			if (tempGroup.Invitations.Count < 3)
			{
				if (AllGroups != null)
				{
					ID = ID.Replace("user_", "");
					var findUser = allUsers.Users.Where(u => u.Id == ID && u.Status == "lfg").FirstOrDefault();
					if (findUser != null)
					{
						var tempUser = (MyUser)AllGroups.Groups.SelectMany(a => a.GroupUsers).Select(b => b).Where(c => c.Id == ID).FirstOrDefault();
						if (tempUser == null)
						{
							//)).ID == Int16.Parse(ID)) {
							//
							info = new { InviteNote = findUser.Name + " has been invited to join your group." };
							tempGroup.Invitations.Add(new Invite(From: "ABC1", To: ID, messageList: messageList));
						}
						else
						{
							info = new { Test = "This person is in a group already!" };
						}
					}
					else
					{
					info = new { Test = "This person isn't available!" };
					}
				}
				else
				{
				info = new { Test = "Big Fail." };
				}
			}
			else
			{
				info = new { Test = "Too many invitations!" };
			}

			var TempViewData = new ViewDataDictionary<MessageList>(ViewData, messageList);
			return new PartialViewResult()
			{
				ViewName = "Messages",
				ViewData = TempViewData,

			};
		}
		public PartialViewResult OnPostReady(string GroupID)
		{
			LoadFromMemory();

			int tempID = Int32.Parse(GroupID.Replace("group_", ""));
			Group tempGroup = AllGroups.Groups.Find(g => g.ID == tempID);
			tempGroup.InQueue = true;
			Random rnd = new Random();
			int num = rnd.Next(0, currentPairs.Pairs.Count());
			currentPairs.Pairs[num].AddNext(tempGroup);
			//AllGroups.AddGroup(tempGroup);
			var TempViewData = new ViewDataDictionary<AllGroups>(ViewData, AllGroups);
			return new PartialViewResult()
			{
				ViewName = "_Groups",
				ViewData = TempViewData,

			};
			//var TempViewData2 = new ViewDataDictionary<CurrentPairs>(ViewData, currentPairs);
			//return new PartialViewResult()
			//{
			//	ViewName = "_Pairs",
			//	ViewData = TempViewData,

			//};

		}
		public PartialViewResult OnPostLeave(string GroupID, string userID)
		{
			LoadFromMemory();
			AllGroups = (AllGroups)_memoryCache.Get("AllGroups");
			currentPairs = (CurrentPairs)_memoryCache.Get("CurrentPairs");
			int tempID = Int32.Parse(GroupID.Replace("group_", ""));
			Group tempGroup = AllGroups.Groups.Find(g => g.ID == tempID);
			MyUser tempUser = tempGroup.GroupUsers.Find(u => u.Id == userID);
			tempGroup.GroupUsers.Remove(tempUser);

			var TempViewData = new ViewDataDictionary<AllGroups>(ViewData, AllGroups);
			return new PartialViewResult()
			{
				ViewName = "_Groups",
				ViewData = TempViewData,

			};
			//var TempViewData2 = new ViewDataDictionary<CurrentPairs>(ViewData, currentPairs);
			//return new PartialViewResult()
			//{
			//	ViewName = "_Pairs",
			//	ViewData = TempViewData,

			//};

		}

		public JsonResult OnPostRefresh(string userID)
		{
			LoadFromMemory();
			var TempViewData = new ViewDataDictionary<AllGroups>(ViewData, AllGroups);
			var testData = new { Test = "test" };
			return new JsonResult(testData);

		}
			public void OnGet()
		{
			LoadFromMemory();
		}

		public void LoadFromMemory()
		{
			var groups = _memoryCache.TryGetValue("AllGroups", out AllGroups);

			if (AllGroups == null)
			{
				AllGroups = new AllGroups();
				allUsers = new AllUsers();
				currentPairs = new CurrentPairs();
				Group tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "Joe", Id = "ABC1", Crowns = 1, Wins = 4, Losses = 3 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Dave", Id = "ABC2", Crowns = 1, Wins = 4, Losses = 0 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Jen", Id = "ABC3", Crowns = 1, Wins = 4, Losses = 2 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Steve", Id = "ABC4", Crowns = 1, Wins = 4, Losses = 1 });
				AllGroups.AddGroup(tempGroup);

				tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "Adam", Id = "ABC5", Crowns = 0, Wins = 2, Losses = 2 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "PSim", Id = "ABC6", Crowns = 1, Wins = 4, Losses = 0 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Abduul", Id = "ABC7", Crowns = 1, Wins = 6, Losses = 2 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Nevermore", Id = "ABC8", Crowns = 2, Wins = 4, Losses = 5 });
				AllGroups.AddGroup(tempGroup);


				tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "Blue", Id = "ABC9", Crowns = 0, Wins = 3, Losses = 1 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Rising", Id = "ABC10", Crowns = 0, Wins = 5, Losses = 0 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Leky", Id = "ABC11", Crowns = 3, Wins = 7, Losses = 3 });
				tempGroup.GroupUsers.Add(new MyUser { Name = "Yamz", Id = "ABC12", Crowns = 1, Wins = 8, Losses = 0 });
				AllGroups.AddGroup(tempGroup);

				tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "User1", Id = "ABC13", Crowns = 1, Wins = 4, Losses = 0 , Status = "ingame"});
				tempGroup.GroupUsers.Add(new MyUser { Name = "User2", Id = "ABC14", Crowns = 0, Wins = 0, Losses = 5 , Status = "ingame"});
				tempGroup.GroupUsers.Add(new MyUser { Name = "User3", Id = "ABC15", Crowns = 2, Wins = 9, Losses = 1 , Status = "ingame"});
				tempGroup.GroupUsers.Add(new MyUser { Name = "User4", Id = "ABC16", Crowns = 0, Wins = 0, Losses = 0 , Status = "ingame"});
				AllGroups.AddGroup(tempGroup);

				tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserA", Id = "ABC17", Crowns = 1, Wins = 6, Losses = 0, Status="lfg" });
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserB", Id = "ABC18", Crowns = 0, Wins = 3, Losses = 3, Status="lfg" });
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserC", Id = "ABC19", Crowns = 3, Wins = 4, Losses = 0, Status="lfg" });
				AllGroups.AddGroup(tempGroup);

				tempGroup = new Group();
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserE", Id = "ABC21", Crowns = 2, Wins = 3, Losses = 2, Status="away" });
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserF", Id = "ABC22", Crowns = 1, Wins = 3, Losses = 4, Status="away" });
				tempGroup.GroupUsers.Add(new MyUser { Name = "UserG", Id = "ABC23", Crowns = 0, Wins = 5, Losses = 0, Status="away" });
				tempGroup.Public = true;
				AllGroups.AddGroup(tempGroup);
				allUsers.AddAllUsers(AllGroups);
				allUsers.Users.Add(new MyUser() { Name = "ZZ", Id = "QQQQ", Status = "lfg" });
				_memoryCache.Set("allUsers", allUsers);
				_memoryCache.Set("AllGroups", AllGroups);

				currentPairs.Pairs.Add(new Pair(AllGroups.Groups[0], AllGroups.Groups[1]));
				currentPairs.Pairs.Add(new Pair(AllGroups.Groups[2], AllGroups.Groups[3]));
				currentPairs.Pairs[0].AddNext(AllGroups.Groups[2]);
				_memoryCache.Set("CurrentPairs", currentPairs);
				_memoryCache.Set("allUsers", allUsers);
				_memoryCache.Set("messageList", messageList);
				//currentPairs.Pairs[0].AddNext(AllGroups[3]);
			}
			else
			{
				AllGroups = (AllGroups)_memoryCache.Get("AllGroups");
				currentPairs = (CurrentPairs)_memoryCache.Get("CurrentPairs");
				allUsers= (AllUsers)_memoryCache.Get("allUsers");
				messageList = (MessageList)_memoryCache.Get("messageList");
			}
		}
	}
}

public class MyUser
{
	public string Id { get; set; }
	public string Name { get; set; }
	public int Crowns { get; set; }
	public int Wins { get; set; }
	public int Losses { get; set; }
	public int GroupID { get; set; }
	public string Status { get; set; }
}

public class Invite
{
	public  Invite(string From, string To, MessageList messageList = null)
	{
		int tempID = new int();

		InviteFrom = From;
		InviteTo = To;
		if(messageList != null) {
			messageList.messages.Add(new MessageInfo("You have been invited to join " + From + "'s group!", To, ref tempID, "user", ""));
			ToMessageId = tempID;
			messageList.messages.Add(new MessageInfo("You have invited " + To + " to join your group!", From, ref tempID, "basic", ""));
			FromMessageId = tempID;
			//FromMessageId = messageList.messages.Add(new MessageInfo() { MessageFor = From, Message = "You have invited " + To + "to join your group!" });
		}
	}
	public int ID { get; set; }
	public string InviteFrom { get; set; }
	public string InviteTo { get; set; }

	public int ToMessageId;
	public int FromMessageId;
}

public class AllUsers
{
	public List<MyUser> Users = new List<MyUser>();
	public bool AddAllUsers(AllGroups allGroups)
	{
		foreach(Group group in allGroups.Groups)
		{
			foreach(MyUser user in group.GroupUsers)
			{
				Users.Add(user);
			}
		}
		return true;
	}
}

public class MatchMaker
{

}
