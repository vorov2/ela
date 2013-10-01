﻿using System;

namespace Elide.Scintilla.Internal
{
	internal static class Sci
	{
		internal const int NIL = 0;
		internal const int TRUE = 1;
		internal const int FALSE = 0;
		internal const int STYLE_MASK = 31;//255;

		internal const int MARGIN_MARKER = 0;
		internal const int MARGIN_LINENUMBERS = 1;
		internal const int MARGIN_TRACKING = 2;
		internal const int MARGIN_FOLDING = 3;

        internal const int SCI_MOVESELECTEDLINESUP = 2620;
        internal const int SCI_MOVESELECTEDLINESDOWN = 2621;
		
		internal const int INVALID_POSITION = -1;
		internal const int SCI_START = 2000;
		internal const int SCI_OPTIONAL_START = 3000;
		internal const int SCI_LEXER_START = 4000;
		internal const int SCI_ADDTEXT = 2001;
		internal const int SCI_ADDSTYLEDTEXT = 2002;
		internal const int SCI_INSERTTEXT = 2003;
		internal const int SCI_CLEARALL = 2004;
		internal const int SCI_CLEARDOCUMENTSTYLE = 2005;
		internal const int SCI_GETLENGTH = 2006;
		internal const int SCI_GETCHARAT = 2007;
		internal const int SCI_GETCURRENTPOS = 2008;
		internal const int SCI_GETANCHOR = 2009;
		internal const int SCI_GETSTYLEAT = 2010;
		internal const int SCI_REDO = 2011;
		internal const int SCI_SETUNDOCOLLECTION = 2012;
		internal const int SCI_SELECTALL = 2013;
		internal const int SCI_SETSAVEPOINT = 2014;
		internal const int SCI_GETSTYLEDTEXT = 2015;
		internal const int SCI_CANREDO = 2016;
		internal const int SCI_MARKERLINEFROMHANDLE = 2017;
		internal const int SCI_MARKERDELETEHANDLE = 2018;
		internal const int SCI_GETUNDOCOLLECTION = 2019;
		internal const int SCWS_INVISIBLE = 0;
		internal const int SCWS_VISIBLEALWAYS = 1;
		internal const int SCWS_VISIBLEAFTERINDENT = 2;
		internal const int SCI_GETVIEWWS = 2020;
		internal const int SCI_SETVIEWWS = 2021;
		internal const int SCI_POSITIONFROMPOINT = 2022;
		internal const int SCI_POSITIONFROMPOINTCLOSE = 2023;
		internal const int SCI_GOTOLINE = 2024;
		internal const int SCI_GOTOPOS = 2025;
		internal const int SCI_SETANCHOR = 2026;
		internal const int SCI_GETCURLINE = 2027;
		internal const int SCI_GETENDSTYLED = 2028;
		internal const int SC_EOL_CRLF = 0;
		internal const int SC_EOL_CR = 1;
		internal const int SC_EOL_LF = 2;
		internal const int SCI_CONVERTEOLS = 2029;
		internal const int SCI_GETEOLMODE = 2030;
		internal const int SCI_SETEOLMODE = 2031;
		internal const int SCI_STARTSTYLING = 2032;
		internal const int SCI_SETSTYLING = 2033;
		internal const int SCI_GETBUFFEREDDRAW = 2034;
		internal const int SCI_SETBUFFEREDDRAW = 2035;
		internal const int SCI_SETTABWIDTH = 2036;
		internal const int SCI_GETTABWIDTH = 2121;
		internal const int SC_CP_UTF8 = 65001;
		internal const int SC_CP_DBCS = 1;
		internal const int SCI_SETCODEPAGE = 2037;
		internal const int SCI_SETUSEPALETTE = 2039;
		internal const int MARKER_MAX = 31;
		internal const int SC_MARK_CIRCLE = 0;
		internal const int SC_MARK_ROUNDRECT = 1;
		internal const int SC_MARK_ARROW = 2;
		internal const int SC_MARK_SMALLRECT = 3;
		internal const int SC_MARK_SHORTARROW = 4;
		internal const int SC_MARK_EMPTY = 5;
		internal const int SC_MARK_ARROWDOWN = 6;
		internal const int SC_MARK_MINUS = 7;
		internal const int SC_MARK_PLUS = 8;
		internal const int SC_MARK_VLINE = 9;
		internal const int SC_MARK_LCORNER = 10;
		internal const int SC_MARK_TCORNER = 11;
		internal const int SC_MARK_BOXPLUS = 12;
		internal const int SC_MARK_BOXPLUSCONNECTED = 13;
		internal const int SC_MARK_BOXMINUS = 14;
		internal const int SC_MARK_BOXMINUSCONNECTED = 15;
		internal const int SC_MARK_LCORNERCURVE = 16;
		internal const int SC_MARK_TCORNERCURVE = 17;
		internal const int SC_MARK_CIRCLEPLUS = 18;
		internal const int SC_MARK_CIRCLEPLUSCONNECTED = 19;
		internal const int SC_MARK_CIRCLEMINUS = 20;
		internal const int SC_MARK_CIRCLEMINUSCONNECTED = 21;
		internal const int SC_MARK_BACKGROUND = 22;
		internal const int SC_MARK_DOTDOTDOT = 23;
		internal const int SC_MARK_ARROWS = 24;
		internal const int SC_MARK_PIXMAP = 25;
		internal const int SC_MARK_FULLRECT = 26;
		internal const int SC_MARK_LEFTRECT = 27;
		internal const int SC_MARK_AVAILABLE = 28;
		internal const int SC_MARK_UNDERLINE = 29;
		internal const int SC_MARK_CHARACTER = 10000;
		internal const int SC_MARKNUM_FOLDEREND = 25;
		internal const int SC_MARKNUM_FOLDEROPENMID = 26;
		internal const int SC_MARKNUM_FOLDERMIDTAIL = 27;
		internal const int SC_MARKNUM_FOLDERTAIL = 28;
		internal const int SC_MARKNUM_FOLDERSUB = 29;
		internal const int SC_MARKNUM_FOLDER = 30;
		internal const int SC_MARKNUM_FOLDEROPEN = 31;
		internal const uint SC_MASK_FOLDERS = 0xFE000000;
		internal const int SCI_MARKERDEFINE = 2040;
		internal const int SCI_MARKERSETFORE = 2041;
		internal const int SCI_MARKERSETBACK = 2042;
		internal const int SCI_MARKERADD = 2043;
		internal const int SCI_MARKERDELETE = 2044;
		internal const int SCI_MARKERDELETEALL = 2045;
		internal const int SCI_MARKERGET = 2046;
		internal const int SCI_MARKERNEXT = 2047;
		internal const int SCI_MARKERPREVIOUS = 2048;
		internal const int SCI_MARKERDEFINEPIXMAP = 2049;
		internal const int SCI_MARKERADDSET = 2466;
		internal const int SCI_MARKERSETALPHA = 2476;
		internal const int SC_MARGIN_SYMBOL = 0;
		internal const int SC_MARGIN_NUMBER = 1;
		internal const int SC_MARGIN_BACK = 2;
		internal const int SC_MARGIN_FORE = 3;
		internal const int SC_MARGIN_TEXT = 4;
		internal const int SC_MARGIN_RTEXT = 5;
		internal const int SCI_SETMARGINTYPEN = 2240;
		internal const int SCI_GETMARGINTYPEN = 2241;
		internal const int SCI_SETMARGINWIDTHN = 2242;
		internal const int SCI_GETMARGINWIDTHN = 2243;
		internal const int SCI_SETMARGINMASKN = 2244;
		internal const int SCI_GETMARGINMASKN = 2245;
		internal const int SCI_SETMARGINSENSITIVEN = 2246;
		internal const int SCI_GETMARGINSENSITIVEN = 2247;
		internal const int STYLE_DEFAULT = 32;
		internal const int STYLE_LINENUMBER = 33;
		internal const int STYLE_BRACELIGHT = 34;
		internal const int STYLE_BRACEBAD = 35;
		internal const int STYLE_CONTROLCHAR = 36;
		internal const int STYLE_INDENTGUIDE = 37;
		internal const int STYLE_CALLTIP = 38;
		internal const int STYLE_LASTPREDEFINED = 39;
		internal const int STYLE_MAX = 255;
		internal const int SC_CHARSET_ANSI = 0;
		internal const int SC_CHARSET_DEFAULT = 1;
		internal const int SC_CHARSET_BALTIC = 186;
		internal const int SC_CHARSET_CHINESEBIG5 = 136;
		internal const int SC_CHARSET_EASTEUROPE = 238;
		internal const int SC_CHARSET_GB2312 = 134;
		internal const int SC_CHARSET_GREEK = 161;
		internal const int SC_CHARSET_HANGUL = 129;
		internal const int SC_CHARSET_MAC = 77;
		internal const int SC_CHARSET_OEM = 255;
		internal const int SC_CHARSET_RUSSIAN = 204;
		internal const int SC_CHARSET_CYRILLIC = 1251;
		internal const int SC_CHARSET_SHIFTJIS = 128;
		internal const int SC_CHARSET_SYMBOL = 2;
		internal const int SC_CHARSET_TURKISH = 162;
		internal const int SC_CHARSET_JOHAB = 130;
		internal const int SC_CHARSET_HEBREW = 177;
		internal const int SC_CHARSET_ARABIC = 178;
		internal const int SC_CHARSET_VIETNAMESE = 163;
		internal const int SC_CHARSET_THAI = 222;
		internal const int SC_CHARSET_8859_15 = 1000;
		internal const int SCI_STYLECLEARALL = 2050;
		internal const int SCI_STYLESETFORE = 2051;
		internal const int SCI_STYLESETBACK = 2052;
		internal const int SCI_STYLESETBOLD = 2053;
		internal const int SCI_STYLESETITALIC = 2054;
		internal const int SCI_STYLESETSIZE = 2055;
		internal const int SCI_STYLESETFONT = 2056;
		internal const int SCI_STYLESETEOLFILLED = 2057;
		internal const int SCI_STYLERESETDEFAULT = 2058;
		internal const int SCI_STYLESETUNDERLINE = 2059;
		internal const int SC_CASE_MIXED = 0;
		internal const int SC_CASE_UPPER = 1;
		internal const int SC_CASE_LOWER = 2;
		internal const int SCI_STYLEGETFORE = 2481;
		internal const int SCI_STYLEGETBACK = 2482;
		internal const int SCI_STYLEGETBOLD = 2483;
		internal const int SCI_STYLEGETITALIC = 2484;
		internal const int SCI_STYLEGETSIZE = 2485;
		internal const int SCI_STYLEGETFONT = 2486;
		internal const int SCI_STYLEGETEOLFILLED = 2487;
		internal const int SCI_STYLEGETUNDERLINE = 2488;
		internal const int SCI_STYLEGETCASE = 2489;
		internal const int SCI_STYLEGETCHARACTERSET = 2490;
		internal const int SCI_STYLEGETVISIBLE = 2491;
		internal const int SCI_STYLEGETCHANGEABLE = 2492;
		internal const int SCI_STYLEGETHOTSPOT = 2493;
		internal const int SCI_STYLESETCASE = 2060;
		internal const int SCI_STYLESETCHARACTERSET = 2066;
		internal const int SCI_STYLESETHOTSPOT = 2409;
		internal const int SCI_SETSELFORE = 2067;
		internal const int SCI_SETSELBACK = 2068;
		internal const int SCI_GETSELALPHA = 2477;
		internal const int SCI_SETSELALPHA = 2478;
		internal const int SCI_GETSELEOLFILLED = 2479;
		internal const int SCI_SETSELEOLFILLED = 2480;
		internal const int SCI_SETCARETFORE = 2069;
		internal const int SCI_ASSIGNCMDKEY = 2070;
		internal const int SCI_CLEARCMDKEY = 2071;
		internal const int SCI_CLEARALLCMDKEYS = 2072;
		internal const int SCI_SETSTYLINGEX = 2073;
		internal const int SCI_STYLESETVISIBLE = 2074;
		internal const int SCI_GETCARETPERIOD = 2075;
		internal const int SCI_SETCARETPERIOD = 2076;
		internal const int SCI_SETWORDCHARS = 2077;
		internal const int SCI_BEGINUNDOACTION = 2078;
		internal const int SCI_ENDUNDOACTION = 2079;
		internal const int INDIC_PLAIN = 0;
		internal const int INDIC_SQUIGGLE = 1;
		internal const int INDIC_TT = 2;
		internal const int INDIC_DIAGONAL = 3;
		internal const int INDIC_STRIKE = 4;
		internal const int INDIC_HIDDEN = 5;
		internal const int INDIC_BOX = 6;
		internal const int INDIC_ROUNDBOX = 7;
		internal const int INDIC_MAX = 31;
		internal const int INDIC_CONTAINER = 8;
		internal const int INDIC0_MASK = 0x20;
		internal const int INDIC1_MASK = 0x40;
		internal const int INDIC2_MASK = 0x80;
		internal const int INDICS_MASK = 0xE0;
		internal const int SCI_INDICSETSTYLE = 2080;
		internal const int SCI_INDICGETSTYLE = 2081;
		internal const int SCI_INDICSETFORE = 2082;
		internal const int SCI_INDICGETFORE = 2083;
		internal const int SCI_INDICSETUNDER = 2510;
		internal const int SCI_INDICGETUNDER = 2511;
		internal const int SCI_SETWHITESPACEFORE = 2084;
		internal const int SCI_SETWHITESPACEBACK = 2085;
		internal const int SCI_SETSTYLEBITS = 2090;
		internal const int SCI_GETSTYLEBITS = 2091;
		internal const int SCI_SETLINESTATE = 2092;
		internal const int SCI_GETLINESTATE = 2093;
		internal const int SCI_GETMAXLINESTATE = 2094;
		internal const int SCI_GETCARETLINEVISIBLE = 2095;
		internal const int SCI_SETCARETLINEVISIBLE = 2096;
		internal const int SCI_GETCARETLINEBACK = 2097;
		internal const int SCI_SETCARETLINEBACK = 2098;
		internal const int SCI_STYLESETCHANGEABLE = 2099;
		internal const int SCI_AUTOCSHOW = 2100;
		internal const int SCI_AUTOCCANCEL = 2101;
		internal const int SCI_AUTOCACTIVE = 2102;
		internal const int SCI_AUTOCPOSSTART = 2103;
		internal const int SCI_AUTOCCOMPLETE = 2104;
		internal const int SCI_AUTOCSTOPS = 2105;
		internal const int SCI_AUTOCSETSEPARATOR = 2106;
		internal const int SCI_AUTOCGETSEPARATOR = 2107;
		internal const int SCI_AUTOCSELECT = 2108;
		internal const int SCI_AUTOCSETCANCELATSTART = 2110;
		internal const int SCI_AUTOCGETCANCELATSTART = 2111;
		internal const int SCI_AUTOCSETFILLUPS = 2112;
		internal const int SCI_AUTOCSETCHOOSESINGLE = 2113;
		internal const int SCI_AUTOCGETCHOOSESINGLE = 2114;
		internal const int SCI_AUTOCSETIGNORECASE = 2115;
		internal const int SCI_AUTOCGETIGNORECASE = 2116;
		internal const int SCI_USERLISTSHOW = 2117;
		internal const int SCI_AUTOCSETAUTOHIDE = 2118;
		internal const int SCI_AUTOCGETAUTOHIDE = 2119;
		internal const int SCI_AUTOCSETDROPRESTOFWORD = 2270;
		internal const int SCI_AUTOCGETDROPRESTOFWORD = 2271;
		internal const int SCI_REGISTERIMAGE = 2405;
		internal const int SCI_CLEARREGISTEREDIMAGES = 2408;
		internal const int SCI_AUTOCGETTYPESEPARATOR = 2285;
		internal const int SCI_AUTOCSETTYPESEPARATOR = 2286;
		internal const int SCI_AUTOCSETMAXWIDTH = 2208;
		internal const int SCI_AUTOCGETMAXWIDTH = 2209;
		internal const int SCI_AUTOCSETMAXHEIGHT = 2210;
		internal const int SCI_AUTOCGETMAXHEIGHT = 2211;
		internal const int SCI_SETINDENT = 2122;
		internal const int SCI_GETINDENT = 2123;
		internal const int SCI_SETUSETABS = 2124;
		internal const int SCI_GETUSETABS = 2125;
		internal const int SCI_SETLINEINDENTATION = 2126;
		internal const int SCI_GETLINEINDENTATION = 2127;
		internal const int SCI_GETLINEINDENTPOSITION = 2128;
		internal const int SCI_GETCOLUMN = 2129;
		internal const int SCI_SETHSCROLLBAR = 2130;
		internal const int SCI_GETHSCROLLBAR = 2131;
		internal const int SC_IV_NONE = 0;
		internal const int SC_IV_REAL = 1;
		internal const int SC_IV_LOOKFORWARD = 2;
		internal const int SC_IV_LOOKBOTH = 3;
		internal const int SCI_SETINDENTATIONGUIDES = 2132;
		internal const int SCI_GETINDENTATIONGUIDES = 2133;
		internal const int SCI_SETHIGHLIGHTGUIDE = 2134;
		internal const int SCI_GETHIGHLIGHTGUIDE = 2135;
		internal const int SCI_GETLINEENDPOSITION = 2136;
		internal const int SCI_GETCODEPAGE = 2137;
		internal const int SCI_GETCARETFORE = 2138;
		internal const int SCI_GETUSEPALETTE = 2139;
		internal const int SCI_GETREADONLY = 2140;
		internal const int SCI_SETCURRENTPOS = 2141;
		internal const int SCI_SETSELECTIONSTART = 2142;
		internal const int SCI_GETSELECTIONSTART = 2143;
		internal const int SCI_SETSELECTIONEND = 2144;
		internal const int SCI_GETSELECTIONEND = 2145;
		internal const int SCI_SETPRINTMAGNIFICATION = 2146;
		internal const int SCI_GETPRINTMAGNIFICATION = 2147;
		internal const int SC_PRINT_NORMAL = 0;
		internal const int SC_PRINT_INVERTLIGHT = 1;
		internal const int SC_PRINT_BLACKONWHITE = 2;
		internal const int SC_PRINT_COLOURONWHITE = 3;
		internal const int SC_PRINT_COLOURONWHITEDEFAULTBG = 4;
		internal const int SCI_SETPRINTCOLOURMODE = 2148;
		internal const int SCI_GETPRINTCOLOURMODE = 2149;
		internal const int SCFIND_WHOLEWORD = 2;
		internal const int SCFIND_MATCHCASE = 4;
		internal const int SCFIND_WORDSTART = 0x00100000;
		internal const int SCFIND_REGEXP = 0x00200000;
		internal const int SCFIND_POSIX = 0x00400000;
		internal const int SCI_FINDTEXT = 2150;
		internal const int SCI_FORMATRANGE = 2151;
		internal const int SCI_GETFIRSTVISIBLELINE = 2152;
		internal const int SCI_GETLINE = 2153;
		internal const int SCI_GETLINECOUNT = 2154;
		internal const int SCI_SETMARGINLEFT = 2155;
		internal const int SCI_GETMARGINLEFT = 2156;
		internal const int SCI_SETMARGINRIGHT = 2157;
		internal const int SCI_GETMARGINRIGHT = 2158;
		internal const int SCI_GETMODIFY = 2159;
		internal const int SCI_SETSEL = 2160;
		internal const int SCI_GETSELTEXT = 2161;
		internal const int SCI_GETTEXTRANGE = 2162;
		internal const int SCI_HIDESELECTION = 2163;
		internal const int SCI_POINTXFROMPOSITION = 2164;
		internal const int SCI_POINTYFROMPOSITION = 2165;
		internal const int SCI_LINEFROMPOSITION = 2166;
		internal const int SCI_POSITIONFROMLINE = 2167;
		internal const int SCI_LINESCROLL = 2168;
		internal const int SCI_SCROLLCARET = 2169;
		internal const int SCI_REPLACESEL = 2170;
		internal const int SCI_SETREADONLY = 2171;
		internal const int SCI_NULL = 2172;
		internal const int SCI_CANPASTE = 2173;
		internal const int SCI_CANUNDO = 2174;
		internal const int SCI_EMPTYUNDOBUFFER = 2175;
		internal const int SCI_UNDO = 2176;
		internal const int SCI_CUT = 2177;
		internal const int SCI_COPY = 2178;
		internal const int SCI_PASTE = 2179;
		internal const int SCI_CLEAR = 2180;
		internal const int SCI_SETTEXT = 2181;
		internal const int SCI_GETTEXT = 2182;
		internal const int SCI_GETTEXTLENGTH = 2183;
		internal const int SCI_GETDIRECTFUNCTION = 2184;
		internal const int SCI_GETDIRECTPOINTER = 2185;
		internal const int SCI_SETOVERTYPE = 2186;
		internal const int SCI_GETOVERTYPE = 2187;
		internal const int SCI_SETCARETWIDTH = 2188;
		internal const int SCI_GETCARETWIDTH = 2189;
		internal const int SCI_SETTARGETSTART = 2190;
		internal const int SCI_GETTARGETSTART = 2191;
		internal const int SCI_SETTARGETEND = 2192;
		internal const int SCI_GETTARGETEND = 2193;
		internal const int SCI_REPLACETARGET = 2194;
		internal const int SCI_REPLACETARGETRE = 2195;
		internal const int SCI_SEARCHINTARGET = 2197;
		internal const int SCI_SETSEARCHFLAGS = 2198;
		internal const int SCI_GETSEARCHFLAGS = 2199;
		internal const int SCI_CALLTIPSHOW = 2200;
		internal const int SCI_CALLTIPCANCEL = 2201;
		internal const int SCI_CALLTIPACTIVE = 2202;
		internal const int SCI_CALLTIPPOSSTART = 2203;
		internal const int SCI_CALLTIPSETHLT = 2204;
		internal const int SCI_CALLTIPSETBACK = 2205;
		internal const int SCI_CALLTIPSETFORE = 2206;
		internal const int SCI_CALLTIPSETFOREHLT = 2207;
		internal const int SCI_CALLTIPUSESTYLE = 2212;
		internal const int SCI_VISIBLEFROMDOCLINE = 2220;
		internal const int SCI_DOCLINEFROMVISIBLE = 2221;
		internal const int SCI_WRAPCOUNT = 2235;
		internal const int SC_FOLDLEVELBASE = 0x400;
		internal const int SC_FOLDLEVELWHITEFLAG = 0x1000;
		internal const int SC_FOLDLEVELHEADERFLAG = 0x2000;
		internal const int SC_FOLDLEVELNUMBERMASK = 0x0FFF;
		internal const int SCI_SETFOLDLEVEL = 2222;
		internal const int SCI_GETFOLDLEVEL = 2223;
		internal const int SCI_GETLASTCHILD = 2224;
		internal const int SCI_GETFOLDPARENT = 2225;
		internal const int SCI_SHOWLINES = 2226;
		internal const int SCI_HIDELINES = 2227;
		internal const int SCI_GETLINEVISIBLE = 2228;
		internal const int SCI_SETFOLDEXPANDED = 2229;
		internal const int SCI_GETFOLDEXPANDED = 2230;
		internal const int SCI_TOGGLEFOLD = 2231;
		internal const int SCI_ENSUREVISIBLE = 2232;
		internal const int SC_FOLDFLAG_LINEBEFORE_EXPANDED = 0x0002;
		internal const int SC_FOLDFLAG_LINEBEFORE_CONTRACTED = 0x0004;
		internal const int SC_FOLDFLAG_LINEAFTER_EXPANDED = 0x0008;
		internal const int SC_FOLDFLAG_LINEAFTER_CONTRACTED = 0x0010;
		internal const int SC_FOLDFLAG_LEVELNUMBERS = 0x0040;
		internal const int SCI_SETFOLDFLAGS = 2233;
		internal const int SCI_ENSUREVISIBLEENFORCEPOLICY = 2234;
		internal const int SCI_SETTABINDENTS = 2260;
		internal const int SCI_GETTABINDENTS = 2261;
		internal const int SCI_SETBACKSPACEUNINDENTS = 2262;
		internal const int SCI_GETBACKSPACEUNINDENTS = 2263;
		internal const int SC_TIME_FOREVER = 10000000;
		internal const int SCI_SETMOUSEDWELLTIME = 2264;
		internal const int SCI_GETMOUSEDWELLTIME = 2265;
		internal const int SCI_WORDSTARTPOSITION = 2266;
		internal const int SCI_WORDENDPOSITION = 2267;
		internal const int SC_WRAP_NONE = 0;
		internal const int SC_WRAP_WORD = 1;
		internal const int SC_WRAP_CHAR = 2;
		internal const int SCI_SETWRAPMODE = 2268;
		internal const int SCI_GETWRAPMODE = 2269;
		internal const int SC_WRAPVISUALFLAG_NONE = 0x0000;
		internal const int SC_WRAPVISUALFLAG_END = 0x0001;
		internal const int SC_WRAPVISUALFLAG_START = 0x0002;
		internal const int SCI_SETWRAPVISUALFLAGS = 2460;
		internal const int SCI_GETWRAPVISUALFLAGS = 2461;
		internal const int SC_WRAPVISUALFLAGLOC_DEFAULT = 0x0000;
		internal const int SC_WRAPVISUALFLAGLOC_END_BY_TEXT = 0x0001;
		internal const int SC_WRAPVISUALFLAGLOC_START_BY_TEXT = 0x0002;
		internal const int SCI_SETWRAPVISUALFLAGSLOCATION = 2462;
		internal const int SCI_GETWRAPVISUALFLAGSLOCATION = 2463;
		internal const int SCI_SETWRAPSTARTINDENT = 2464;
		internal const int SCI_GETWRAPSTARTINDENT = 2465;
		internal const int SC_WRAPINDENT_FIXED = 0;
		internal const int SC_WRAPINDENT_SAME = 1;
		internal const int SC_WRAPINDENT_INDENT = 2;
		internal const int SCI_SETWRAPINDENTMODE = 2472;
		internal const int SCI_GETWRAPINDENTMODE = 2473;
		internal const int SC_CACHE_NONE = 0;
		internal const int SC_CACHE_CARET = 1;
		internal const int SC_CACHE_PAGE = 2;
		internal const int SC_CACHE_DOCUMENT = 3;
		internal const int SCI_SETLAYOUTCACHE = 2272;
		internal const int SCI_GETLAYOUTCACHE = 2273;
		internal const int SCI_SETSCROLLWIDTH = 2274;
		internal const int SCI_GETSCROLLWIDTH = 2275;
		internal const int SCI_SETSCROLLWIDTHTRACKING = 2516;
		internal const int SCI_GETSCROLLWIDTHTRACKING = 2517;
		internal const int SCI_TEXTWIDTH = 2276;
		internal const int SCI_SETENDATLASTLINE = 2277;
		internal const int SCI_GETENDATLASTLINE = 2278;
		internal const int SCI_TEXTHEIGHT = 2279;
		internal const int SCI_SETVSCROLLBAR = 2280;
		internal const int SCI_GETVSCROLLBAR = 2281;
		internal const int SCI_APPENDTEXT = 2282;
		internal const int SCI_GETTWOPHASEDRAW = 2283;
		internal const int SCI_SETTWOPHASEDRAW = 2284;
		internal const int SCI_TARGETFROMSELECTION = 2287;
		internal const int SCI_LINESJOIN = 2288;
		internal const int SCI_LINESSPLIT = 2289;
		internal const int SCI_SETFOLDMARGINCOLOUR = 2290;
		internal const int SCI_SETFOLDMARGINHICOLOUR = 2291;
		internal const int SCI_LINEDOWN = 2300;
		internal const int SCI_LINEDOWNEXTEND = 2301;
		internal const int SCI_LINEUP = 2302;
		internal const int SCI_LINEUPEXTEND = 2303;
		internal const int SCI_CHARLEFT = 2304;
		internal const int SCI_CHARLEFTEXTEND = 2305;
		internal const int SCI_CHARRIGHT = 2306;
		internal const int SCI_CHARRIGHTEXTEND = 2307;
		internal const int SCI_WORDLEFT = 2308;
		internal const int SCI_WORDLEFTEXTEND = 2309;
		internal const int SCI_WORDRIGHT = 2310;
		internal const int SCI_WORDRIGHTEXTEND = 2311;
		internal const int SCI_HOME = 2312;
		internal const int SCI_HOMEEXTEND = 2313;
		internal const int SCI_LINEEND = 2314;
		internal const int SCI_LINEENDEXTEND = 2315;
		internal const int SCI_DOCUMENTSTART = 2316;
		internal const int SCI_DOCUMENTSTARTEXTEND = 2317;
		internal const int SCI_DOCUMENTEND = 2318;
		internal const int SCI_DOCUMENTENDEXTEND = 2319;
		internal const int SCI_PAGEUP = 2320;
		internal const int SCI_PAGEUPEXTEND = 2321;
		internal const int SCI_PAGEDOWN = 2322;
		internal const int SCI_PAGEDOWNEXTEND = 2323;
		internal const int SCI_EDITTOGGLEOVERTYPE = 2324;
		internal const int SCI_CANCEL = 2325;
		internal const int SCI_DELETEBACK = 2326;
		internal const int SCI_TAB = 2327;
		internal const int SCI_BACKTAB = 2328;
		internal const int SCI_NEWLINE = 2329;
		internal const int SCI_FORMFEED = 2330;
		internal const int SCI_VCHOME = 2331;
		internal const int SCI_VCHOMEEXTEND = 2332;
		internal const int SCI_ZOOMIN = 2333;
		internal const int SCI_ZOOMOUT = 2334;
		internal const int SCI_DELWORDLEFT = 2335;
		internal const int SCI_DELWORDRIGHT = 2336;
		internal const int SCI_DELWORDRIGHTEND = 2518;
		internal const int SCI_LINECUT = 2337;
		internal const int SCI_LINEDELETE = 2338;
		internal const int SCI_LINETRANSPOSE = 2339;
		internal const int SCI_LINEDUPLICATE = 2404;
		internal const int SCI_LOWERCASE = 2340;
		internal const int SCI_UPPERCASE = 2341;
		internal const int SCI_LINESCROLLDOWN = 2342;
		internal const int SCI_LINESCROLLUP = 2343;
		internal const int SCI_DELETEBACKNOTLINE = 2344;
		internal const int SCI_HOMEDISPLAY = 2345;
		internal const int SCI_HOMEDISPLAYEXTEND = 2346;
		internal const int SCI_LINEENDDISPLAY = 2347;
		internal const int SCI_LINEENDDISPLAYEXTEND = 2348;
		internal const int SCI_HOMEWRAP = 2349;
		internal const int SCI_HOMEWRAPEXTEND = 2450;
		internal const int SCI_LINEENDWRAP = 2451;
		internal const int SCI_LINEENDWRAPEXTEND = 2452;
		internal const int SCI_VCHOMEWRAP = 2453;
		internal const int SCI_VCHOMEWRAPEXTEND = 2454;
		internal const int SCI_LINECOPY = 2455;
		internal const int SCI_MOVECARETINSIDEVIEW = 2401;
		internal const int SCI_LINELENGTH = 2350;
		internal const int SCI_BRACEHIGHLIGHT = 2351;
		internal const int SCI_BRACEBADLIGHT = 2352;
		internal const int SCI_BRACEMATCH = 2353;
		internal const int SCI_GETVIEWEOL = 2355;
		internal const int SCI_SETVIEWEOL = 2356;
		internal const int SCI_GETDOCPOINTER = 2357;
		internal const int SCI_SETDOCPOINTER = 2358;
		internal const int SCI_SETMODEVENTMASK = 2359;
		internal const int EDGE_NONE = 0;
		internal const int EDGE_LINE = 1;
		internal const int EDGE_BACKGROUND = 2;
		internal const int SCI_GETEDGECOLUMN = 2360;
		internal const int SCI_SETEDGECOLUMN = 2361;
		internal const int SCI_GETEDGEMODE = 2362;
		internal const int SCI_SETEDGEMODE = 2363;
		internal const int SCI_GETEDGECOLOUR = 2364;
		internal const int SCI_SETEDGECOLOUR = 2365;
		internal const int SCI_SEARCHANCHOR = 2366;
		internal const int SCI_SEARCHNEXT = 2367;
		internal const int SCI_SEARCHPREV = 2368;
		internal const int SCI_LINESONSCREEN = 2370;
		internal const int SCI_USEPOPUP = 2371;
		internal const int SCI_SELECTIONISRECTANGLE = 2372;
		internal const int SCI_SETZOOM = 2373;
		internal const int SCI_GETZOOM = 2374;
		internal const int SCI_CREATEDOCUMENT = 2375;
		internal const int SCI_ADDREFDOCUMENT = 2376;
		internal const int SCI_RELEASEDOCUMENT = 2377;
		internal const int SCI_GETMODEVENTMASK = 2378;
		internal const int SCI_SETFOCUS = 2380;
		internal const int SCI_GETFOCUS = 2381;
		internal const int SC_STATUS_OK = 0;
		internal const int SC_STATUS_FAILURE = 1;
		internal const int SC_STATUS_BADALLOC = 2;
		internal const int SCI_SETSTATUS = 2382;
		internal const int SCI_GETSTATUS = 2383;
		internal const int SCI_SETMOUSEDOWNCAPTURES = 2384;
		internal const int SCI_GETMOUSEDOWNCAPTURES = 2385;
		internal const int SC_CURSORNORMAL = -1;
		internal const int SC_CURSORWAIT = 4;
		internal const int SCI_SETCURSOR = 2386;
		internal const int SCI_GETCURSOR = 2387;
		internal const int SCI_SETCONTROLCHARSYMBOL = 2388;
		internal const int SCI_GETCONTROLCHARSYMBOL = 2389;
		internal const int SCI_WORDPARTLEFT = 2390;
		internal const int SCI_WORDPARTLEFTEXTEND = 2391;
		internal const int SCI_WORDPARTRIGHT = 2392;
		internal const int SCI_WORDPARTRIGHTEXTEND = 2393;
		internal const int VISIBLE_SLOP = 0x01;
		internal const int VISIBLE_STRICT = 0x04;
		internal const int SCI_SETVISIBLEPOLICY = 2394;
		internal const int SCI_DELLINELEFT = 2395;
		internal const int SCI_DELLINERIGHT = 2396;
		internal const int SCI_SETXOFFSET = 2397;
		internal const int SCI_GETXOFFSET = 2398;
		internal const int SCI_CHOOSECARETX = 2399;
		internal const int SCI_GRABFOCUS = 2400;
		internal const int CARET_SLOP = 0x01;
		internal const int CARET_STRICT = 0x04;
		internal const int CARET_JUMPS = 0x10;
		internal const int CARET_EVEN = 0x08;
		internal const int SCI_SETXCARETPOLICY = 2402;
		internal const int SCI_SETYCARETPOLICY = 2403;
		internal const int SCI_SETPRINTWRAPMODE = 2406;
		internal const int SCI_GETPRINTWRAPMODE = 2407;
		internal const int SCI_SETHOTSPOTACTIVEFORE = 2410;
		internal const int SCI_GETHOTSPOTACTIVEFORE = 2494;
		internal const int SCI_SETHOTSPOTACTIVEBACK = 2411;
		internal const int SCI_GETHOTSPOTACTIVEBACK = 2495;
		internal const int SCI_SETHOTSPOTACTIVEUNDERLINE = 2412;
		internal const int SCI_GETHOTSPOTACTIVEUNDERLINE = 2496;
		internal const int SCI_SETHOTSPOTSINGLELINE = 2421;
		internal const int SCI_GETHOTSPOTSINGLELINE = 2497;
		internal const int SCI_PARADOWN = 2413;
		internal const int SCI_PARADOWNEXTEND = 2414;
		internal const int SCI_PARAUP = 2415;
		internal const int SCI_PARAUPEXTEND = 2416;
		internal const int SCI_POSITIONBEFORE = 2417;
		internal const int SCI_POSITIONAFTER = 2418;
		internal const int SCI_COPYRANGE = 2419;
		internal const int SCI_COPYTEXT = 2420;
		internal const int SC_SEL_STREAM = 0;
		internal const int SC_SEL_RECTANGLE = 1;
		internal const int SC_SEL_LINES = 2;
		internal const int SC_SEL_THIN = 3;
		internal const int SCI_SETSELECTIONMODE = 2422;
		internal const int SCI_GETSELECTIONMODE = 2423;
		internal const int SCI_GETLINESELSTARTPOSITION = 2424;
		internal const int SCI_GETLINESELENDPOSITION = 2425;
		internal const int SCI_LINEDOWNRECTEXTEND = 2426;
		internal const int SCI_LINEUPRECTEXTEND = 2427;
		internal const int SCI_CHARLEFTRECTEXTEND = 2428;
		internal const int SCI_CHARRIGHTRECTEXTEND = 2429;
		internal const int SCI_HOMERECTEXTEND = 2430;
		internal const int SCI_VCHOMERECTEXTEND = 2431;
		internal const int SCI_LINEENDRECTEXTEND = 2432;
		internal const int SCI_PAGEUPRECTEXTEND = 2433;
		internal const int SCI_PAGEDOWNRECTEXTEND = 2434;
		internal const int SCI_STUTTEREDPAGEUP = 2435;
		internal const int SCI_STUTTEREDPAGEUPEXTEND = 2436;
		internal const int SCI_STUTTEREDPAGEDOWN = 2437;
		internal const int SCI_STUTTEREDPAGEDOWNEXTEND = 2438;
		internal const int SCI_WORDLEFTEND = 2439;
		internal const int SCI_WORDLEFTENDEXTEND = 2440;
		internal const int SCI_WORDRIGHTEND = 2441;
		internal const int SCI_WORDRIGHTENDEXTEND = 2442;
		internal const int SCI_SETWHITESPACECHARS = 2443;
		internal const int SCI_SETCHARSDEFAULT = 2444;
		internal const int SCI_AUTOCGETCURRENT = 2445;
		internal const int SCI_ALLOCATE = 2446;
		internal const int SCI_TARGETASUTF8 = 2447;
		internal const int SCI_SETLENGTHFORENCODE = 2448;
		internal const int SCI_ENCODEDFROMUTF8 = 2449;
		internal const int SCI_FINDCOLUMN = 2456;
		internal const int SCI_GETCARETSTICKY = 2457;
		internal const int SCI_SETCARETSTICKY = 2458;
		internal const int SCI_TOGGLECARETSTICKY = 2459;
		internal const int SCI_SETPASTECONVERTENDINGS = 2467;
		internal const int SCI_GETPASTECONVERTENDINGS = 2468;
		internal const int SCI_SELECTIONDUPLICATE = 2469;
		internal const int SC_ALPHA_TRANSPARENT = 0;
		internal const int SC_ALPHA_OPAQUE = 255;
		internal const int SC_ALPHA_NOALPHA = 256;
		internal const int SCI_SETCARETLINEBACKALPHA = 2470;
		internal const int SCI_GETCARETLINEBACKALPHA = 2471;
		internal const int CARETSTYLE_INVISIBLE = 0;
		internal const int CARETSTYLE_LINE = 1;
		internal const int CARETSTYLE_BLOCK = 2;
		internal const int SCI_SETCARETSTYLE = 2512;
		internal const int SCI_GETCARETSTYLE = 2513;
		internal const int SCI_SETINDICATORCURRENT = 2500;
		internal const int SCI_GETINDICATORCURRENT = 2501;
		internal const int SCI_SETINDICATORVALUE = 2502;
		internal const int SCI_GETINDICATORVALUE = 2503;
		internal const int SCI_INDICATORFILLRANGE = 2504;
		internal const int SCI_INDICATORCLEARRANGE = 2505;
		internal const int SCI_INDICATORALLONFOR = 2506;
		internal const int SCI_INDICATORVALUEAT = 2507;
		internal const int SCI_INDICATORSTART = 2508;
		internal const int SCI_INDICATOREND = 2509;
		internal const int SCI_SETPOSITIONCACHE = 2514;
		internal const int SCI_GETPOSITIONCACHE = 2515;
		internal const int SCI_COPYALLOWLINE = 2519;
		internal const int SCI_GETCHARACTERPOINTER = 2520;
		internal const int SCI_SETKEYSUNICODE = 2521;
		internal const int SCI_GETKEYSUNICODE = 2522;
		internal const int SCI_INDICSETALPHA = 2523;
		internal const int SCI_INDICGETALPHA = 2524;
		internal const int SCI_SETEXTRAASCENT = 2525;
		internal const int SCI_GETEXTRAASCENT = 2526;
		internal const int SCI_SETEXTRADESCENT = 2527;
		internal const int SCI_GETEXTRADESCENT = 2528;
		internal const int SCI_MARKERSYMBOLDEFINED = 2529;
		internal const int SCI_MARGINSETTEXT = 2530;
		internal const int SCI_MARGINGETTEXT = 2531;
		internal const int SCI_MARGINSETSTYLE = 2532;
		internal const int SCI_MARGINGETSTYLE = 2533;
		internal const int SCI_MARGINSETSTYLES = 2534;
		internal const int SCI_MARGINGETSTYLES = 2535;
		internal const int SCI_MARGINTEXTCLEARALL = 2536;
		internal const int SCI_MARGINSETSTYLEOFFSET = 2537;
		internal const int SCI_MARGINGETSTYLEOFFSET = 2538;
		internal const int SCI_ANNOTATIONSETTEXT = 2540;
		internal const int SCI_ANNOTATIONGETTEXT = 2541;
		internal const int SCI_ANNOTATIONSETSTYLE = 2542;
		internal const int SCI_ANNOTATIONGETSTYLE = 2543;
		internal const int SCI_ANNOTATIONSETSTYLES = 2544;
		internal const int SCI_ANNOTATIONGETSTYLES = 2545;
		internal const int SCI_ANNOTATIONGETLINES = 2546;
		internal const int SCI_ANNOTATIONCLEARALL = 2547;
		internal const int ANNOTATION_HIDDEN = 0;
		internal const int ANNOTATION_STANDARD = 1;
		internal const int ANNOTATION_BOXED = 2;
		internal const int SCI_ANNOTATIONSETVISIBLE = 2548;
		internal const int SCI_ANNOTATIONGETVISIBLE = 2549;
		internal const int SCI_ANNOTATIONSETSTYLEOFFSET = 2550;
		internal const int SCI_ANNOTATIONGETSTYLEOFFSET = 2551;
		internal const int UNDO_MAY_COALESCE = 1;
		internal const int SCI_ADDUNDOACTION = 2560;
		internal const int SCI_CHARPOSITIONFROMPOINT = 2561;
		internal const int SCI_CHARPOSITIONFROMPOINTCLOSE = 2562;
		internal const int SCI_SETMULTIPLESELECTION = 2563;
		internal const int SCI_GETMULTIPLESELECTION = 2564;

        internal const int SCI_SETMULTIPASTE = 2614;
        internal const int SCI_GETMULTIPASTE = 2615;

		internal const int SCI_SETADDITIONALSELECTIONTYPING = 2565;
		internal const int SCI_GETADDITIONALSELECTIONTYPING = 2566;
		internal const int SCI_SETADDITIONALCARETSBLINK = 2567;
		internal const int SCI_GETADDITIONALCARETSBLINK = 2568;
		internal const int SCI_GETSELECTIONS = 2570;
		internal const int SCI_CLEARSELECTIONS = 2571;
		internal const int SCI_SETSELECTION = 2572;
		internal const int SCI_ADDSELECTION = 2573;
		internal const int SCI_SETMAINSELECTION = 2574;
		internal const int SCI_GETMAINSELECTION = 2575;
		internal const int SCI_SETSELECTIONNCARET = 2576;
		internal const int SCI_GETSELECTIONNCARET = 2577;
		internal const int SCI_SETSELECTIONNANCHOR = 2578;
		internal const int SCI_GETSELECTIONNANCHOR = 2579;
		internal const int SCI_SETSELECTIONNCARETVIRTUALSPACE = 2580;
		internal const int SCI_GETSELECTIONNCARETVIRTUALSPACE = 2581;
		internal const int SCI_SETSELECTIONNANCHORVIRTUALSPACE = 2582;
		internal const int SCI_GETSELECTIONNANCHORVIRTUALSPACE = 2583;
		internal const int SCI_SETSELECTIONNSTART = 2584;
		internal const int SCI_GETSELECTIONNSTART = 2585;
		internal const int SCI_SETSELECTIONNEND = 2586;
		internal const int SCI_GETSELECTIONNEND = 2587;
		internal const int SCI_SETRECTANGULARSELECTIONCARET = 2588;
		internal const int SCI_GETRECTANGULARSELECTIONCARET = 2589;
		internal const int SCI_SETRECTANGULARSELECTIONANCHOR = 2590;
		internal const int SCI_GETRECTANGULARSELECTIONANCHOR = 2591;
		internal const int SCI_SETRECTANGULARSELECTIONCARETVIRTUALSPACE = 2592;
		internal const int SCI_GETRECTANGULARSELECTIONCARETVIRTUALSPACE = 2593;
		internal const int SCI_SETRECTANGULARSELECTIONANCHORVIRTUALSPACE = 2594;
		internal const int SCI_GETRECTANGULARSELECTIONANCHORVIRTUALSPACE = 2595;
		internal const int SCVS_NONE = 0;
		internal const int SCVS_RECTANGULARSELECTION = 1;
		internal const int SCVS_USERACCESSIBLE = 2;
		internal const int SCI_SETVIRTUALSPACEOPTIONS = 2596;
		internal const int SCI_GETVIRTUALSPACEOPTIONS = 2597;
		internal const int SCI_SETRECTANGULARSELECTIONMODIFIER = 2598;
		internal const int SCI_GETRECTANGULARSELECTIONMODIFIER = 2599;
		internal const int SCI_SETADDITIONALSELFORE = 2600;
		internal const int SCI_SETADDITIONALSELBACK = 2601;
		internal const int SCI_SETADDITIONALSELALPHA = 2602;
		internal const int SCI_GETADDITIONALSELALPHA = 2603;
		internal const int SCI_SETADDITIONALCARETFORE = 2604;
		internal const int SCI_GETADDITIONALCARETFORE = 2605;
		internal const int SCI_ROTATESELECTION = 2606;
		internal const int SCI_SWAPMAINANCHORCARET = 2607;
		internal const int SCI_STARTRECORD = 3001;
		internal const int SCI_STOPRECORD = 3002;
		internal const int SCI_SETLEXER = 4001;
		internal const int SCI_GETLEXER = 4002;
		internal const int SCI_COLOURISE = 4003;
		internal const int SCI_SETPROPERTY = 4004;
		internal const int KEYWORDSET_MAX = 8;
		internal const int SCI_SETKEYWORDS = 4005;
		internal const int SCI_SETLEXERLANGUAGE = 4006;
		internal const int SCI_LOADLEXERLIBRARY = 4007;
		internal const int SCI_GETPROPERTY = 4008;
		internal const int SCI_GETPROPERTYEXPANDED = 4009;
		internal const int SCI_GETPROPERTYINT = 4010;
		internal const int SCI_GETSTYLEBITSNEEDED = 4011;
		internal const int SC_MOD_INSERTTEXT = 0x1;
		internal const int SC_MOD_DELETETEXT = 0x2;
		internal const int SC_MOD_CHANGESTYLE = 0x4;
		internal const int SC_MOD_CHANGEFOLD = 0x8;
		internal const int SC_PERFORMED_USER = 0x10;
		internal const int SC_PERFORMED_UNDO = 0x20;
		internal const int SC_PERFORMED_REDO = 0x40;
		internal const int SC_MULTISTEPUNDOREDO = 0x80;
		internal const int SC_LASTSTEPINUNDOREDO = 0x100;
		internal const int SC_MOD_CHANGEMARKER = 0x200;
		internal const int SC_MOD_BEFOREINSERT = 0x400;
		internal const int SC_MOD_BEFOREDELETE = 0x800;
		internal const int SC_MULTILINEUNDOREDO = 0x1000;
		internal const int SC_STARTACTION = 0x2000;
		internal const int SC_MOD_CHANGEINDICATOR = 0x4000;
		internal const int SC_MOD_CHANGELINESTATE = 0x8000;
		internal const int SC_MOD_CHANGEMARGIN = 0x10000;
		internal const int SC_MOD_CHANGEANNOTATION = 0x20000;
		internal const int SC_MOD_CONTAINER = 0x40000;
		internal const int SC_MODEVENTMASKALL = 0x7FFFF;
		internal const int SCEN_CHANGE = 768;
		internal const int SCEN_SETFOCUS = 512;
		internal const int SCEN_KILLFOCUS = 256;
		internal const int SCK_DOWN = 300;
		internal const int SCK_UP = 301;
		internal const int SCK_LEFT = 302;
		internal const int SCK_RIGHT = 303;
		internal const int SCK_HOME = 304;
		internal const int SCK_END = 305;
		internal const int SCK_PRIOR = 306;
		internal const int SCK_NEXT = 307;
		internal const int SCK_DELETE = 308;
		internal const int SCK_INSERT = 309;
		internal const int SCK_ESCAPE = 7;
		internal const int SCK_BACK = 8;
		internal const int SCK_TAB = 9;
		internal const int SCK_RETURN = 13;
		internal const int SCK_ADD = 310;
		internal const int SCK_SUBTRACT = 311;
		internal const int SCK_DIVIDE = 312;
		internal const int SCK_WIN = 313;
		internal const int SCK_RWIN = 314;
		internal const int SCK_MENU = 315;
		internal const int SCMOD_NORM = 0;
		internal const int SCMOD_SHIFT = 1;
		internal const int SCMOD_CTRL = 2;
		internal const int SCMOD_ALT = 4;
		internal const int SCMOD_SUPER = 8;
		internal const int SCN_STYLENEEDED = 2000;
		internal const int SCN_CHARADDED = 2001;
		internal const int SCN_SAVEPOINTREACHED = 2002;
		internal const int SCN_SAVEPOINTLEFT = 2003;
		internal const int SCN_MODIFYATTEMPTRO = 2004;
		internal const int SCN_KEY = 2005;
		internal const int SCN_DOUBLECLICK = 2006;
		internal const int SCN_UPDATEUI = 2007;
		internal const int SCN_MODIFIED = 2008;
		internal const int SCN_MACRORECORD = 2009;
		internal const int SCN_MARGINCLICK = 2010;
		internal const int SCN_NEEDSHOWN = 2011;
		internal const int SCN_PAINTED = 2013;
		internal const int SCN_USERLISTSELECTION = 2014;
		internal const int SCN_URIDROPPED = 2015;
		internal const int SCN_DWELLSTART = 2016;
		internal const int SCN_DWELLEND = 2017;
		internal const int SCN_ZOOM = 2018;
		internal const int SCN_HOTSPOTCLICK = 2019;
		internal const int SCN_HOTSPOTDOUBLECLICK = 2020;
		internal const int SCN_CALLTIPCLICK = 2021;
		internal const int SCN_AUTOCSELECTION = 2022;
		internal const int SCN_INDICATORCLICK = 2023;
		internal const int SCN_INDICATORRELEASE = 2024;
		internal const int SCN_AUTOCCANCELLED = 2025;
		internal const int SCN_AUTOCCHARDELETED = 2026;
	}
}
