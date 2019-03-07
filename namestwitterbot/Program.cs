using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace namestwitterbot
{
	class Program
	{
		static void Main(string[] args)
		{
			ScrapeAllowedNames.GetUpdatedNameList();

			List<string> newNamesBoys = CheckForNewNames(Constants.AllowedBoyNamesFile, Constants.AllowedBoyNamesFileOld);
			List<string> newNamesGirls = CheckForNewNames(Constants.AllowedGirlNamesFile, Constants.AllowedGirlNamesFileOld);

			foreach (var boy in newNamesBoys)
			{
				Console.WriteLine(boy);
			}

			foreach (var girl in newNamesGirls)
			{
				Console.WriteLine(girl);
			}

			if (newNamesGirls.Count > 0)
			{

			}

			if (newNamesBoys.Count > 0)
			{

			}

			Auth.SetUserCredentials("", "", "", "");
			var user = User.GetAuthenticatedUser();
			Console.WriteLine(user);

		}

		private static List<string> CheckForNewNames(string names, string namesold)
		{
			List<string> newNames = new List<string>();
			List<string> oldNames = new List<string>();

			using (StreamReader reader =
				new StreamReader(namesold))
			{
				string name;
				while ((name = reader.ReadLine()) != null)
				{
					oldNames.Add(name.ToString());
				}
			}

			using (StreamReader reader =
				new StreamReader(names))
			{
				string name;
				while ((name = reader.ReadLine()) != null)
				{
					if(oldNames.Contains(name))
						continue;
					newNames.Add(name.ToString());
				}
			}

			return newNames;
		}
	}
}
