
// MIT License
// 
// Copyright (c) 2009-2017 Luca Piccioni
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
// 
// This file is automatically generated

#pragma warning disable 649, 1572, 1573

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

using Khronos;

namespace OpenGL
{
	public partial class Gl
	{
		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_API_ERROR_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_API_ERROR_AMD = 0x9149;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_WINDOW_SYSTEM_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_WINDOW_SYSTEM_AMD = 0x914A;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_DEPRECATION_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_DEPRECATION_AMD = 0x914B;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_UNDEFINED_BEHAVIOR_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_UNDEFINED_BEHAVIOR_AMD = 0x914C;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_PERFORMANCE_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_PERFORMANCE_AMD = 0x914D;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_SHADER_COMPILER_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_SHADER_COMPILER_AMD = 0x914E;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_APPLICATION_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_APPLICATION_AMD = 0x914F;

		/// <summary>
		/// [GL] Value of GL_DEBUG_CATEGORY_OTHER_AMD symbol.
		/// </summary>
		[RequiredByFeature("GL_AMD_debug_output")]
		public const int DEBUG_CATEGORY_OTHER_AMD = 0x9150;

		/// <summary>
		/// [GL] glDebugMessageEnableAMD: Binding for glDebugMessageEnableAMD.
		/// </summary>
		/// <param name="category">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="severity">
		/// A <see cref="T:DebugSeverity"/>.
		/// </param>
		/// <param name="count">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="ids">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		/// <param name="enabled">
		/// A <see cref="T:bool"/>.
		/// </param>
		[RequiredByFeature("GL_AMD_debug_output")]
		public static void DebugMessageEnableAMD(Int32 category, DebugSeverity severity, Int32 count, UInt32[] ids, bool enabled)
		{
			unsafe {
				fixed (UInt32* p_ids = ids)
				{
					Debug.Assert(Delegates.pglDebugMessageEnableAMD != null, "pglDebugMessageEnableAMD not implemented");
					Delegates.pglDebugMessageEnableAMD(category, (Int32)severity, count, p_ids, enabled);
					LogCommand("glDebugMessageEnableAMD", null, category, severity, count, ids, enabled					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glDebugMessageEnableAMD: Binding for glDebugMessageEnableAMD.
		/// </summary>
		/// <param name="category">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="severity">
		/// A <see cref="T:DebugSeverity"/>.
		/// </param>
		/// <param name="ids">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		/// <param name="enabled">
		/// A <see cref="T:bool"/>.
		/// </param>
		[RequiredByFeature("GL_AMD_debug_output")]
		public static void DebugMessageEnableAMD(Int32 category, DebugSeverity severity, UInt32[] ids, bool enabled)
		{
			unsafe {
				fixed (UInt32* p_ids = ids)
				{
					Debug.Assert(Delegates.pglDebugMessageEnableAMD != null, "pglDebugMessageEnableAMD not implemented");
					Delegates.pglDebugMessageEnableAMD(category, (Int32)severity, (Int32)ids.Length, p_ids, enabled);
					LogCommand("glDebugMessageEnableAMD", null, category, severity, ids.Length, ids, enabled					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glDebugMessageInsertAMD: Binding for glDebugMessageInsertAMD.
		/// </summary>
		/// <param name="category">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="severity">
		/// A <see cref="T:DebugSeverity"/>.
		/// </param>
		/// <param name="id">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="length">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="buf">
		/// A <see cref="T:String"/>.
		/// </param>
		[RequiredByFeature("GL_AMD_debug_output")]
		public static void DebugMessageInsertAMD(Int32 category, DebugSeverity severity, UInt32 id, Int32 length, String buf)
		{
			Debug.Assert(Delegates.pglDebugMessageInsertAMD != null, "pglDebugMessageInsertAMD not implemented");
			Delegates.pglDebugMessageInsertAMD(category, (Int32)severity, id, length, buf);
			LogCommand("glDebugMessageInsertAMD", null, category, severity, id, length, buf			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glDebugMessageCallbackAMD: Binding for glDebugMessageCallbackAMD.
		/// </summary>
		/// <param name="callback">
		/// A <see cref="T:Gl.DebugProc"/>.
		/// </param>
		/// <param name="userParam">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		[RequiredByFeature("GL_AMD_debug_output")]
		public static void DebugMessageCallbackAMD(Gl.DebugProc callback, IntPtr userParam)
		{
			Debug.Assert(Delegates.pglDebugMessageCallbackAMD != null, "pglDebugMessageCallbackAMD not implemented");
			Delegates.pglDebugMessageCallbackAMD(callback, userParam);
			LogCommand("glDebugMessageCallbackAMD", null, callback, userParam			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glGetDebugMessageLogAMD: Binding for glGetDebugMessageLogAMD.
		/// </summary>
		/// <param name="bufsize">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="categories">
		/// A <see cref="T:Int32[]"/>.
		/// </param>
		/// <param name="severities">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		/// <param name="ids">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		/// <param name="lengths">
		/// A <see cref="T:Int32[]"/>.
		/// </param>
		/// <param name="message">
		/// A <see cref="T:StringBuilder"/>.
		/// </param>
		[RequiredByFeature("GL_AMD_debug_output")]
		public static UInt32 GetDebugMessageLogAMD(Int32 bufsize, [Out] Int32[] categories, [Out] UInt32[] severities, [Out] UInt32[] ids, [Out] Int32[] lengths, [Out] StringBuilder message)
		{
			UInt32 retValue;

			unsafe {
				fixed (Int32* p_categories = categories)
				fixed (UInt32* p_severities = severities)
				fixed (UInt32* p_ids = ids)
				fixed (Int32* p_lengths = lengths)
				{
					Debug.Assert(Delegates.pglGetDebugMessageLogAMD != null, "pglGetDebugMessageLogAMD not implemented");
					retValue = Delegates.pglGetDebugMessageLogAMD((UInt32)categories.Length, bufsize, p_categories, p_severities, p_ids, p_lengths, message);
					LogCommand("glGetDebugMessageLogAMD", retValue, categories.Length, bufsize, categories, severities, ids, lengths, message					);
				}
			}
			DebugCheckErrors(retValue);

			return (retValue);
		}

		internal unsafe static partial class Delegates
		{
			[RequiredByFeature("GL_AMD_debug_output")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glDebugMessageEnableAMD(Int32 category, Int32 severity, Int32 count, UInt32* ids, [MarshalAs(UnmanagedType.I1)] bool enabled);

			[RequiredByFeature("GL_AMD_debug_output")]
			[ThreadStatic]
			internal static glDebugMessageEnableAMD pglDebugMessageEnableAMD;

			[RequiredByFeature("GL_AMD_debug_output")]
			[SuppressUnmanagedCodeSecurity()]
			internal delegate void glDebugMessageInsertAMD(Int32 category, Int32 severity, UInt32 id, Int32 length, String buf);

			[RequiredByFeature("GL_AMD_debug_output")]
			[ThreadStatic]
			internal static glDebugMessageInsertAMD pglDebugMessageInsertAMD;

			[RequiredByFeature("GL_AMD_debug_output")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glDebugMessageCallbackAMD(Gl.DebugProc callback, IntPtr userParam);

			[RequiredByFeature("GL_AMD_debug_output")]
			[ThreadStatic]
			internal static glDebugMessageCallbackAMD pglDebugMessageCallbackAMD;

			[RequiredByFeature("GL_AMD_debug_output")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate UInt32 glGetDebugMessageLogAMD(UInt32 count, Int32 bufsize, Int32* categories, UInt32* severities, UInt32* ids, Int32* lengths, [Out] StringBuilder message);

			[RequiredByFeature("GL_AMD_debug_output")]
			[ThreadStatic]
			internal static glGetDebugMessageLogAMD pglGetDebugMessageLogAMD;

		}
	}

}
