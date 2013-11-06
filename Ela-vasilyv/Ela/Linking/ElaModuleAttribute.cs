using System;

namespace Ela.Linking
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public sealed class ElaModuleAttribute : Attribute
	{
		#region Construction
		public ElaModuleAttribute(string moduleName, Type classType)
		{
			ModuleName = moduleName;
			ClassType = classType;
		}
		#endregion


		#region Properties
		public string ModuleName { get; private set; }

		public Type ClassType { get; private set; }
		#endregion
	}
}
