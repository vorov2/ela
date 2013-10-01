<%@ Page Language="C#" ValidateRequest="false" %>
<%@ Assembly Name="Ela" %>
<html>
	<head>
		<title>Ela Online Interactive Console</title>
		<style>
		.topTable
		{
			width:100%;
			height:100%;
			border:none;
		}
				
		.marginCell
		{
		width:33%;
		}
		
		.middleCell
		{
			width:640px;
			max-width:640px;
			vertical-align:top;
			padding-top:10px;
		}
		
		.consoleTable
		{
			width:640px;
			border:none;
		}
		
		.execButtonCell
		{
			padding-top:10px;
		}
		
		.console
		{
			font-family:Consolas,Courier New;
			font-size:10pt;
			border:none;
			width:100%;
			height:100%;
		}
		
		.execButton,.execButtonHover
		{
			font-family:Verdana,Arial;
			font-size:14pt;
			font-weight:bold;
			color:#515151;
			background-color:#D6D6D6;
			border:solid 1px darkgray;
			border-left:none;
			padding:2px 2px 2px 2px;
			cursor:pointer;
			width:100px;
			height:35px;
		}
		
		.execButtonHover
		{
			color:#D6D6D6;
			background-color:#515151;
		}
		
		.resultTable
		{
			width:620px;
		}
		
		.resultCell
		{
			font-family:Consolas,Courier New;
			font-size:10pt;
			width:620px;
			max-width:620px;
			vertical-align:top;
			word-wrap:break-word;
		}
		
		.err
		{
			color:red
		}
		
		.getLinkCell,.getLinkCellHover
		{
			border:solid 1px #515151;
			width:140px;
			height:40px;
			background-color:#D6D6D6;
			font-family:Verdana,Arial;
			font-size:12pt;
			font-weight:bold;
			color:#515151;
			text-align:center;
		}
		
		.getLinkCellHover
		{
			color:#D6D6D6;
			background-color:#515151;
			cursor:pointer;
		}
		
		a,a:hover,a:visited,a:active
		{
			color:#800000;
			text-decoration:none;
		}
		
		a:hover
		{
			text-decoration:underline;
		}
		
		.resa,.err
		{
		   border-bottom:dashed 1px darkgray;
		   padding-bottom:10px;
		   padding-top:10px;
		}
		</style>
		
		<script language="javascript">
		function createXMLHttpRequest() {
			var resObject = null;
			
			try {
				resObject = new ActiveXObject("Mircosoft.XMLHTTP");
			}			
			catch (error) {
				try {
					resObject = new ActiveXObject("MSXML2.XMLHTTP")
				}
				catch(error) {
					try {
						resObject = new XMLHttpRequest();
					}
					catch (error) {
						alert("XMLHttpRequest not available");
					}
				}
			}
			
			return resObject;
		}

		function execEla() {
			var src = document.getElementById("console").value;
			start();
			var req = createXMLHttpRequest();
			req.open("POST", "/exec.aspx?tag=" + getTag());
            req.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            var frm = "src=" + encodeURIComponent(src);
            
			req.onreadystatechange = function() {
				if (req.readyState == 4) {
					var xml = req.responseXML;
					var root = null;
					
					if (typeof(xml.selectSingleNode) != "function")
						root = xml.getElementsByTagName("Result")[0];
					else
						root = xml.selectSingleNode("//Result");
                        
                    if (root != null) {
						var div = document.getElementById("resultCell");
						var apd = "";			
			
						if (root.getAttribute("Type") == "Timeout")
							apd = "<div class='err'>Your script timed out. Please don't use time consuming scripts.</div>" ;
						else if (root.getAttribute("Type") == "Overload")
							apd = "<div class='err'>Too many connections. Please try again later.</div>" ;
						else if (root.getAttribute("Type") != "Success")
							apd = "<div class='err'>" + root.getAttribute("Result") + "</div>";
						else
							apd = "<div class='resa'>" + root.getAttribute("Result") + "</div>";
					
						div.innerHTML = apd + div.innerHTML;					
					}
					else	
						alert("Unexpected error occured!");
					
					end();
				}			
			};
			
			req.send(frm);			
		}
		
		function resetEla() {
			var src = document.getElementById("console").value;
			start();
			var req = createXMLHttpRequest();
			req.open("GET", "/exec.aspx?reset=1&tag=" + getTag());
			req.onreadystatechange = function() {
				if (req.readyState == 4) {
					var div = document.getElementById("resultCell");
					div.innerHTML = "<div class='err'>Virtual machine reseted</div>" + div.innerHTML;
					end();
				}			
			};
			
			req.send(null);			
		}
		
		function getTag() {
			return new Date().toString();
		}
		
		function start() {
			document.getElementById("reset").style.visibility = "hidden";
			document.getElementById("exec").style.visibility = "hidden";
			var statCell = document.getElementById("labelCell");
			statCell.innerHTML = "<span style='color:red'>Executing...</span>";
		}
		
		
		function end() {
			document.getElementById("reset").style.visibility = "visible";
			document.getElementById("exec").style.visibility = "visible";
			var statCell = document.getElementById("labelCell");
			statCell.innerHTML = "output";
		}
		</script>
	</head>
	<body style="margin:0px 0px 0px 0px">
		<table cellpadding="0" cellspacing="0" class="topTable">
			<tr>
				<td class="marginCell" style="background-color:#E2E2E2;border-bottom:solid 1px darkgray">&nbsp;</td>
				<td style="width:640px;vertical-align:top;padding-top:10px;height:10px;padding-bottom:10px;background-color:#E2E2E2;border-bottom:solid 1px darkgray">
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td>
								<img src="images/logobig.png" />
							</td>
							<td style="vertical-align:top">
								<div style="padding-left:10px;padding-top:0px;font-family:Verdana,Arial;font-size:22pt;font-weight:bold;color:#515151">Ela Interactive Console</div>
								<div style="padding-left:10px;padding-top:5px;font-style:italic;font-family:Verdana,Arial;font-size:12pt;">using Ela <%= typeof(Ela.ElaMessage).Assembly.GetName().Version.ToString() %></div>
							</td>
						</tr>
					</table>
				</td>
				<td class="marginCell" style="background-color:#E2E2E2;border-bottom:solid 1px darkgray">&nbsp;</td>
			</tr>
			<tr>
				<td class="marginCell">&nbsp;</td>
				<td style="height:200px;padding-top:0px;border-left:solid 1px darkgray;border-right:solid 1px darkgray">
					<textarea id="console" class="console"></textarea>					
				</td>		
				<td class="marginCell"  style="vertical-align:top;padding-top:5px;">
					<input id="exec" type="button" onclick="execEla()" 
							onmouseover="this.className='execButtonHover'" onmouseout="this.className='execButton'" 
							class="execButton" value="run"/>
					<div style="padding-top:5px;"></div>
					<input id="reset" type="button" onclick="resetEla()" 
						onmouseover="this.className='execButtonHover'" onmouseout="this.className='execButton'" 
						class="execButton" value="reset"/>
				</td>
			</tr>
			
			<tr>
				<td class="marginCell" style="padding-bottom:0px;background-color:#E2E2E2;border-top:solid 1px darkgray;border-bottom:solid 1px darkgray">&nbsp;</td>
				<td id="labelCell" style="width:640px;padding-bottom:0px;font-family:Verdana,Arial;color:#515151;font-weight:bold;font-size:14pt;height:30px;background-color:#E2E2E2;border-top:solid 1px darkgray;border-bottom:solid 1px darkgray">
					output
				</td>				
				<td class="marginCell" style="padding-bottom:0px;background-color:#E2E2E2;border-top:solid 1px darkgray;border-bottom:solid 1px darkgray">&nbsp;</td>
			</tr>
			
			<tr>
				<td class="marginCell" style="padding-top:0px">&nbsp;</td>
				<td style="padding-top:0px;border-left:solid 1px darkgray;border-right:solid 1px darkgray;vertical-align:top;width:640px">
					<table cellpadding="0" cellspacing="0" class="resultTable">
						<tr>
							<td id="resultCell" class="resultCell" style="padding-left:10px;padding-right:10px;vertical-align:top;">
							</td>
						</tr>
					</table>
				</td>
				<td class="marginCell" style="padding-top:0px">&nbsp;</td>
			</tr>
			
		</table>		
		<script language="javascript">
			var p = '<%=Request["eval"]%>';
			
			if (p != null && p != '') {
				document.getElementById("console").value = p;
				execEla();
			}
		</script>
	</body>
</html>
<!--