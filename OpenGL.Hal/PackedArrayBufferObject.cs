﻿
// Copyright (C) 2015 Luca Piccioni
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301
// USA

using System;
using System.Reflection;
using System.Runtime.InteropServices;

namespace OpenGL
{
	/// <summary>
	/// Array buffer object aggregating multiple arrays.
	/// </summary>
	public class PackedArrayBufferObject : AggregatedArrayBufferObject
	{
		#region Constructors

		/// <summary>
		/// Construct an InterleavedArrayBufferObject specifying its item layout on CPU side.
		/// </summary>
		/// <param name="arrayItemType">
		/// A <see cref="Type"/> describing the type of the array item.
		/// </param>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public PackedArrayBufferObject(Type arrayItemType, BufferObjectHint hint) :
			base(hint)
		{
			if (arrayItemType == null)
				throw new ArgumentNullException("arrayItemType");
			if (arrayItemType.IsValueType == false)
				throw new ArgumentException("not a value type", "arrayItemType");

			// Get fields for defining array item definition
			FieldInfo[] arrayItemTypeFields = arrayItemType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
			if (arrayItemTypeFields.Length == 0)
				throw new ArgumentException("no public fields", "arrayItemType");

			// Determine array sections stride
			int structStride = Marshal.SizeOf(arrayItemType);

			foreach (FieldInfo arrayItemTypeField in arrayItemTypeFields) {
				ArraySection arraySection = new ArraySection(arrayItemTypeField.FieldType);

				// Determine array section offset
				arraySection.ItemOffset = Marshal.OffsetOf(arrayItemType, arrayItemTypeField.Name);
				// Determine array section stride
				arraySection.ItemStride = new IntPtr(structStride);
				// Mission Normalized property management: add attributes?
				arraySection.Normalized = false;

				ArraySections.Add(arraySection);
			}
		}

		#endregion
	}

	/// <summary>
	/// Array buffer object aggregating multiple arrays.
	/// </summary>
	public class InterleavedArrayBufferObject<T> : InterleavedArrayBufferObject
	{
		#region Constructors

		/// <summary>
		/// Construct an InterleavedArrayBufferObject.
		/// </summary>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public InterleavedArrayBufferObject(BufferObjectHint hint) :
			base(typeof(T), hint)
		{

		}

		#endregion
	}
}
