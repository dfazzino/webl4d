using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Shared
{
    public class _GroupsModel : List<Group>
    {
        public void OnGet()
        {
        }
    }
}

public class Group
{
	public List<MyUser> GroupUsers = new List<MyUser>();
	public bool InQueue = false;
	public bool Public = false;
	public int ID;
	public int GroupWins { get; set; }
	public int TotalCrowns
	{
		get
		{
			return GroupUsers.Sum(u => u.Crowns);
		}
	}
	public int AverageCrowns
	{
		get { return GroupUsers.Sum(u => u.Crowns) / GroupUsers.Count(); }
	}
	public int PlayerWinCount
	{
		get { return GroupUsers.Sum(u => u.Wins); }
	}
	public int PlayerLossCount
	{
		get { return GroupUsers.Sum(u => u.Losses); }
	}
	public List<Invite> Invitations = new List<Invite>();

}

public class AllGroups
{
	public List<Group> Groups = new List<Group>();
	public int AddGroup(Group g)
	{
		g.ID = Groups.Count();
		Groups.Add(g);
		return Groups.Count();
	}
}