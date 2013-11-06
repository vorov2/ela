using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Ela.Parsing
{
	internal static class EscapeCodeParser
	{
		internal static int Parse(ref string str)
		{
			var buffer = str.ToCharArray(1, str.Length - 2);
			var len = buffer.Length;
			var sb = new StringBuilder(str.Length);
			
			for (var i = 0; i < len; i++)
			{
				var c = buffer[i];
				
				if (c == '\\')
				{
					i++;
					
					if (i < len)
					{
						var cn = buffer[i];
						
						switch (cn)
						{
							case 't': 
								sb.Append('\t');
								break;
							case 'r':
								sb.Append('\r');
								break;
							case 'n':
								sb.Append('\n');
								break;
							case 'b':
								sb.Append('\b');
								break;
							case '"':
								sb.Append('"');
								break;
							case '\'':
								sb.Append('\'');
								break;
							case '\\':
								sb.Append('\\');
								break;
							case '0':
								sb.Append('\0');
								break;
							case 'u':
								{
									if (i + 3 < len)
									{
										var ns = new String(buffer, i + 1, 4);
										var ci = 0;
										
										if (!Int32.TryParse(ns, NumberStyles.HexNumber, Culture.NumberFormat, out ci))
											return i;
										else
										{
											sb.Append((Char)ci);
											i += 4;
										}
									}
									else
										return i;
								}
                                break;
                            case 'x':
                                {
                                    if (i + 4 < len)
                                    {
                                        var ns = new String(buffer, i + 1, 5);
                                        var ci = 0;

                                        if (!Int32.TryParse(ns, NumberStyles.Integer, Culture.NumberFormat, out ci))
                                            return i;
                                        else
                                        {
                                            sb.Append((Char)ci);
                                            i += 5;
                                        }
                                    }
                                    else
                                        return i;
                                }
                                break;
							default:
								return i;
						}
					}			
					else
						return i;
				}
				else
					sb.Append(c);
			}
			
			str = sb.ToString();
			return 0;
		}
	}
}
