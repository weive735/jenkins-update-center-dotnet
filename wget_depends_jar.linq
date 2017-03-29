<Query Kind="Statements">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>HtmlAgilityPack</Namespace>
</Query>

var dir = @"C:\Users\hiro\work\jenkins-update-center-dotnet\.m2\repository\";
var jars = Directory
	.GetFiles(dir, "*.jar", SearchOption.AllDirectories)
	.Select (p => p.Replace(dir, ""))
	.ToArray();

var repoRoot = @"https://mvnrepository.com/artifact";

var urls = jars
	.Select (p => p.Split('\\'))
	.Select (x => string.Join("/", repoRoot, string.Join(".", x.Take(x.Length - 3)), x[x.Length - 3], x[x.Length -2]))
	.ToArray();

var directUrls = new string[jars.Length];

Parallel.For(0, jars.Length - 1, i => {
	using (var wc = new WebClient())
	{
		var dst = Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "dump", jars[i]);
		var dstDir = Path.GetDirectoryName(dst);
		Directory.CreateDirectory(dstDir);
		
		var htmlText = wc.DownloadString(urls[i]);
		var doc = new HtmlAgilityPack.HtmlDocument();
		doc.LoadHtml(htmlText);
		var anchors = doc.DocumentNode.SelectNodes("//*[@id=\"maincontent\"]/table/tr/td/a[@class=\"vbtn\"]");
		if (anchors != null && anchors.Count > 0)
			directUrls[i] = anchors[0].Attributes["href"].Value;
	}
});

var html = new StringBuilder();
html.AppendLine("<html>");
html.AppendLine("<body>");
html.AppendLine("<table>");
html.AppendLine("<tr>");
html.AppendLine("<th>Direct Link</th>");
html.AppendLine("<th>Site Link</th>");
html.AppendLine("<th>Repository Path</th>");
html.AppendLine("</tr>");
for (int i=0; i<jars.Length; ++i)
{
	html.AppendLine("<tr>");
	
	html.AppendLine("<td>");
	if (directUrls[i] != null) {
		html.AppendFormat("<a href=\"{0}\">{1}</a>", directUrls[i], jars[i].Split('\\').Last());
		html.AppendLine();
	}
	html.AppendLine("</td>");
	
	html.AppendLine("<td>");
	html.AppendFormat("<a href=\"{0}\">{1}</a>", urls[i], Path.GetFileNameWithoutExtension(jars[i].Split('\\').Last()));
	html.AppendLine();
	html.AppendLine("</td>");
	
	html.AppendLine("<td>");
	html.AppendLine(jars[i]);
	html.AppendLine("</td>");

	html.AppendLine("</tr>");
}
html.AppendLine("</table>");
html.AppendLine("</body>");
html.AppendLine("</html>");

html.ToString().Dump();

File.WriteAllText(Path.Combine(Path.GetDirectoryName(Util.CurrentQueryPath), "index.html"), html.ToString());
