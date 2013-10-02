<%@ Page Language="C#" MasterPageFile="MasterPage.master" Title="Untitled Page" %>
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        NewsReader nr = new NewsReader();
        News.InnerHtml = nr.RenderNews(null);
    }
</script>

<asp:Content ID="Title" ContentPlaceHolderID="Title" runat="server">Ela.News</asp:Content>
<asp:Content ID="MainArea" ContentPlaceHolderID="MainArea" Runat="Server">
   <script language="javascript" type="text/javascript">selPage="news";</script>
   <div id="News" runat="server"></div>
</asp:Content>

