 <%@ Page Language="C#" Debug="true"%>
 <%@ Assembly Name="Ela" %>
 <%@ Import Namespace="System.Threading" %>
 <%@ Import Namespace="System.Xml" %>
 <%@ Import Namespace="System.IO" %>
 <%@ Import Namespace="Ela" %>
 <%@ Import Namespace="Ela.Runtime" %>
 <%@ Import Namespace="Ela.Runtime.ObjectModel" %>
 <%@ Import Namespace="Ela.Linking" %>
 <%@ Import Namespace="Ela.Parsing" %>
 <%@ Import Namespace="Ela.Compilation" %>
 <%@ Import Namespace="System.Collections.Generic" %>
 <%@ Import Namespace="Ela.CodeModel" %>
 
 <script runat="server">
	private ElaIncrementalLinker linker;
	private ElaMachine vm;
	private int lastOffset;
		
	private sealed class ReturnData
	{
		public string Status;
		public string Result;
	}	
	
	protected override void OnLoad(EventArgs e)
	{
		if (!String.IsNullOrEmpty(Request["reset"]))
		{
			Session["Linker"] = null;
			Session["VM"] = null;
			Session["LastOffset"] = null;
			return;
		}
	
		try
		{
			Application.Lock();			
			int curs = Application["Curs"] == null ? 0 : (Int32)Application["Curs"];
			
			if (curs >= 10)
			{
				WriteResponse("Overload", String.Empty);
				return;
			}
			else
				Application["Curs"] = ++curs;
		}
		finally
		{
			Application.UnLock();
		}		
		
		ReturnData res = new ReturnData();
		Thread th = new Thread(new ThreadStart(delegate() { RunCode(res); }));
		th.Start();		
		bool fin = th.Join(1500);
		
		if (!fin)
		{
			WriteResponse("Timeout", String.Empty);
			th.Abort();
		}
		else
			WriteResponse(res.Status, res.Result);
			
		try
		{
			Application.Lock();			
			int curs = Application["Curs"] == null ? 0 : (Int32)Application["Curs"];			
			Application["Curs"] = --curs;
		}
		finally
		{
			Application.UnLock();
		}	
	}
	
	
	private static List<String> buffer = new List<String>();
	private ElaValue WebOut(ElaValue data)
	{
		buffer.Add(data.ToString());
		return new ElaValue(ElaUnit.Instance);
	}
	
	private static ElaFunction webOutDelegate;
	private void RunCode(ReturnData res)
	{
		if (webOutDelegate == null)
			 webOutDelegate= ElaFunction.Create<ElaValue,ElaValue>(WebOut);
	
		try
		{
			linker = Session["Linker"] as ElaIncrementalLinker;
			vm = Session["VM"] as ElaMachine;
			lastOffset = Session["LastOffset"] == null ? 0 : (Int32)Session["LastOffset"];		
			string source = Request["src"];
					
			if (linker == null)
			{
				LinkerOptions lo = new LinkerOptions();
				lo.CodeBase.Directories.Add(new System.IO.DirectoryInfo(@"D:\Hosting\7431445\html\bin"));
				CompilerOptions co = new CompilerOptions();
				co.Prelude = "Prelude";
				linker = new ElaIncrementalLinker(lo, co, new FileInfo(@"D:\Hosting\7431445\html\bin\online"));
				linker.AddArgument("webOut", webOutDelegate);				
				Session["Linker"] = linker;
			}

			linker.SetSource(source);
			LinkerResult lres = linker.Build();
			
			foreach (ElaMessage m in lres.Messages)
			{
				string format = "<span style='color:{0}'>{1}</span><br>";			
				
				res.Result += String.Format(format,
					m.Type == MessageType.Error ? "red" :
					m.Type == MessageType.Warning ? "#FF9900" : "blue", m.ToString());
			}
				
			if (!lres.Success)
				res.Status = "Error"; 
			else
				Execute(lres.Assembly, res);
		}
		catch (Exception ex)
		{
			res.Status = "Error";
			res.Result = ex.ToString();
		}
	}
 
 
	private void Execute(CodeAssembly asm, ReturnData res)
	{
		CodeFrame mod = asm.GetRootModule();
		string statusLong = !String.IsNullOrEmpty(res.Result) ? res.Result + "<br>" : String.Empty;

		try
		{
			if (vm == null)
			{
				vm = new ElaMachine(asm);
				Session["VM"] = vm;
			}
			else
				vm.RefreshState();
			
			int os = lastOffset;
			lastOffset = mod.Ops.Count;
			Session["LastOffset"] = lastOffset;
			DateTime dt = DateTime.Now;
			ExecutionResult exer = vm.Run(os);
			string ret = vm.PrintValue(exer.ReturnValue);	
			res.Status = "Success";
			
			string outp = "";
			
			foreach (string s in buffer)
				outp += s + "<br>";
			
			buffer.Clear();
			res.Result = outp + statusLong + ret;
		}
		catch (ElaCodeException ex)
		{
			string outp = "";
			
			foreach (string s in buffer)
				outp += s + "<br>";
				
			buffer.Clear();
			vm.Recover();			
			res.Status = "Error";
			res.Result = outp + statusLong + ex.ToString();
		}
	}
	
	
	private void WriteResponse(string type, string result)
	{
		Response.ContentType = "text/xml";
		
		StringWriter sw = new StringWriter();
		XmlWriter xw = new XmlTextWriter(sw);
		xw.WriteStartElement("Result");
		xw.WriteAttributeString("Type", type);
		xw.WriteAttributeString("Result", result.Replace(@"D:\Hosting\7431445\html\bin\", String.Empty));
		xw.WriteEndElement();
		
		Response.Write(sw.ToString());
	}
</script>