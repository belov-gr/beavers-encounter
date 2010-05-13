using System.Text.RegularExpressions;

namespace Beavers.Encounter.Common
{
    public static class BBCode
    {
        /// <summary>
        /// Converts the input plain-text BBCode to HTML output and replacing carriage returns
        /// and spaces with <br /> and   etc...
        /// Recommended: Use this function only during storage and updates.
        /// Keep a seperate field in your database for HTML formatted content and raw text.
        /// An optional third, plain text field, with no formatting info will make full text searching
        /// more accurate.
        /// E.G. BodyText(with BBCode for textarea/WYSIWYG), BodyPlain(plain text for searching),
        /// BodyHtml(formatted HTML for output pages)
        /// </summary>
        public static string ConvertToHtml(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;
            
            // Clean your content here... E.G.:
            // content = CleanText(content);   

            // Basic tag stripping for this example (PLEASE EXTEND THIS!)
            content = StripTags(content);

            content = MatchReplace(@"\[b\]([^\]]+)\[\/b\]", "<strong>$1</strong>", content);
            content = MatchReplace(@"\[i\]([^\]]+)\[\/i\]", "<em>$1</em>", content);
            content = MatchReplace(@"\[u\]([^\]]+)\[\/u\]", @"<span style=""text-decoration:underline"">$1</span>", content);
            content = MatchReplace(@"\[s\]([^\]]+)\[\/s\]", @"<span style=""text-decoration:line-through"">$1</span>", content);   

            //// Colors and sizes
            content = MatchReplace(@"\[color=(#[0-9a-fA-F]{6}|[a-z-]+)]([^\]]+)\[\/color\]", @"<span style=""color:$1;"">$2</span>", content);
            content = MatchReplace(@"\[size=([2-5])]([^\]]+)\[\/size\]", @"<span style=""font-size:$1em; font-weight:normal;"">$2</span>", content);   

            //// Text alignment
            content = MatchReplace(@"\[left\]([^\]]+)\[\/left\]", @"<span style=""text-align:left"">$1</span>", content);
            content = MatchReplace(@"\[right\]([^\]]+)\[\/right\]", @"<span style=""text-align:right"">$1</span>", content);
            content = MatchReplace(@"\[center\]([^\]]+)\[\/center\]", @"<span style=""text-align:center"">$1</span>", content);
            content = MatchReplace(@"\[justify\]([^\]]+)\[\/justify\]", @"<span style=""text-align:justify"">$1</span>", content);   

            //// HTML Links
            content = MatchReplace(@"\[url\]([^\]]+)\[\/url\]", @"<a href=""$1"">$1</a>", content);
            content = MatchReplace(@"\[url=([^\]]+)]([^\]]+)\[\/url\]", @"<a href=""$1"">$2</a>", content);   

            //// Images
            content = MatchReplace(@"\[img\]([^\]]+)\[\/img\]", @"<img src=""$1"" alt="""" />", content);
            content = MatchReplace(@"\[img=([^\]]+)]([^\]]+)\[\/img\]", @"<img src=""$2"" alt=""$1"" />", content);   

            //// Headers
            content = MatchReplace(@"\[h1\]([^\]]+)\[\/h1\]", "<h1>$1</h1>", content);
            content = MatchReplace(@"\[h2\]([^\]]+)\[\/h2\]", "<h2>$1</h2>", content);
            content = MatchReplace(@"\[h3\]([^\]]+)\[\/h3\]", "<h3>$1</h3>", content);
            content = MatchReplace(@"\[h4\]([^\]]+)\[\/h4\]", "<h4>$1</h4>", content);
            content = MatchReplace(@"\[h5\]([^\]]+)\[\/h5\]", "<h5>$1</h5>", content);
            content = MatchReplace(@"\[h6\]([^\]]+)\[\/h6\]", "<h6>$1</h6>", content);   

            //// Horizontal rule
            content = MatchReplace(@"\[hr\]", "<hr />", content);

            //// Lists
            content = MatchReplace(@"\[*\]([^\[]+)", "<li>$1</li>", content);
            content = MatchReplace(@"\[list\]([^\]]+)\[\/list\]", "<ul>$1</ul><p>", content);
            content = MatchReplace(@"\[list=1\]([^\]]+)\[\/list\]", "</p><ol>$1</ol><p>", content);

            //// Set a maximum quote depth (In this case, hard coded to 3)
            //for (int i = 1; i < 3; i++)
            //{
            //    // Quotes
            //    content = MatchReplace(@"\[quote=([^\]]+)@([^\]]+)|([^\]]+)]([^\]]+)\[\/quote\]", @"</p><div class=""block""><blockquote><cite>$1 <a href=""" + QuoteUrl("$3") + @""">wrote</a> on $2</cite><hr /><p>$4</p></blockquote></div></p><p>", content);
            //    content = MatchReplace(@"\[quote=([^\]]+)@([^\]]+)]([^\]]+)\[\/quote\]", @"</p><div class=""block""><blockquote><cite>$1 wrote on $2</cite><hr /><p>$3</p></blockquote></div><p>", content);
            //    content = MatchReplace(@"\[quote=([^\]]+)]([^\]]+)\[\/quote\]", @"</p><div class=""block""><blockquote><cite>$1 wrote</cite><hr /><p>$2</p></blockquote></div><p>", content);
            //    content = MatchReplace(@"\[quote\]([^\]]+)\[\/quote\]", @"</p><div class=""block""><blockquote><p>$1</p></blockquote></div><p>", content);
            //}   

            // Put the content in a paragraph
            //content = "<p>" + content + "</p>";   

            // Clean up a few potential markup problems
            content = content.Replace("\t", "    ")
                .Replace("\r\n", "<br />")
                .Replace("</p></li></p>", "");   

            return content;
        }

        public static string QuoteUrl(string ID)
        {
            return "/getpost.aspx?id="+ID;
        }

        /// <summary>
        /// Strip any existing HTML tags
        /// </summary>
        /// <param name="content">Raw input from user</param>
        /// <returns>Tag stripped storage safe text</returns>
        public static string StripTags(string content)
        {
	        return MatchReplace(@"< [^>]+>", "", content, true, true, true);
        }

        public static string MatchReplace(string pattern, string match, string content)
        {
	        return MatchReplace(pattern, match, content, false, false, false);
        }

        public static string MatchReplace(string pattern, string match, string content, bool multi)
        {
	        return MatchReplace(pattern, match, content, multi, false, false);
        }

        public static string MatchReplace(string pattern, string match, string content, bool multi, bool white)
        {
	        return MatchReplace(pattern, match, content, multi, white);
        }

        /// <summary>
        /// Match and replace a specific pattern with formatted text
        /// </summary>
        /// <param name="pattern">Regular expression pattern</param>
        /// <param name="match">Match replacement</param>
        /// <param name="content">Text to format</param>
        /// <param name="multi">Multiline text (optional)</param>
        /// <param name="white">Ignore white space (optional)</param>
        /// <returns>HTML Formatted from the original BBCode</returns>
        public static string MatchReplace(string pattern, string match, string content, bool multi, bool white, bool cult)
        {
	        if (multi && white && cult)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
	        else if (multi && white)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnoreCase);
	        else if (multi && cult)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.CultureInvariant);
	        else if (white && cult)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.CultureInvariant);
	        else if (multi)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.Multiline);
	        else if (white)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
	        else if (cult)
		        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

	        // Default
	        return Regex.Replace(content, pattern, match, RegexOptions.IgnoreCase);
        }
    }
}
