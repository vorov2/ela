using System;

namespace Ela.Linking
{
	public enum ElaLinkerError
	{
		None = 0,


		AssemblyLoad = 600,

		DuplicateModuleInAssembly = 601,

		ForeignModuleInitFailed = 602,

		ForeignModuleDescriptorMissing = 603,

		ForeignModuleInvalidType = 604,

		ModuleNameInvalid = 605,

		ModuleNotFoundInAssembly = 606,

		ObjectFileReadFailed = 607,

		UnresolvedModule = 608,

		ModuleLinkFailed = 609,

        ExportedNameRemoved = 610,

        ExportedNameChanged = 611,

        InstanceAlreadyExists = 612,

        CyclicReference = 613,
	}
}
