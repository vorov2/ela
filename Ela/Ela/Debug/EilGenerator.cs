using System;
using System.Text;
using Ela.Compilation;

namespace Ela.Debug
{
	public sealed class EilGenerator
	{
		#region Construction
		private const string STR = "\"{0}\"";
		private const string VAR = "#{0}";
		private const string OP_FORMAT = "[{0}] {1} {2}\r\n";
        private const string OP_FORMAT2 = "{0} {1}\r\n";

		private CodeFrame frame;

		public EilGenerator(CodeFrame frame)
		{
			this.frame = frame;
		}
		#endregion


		#region Methods
		public string Generate()
		{
			return InternalGenerate(0, frame.Ops.Count);
		}


		public string Generate(int offset)
		{
			return InternalGenerate(offset, frame.Ops.Count);
		}


		public string Generate(int offset, int length)
		{
			return InternalGenerate(offset, length);
		}


		private string InternalGenerate(int offset, int length)
		{
			if (offset > frame.Ops.Count - 1 || length > frame.Ops.Count)
				throw new ElaDebugException(Ela.Runtime.Strings.GetMessage("InvalidEilRange"));

			var dr = frame.Symbols != null && !IgnoreDebugInfo ? new DebugReader(frame.Symbols) : null;
			var sb = new StringBuilder();
			var numLen = (frame.Ops.Count).ToString().Length + 1;
			var mask = new string[numLen];

			for (var i = 0; i < numLen; i++)
				mask[i] = new String('0', i);

			for (var i = offset; i < length; i++)
			{
				var o = frame.Ops[i];
				var od = frame.OpData[i];
				var val = String.Empty;

				switch (o)
				{
					case Op.Pushstr:
						val = String.Format(STR, frame.Strings[od]);
						break;
                    case Op.Pushloc:
                    case Op.Poploc:
                        val = null;

                        if (dr != null)
                        {
                            var scope = dr.FindScopeSym(i);
                            
                            if (scope != null)
                            {
                                var idx = scope.Index;
                                var var = default(VarSym);

                                while (idx > -1)
                                {
                                    if ((var = dr.FindVarSym(od, scope.Index)) != null)
                                    {
                                        val = var.Name;
                                        break;
                                    }

                                    idx--;
                                }
                            }
                        }

                        if (val == null)
                            val = String.Format(VAR, od);
                        break;
					case Op.Pushvar:
					case Op.Popvar:
						if (dr != null)
						{
							var scope = dr.FindScopeSym(i);
							var var = default(VarSym);

							if (scope != null && (var = dr.FindVarSym(od >> 8,
								scope.Index - (od & Byte.MaxValue))) != null)
							{
								val = var.Name;
								break;
							}
						}

						val = String.Format(VAR, od);
						break;
					default:
						var size = OpSizeHelper.OpSize[(Int32)o];
						val = size > 1 ? od.ToString() : String.Empty;
						break;
				}

				var num = i.ToString();
				num = mask[numLen - num.Length] + num;
				
                if (OmitOffsets)
                    sb.AppendFormat(OP_FORMAT2, o.ToString(), val);
                else
                    sb.AppendFormat(OP_FORMAT, num, o.ToString(), val);
			}

			return sb.ToString();
		}

        public bool IgnoreDebugInfo { get; set; }

        public bool OmitOffsets { get; set; }
		#endregion
	}
}