<%@ Page Language="C#" %>
<%@ Import Namespace="System.IO"%>

<!DOCTYPE html>

<html>
<body>
    <%
        string pg = Page.Request["p"];

        if (!String.IsNullOrEmpty(pg))
        {
            FileInfo fl = new FileInfo(Server.MapPath("docs/"+pg));
            string src;
            
            using (StreamReader sr = new StreamReader(fl.OpenRead()))
                src = sr.ReadToEnd();
            
            %>
                <div style="text-align:right;background-color:gray;color:White;font-family:Segoe UI,Verdana,Arial;font-size:8pt;padding:3px 5px 3px 2px;">
                    <a style="color:white" href="Default.aspx">Contents</a>
                </div>
            <%
            Response.Write(src);
        }
    %>
</body>
</html>
