
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
		/// [GL] Value of GL_SPARSE_STORAGE_BIT_ARB symbol.
		/// </summary>
		[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
		[Log(BitmaskName = "GL")]
		public const int SPARSE_STORAGE_BIT_ARB = 0x0400;

		/// <summary>
		/// [GL] Value of GL_SPARSE_BUFFER_PAGE_SIZE_ARB symbol.
		/// </summary>
		[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
		public const int SPARSE_BUFFER_PAGE_SIZE_ARB = 0x82F8;

		/// <summary>
		/// [GL] glBufferPageCommitmentARB: Binding for glBufferPageCommitmentARB.
		/// </summary>
		/// <param name="target">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="offset">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="size">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="commit">
		/// A <see cref="T:bool"/>.
		/// </param>
		[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
		public static void BufferPageCommitmentARB(Int32 target, IntPtr offset, UInt32 size, bool commit)
		{
			Debug.Assert(Delegates.pglBufferPageCommitmentARB != null, "pglBufferPageCommitmentARB not implemented");
			Delegates.pglBufferPageCommitmentARB(target, offset, size, commit);
			LogCommand("glBufferPageCommitmentARB", null, target, offset, size, commit			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glNamedBufferPageCommitmentEXT: Binding for glNamedBufferPageCommitmentEXT.
		/// </summary>
		/// <param name="buffer">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="offset">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="size">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="commit">
		/// A <see cref="T:bool"/>.
		/// </param>
		[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
		public static void NamedBufferPageCommitmentEXT(UInt32 buffer, IntPtr offset, UInt32 size, bool commit)
		{
			Debug.Assert(Delegates.pglNamedBufferPageCommitmentEXT != null, "pglNamedBufferPageCommitmentEXT not implemented");
			Delegates.pglNamedBufferPageCommitmentEXT(buffer, offset, size, commit);
			LogCommand("glNamedBufferPageCommitmentEXT", null, buffer, offset, size, commit			);
			DebugCheckErrors(null);
		}

		/// <summary>
		/// [GL] glNamedBufferPageCommitmentARB: Binding for glNamedBufferPageCommitmentARB.
		/// </summary>
		/// <param name="buffer">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="offset">
		/// A <see cref="T:IntPtr"/>.
		/// </param>
		/// <param name="size">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="commit">
		/// A <see cref="T:bool"/>.
		/// </param>
		[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
		public static void NamedBufferPageCommitmentARB(UInt32 buffer, IntPtr offset, UInt32 size, bool commit)
		{
			Debug.Assert(Delegates.pglNamedBufferPageCommitmentARB != null, "pglNamedBufferPageCommitmentARB not implemented");
			Delegates.pglNamedBufferPageCommitmentARB(buffer, offset, size, commit);
			LogCommand("glNamedBufferPageCommitmentARB", null, buffer, offset, size, commit			);
			DebugCheckErrors(null);
		}

		internal unsafe static partial class Delegates
		{
			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glBufferPageCommitmentARB(Int32 target, IntPtr offset, UInt32 size, [MarshalAs(UnmanagedType.I1)] bool commit);

			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[ThreadStatic]
			internal static glBufferPageCommitmentARB pglBufferPageCommitmentARB;

			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glNamedBufferPageCommitmentEXT(UInt32 buffer, IntPtr offset, UInt32 size, [MarshalAs(UnmanagedType.I1)] bool commit);

			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[ThreadStatic]
			internal static glNamedBufferPageCommitmentEXT pglNamedBufferPageCommitmentEXT;

			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[SuppressUnmanagedCodeSecurity()]
			internal unsafe delegate void glNamedBufferPageCommitmentARB(UInt32 buffer, IntPtr offset, UInt32 size, [MarshalAs(UnmanagedType.I1)] bool commit);

			[RequiredByFeature("GL_ARB_sparse_buffer", Api = "gl|glcore")]
			[ThreadStatic]
			internal static glNamedBufferPageCommitmentARB pglNamedBufferPageCommitmentARB;

		}
	}

}
