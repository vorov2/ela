<%@ Page Language="C#" MasterPageFile="MasterPage.master" Title="Untitled Page" %>
<script runat="server">

</script>
<asp:Content ID="Title" ContentPlaceHolderID="Title" runat="server">Ela.Community</asp:Content>
<asp:Content ID="MainArea" ContentPlaceHolderID="MainArea" Runat="Server">
    <script language="javascript" type="text/javascript">selPage="community";</script>
   
    <div class="par" style="padding-top:0px;margin-bottom:15px;">
      <div class="title">Ela is</div>                  
        <div class="txt">
          a very young language that is currently under active development. You can influence Ela design and take part in discussion
          of future Ela versions. Any help is appreciated. If you are interested in getting involved, below are a couple of places where
          you can start.
        </div>
    </div>   
   
   
    <div style="background-color:#EAF7FF;margin-top:20px;margin-left:-20px;margin-right:-20px;padding:10px 20px 20px 20px;">
    <div class="txt">
      <div class="title">Ela News Group</div>                  
      Track Ela releases, ask questions about the language and development tools, request new features 
      and support.
      <div>
        <a class="dl" target="_blank" href="http://groups.google.com/group/elalang">News Group</a>
      </div>
    </div>
    
    <div class="txt" style="margin-top:20px">
      <div class="title">Ela Tracker</div>                  
      Found a bug? Want to request a feature? You can submit all new work items directly to Ela tracker,
      which is opened for everyone.
      <div>
        <a class="dl" target="_blank" href="http://code.google.com/p/elalang/issues/list">Tracker</a>
      </div>
    </div>
    
    <div class="txt" style="margin-top:20px">
      <div class="title">Ela at Rosetta Code</div>                  
      Want to solve some challengers in Ela? Peek any task you like at Rosetta Code or improve any
      of the solutions, which are already implemented.
      <div>
        <a class="dl" target="_blank" href="http://rosettacode.org/wiki/Ela">Rosetta Code</a>
      </div>
    </div>
    
    <div class="txt" style="margin-top:20px">
      <div class="title">Source Code</div>                  
      Ela source code is available under GPL v2 license. You can browse in online or connect to an TFS
      repository using a client of your choice (anonymous access is allowed).
      <div>
        <a class="dl" target="_blank" href="https://ela.codeplex.com/SourceControl/latest">TFS repository</a>
      </div>
    </div>
    </div>
   
</asp:Content>

