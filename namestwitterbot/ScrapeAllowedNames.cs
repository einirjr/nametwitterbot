using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;

namespace namestwitterbot
{
	/// <summary>
	/// This scraper uses the HtmlAgilityPack to fetch a list of allowed names from the
	/// Icelandic National Register's (Þjóðskrá Íslands) website, www.island.is and
	/// writes them to two separate files, one for boy names and another for girl names.
	/// </summary>
	public static class ScrapeAllowedNames
	{
		public static void GetUpdatedNameList()
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument doc = web.Load(Constants.AllowedBoyNamesUrl);

			if (File.Exists(Constants.AllowedBoyNamesFile))
			{
				if (File.Exists(Constants.AllowedBoyNamesFileOld))
					File.Delete(Constants.AllowedBoyNamesFileOld);
				File.Move(Constants.AllowedBoyNamesFile, Constants.AllowedBoyNamesFileOld);
			}
			GenerateNameList(Constants.AllowedBoyNamesFile, doc, out int counter);
			Console.WriteLine($"\n{counter} boy names scraped, last scrape was {File.ReadAllLines(Constants.AllowedBoyNamesFileOld).Length} names.");

			doc = web.Load(Constants.AllowedGirlNamesUrl);

			if (File.Exists(Constants.AllowedGirlNamesFile))
			{
				if (File.Exists(Constants.AllowedGirlNamesFileOld))
					File.Delete(Constants.AllowedGirlNamesFileOld);
				File.Move(Constants.AllowedGirlNamesFile, Constants.AllowedGirlNamesFileOld);
			}
			GenerateNameList(Constants.AllowedGirlNamesFile, doc, out counter);
			Console.WriteLine($"{counter} girl names scraped, last scrape was {File.ReadAllLines(Constants.AllowedGirlNamesFileOld).Length} names.\n");
		}

		/// <summary>
		/// Called to actually scrape the names from the website, given the filepath to write names to,
		/// html document to scrape and a counter to show how many names were added to the file.
		/// </summary>
		/// <param name="filepath">Path to the local file to write names to</param>
		/// <param name="doc">Document containing names in HTML format</param>
		/// <param name="counter">Counter used to show how many names were added to file</param>
		private static void GenerateNameList(string filepath, HtmlDocument doc, out int counter)
		{
			Dictionary<string, int> removeDuplicates = new Dictionary<string, int>();
			counter = 0;

			using (StreamWriter file =
				new StreamWriter(filepath))
			{
				foreach (var letter in doc.DocumentNode.SelectNodes("/html/body/div[1]/div/main/div[2]/div/div[2]/ul"))
				{
					foreach (var childNode in letter.ChildNodes)
					{
						string name = childNode.InnerText.Remove(childNode.InnerText.IndexOf(' '));
						if (!removeDuplicates.ContainsKey(name))
						{
							//Console.WriteLine($"Adding {name}");
							removeDuplicates.Add(name, 1);
							file.WriteLine(name);
							counter++;
						}
					}
				}
			}
		}
	}
}