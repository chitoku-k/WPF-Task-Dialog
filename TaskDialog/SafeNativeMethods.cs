using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace TaskDialogInterop
{
	/// <summary>
	/// Provides safe Win32 API wrapper calls for various actions not directly
	/// supported by WPF classes out of the box.
	/// </summary>
	internal class SafeNativeMethods
	{
		/// <summary>
		/// Sets the window's close button visibility.
		/// </summary>
		/// <param name="window">The window to set.</param>
		/// <param name="showCloseButton"><c>true</c> to show the close button; otherwise, <c>false</c></param>
		public static void SetWindowCloseButtonVisibility(Window window, bool showCloseButton)
		{
			WindowInteropHelper wih = new WindowInteropHelper(window);

			int style = NativeMethods.GetWindowLong(wih.Handle, NativeMethods.GWL_STYLE);

			if (showCloseButton)
				NativeMethods.SetWindowLong(wih.Handle, NativeMethods.GWL_STYLE, style & NativeMethods.WS_SYSMENU);
			else
				NativeMethods.SetWindowLong(wih.Handle, NativeMethods.GWL_STYLE, style & ~NativeMethods.WS_SYSMENU);
		}
		/// <summary>
		/// Sets the window's icon visibility.
		/// </summary>
		/// <param name="window">The window to set.</param>
		/// <param name="showIcon"><c>true</c> to show the icon in the caption; otherwise, <c>false</c></param>
		public static void SetWindowIconVisibility(Window window, bool showIcon)
		{
			WindowInteropHelper wih = new WindowInteropHelper(window);

			IntPtr icon = IntPtr.Zero;
			int style = NativeMethods.GetWindowLong(wih.Handle, NativeMethods.GWL_EXSTYLE) | NativeMethods.WS_EX_DLGMODALFRAME;

			if (showIcon)
			{
				style &= ~NativeMethods.WS_EX_DLGMODALFRAME;
				icon = NativeMethods.DefWindowProc(wih.Handle, NativeMethods.WM_SETICON, new IntPtr(0), IntPtr.Zero);
			}

			NativeMethods.SetWindowLong(wih.Handle, NativeMethods.GWL_EXSTYLE, style);
			NativeMethods.SendMessage(wih.Handle, NativeMethods.WM_SETICON, (IntPtr)NativeMethods.ICON_SMALL, icon);
			NativeMethods.SendMessage(wih.Handle, NativeMethods.WM_SETICON, (IntPtr)NativeMethods.ICON_BIG, icon);

			NativeMethods.SetWindowPos(wih.Handle, IntPtr.Zero, 0, 0, 0, 0,
				NativeMethods.SWP_NOMOVE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOZORDER | NativeMethods.SWP_FRAMECHANGED);
		}
	}
}
