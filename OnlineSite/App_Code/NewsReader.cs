using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Xml;
using System.Text;

public class NewsReader
{
    public IEnumerable<NewsItem> Read()
    {
        using (XmlReader xr = XmlReader.Create(HttpContext.Current.Server.MapPath("news.xml")))
        {
            while (xr.ReadToFollowing("item"))
            {
                NewsItem ret = new NewsItem();
                ret.Title = xr.GetAttribute("title");
                ret.Date = DateTime.Parse(xr.GetAttribute("date"));
                ret.Body = xr.ReadString();
                yield return ret;
            }
        }
    }

    public string RenderNews(int? count)
    {
        StringBuilder sb = new StringBuilder();
        int c = 0;

        foreach (NewsItem n in count != null ? Take(Read(), count.Value) : Read())
        {
            if (c++ % 2 == 0)
                sb.Append("<div style='background-color:#EAF7FF;margin-top:10px;margin-left:-20px;margin-right:-20px;padding:10px 20px 10px 20px;'>");
            else
                sb.Append("<div style='background-color:white;margin-top:10px;margin-left:-20px;margin-right:-20px;padding:10px 20px 10px 20px;'>");

            sb.AppendFormat("<div class=\"title\">{0}</div>", n.Title);
            sb.AppendFormat("<div class=\"newsBody\">{0}</div>", n.Body);
            sb.AppendFormat("<div class=\"date\">Posted {0}</div>", n.Date.ToString("dd/MM/yyyy"));

            sb.Append("</div>");
        }

        return sb.ToString();
    }

    private IEnumerable<NewsItem> Take(IEnumerable<NewsItem> seq, int count)
    {
        int c = 0;

        foreach (NewsItem e in seq)
        {
            if (c++ > count)
                break;

            yield return e;
        }
            
    }
}

public sealed class NewsItem
{
    public string Title;
    public DateTime Date;
    public string Body;
}
