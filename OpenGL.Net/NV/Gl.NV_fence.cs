
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
		/// [GL] Value of GL_ALL_COMPLETED_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public const int ALL_COMPLETED_NV = 0x84F2;

		/// <summary>
		/// [GL] Value of GL_FENCE_STATUS_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public const int FENCE_STATUS_NV = 0x84F3;

		/// <summary>
		/// [GL] Value of GL_FENCE_CONDITION_NV symbol.
		/// </summary>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public const int FENCE_CONDITION_NV = 0x84F4;

		/// <summary>
		/// [GL] glDeleteFencesNV: Binding for glDeleteFencesNV.
		/// </summary>
		/// <param name="fences">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static void DeleteFencesNV(params UInt32[] fences)
		{
			unsafe {
				fixed (UInt32* p_fences = fences)
				{
					Debug.Assert(Delegates.pglDeleteFencesNV != null, "pglDeleteFencesNV not implemented");
					Delegates.pglDeleteFencesNV((Int32)fences.Length, p_fences);
					LogCommand("glDeleteFencesNV", null, fences.Length, fences					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glGenFencesNV: Binding for glGenFencesNV.
		/// </summary>
		/// <param name="fences">
		/// A <see cref="T:UInt32[]"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static void GenFencesNV(UInt32[] fences)
		{
			unsafe {
				fixed (UInt32* p_fences = fences)
				{
					Debug.Assert(Delegates.pglGenFencesNV != null, "pglGenFencesNV not implemented");
					Delegates.pglGenFencesNV((Int32)fences.Length, p_fences);
					LogCommand("glGenFencesNV", null, fences.Length, fences					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glGenFencesNV: Binding for glGenFencesNV.
		/// </summary>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static UInt32 GenFenceNV()
		{
			UInt32 retValue;
			unsafe {
				Delegates.pglGenFencesNV(1, &retValue);
				LogCommand("glGenFencesNV", null, 1, "{ " + retValue + " }"				);
			}
			DebugCheckErrors(null);
			return (retValue);
		}

		/// <summary>
		/// [GL] glIsFenceNV: Binding for glIsFenceNV.
		/// </summary>
		/// <param name="fence">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static bool IsFenceNV(UInt32 fence)
		{
			bool retValue;

			Debug.Assert(Delegates.pglIsFenceNV != null, "pglIsFenceNV not implemented");
			retValue = Delegates.pglIsFenceNV(fence);
			LogCommand("glIsFenceNV", retValue, fence			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glTestFenceNV: Binding for glTestFenceNV.
		/// </summary>
		/// <param name="fence">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static bool TestFenceNV(UInt32 fence)
		{
			bool retValue;

			Debug.Assert(Delegates.pglTestFenceNV != null, "pglTestFenceNV not implemented");
			retValue = Delegates.pglTestFenceNV(fence);
			LogCommand("glTestFenceNV", retValue, fence			);
			DebugCheckErrors(retValue);

			return (retValue);
		}

		/// <summary>
		/// [GL] glGetFenceivNV: Binding for glGetFenceivNV.
		/// </summary>
		/// <param name="fence">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="pname">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="params">
		/// A <see cref="T:Int32[]"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static void GetFenceNV(UInt32 fence, Int32 pname, [Out] Int32[] @params)
		{
			unsafe {
				fixed (Int32* p_params = @params)
				{
					Debug.Assert(Delegates.pglGetFenceivNV != null, "pglGetFenceivNV not implemented");
					Delegates.pglGetFenceivNV(fence, pname, p_params);
					LogCommand("glGetFenceivNV", null, fence, pname, @params					);
				}
			}
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glFinishFenceNV: Binding for glFinishFenceNV.
		/// </summary>
		/// <param name="fence">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static void FinishFenceNV(UInt32 fence)
		{
			Debug.Assert(Delegates.pglFinishFenceNV != null, "pglFinishFenceNV not implemented");
			Delegates.pglFinishFenceNV(fence);
			LogCommand("glFinishFenceNV", null, fence			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glSetFenceNV: Binding for glSetFenceNV.
		/// </summary>
		/// <param name="fence">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="condition">
		/// A <see cref="T:Int32"/>.
		/// </param>
		[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
		public static void SetFenceNV(UInt32 fence, Int32 condition)
		{
			Debug.Assert(Delegates.pglSetFenceNV != null, "pglSetFenceNV not implemented");
			Delegates.pglSetFenceNV(fence, condition);
			LogCommand("glSetFenceNV", null, fence, condition			);
			DebugCheckErrors(null);
		}

		internal unsafe static partial class Delegates
		{
			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glDeleteFencesNV(Int32 n, UInt32* fences);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glDeleteFencesNV pglDeleteFencesNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glGenFencesNV(Int32 n, UInt32* fences);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glGenFencesNV pglGenFencesNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			[return: MarshalAs(UnmanagedType.I1)]
			internal delegate bool glIsFenceNV(UInt32 fence);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glIsFenceNV pglIsFenceNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			[return: MarshalAs(UnmanagedType.I1)]
			internal delegate bool glTestFenceNV(UInt32 fence);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glTestFenceNV pglTestFenceNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glGetFenceivNV(UInt32 fence, Int32 pname, Int32* @params);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glGetFenceivNV pglGetFenceivNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			internal delegate void glFinishFenceNV(UInt32 fence);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glFinishFenceNV pglFinishFenceNV;

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[SuppressUnmanagedCodeSecurity()]
			internal delegate void glSetFenceNV(UInt32 fence, Int32 condition);

			[RequiredByFeature("GL_NV_fence", Api = "gl|gles1|gles2")]
			[ThreadStatic]
			internal static glSetFenceNV pglSetFenceNV;

		}
	}

}
