using System;

namespace Elide.Scintilla.Internal
{
	internal static class NativeConstants
	{
		internal const uint WM_PAINT = 0x000F;

		internal const uint WM_NOTIFY = 0x004E;

		internal const uint WM_USER = 0x0400;

		internal const uint OCM_BASE = WM_USER + 0x1C00;

		internal const uint WM_REFLECT = WM_USER + 0x1c00;

		internal const uint OCM_NOTIFY = WM_REFLECT + WM_NOTIFY;
	}
}
