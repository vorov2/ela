<%@ Page Language="C#" MasterPageFile="MasterPage.master" Title="Untitled Page" %>
<%@ Assembly Name="Eladoc" %>
<%@ Import Namespace="Eladoc.Lexers" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Collections.Generic" %>
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ShowSnippet();
        ReadNews();
    }

    void ReadNews()
    {
        NewsReader nr = new NewsReader();
        News.InnerHtml = nr.RenderNews(3);
    }
    
    void ShowSnippet()
    {
        string p = Server.MapPath("snippets/");
        DirectoryInfo dir = new DirectoryInfo(p);
        FileInfo[] arr = dir.GetFiles("*.ela");

        Random rnd = new Random(DateTime.Now.Millisecond);
        int v = rnd.Next(0, arr.Length * 10);
        FileInfo fi = arr[v / 10];

        SnippetReader sr = new SnippetReader();
        ElaCode.InnerHtml = sr.ReadSnippet(fi.FullName);
    }
</script>

<asp:Content ID="Title" ContentPlaceHolderID="Title" runat="server">Ela, dynamic functional language</asp:Content>

<asp:Content ID="MainArea" ContentPlaceHolderID="MainArea" Runat="Server">
    <script language="javascript" type="text/javascript">selPage="home";</script>
    <div class="txt">
      <div class="title">Ela is</div>                  
      a simple, yet powerful modern functional language with a state-of-art syntax. Ela combines 
      strict and lazy evaluation, dynamic typing and features, which are normally adopted by statically
      typed languages, such as algebraic data types and Haskell style type classes.
    </div>

    <div class="txt" style="margin-top:20px">
      <div class="title">Ela can</div>                  
      be used to study and teach functional programming, for prototyping, for writing theorem provers, for scripting, 
      as well as for development of applications in a pure functional way. Ela comes with a rich standard
      library, interactive console and a graphical development environment. Ela also offers a flexible and powerful 
      interface to .NET programming languages, such as C#.
    </div>
    
    <div style="margin-top:10px;margin-bottom:10px">
        <img src="includes\line.png" />
    </div>
    
    <div style="width:380px;float:left;">
      <div class="title">A taste of Ela</div>                  
      <pre id="ElaCode" tab-width="2" runat="server">          
      </pre>
    </div>
    
    <div style="width:340px;float:left;margin-left:15px">
      <div class="title" style="margin-bottom:7px">Getting started with Ela</div>                  
      <div class="bullet">•&nbsp;<a target="_blank" href="http://elalang.net/elac.aspx" class="dl"><b>Try it in your browser!</b></a></div>
      <div class="bullet">•&nbsp;<a target="_blank" href="About.aspx#features" class="dl">Ela distinctive features</a></div>
      <div class="bullet">•&nbsp;<a target="_blank" href="http://groups.google.com/group/elalang" class="dl"><b>Ela news group (questions and support)</b></a></div>
      <div class="bullet">•&nbsp;<a target="_blank" href="http://elalang.net/docs/Article.aspx?p=whatsnew.htm" class="dl">What's new in the last release</a></div>
      <div class="bullet">•&nbsp;<a target="_blank" href="http://elalang.net/docs/" class="dl"><b>Language and library reference</b></a></div>
      <div class="bullet">•&nbsp;<a target="_blank" href="https://ela.codeplex.com/discussions" class="dl">Ela discussion board</a></div>
    </div>
    
    <div style="margin-top:10px;margin-bottom:10px">
        <img src="includes\line.png" />
    </div>
    
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:295px;border:solid 1px darkgray">
                <img src="includes\elide_screen1.png" />        
            </td>
            <td style="vertical-align:top;padding-left:10px">
                <div class="txt">
                    Ela is distributed as a part of Ela Platform, that includes 
                    an integrated graphical development environment <i>Elide</i> that will help you to quickly get started with Ela.
                    <i>Elide</i> features a powerful Ela code editor with code folding, autocomplete and support for "highlight errors as you type" feature. Elide also
                    includes Outline view for Ela source code files, task management, debugging capabilities, Ela object file editor 
                    and many other productivity tools.<br />
                    <a class="dl" href="https://ela.codeplex.com/releases" target="_blank"><b>Download Elide with Ela Platform</b></a>
                </div>
            </td>
            
            <td style="width:120px;text-align:center;vertical-align:top">
                <a href="http://elalang.net/ElaBook.aspx" style="text-decoration:none;font-weight:bold;" target="_blank">
                    <img src="includes\ela_cover.png" border="0" />
                    <div style="font-size:8pt;text-align:center">Download a book <br />about Ela language</div>
                </a>
            </td>
        </tr>
    </table>
    
    <div style="margin-top:10px;margin-bottom:10px">
        <img src="includes\line.png" />
    </div>
    
    <div id="News" runat="server">
    </div>
</asp:Content>

