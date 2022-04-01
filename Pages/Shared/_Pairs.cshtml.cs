using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages.Shared
{
    public class _PairsModel : CurrentPairs
	{
        public void OnGet()
        {
        }
    }
}

public class Pair
{
	public List<Group> Groups = new List<Group>();
	public List<Group> Next = new List<Group>();

	public Pair(Group g1, Group g2)
	{
		Groups.Add(g1);
		Groups.Add(g2);
		g1.InQueue = true;
		g2.InQueue = true;
	}
	public void AddNext(Group g1)
	{
		Next.Add(g1);
		g1.InQueue = true;
	}

}

public class CurrentPairs
{
	public List<Pair> Pairs = new List<Pair>();

}