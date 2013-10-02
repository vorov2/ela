<%@ Page Language="C#" MasterPageFile="MasterPage.master" Title="Untitled Page" %>
<script runat="server">
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        SnippetReader sr = new SnippetReader();
        ElaCode1.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/fact.ela"));
        ElaCode2.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/fun.ela"));
        ElaCode3.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/modules.ela"));
        ElaCode4.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/algebraic.ela"));
        ElaCode5.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/class.ela"));
        ElaCode6.InnerHtml = sr.ReadSnippet(Server.MapPath("snippets/lazy.ela"));
    }
</script>

<asp:Content ID="Title" ContentPlaceHolderID="Title" runat="server">Ela.About</asp:Content>
<asp:Content ID="MainArea" ContentPlaceHolderID="MainArea" Runat="Server">
    <script language="javascript" type="text/javascript">selPage="about";</script>
   
   
    <div class="par" style="padding-top:0px">
      <div class="title">Ela is</div>                  
        <div class="txt">
          is a free (both noncommercial and open source) functional programming language. Ela runs      
          on .NET and Mono and supports Windows, Linux, Mac OS and many other environments. Ela can run in
          both 32-bit and 64-bit modes. Ela programs can be distributed and executed either as source code 
          files or as binary object files. Ela fully supports interactive mode (REPL) and is shipped with
          a graphical development environment (Elide).
        </div>
    </div>

    <div class="par">
        <pre id="ElaCode1" tab-width="2" runat="server" style="float:left;margin-right:10px;width:340px;"> 
        </pre>
        <div class="txt">
          For those coming from C style languages, such as C++, Java or C#, Ela might look a little bit
          cryptic. But new things are not necessarily bad, right? Also Ela is in fact a very simple
          language and is easy to learn. Hard to believe in that? Just give it a try! Why bother? Because, in
          spite of its simplicity, Ela is a powerful and expressive language that allows to solve 
          complex tasks in a simple way. What's the catch?
        </div>
    </div>
    
    <div style="background-color:#EAF7FF;margin-top:20px;margin-left:-20px;margin-right:-20px;padding:0px 20px 20px 20px;">
    <div class="par" style="margin-top:15px;">
       <div class="title" style="clear:both">Ela is a functional language</div>                  
        <div class="txt" style="margin-top:5px;">
          You've probably heard this multiple times; there are plenty of languages that claim to be
          hybryd and to provide full support for functional programming. However, Ela is quite different
          from them. Unlike many other languages, Ela doesn't make any trade-offs, trying to combine
          distinct programming paradigms that don't fit well together. And it makes Ela a more 
          powerful and flexible language than any of these hybrids, which is perfectly logical if
          you think about it. Why would you need to extend a modern car with a steam engine? Or why would
          you need to extend a functional language with object orientation in mainstream style? If you want 
          to argue with that, first give Ela a try.
        </div>
    </div>
    
    <div class="par" style="clear:both;margin-top:15px">
      <div class="title">Ela is a dynamic language</div>                  
        <div class="txt" style="margin-top:5px;">
          If the first thing that comes to your head, when somebody says "dynamic language", is JavaScript,
          than think twice. Dynamic typing in Ela is lambda calculus and metaprogramming over types, not the
          "let's scrap all types and formal operation semantics" from many popular dynamic languages. Ela
          offers you much more static control than most of other dynamic languages. Ela is a type safe,
          strictly typed language, which is, however, not burden by limitations of any particular type checker.
        </div>
    </div>
    
    <div class="par" style="clear:both;margin-top:15px">
      <div class="title">Ela is one of a kind</div>                  
        <div class="txt" style="margin-top:5px;">
          Ela is the first and the only pure functional programming language that combines dynamic typing, 
          algebraic types and type classes and, at the same time, features support for both strict and lazy 
          evaluation strategies. Comparing it to big functional languages such as Haskell, Ela is relatively
          simple and has a very low entry cost - it can be even used as a scripting language of your choice
          or be embedded in .NET applications.
        </div>
    </div>
    </div>
    
    <a name="features"></a>
    <div class="par" style="clear:both;margin-top:0px">
      <div class="title">Functions, first class</div>                  
      <pre id="ElaCode2" tab-width="2" runat="server" style="float:right;margin-left:10px;width:350px;"> 
      </pre>
      <div class="txt">
        In Ela the concept of first class functions is taken to a whole new level. Functions are not just
          first class, thus rendering some nice programming techniques and capabilities. Functions are the 
          main and the most basic building block in Ela programs, functions are used as a primary tool for 
          abstractions, new functions are defined by partially applying existing functions, even operators
          in Ela are just regular functions. And all these things are presented through a well thought and
          elegant syntax, with extensive support for pattern matching, laziness and recursive definitions.      
      </div>
    </div>
    
    <div class="par" style="clear:both;margin-top:15px">
      <div class="title">Modules, also first class</div>                  
      <pre id="ElaCode3" tab-width="2" runat="server" style="float:right;margin-left:10px;width:370px;"> 
      </pre>
      <div class="txt">
        Ela features an expressive and flexible module system. Ela is a unit based compilation language,
          so that each module can be compiled separately. Compiled modules (.elaobj files) has fast indexed
          metadata tables which enables high performance run-time reflection. Also modules provides a support
          for namespacing. Last, but not least, modules in Ela are first class objects just like functions - 
          they be passed as arguments to functions, you can write generic function that can operate with
          modules and with records at the same time, you can even pattern match modules just like records.
        </div>
    </div>
    
    <div class="par" style="clear:both;margin-top:15px">
      <div class="title">Algebraic types</div>                  
      <pre id="ElaCode4" tab-width="2" runat="server" style="float:left;margin-right:10px;width:360px;"> 
      </pre>
      <div class="txt">
         Algebraic types have a set of really useful properties - because of their nature, a lot of standard operations
         (such as equality, comparisons, formatting to strings, iteration, etc.), which you would normally
         define manually over and over again, can be inferred automatically by a compiler or by a run-time
         environment. That saves you a "couple" of key strokes. Also Ela allows you to create open algebraic
         types which can be extended with new cases at any moment.
        </div>
    </div>
    
    <div class="par" style="margin-top:15px">
      <div class="title" style="clear:both">Type classes</div>                  
       <pre id="ElaCode5" tab-width="2" runat="server" style="float:left;margin-right:10px;width:370px;"> 
       </pre>
       <div class="txt">
          Types classes in Ela, inspired by Haskell, along with dynamic dispatch and support for function
          (and constant) overloading by return type, provide a powerful abstraction mechanism. Classes are like
          interfaces in object oriented languages but they don't unnecessarily tie up functions and values 
          together and are, in fact, considerably more flexible. You can provide your own overloading rules per
          every function without the need to stick with "dispatch by first argument" rule adoped by OOP. You can even
          define instances of classes for already existing types, including standard types, such as integers and
          floats. And in many cases class instances can be inferred for you automatically. In fact most of 
          standard functions in Ela, including arithmetic functions, equality operators, comparison functions and
          so forth are defined through classes Additive, Ring, Field, Eq, Ord, etc.
        </div>
    </div>
    
    <div class="par" style="clear:both;margin-top:15px">
      <div class="title">Lazy when you need it</div>                  
      <pre id="ElaCode6" tab-width="2" runat="server" style="float:left;margin-right:10px;width:360px;"> 
      </pre>
      <div class="txt">
         Ela provides an extensive support for both strict and non-strict evaluation. Moreover, while preferring
         eager evaluation, Ela compiler analyzes your program to understand whether it should be executed in a 
         strict or in a lazy manner, and postpones evaluation of certain expressions if this is needed. You can
         also explicitly mark sections of code as lazy using <code>(&)</code> operator, which creates a thunk
         (similar to futures from Alice ML). Thanks to this all the programming techniques from lazy functional
         languages are possible in Ela, while still maintaining predictable and mostly strict program behavior. Not
         even saying that all bindings in Ela are mutually recursive and their order is generally insignificant
         (like it should be in a declarative language).
        </div>
    </div>
   
</asp:Content>

