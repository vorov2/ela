<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Xml" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    public void Link(XmlNode n)
    {
        Link(n.Attributes["name"].Value, n.Attributes["file"].Value);
    }
    
    public void Link(string title, string page)
    {
        Write("<div class='acont'><a href='Article.aspx?p={0}'>{1}</a></div>", page, title);
    }

    public void StartCategory(XmlNode n)
    {
        StartCategory(n.Attributes["name"].Value);
    }

    public void StartCategory(string title)
    {
        Write("<h2>{0}</h2>", title);
        Write("<div style='padding-left:5px'>");
    }

    public void EndCategory()
    {
        Write("</div>");
    }

    public void StartFolder(XmlNode n)
    {
        StartFolder(n.Attributes["name"].Value);
    }

    public void StartFolder(string title)
    {
        Write("<h3>{0}</h3>", title);
        Write("<div style='padding-left:10px'>");
    }

    public void EndFolder()
    {
        Write("</div>");
    }

    private StringBuilder sb = new StringBuilder();
    private void Write(string format, params object[] args)
    {
       sb.AppendFormat(String.Format(format, args));   
    }

    private void GenFolder(XmlNode n, bool silent)
    {
        if (!silent)
            StartFolder(n);
        
        XmlNodeList folders = n.SelectNodes("folder");

        foreach (XmlNode f in folders)
            GenFolder(f, false);

        XmlNodeList nodes = n.SelectNodes("doc");

        foreach (XmlNode nn in nodes)
            Link(nn);
        
        if (!silent)
            EndFolder();
    }
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ela Documentation Library</title>
    <style>
    body,h1,h2,h3,html
    {
        font-family:Segoe UI,Verdana,Arial;
        font-size:9pt;
        width:100%;
        height:100%;
    }
    
    body 
    {
        overflow:hidden;
    }
    
    h2
    {
        font-weight:bold;
        color:Gray;
        font-size:12pt;
        margin:0px;
        margin-top:15px;
        margin-bottom:5px;
        padding:0px;
    }
    
    h3
    {
        font-weight:bold;
        margin:0px;
        padding:0px;
    }
    
    div.acont
    {
        padding:2px;
    }
    
    a,a:hover,a:active
    {
        color:blue;
        text-decoration:none;
    }
    
    a:hover
    {
        text-decoration:underline;
    }
    </style>
    <script language="javascript">
    function adjust() {
      document.getElementById("cco").style.height = document.body.offsetHeight + "px"
    }
    </script>
</head>
<body style="margin:0px" onload="adjust()" onresize="adjust()">
    <%
        string xml = Server.MapPath("docs/_dir.xml");
        XmlDocument doc = new XmlDocument();
        doc.Load(xml);

        StartCategory("Getting Started");
        {
            XmlNodeList nodes = doc.SelectNodes("//documentation/doc");

            foreach (XmlNode n in nodes)
                Link(n);
        }
        EndCategory();

        StartCategory("Language Reference");
        {
            XmlNodeList nodes = doc.SelectNodes("//folder[@name='Language Reference']/doc");

            foreach (XmlNode n in nodes)
                Link(n);
        }
        EndCategory();

        StartCategory("Standard Library");
        {
            XmlNode folder = doc.SelectSingleNode("//folder[@name='Standard Library']");
            GenFolder(folder, true);            
        }
        EndCategory();
    %>
    <table cellpadding="0" cellspacing="0" style="width:100%;height:100%;">
        <tr>
            <td style="width:50px;border-right:solid 1px darkgray;background-color:gray;vertical-align:bottom;height:100%">
                <img src="contents.png"/>
            </td>
            <td style="vertical-align:top">
                <div id="cco" style="overflow:auto;height:1000px">
                  <table>
                      <tr>
                           <td style="padding-left:20px;width:100%">
                              <%= sb.ToString() %>
                          </td>
                          <td style="text-align:right;vertical-align:top;padding-right:10px;padding-top:20px;font-size:14pt;font-weight:bold;color:gray">
                              <img src="logobig.png" style="width:221px;height:104px" align="absmiddle" />
                              <div style="padding-top:5px;padding-right:10px">
                                  Documentation Library
                              </div>
                          </td>
                      </tr>
                  </table>
                </div>
            </td>
        </tr>
    </table>
</body>
</html>
