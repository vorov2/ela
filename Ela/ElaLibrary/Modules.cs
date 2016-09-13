using Ela.Linking;
using Ela.Library.General;
//using Ela.Library.Collections;

[assembly: ElaModule("libCon", typeof(ConModule))]
[assembly: ElaModule("libCore", typeof(CoreModule))]
[assembly: ElaModule("libCell", typeof(CellModule))]
[assembly: ElaModule("libIO", typeof(IOModule))]
[assembly: ElaModule("libChar", typeof(CharModule))]
[assembly: ElaModule("libString", typeof(StringModule))]
[assembly: ElaModule("libNumber", typeof(NumberModule))]
[assembly: ElaModule("libDateTime", typeof(DateTimeModule))]
[assembly: ElaModule("libRecord", typeof(RecordModule))]
[assembly: ElaModule("libReflect", typeof(ReflectModule))]
[assembly: ElaModule("libXml", typeof(XmlModule))]
[assembly: ElaModule("libBigInteger", typeof(BigIntegerModule))]
[assembly: ElaModule("libMoney", typeof(MoneyModule))]
[assembly: ElaModule("libGuid", typeof(GuidModule))]
[assembly: ElaModule("libComplex", typeof(ComplexModule))]
[assembly: ElaModule("libJson", typeof(JsonModule))]

[assembly: ElaModule("experimental", typeof(Experimental))]
[assembly: ElaModule("debug", typeof(DebugModule))]
//[assembly: ElaModule("StringBuilder", typeof(StringBuilderModule))]
//[assembly: ElaModule("Shell", typeof(ShellModule))]
//[assembly: ElaModule("async", typeof(AsyncModule))]

//[assembly: ElaModule("MutableMap", typeof(MutableMapModule))]
//[assembly: ElaModule("Map", typeof(MapModule))]
//[assembly: ElaModule("Set", typeof(SetModule))]
//[assembly: ElaModule("Array", typeof(ArrayModule))]
//[assembly: ElaModule("Queue", typeof(QueueModule))]
