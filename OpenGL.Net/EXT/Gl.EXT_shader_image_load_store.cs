
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
		/// [GL] glBindImageTextureEXT: Binding for glBindImageTextureEXT.
		/// </summary>
		/// <param name="index">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="texture">
		/// A <see cref="T:UInt32"/>.
		/// </param>
		/// <param name="level">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="layered">
		/// A <see cref="T:bool"/>.
		/// </param>
		/// <param name="layer">
		/// A <see cref="T:Int32"/>.
		/// </param>
		/// <param name="access">
		/// A <see cref="T:BufferAccess"/>.
		/// </param>
		/// <param name="format">
		/// A <see cref="T:InternalFormat"/>.
		/// </param>
		[RequiredByFeature("GL_EXT_shader_image_load_store")]
		public static void BindImageTextureEXT(UInt32 index, UInt32 texture, Int32 level, bool layered, Int32 layer, BufferAccess access, InternalFormat format)
		{
			Debug.Assert(Delegates.pglBindImageTextureEXT != null, "pglBindImageTextureEXT not implemented");
			Delegates.pglBindImageTextureEXT(index, texture, level, layered, layer, (Int32)access, (Int32)format);
			LogCommand("glBindImageTextureEXT", null, index, texture, level, layered, layer, access, format			);
			DebugCheckErrors(null);
		}

		internal unsafe static partial class Delegates
		{
			[RequiredByFeature("GL_EXT_shader_image_load_store")]
			[SuppressUnmanagedCodeSecurity()]
			internal delegate void glBindImageTextureEXT(UInt32 index, UInt32 texture, Int32 level, [MarshalAs(UnmanagedType.I1)] bool layered, Int32 layer, Int32 access, Int32 format);

			[RequiredByFeature("GL_EXT_shader_image_load_store")]
			[ThreadStatic]
			internal static glBindImageTextureEXT pglBindImageTextureEXT;

		}
	}

}
