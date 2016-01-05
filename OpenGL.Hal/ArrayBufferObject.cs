
// Copyright (C) 2009-2015 Luca Piccioni
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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace OpenGL
{
	/// <summary>
	/// Single array buffer object.
	/// </summary>
	/// <remarks>
	/// <para>
	/// This class is a <see cref="BufferObject"/> specialized for storing data to be issued to a shader program execution.
	/// </para>
	/// <para>
	/// The following information defines the array items layout:
	/// - <see cref="ArrayType"/>: the property describe entirely the single item data layout.
	/// - <see cref="ArrayBaseType"/>: the base type of the data component.
	/// - <see cref="ItemCount"/>: the number of items stored in this ArrayBufferObject.
	/// - <see cref="ItemSize"/>: the size of each item in array, in basic machine units.
	/// - <see cref="Interleaved"/>: a boolean flag indicating whether different data types are interleaved regularly in the array.
	/// </para>
	/// </remarks>
	public class ArrayBufferObject : ArrayBufferObjectBase, ArrayBufferObjectBase.IArraySection
	{
		#region Constructors

		/// <summary>
		/// Construct an ArrayBufferObject specifying its item layout on CPU side.
		/// </summary>
		/// <param name="vertexBaseType">
		/// A <see cref="VertexBaseType"/> describing the item components base type on CPU side.
		/// </param>
		/// <param name="vertexLength">
		/// A <see cref="UInt32"/> that specify how many components have the array item.
		/// </param>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public ArrayBufferObject(VertexBaseType vertexBaseType, uint vertexLength, BufferObjectHint hint) :
			this(vertexBaseType, vertexLength, 1, hint)
		{

		}

		/// <summary>
		/// Construct an ArrayBufferObject specifying its item layout on CPU side.
		/// </summary>
		/// <param name="vertexBaseType">
		/// A <see cref="VertexBaseType"/> describing the item components base type on CPU side.
		/// </param>
		/// <param name="vertexLength">
		/// A <see cref="UInt32"/> that specify how many components have the array item.
		/// </param>
		/// <param name="vertexRank">
		/// A <see cref="UInt32"/> that specify how many columns have the array item of matrix type.
		/// </param>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public ArrayBufferObject(VertexBaseType vertexBaseType, uint vertexLength, uint vertexRank, BufferObjectHint hint) :
			this(ArrayBufferItem.GetArrayType(vertexBaseType, vertexLength, vertexRank), hint)
		{
			
		}

		/// <summary>
		/// Construct an ArrayBufferObject specifying its item layout on GPU side.
		/// </summary>
		/// <param name="format">
		/// A <see cref="ArrayBufferItemType"/> describing the item base type on GPU side.
		/// </param>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public ArrayBufferObject(ArrayBufferItemType format, BufferObjectHint hint) :
			base(hint)
		{
			try {
				// Store array type
				_ArrayType = format;
				// Determine array item size
				ItemSize = ArrayBufferItem.GetArrayItemSize(format);
			} catch {
				// Avoid finalizer assertion failure (don't call dispose since it's virtual)
				GC.SuppressFinalize(this);
				throw;
			}
		}

		#endregion

		#region Array Buffer Information

		/// <summary>
		/// The array buffer object element type, on CPU side.
		/// </summary>
		public ArrayBufferItemType ArrayType { get { return (_ArrayType); } }

		/// <summary>
		/// The array buffer object element type.
		/// </summary>
		private readonly ArrayBufferItemType _ArrayType;

		#endregion

		#region Create

		#region Create(uint itemsCount)

		/// <summary>
		/// Create this ArrayBufferObject by specifing only the number of items.
		/// </summary>
		/// <param name="itemsCount">
		/// A <see cref="UInt32"/> that specify the number of elements hold by this ArrayBufferObject.
		/// </param>
		/// <remarks>
		/// <para>
		/// Previous content of the client buffer is discarded.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="itemsCount"/> is zero.
		/// </exception>
		public void Create(uint itemsCount)
		{
			if (itemsCount == 0)
				throw new ArgumentException("invalid", "itemsCount");

			// Allocate buffer
			Allocate(itemsCount * ItemSize);
		}

		#endregion

		#region Create(GraphicsContext ctx, uint itemsCount)

		/// <summary>
		/// Create this ArrayBufferObject by specifing only the number of items.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/> used to define this ArrayBufferObject.
		/// </param>
		/// <param name="itemsCount">
		/// A <see cref="UInt32"/> that specify the number of elements hold by this ArrayBufferObject.
		/// </param>
		/// <remarks>
		/// <para>
		/// Previous content of the client buffer is discarded, if any was defined.
		/// </para>
		/// </remarks>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="itemsCount"/> is zero.
		/// </exception>
		public void Create(GraphicsContext ctx, uint itemsCount)
		{
			if (ctx == null)
				throw new ArgumentNullException("ctx");
			if (itemsCount == 0)
				throw new ArgumentException("invalid", "itemsCount");

			// Object already existing: resize client buffer, if any
			if (ClientBufferAddress != IntPtr.Zero)
				Allocate(itemsCount * ItemSize);
			// If not exists, set GPU buffer size; otherwise keep in synch with client buffer size
			ClientBufferSize = itemsCount * ItemSize;
			// Allocate object
			Create(ctx);
		}

		#endregion

		#region Create(Array array, uint offset, uint count)

		/// <summary>
		/// Copy data from any source supported.
		/// </summary>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		public void Create(Array array)
		{
			Create(array, 0, (uint)array.Length);
		}

		/// <summary>
		/// Copy data from any source supported.
		/// </summary>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="count"/> let exceed the array boundaries.
		/// </exception>
		public void Create(Array array, uint count)
		{
			Create(array, 0, count);
		}

		/// <summary>
		/// Copy data from any source supported.
		/// </summary>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="offset"/> or <paramref name="count"/> let exceed the array boundaries.
		/// </exception>
		public void Create(Array array, uint offset, uint count)
		{
			if (array == null)
				throw new ArgumentNullException("array");
			if (array.Rank != 1)
				throw new ArgumentException(String.Format("copying from array of rank {0} not supported", array.Rank));
			if (count == 0)
				return;

			Type arrayElementType = array.GetType().GetElementType();
			if (arrayElementType == null || !arrayElementType.IsValueType)
				throw new ArgumentException("invalid array element type", "array");

			// The base type should be corresponding
			ArrayBufferItemType arrayElementVertexType = ArrayBufferItem.GetArrayType(arrayElementType);
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(arrayElementVertexType))
				throw new ArgumentException(String.Format("source base type of {0} incompatible with destination base type of {1}", arrayElementType.Name, ArrayBufferItem.GetArrayBaseType(_ArrayType)), "array");

			// Array element item size cannot exceed ItemSize
			uint arrayItemSize = ArrayBufferItem.GetArrayItemSize(arrayElementVertexType);
			if (arrayItemSize > ItemSize)
				throw new ArgumentException("array element type too big", "array");

			// Ensure that ClientBufferSize returns the correct client buffer size
			ClientBufferSize = 0;
			// Memory buffer shall be able to contains all data
			if (ClientBufferSize < ItemSize * count)
				Allocate(ItemSize * count);

			// Copy on buffer
			CopyArray(ClientBufferAddress, array, arrayItemSize, offset, count);
		}

		#endregion

		#region Create(GraphicsContext ctx, Array array, uint offset, uint count)

		/// <summary>
		/// Copy data from any source supported, uploading data.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> used for copying data.
		/// </param>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="ctx"/> or <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="ctx"/> is not current on the calling thread.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		public void Create(GraphicsContext ctx, Array array)
		{
			Create(ctx, array, 0, (uint)array.Length);
		}

		/// <summary>
		/// Copy data from any source supported, uploading data.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> used for copying data.
		/// </param>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="ctx"/> or <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="ctx"/> is not current on the calling thread.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="count"/> let exceed the array boundaries.
		/// </exception>
		public void Create(GraphicsContext ctx, Array array, uint count)
		{
			Create(ctx, array, 0, count);
		}

		/// <summary>
		/// Copy data from any source supported, uploading data.
		/// </summary>
		/// <param name="ctx">
		/// The <see cref="GraphicsContext"/> used for copying data.
		/// </param>
		/// <param name="array">
		/// The source array where the data comes from.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first element to be copied.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify the number of items to copy. The items to copy are referred in terms
		/// of the data layout of this <see cref="ArrayBufferObject"/>, not of the element type of <paramref name="array"/>!
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="ctx"/> or <paramref name="array"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="ctx"/> is not current on the calling thread.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the rank of <paramref name="array"/> is different from 1.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if it is not possible to determine the element type of the array <paramref name="array"/>, or if the base type of
		/// the array elements is not compatible with the base type of this <see cref="ArrayBufferObject"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the size of the elements of <paramref name="array"/> is larger than <see cref="ItemSize"/>.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="offset"/> or <paramref name="count"/> let exceed the array boundaries.
		/// </exception>
		public void Create(GraphicsContext ctx, Array array, uint offset, uint count)
		{
			if (ctx == null)
				throw new ArgumentNullException("ctx");
			if (ctx.IsCurrent == false)
				throw new ArgumentException("not current", "ctx");
			if (array == null)
				throw new ArgumentNullException("array");
			if (array.Rank != 1)
				throw new ArgumentException(String.Format("copying from array of rank {0} not supported", array.Rank));
			if (count == 0)
				return;

			Type arrayElementType = array.GetType().GetElementType();
			if (arrayElementType == null || !arrayElementType.IsValueType)
				throw new ArgumentException("invalid array element type", "array");

			// The base type should be corresponding
			ArrayBufferItemType arrayElementVertexType = ArrayBufferItem.GetArrayType(arrayElementType);
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(arrayElementVertexType))
				throw new ArgumentException(String.Format("source base type of {0} incompatible with destination base type of {1}", arrayElementType.Name, ArrayBufferItem.GetArrayBaseType(_ArrayType)), "array");

			// Array element item size cannot exceed ItemSize
			uint arrayItemSize = ArrayBufferItem.GetArrayItemSize(arrayElementVertexType);
			if (arrayItemSize > ItemSize)
				throw new ArgumentException("array element type too big", "array");

			// Ensure enought buffer
			Create(ctx, count);
			// Copy data mapping GPU buffer
			Map(ctx, BufferAccessARB.WriteOnly);
			try {
				// Copy on buffer
				CopyArray(MappedBuffer, array, arrayItemSize, offset, count);
			} finally {
				Unmap(ctx);
			}
		}

		#endregion

		/// <summary>
		/// Copy to pinned memory the items defined by an array, item by item, respecting item strides.
		/// </summary>
		/// <param name="dst">
		/// The <see cref="IntPtr"/> that specify the destination memory where copy to.
		/// </param>
		/// <param name="src">
		/// The <see cref="Array"/> that specify the source data to be copied.
		/// </param>
		/// <param name="srcItemSize">
		/// A <see cref="UInt32"/> that specify the size of the elements of <paramref name="src"/>.
		/// </param>
		/// <param name="srcOffset">
		/// A <see cref="UInt32"/> that specify the array offset (in elements) where the copy starts.
		/// </param>
		/// <param name="srcCount">
		/// A <see cref="UInt32"/> that specify the number of elements of <paramref name="src"/> must be copied.
		/// </param>
		private void CopyArray(IntPtr dst, Array src, uint srcItemSize, uint srcOffset, uint srcCount)
		{
			if (dst == IntPtr.Zero)
				throw new ArgumentException("invalid pointer", "dst");
			if (src == null)
				throw new ArgumentNullException("src");
			if (srcItemSize > ItemSize)
				throw new ArgumentException("too large", "srcItemSize");
			if (src.Length < srcCount)
				throw new ArgumentException("exceed array length", "srcCount");
			if (src.Length < srcOffset + srcCount)
				throw new ArgumentException("exceed array length", "srcOffset");

			GCHandle arrayHandle = GCHandle.Alloc(src, GCHandleType.Pinned);
			try {
				unsafe
				{
					byte* arrayPtr = (byte*)arrayHandle.AddrOfPinnedObject().ToPointer();
					byte* dstPtr = (byte*)dst.ToPointer();

					// Take into account source offset
					arrayPtr += srcItemSize * srcOffset;

					if (srcItemSize != ItemSize) {
						// Respect this ArrayBufferObject item stride
						for (uint i = 0; i < srcCount; i++, arrayPtr += srcItemSize, dstPtr += ItemSize)
							Memory.MemoryCopy(dstPtr, arrayPtr, srcItemSize);
					} else {
						Memory.MemoryCopy(dstPtr, arrayPtr, srcItemSize * srcCount);
					}
				}
			} finally {
				arrayHandle.Free();
			}
		}

		/// <summary>
		/// Copy from pinned memory the items defined by an array, item by item, respecting item strides.
		/// </summary>
		/// <param name="dst">
		/// The <see cref="Array"/> that specify the destination memory where copy to.
		/// </param>
		/// <param name="src">
		/// The <see cref="IntPtr"/> that specify the source data to be copied.
		/// </param>
		/// <param name="dstItemSize">
		/// A <see cref="UInt32"/> that specify the size of the elements of <paramref name="dst"/>.
		/// </param>
		/// <param name="dstOffset">
		/// A <see cref="UInt32"/> that specify the array offset (in elements) where the copy starts.
		/// </param>
		/// <param name="dstCount">
		/// A <see cref="UInt32"/> that specify the number of elements of <paramref name="dst"/> must be copied.
		/// </param>
		private void CopyArray(Array dst, IntPtr src, uint dstItemSize, uint dstOffset, uint dstCount)
		{
			if (dst == null)
				throw new ArgumentNullException("dst");
			if (src == IntPtr.Zero)
				throw new ArgumentException("invalid pointer", "src");
			if (dstItemSize > ItemSize)
				throw new ArgumentException("too large", "dstItemSize");
			if (dst.Length < dstCount)
				throw new ArgumentException("exceed array length", "dstCount");
			if (dst.Length < dstOffset + dstCount)
				throw new ArgumentException("exceed array length", "dstOffset");

			GCHandle arrayHandle = GCHandle.Alloc(dst, GCHandleType.Pinned);
			try {
				unsafe
				{
					byte* arrayPtr = (byte*)arrayHandle.AddrOfPinnedObject().ToPointer();
					byte* srcPtr = (byte*)src.ToPointer();

					// Take into account destination offset
					arrayPtr += dstItemSize * dstOffset;

					if (dstItemSize != ItemSize) {
						// Respect this ArrayBufferObject item stride
						for (uint i = 0; i < dstCount; i++, arrayPtr += dstItemSize, srcPtr += ItemSize)
							Memory.MemoryCopy(arrayPtr, srcPtr, dstItemSize);
					} else {
						Memory.MemoryCopy(arrayPtr, srcPtr, dstItemSize * dstCount);
					}
				}
			} finally {
				arrayHandle.Free();
			}
		}

		#endregion

		#region Client Buffer Access

		/// <summary>
		/// Gets data from this ArrayBufferObject.
		/// </summary>
		/// <typeparam name='T'>
		/// The type parameter that determine the type of the data to read. This type could be different from the real
		/// ArrayBufferObject items.
		/// </typeparam>
		/// <param name='index'>
		/// The zero-based index of the element to read. The basic machine unit offset of the element to read is
		/// determine by the size of the type <typeparamref name="T"/>.
		/// </param>
		/// <returns>
		/// An element of this ArrayBufferObject, of type <typeparamref name="T"/>, at index <paramref name="index"/>.
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this BufferObject has no client memory allocated.
		/// </exception>
		/// <remarks>
		/// This method differs from <see cref="BufferObject.MapGet{T}(GraphicsContext, long)"/> since it doesn't
		/// require a GraphicsContext for buffer access.
		/// </remarks>
		public T GetClientData<T>(uint index) where T : struct
		{
			IntPtr clientBufferAddress = ClientBufferAddress;

			if (clientBufferAddress == IntPtr.Zero)
				throw new InvalidOperationException("no client buffer");

			IntPtr bufferPtr = new IntPtr(clientBufferAddress.ToInt64() + index * Marshal.SizeOf(typeof(T)));

			return ((T)Marshal.PtrToStructure(bufferPtr, typeof(T)));
		}

		/// <summary>
		/// Sets data to this ArrayBufferObject.
		/// </summary>
		/// <typeparam name="T">
		/// The type parameter that determine the type of the data to read. This type could be different from the real
		/// ArrayBufferObject elements.
		/// </typeparam>
		/// <param name="value">
		/// A <typeparamref name="T"/> that is the value to store in this ArrayBufferObject.
		/// </param>
		/// <param name='index'>
		/// The zero-based index of the element to read. The basic machine unit offset of the element to read is
		/// determine by the size of the type <typeparamref name="T"/>.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this BufferObject has no client memory allocated.
		/// </exception>
		/// <remarks>
		/// This method differs from <see cref="BufferObject.MapSet{T}(GraphicsContext, long)"/> since it doesn't
		/// require a GraphicsContext for buffer access.
		/// </remarks>
		public void SetClientData<T>(T value, uint index) where T : struct
		{
			IntPtr clientBufferAddress = ClientBufferAddress;

			if (clientBufferAddress == IntPtr.Zero)
				throw new InvalidOperationException("no client buffer");

			IntPtr bufferPtr = new IntPtr(clientBufferAddress.ToInt64() + index * Marshal.SizeOf(typeof(T)));

			Marshal.StructureToPtr(value, bufferPtr, false);
		}

		#endregion

		#region To Array

		#region ToArray()

		/// <summary>
		/// Convert this array buffer object in a strongly-typed array.
		/// </summary>
		/// <returns>
		/// It returns an array having all items stored by this ArrayBufferObject.
		/// </returns>
		public Array ToArray()
		{
			if (ClientBufferAddress == IntPtr.Zero)
				throw new InvalidOperationException("no client buffer");

			Array genericArray;

			switch (ArrayType) {
				case ArrayBufferItemType.Byte:
					genericArray = new SByte[ItemCount];
					break;
				case ArrayBufferItemType.Byte2:
					genericArray = new Vertex2b[ItemCount];
					break;
				case ArrayBufferItemType.Byte3:
					genericArray = new Vertex3b[ItemCount];
					break;
				case ArrayBufferItemType.Byte4:
					genericArray = new Vertex4b[ItemCount];
					break;
				case ArrayBufferItemType.UByte:
					genericArray = new Byte[ItemCount];
					break;
				case ArrayBufferItemType.UByte2:
					genericArray = new Vertex2ub[ItemCount];
					break;
				case ArrayBufferItemType.UByte3:
					genericArray = new Vertex3ub[ItemCount];
					break;
				case ArrayBufferItemType.UByte4:
					genericArray = new Vertex4ub[ItemCount];
					break;
				case ArrayBufferItemType.Short:
					genericArray = new Int16[ItemCount];
					break;
				case ArrayBufferItemType.Short2:
					genericArray = new Vertex2s[ItemCount];
					break;
				case ArrayBufferItemType.Short3:
					genericArray = new Vertex3s[ItemCount];
					break;
				case ArrayBufferItemType.Short4:
					genericArray = new Vertex4s[ItemCount];
					break;
				case ArrayBufferItemType.UShort:
					genericArray = new UInt16[ItemCount];
					break;
				case ArrayBufferItemType.UShort2:
					genericArray = new Vertex2us[ItemCount];
					break;
				case ArrayBufferItemType.UShort3:
					genericArray = new Vertex3us[ItemCount];
					break;
				case ArrayBufferItemType.UShort4:
					genericArray = new Vertex4us[ItemCount];
					break;
				case ArrayBufferItemType.Int:
					genericArray = new Int32[ItemCount];
					break;
				case ArrayBufferItemType.Int2:
					genericArray = new Vertex2i[ItemCount];
					break;
				case ArrayBufferItemType.Int3:
					genericArray = new Vertex3i[ItemCount];
					break;
				case ArrayBufferItemType.Int4:
					genericArray = new Vertex4i[ItemCount];
					break;
				case ArrayBufferItemType.UInt:
					genericArray = new UInt32[ItemCount];
					break;
				case ArrayBufferItemType.UInt2:
					genericArray = new Vertex2ui[ItemCount];
					break;
				case ArrayBufferItemType.UInt3:
					genericArray = new Vertex3ui[ItemCount];
					break;
				case ArrayBufferItemType.UInt4:
					genericArray = new Vertex4ui[ItemCount];
					break;
				case ArrayBufferItemType.Float:
					genericArray = new Single[ItemCount];
					break;
				case ArrayBufferItemType.Float2:
					genericArray = new Vertex2f[ItemCount];
					break;
				case ArrayBufferItemType.Float3:
					genericArray = new Vertex3f[ItemCount];
					break;
				case ArrayBufferItemType.Float4:
					genericArray = new Vertex4f[ItemCount];
					break;
				case ArrayBufferItemType.Double:
					genericArray = new Double[ItemCount];
					break;
				case ArrayBufferItemType.Double2:
					genericArray = new Vertex2d[ItemCount];
					break;
				case ArrayBufferItemType.Double3:
					genericArray = new Vertex3d[ItemCount];
					break;
				case ArrayBufferItemType.Double4:
					genericArray = new Vertex4d[ItemCount];
					break;
				case ArrayBufferItemType.Half:
					genericArray = new HalfFloat[ItemCount];
					break;
				case ArrayBufferItemType.Half2:
					genericArray = new Vertex2hf[ItemCount];
					break;
				case ArrayBufferItemType.Half3:
					genericArray = new Vertex3hf[ItemCount];
					break;
				case ArrayBufferItemType.Half4:
					genericArray = new Vertex4hf[ItemCount];
					break;
				default:
					throw new NotImplementedException(String.Format("array type {0} not yet implemented", ArrayType));
			}

			// Copy from buffer data to array data
			Memory.MemoryCopy(genericArray, ClientBufferAddress, ItemCount * ItemSize);

			return (genericArray);
		}

		#endregion

		#region ToArray<T>()

		/// <summary>
		/// Convert this array buffer object in a strongly-typed array.
		/// </summary>
		/// <typeparam name="T">
		/// 
		/// </typeparam>
		/// <returns>
		/// It returns an array having all items stored by this ArrayBufferObject.
		/// </returns>
		public T[] ToArray<T>() where T : struct { return (ToArray<T>(ItemCount)); }

		/// <summary>
		/// Convert this array buffer object in a strongly-typed array.
		/// </summary>
		/// <typeparam name="T">
		/// An arbitrary structure defining the returned array item. It doesn't need to be correlated with the ArrayBufferObject
		/// layout.
		/// </typeparam>
		/// <param name="arrayLength">
		/// A <see cref="UInt32"/> that specify the number of elements of the returned array.
		/// </param>
		/// <returns>
		/// It returns an array having all items stored by this ArrayBufferObject.
		/// </returns>
		public T[] ToArray<T>(uint arrayLength) where T : struct
		{
			if (arrayLength > ItemCount)
				throw new ArgumentOutOfRangeException("arrayLength", arrayLength, "cannot exceed items count");
			if (ClientBufferAddress == IntPtr.Zero)
				throw new InvalidOperationException("no client buffer");

			Type arrayElementType = typeof(T);
			if (arrayElementType == null || !arrayElementType.IsValueType)
				throw new InvalidOperationException("invalid array element type");

			// The base type should be corresponding
			ArrayBufferItemType arrayElementVertexType = ArrayBufferItem.GetArrayType(arrayElementType);
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(arrayElementVertexType))
				throw new InvalidOperationException(String.Format("source base type of {0} incompatible with destination base type of {1}", arrayElementType.Name, ArrayBufferItem.GetArrayBaseType(_ArrayType)));

			// Array element item size cannot exceed ItemSize
			uint arrayItemSize = ArrayBufferItem.GetArrayItemSize(arrayElementVertexType);
			if (arrayItemSize > ItemSize)
				throw new ArgumentException("array element type too big", "array");

			T[] array = new T[arrayLength];

			// Copy from buffer data to array data
			CopyArray(array, ClientBufferAddress, arrayItemSize, 0, arrayLength);

			return (array);
		}

		#endregion

		#endregion

		#region Strongly Typed Array Factory

		/// <summary>
		/// Create an array buffer object, using the generic class <see cref="ArrayBufferObject{T}"/>, depending on a <see cref="ArrayBufferItemType"/>.
		/// </summary>
		/// <param name="vertexArrayType">
		/// A <see cref="ArrayBufferItemType"/> that determine the generic argument of the created array buffer object.
		/// </param>
		/// <param name="hint">
		/// A <see cref="BufferObjectHint"/> required for creating a <see cref="ArrayBufferObject"/>.
		/// </param>
		/// <returns>
		/// 
		/// </returns>
		public static ArrayBufferObject CreateArrayObject(ArrayBufferItemType vertexArrayType, BufferObjectHint hint)
		{
			switch (vertexArrayType) {
				case ArrayBufferItemType.Byte:
					return (new ArrayBufferObject<sbyte>(hint));
				case ArrayBufferItemType.Byte2:
					return (new ArrayBufferObject<Vertex2b>(hint));
				case ArrayBufferItemType.Byte3:
					return (new ArrayBufferObject<Vertex3b>(hint));
				case ArrayBufferItemType.Byte4:
					return (new ArrayBufferObject<Vertex4b>(hint));
				case ArrayBufferItemType.UByte:
					return (new ArrayBufferObject<byte>(hint));
				case ArrayBufferItemType.UByte2:
					return (new ArrayBufferObject<Vertex2ub>(hint));
				case ArrayBufferItemType.UByte3:
					return (new ArrayBufferObject<Vertex3ub>(hint));
				case ArrayBufferItemType.UByte4:
					return (new ArrayBufferObject<Vertex4ub>(hint));
				case ArrayBufferItemType.Short:
					return (new ArrayBufferObject<short>(hint));
				case ArrayBufferItemType.Short2:
					return (new ArrayBufferObject<Vertex2s>(hint));
				case ArrayBufferItemType.Short3:
					return (new ArrayBufferObject<Vertex3s>(hint));
				case ArrayBufferItemType.Short4:
					return (new ArrayBufferObject<Vertex4s>(hint));
				case ArrayBufferItemType.UShort:
					return (new ArrayBufferObject<ushort>(hint));
				case ArrayBufferItemType.UShort2:
					return (new ArrayBufferObject<Vertex2us>(hint));
				case ArrayBufferItemType.UShort3:
					return (new ArrayBufferObject<Vertex3us>(hint));
				case ArrayBufferItemType.UShort4:
					return (new ArrayBufferObject<Vertex4us>(hint));
				case ArrayBufferItemType.Int:
					return (new ArrayBufferObject<Int32>(hint));
				case ArrayBufferItemType.Int2:
					return (new ArrayBufferObject<Vertex2i>(hint));
				case ArrayBufferItemType.Int3:
					return (new ArrayBufferObject<Vertex3i>(hint));
				case ArrayBufferItemType.Int4:
					return (new ArrayBufferObject<Vertex4i>(hint));
				case ArrayBufferItemType.UInt:
					return (new ArrayBufferObject<UInt32>(hint));
				case ArrayBufferItemType.UInt2:
					return (new ArrayBufferObject<Vertex2ui>(hint));
				case ArrayBufferItemType.UInt3:
					return (new ArrayBufferObject<Vertex3ui>(hint));
				case ArrayBufferItemType.UInt4:
					return (new ArrayBufferObject<Vertex4ui>(hint));
				case ArrayBufferItemType.Float:
					return (new ArrayBufferObject<Single>(hint));
				case ArrayBufferItemType.Float2:
					return (new ArrayBufferObject<Vertex2f>(hint));
				case ArrayBufferItemType.Float3:
					return (new ArrayBufferObject<Vertex3f>(hint));
				case ArrayBufferItemType.Float4:
					return (new ArrayBufferObject<Vertex4f>(hint));
				case ArrayBufferItemType.Float2x4:
					return (new ArrayBufferObject<Matrix2x4>(hint));
				case ArrayBufferItemType.Float4x4:
					return (new ArrayBufferObject<Matrix4>(hint));
				case ArrayBufferItemType.Double:
					return (new ArrayBufferObject<Double>(hint));
				case ArrayBufferItemType.Double2:
					return (new ArrayBufferObject<Vertex2d>(hint));
				case ArrayBufferItemType.Double3:
					return (new ArrayBufferObject<Vertex3d>(hint));
				case ArrayBufferItemType.Double4:
					return (new ArrayBufferObject<Vertex4d>(hint));
				case ArrayBufferItemType.Half:
					return (new ArrayBufferObject<HalfFloat>(hint));
				case ArrayBufferItemType.Half2:
					return (new ArrayBufferObject<Vertex2hf>(hint));
				case ArrayBufferItemType.Half3:
					return (new ArrayBufferObject<Vertex3hf>(hint));
				case ArrayBufferItemType.Half4:
					return (new ArrayBufferObject<Vertex4hf>(hint));
				default:
					throw new ArgumentException(String.Format("vertex array type {0} not supported", vertexArrayType));
			}
		}

		#endregion

		#region Copy with Indices

		/// <summary>
		/// Copy from an ArrayBufferObject with an indirection defined by an index.
		/// </summary>
		/// <param name="buffer">
		/// An <see cref="ArrayBufferObject"/> that specify the source data buffer to copy.
		/// </param>
		/// <param name="indices">
		/// An array of indices indicating the order of the vertices copied from <paramref name="buffer"/>.
		/// </param>
		/// <param name="count">
		/// A <see cref="UInt32"/> that specify how many elements to copy from <paramref name="buffer"/>.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first index considered from <paramref name="indices"/>. A
		/// value of 0 indicates that the indices are considered from the first one.
		/// </param>
		/// <param name="stride">
		/// A <see cref="UInt32"/> that specify the offset between two indexes considered for the copy operations
		/// from <paramref name="indices"/>. A value of 1 indicates that all considered indices are contiguos.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="buffer"/> or <paramref name="indices"/> are null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="count"/> or <paramref name="stride"/> equals to 0.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if the combination of <paramref name="count"/>, <paramref name="offset"/> and
		/// <paramref name="stride"/> will cause a <paramref name="indices"/> array access out of its bounds.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this <see cref="ArrayBufferObject"/> have a complex data layout, of it has a vertex
		/// base type different from <paramref name="buffer"/>.
		/// </exception>
		/// <remarks>
		/// <para>
		/// After a successfull copy operation, the previous buffer is discarded replaced by the copied buffer from
		/// <paramref name="buffer"/>.
		/// </para>
		/// </remarks>
		public void Copy(ArrayBufferObject buffer, uint[] indices, uint count, uint offset, uint stride)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			if (indices == null)
				throw new ArgumentNullException("indices");
			if (count == 0)
				throw new ArgumentException("invalid", "count");
			if (stride == 0)
				throw new ArgumentException("invalid", "stride");
			if (offset + ((count - 1) * stride) > indices.Length)
				throw new InvalidOperationException("indices out of bounds");
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(buffer._ArrayType))
				throw new InvalidOperationException("base type mismatch");

			Create(count);

			unsafe {
				byte* dstPtr = (byte*)ClientBufferAddress.ToPointer();

				for (uint i = 0; i < count; i++, dstPtr += ItemSize) {
					uint arrayIndex = indices[(i * stride) + offset];

					// Position 'srcPtr' to the indexed element
					byte* srcPtr = ((byte*)buffer.ClientBufferAddress.ToPointer()) + (ItemSize * arrayIndex);

					// Copy the 'arrayIndex'th element
					Memory.MemoryCopy(dstPtr, srcPtr, ItemSize);
				}
			}
		}

		/// <summary>
		/// Copy from an ArrayBufferObject with an indirection defined by an index (polygon tessellation).
		/// </summary>
		/// <param name="buffer">
		/// An <see cref="ArrayBufferObject"/> that specify the source data buffer to copy.
		/// </param>
		/// <param name="vcount">
		/// An array of integers indicating the number of the vertices of the polygon copied from <paramref name="buffer"/>. This parameter
		/// indicated how many polygons to copy (the array length). Each item specify the number of vertices composing the polygon.
		/// </param>
		/// <param name="indices">
		/// An array of indices indicating the order of the vertices copied from <paramref name="buffer"/>.
		/// </param>
		/// <param name="offset">
		/// A <see cref="UInt32"/> that specify the first index considered from <paramref name="indices"/>. A
		/// value of 0 indicates that the indices are considered from the first one.
		/// </param>
		/// <param name="stride">
		/// A <see cref="UInt32"/> that specify the offset between two indexes considered for the copy operations
		/// from <paramref name="indices"/>. A value of 1 indicates that all considered indices are contiguos.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="buffer"/>, <paramref name="indices"/> or <paramref name="vcount"/> are null.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this <see cref="ArrayBufferObject"/> have a complex data layout, of it has a vertex
		/// base type different from <paramref name="buffer"/>.
		/// </exception>
		/// <remarks>
		/// <para>
		/// After a successfull copy operation, the previous buffer is discarded replaced by the copied buffer from
		/// <paramref name="buffer"/>.
		/// </para>
		/// </remarks>
		public void Copy(ArrayBufferObject buffer, uint[] indices, uint[] vcount, uint offset, uint stride)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			if (indices == null)
				throw new ArgumentNullException("indices");
			if (vcount == null)
				throw new ArgumentNullException("indices");
			if (stride == 0)
				throw new ArgumentException("invalid", "stride");
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(buffer._ArrayType))
				throw new InvalidOperationException("base type mismatch");

			// Allocate array buffer
			uint minVertices = UInt32.MaxValue, maxVertices = UInt32.MinValue;

			Array.ForEach(vcount, delegate (uint v) {
				minVertices = Math.Min(v, minVertices);
				maxVertices = Math.Max(v, maxVertices);
			});

			if ((minVertices < 3) && (maxVertices >= 3))
				throw new ArgumentException("ambigous polygons set", "vcount");

			uint totalVerticesCount = 0;

			Array.ForEach(vcount, delegate (uint v) {
				if (v == 4) {
					totalVerticesCount += 6;            // Triangulate quad with two triangles
				} else if (v > 4) {
					totalVerticesCount += (v - 2) * 3;  // Triangulate as if it is a polygon
				} else {
					Debug.Assert(v == 3);
					totalVerticesCount += 3;            // Exactly a triangle
				}
			});

			Create(totalVerticesCount);

			// Copy polygons (triangulate)
			uint count = 0;

			unsafe {
				byte* dstPtr = (byte*)ClientBufferAddress.ToPointer();
				uint indicesIndex = offset;

				for (uint i = 0; i < vcount.Length; i++) {
					uint verticesCount = vcount[i];
					uint[] verticesIndices;

					if (verticesCount == 4) {
						verticesIndices = new uint[6];
						verticesIndices[0] = indices[indicesIndex + (0 * stride)];
						verticesIndices[1] = indices[indicesIndex + (1 * stride)];
						verticesIndices[2] = indices[indicesIndex + (2 * stride)];
						verticesIndices[3] = indices[indicesIndex + (0 * stride)];
						verticesIndices[4] = indices[indicesIndex + (2 * stride)];
						verticesIndices[5] = indices[indicesIndex + (3 * stride)];

						indicesIndex += 4 * stride;
					} else if (verticesCount > 4) {
						uint triCount = verticesCount - 2;
						uint pivotIndex = indicesIndex;

						verticesIndices = new uint[triCount * 3];

						// Copy polygon indices
						for (uint tri = 0; tri < triCount; tri++) {
							verticesIndices[tri * 3 + 0] = indices[pivotIndex];
							verticesIndices[tri * 3 + 1] = indices[indicesIndex + (tri + 2) * stride];
							verticesIndices[tri * 3 + 2] = indices[indicesIndex + (tri + 1) * stride];
						}

						indicesIndex += verticesCount * stride;
					} else {
						verticesIndices = new uint[verticesCount];
						for (int j = 0; j < verticesCount; j++, indicesIndex += stride)
							verticesIndices[j] = indices[indicesIndex];
					}

					count += (uint)verticesIndices.Length;

					for (uint j = 0; j < verticesIndices.Length; j++, dstPtr += ItemSize) {
						// Position 'srcPtr' to the indexed element
						byte* srcPtr = ((byte*)buffer.ClientBufferAddress.ToPointer()) + (ItemSize * verticesIndices[j]);
						// Copy the 'arrayIndex'th element
						Memory.MemoryCopy(dstPtr, srcPtr, ItemSize);
					}
				}
			}
		}

		/// <summary>
		/// Copy from an ArrayBufferObject with an indirection defined by an index (polygon tessellation).
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="indices"></param>
		/// <param name="count"></param>
		public void Copy(ArrayBufferObject buffer, ElementBufferObject indices, uint count)
		{
			Copy(buffer, indices, count, 0, 1);
		}

		/// <summary>
		/// Copy from an ArrayBufferObject with an indirection defined by an index (polygon tessellation).
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="indices"></param>
		/// <param name="count"></param>
		/// <param name="offset"></param>
		/// <param name="stride"></param>
		public void Copy(ArrayBufferObject buffer, ElementBufferObject indices, uint count, uint offset, uint stride)
		{
			uint[] indicesArray;

			switch (indices.ItemType) {
				case DrawElementsType.UnsignedByte:
					indicesArray = Array.ConvertAll<byte, uint>(indices.ToArray<byte>(), delegate (byte item) { return (uint)item; });
					break;
				case DrawElementsType.UnsignedShort:
					indicesArray = Array.ConvertAll<ushort, uint>(indices.ToArray<ushort>(), delegate (ushort item) { return (uint)item; });
					break;
				case DrawElementsType.UnsignedInt:
					indicesArray = indices.ToArray<uint>();
					break;
				default:
					throw new InvalidOperationException(String.Format("element type {0} not supportted", indices.ItemType));
			}

			Copy(buffer, indicesArray, count, offset, stride);
		}

		#endregion

		#region Array Item Layout Conversion

		/// <summary>
		/// Copy this array buffer object to another one (strongly typed), but having a different array item type.
		/// </summary>
		/// <typeparam name="T">
		/// A structure type used to determine the array items layout of the converted ArrayBufferObject.
		/// </typeparam>
		/// <returns>
		/// It returns a copy of this ArrayBufferObject, but having a different array item. The returned instance is actually
		/// a <see cref="ArrayBufferObject"/>; if it is desiderable a strongly typed <see cref="ArrayBufferObject"/>, use
		/// <see cref="ConvertItemType"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the array base type of this ArrayBufferObject (<see cref="ArrayBaseType"/>) is different to the one
		/// derived from <typeparamref name="T"/>.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if the number of base components of this ArrayBufferObject cannot be mapped into the base components
		/// count derived from <typeparamref name="T"/>.
		/// </exception>
		public ArrayBufferObject<T> ConvertItemType<T>() where T : struct
		{
			ArrayBufferItemType vertexArrayType = ArrayBufferItem.GetArrayType(typeof(T));

			return ((ArrayBufferObject<T>)ConvertItemType(vertexArrayType));
		}

		/// <summary>
		/// Copy this array buffer object to another one, but having a different array item type.
		/// </summary>
		/// <param name="vertexArrayType">
		/// A <see cref="ArrayBufferItemType"/> that specify the returned <see cref="ArrayBufferObject"/> item type.
		/// </param>
		/// <returns>
		/// It returns a copy of this ArrayBufferObject, but having a different array item. The returned instance is actually
		/// a <see cref="ArrayBufferObject"/>.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// Exception thrown if the array base type of this ArrayBufferObject (<see cref="ArrayBaseType"/>) is different to the one
		/// derived from <paramref name="vertexArrayType"/>.
		/// </exception>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if the number of base components of this ArrayBufferObject cannot be mapped into the base components
		/// count derived from <paramref name="vertexArrayType"/>.
		/// </exception>
		public ArrayBufferObject ConvertItemType(ArrayBufferItemType vertexArrayType)
		{
			if (ArrayBufferItem.GetArrayBaseType(_ArrayType) != ArrayBufferItem.GetArrayBaseType(vertexArrayType))
				throw new ArgumentException("base type mismatch", "vertexArrayType");

			uint componentsCount = ItemCount * ArrayBufferItem.GetArrayLength(ArrayType) * ArrayBufferItem.GetArrayRank(ArrayType);
			uint convComponentsCount = ArrayBufferItem.GetArrayLength(vertexArrayType) * ArrayBufferItem.GetArrayRank(vertexArrayType);

			Debug.Assert(componentsCount >= convComponentsCount);
			if ((componentsCount % convComponentsCount) != 0)
				throw new InvalidOperationException("components length incompatibility");

			ArrayBufferObject arrayObject = CreateArrayObject(vertexArrayType, Hint);

			// Different item count due different lengths
			arrayObject.Create(componentsCount / convComponentsCount);
			// Memory is copied
			Memory.MemoryCopy(arrayObject.ClientBufferAddress, ClientBufferAddress, ClientBufferSize);

			return (arrayObject);
		}

		#endregion

		#region ArrayBufferObjectBase Overrides

		/// <summary>
		/// Get the count of the array sections aggregated in this ArrayBufferObjectBase.
		/// </summary>
		protected internal override uint ArraySectionsCount { get { return (1); } }

		/// <summary>
		/// Get the specified section information.
		/// </summary>
		/// <param name="index">
		/// The <see cref="UInt32"/> that specify the array section index.
		/// </param>
		/// <returns>
		/// It returns the <see cref="IArraySection"/> defining the array section.
		/// </returns>
		protected internal override IArraySection GetArraySection(uint index)
		{
			if (index != 0)
				throw new ArgumentOutOfRangeException("different from 0", index, "index");

			return (this);
		}

		#endregion

		#region ArrayBufferObjectBase.IArraySection Implementation

		/// <summary>
		/// The type of the elements of the array section.
		/// </summary>
		ArrayBufferItemType IArraySection.ItemType { get { return (ArrayType); } }

		/// <summary>
		/// Get whether the array elements should be meant normalized (fixed point precision values).
		/// </summary>
		bool IArraySection.Normalized { get { return (false); } }

		/// <summary>
		/// Offset of the first element of the array section, in bytes.
		/// </summary>
		IntPtr IArraySection.Offset { get { return (IntPtr.Zero); } }

		/// <summary>
		/// Offset between two element of the array section, in bytes.
		/// </summary>
		IntPtr IArraySection.Stride { get { return (IntPtr.Zero); } }

		#endregion
	}

	/// <summary>
	/// Stronly typed array buffer object.
	/// </summary>
	/// <typeparam name="T">
	/// A structure that holds the array item data. Typically this type match a <see cref="IVertex"/> or <see cref="IColor"/> implementation,
	/// but it could any value type.
	/// </typeparam>
	public class ArrayBufferObject<T> : ArrayBufferObject where T : struct
	{
		#region Constructors

		/// <summary>
		/// Construct an ArrayBufferObject.
		/// </summary>
		/// <param name="hint">
		/// An <see cref="BufferObjectHint"/> that specify the data buffer usage hints.
		/// </param>
		public ArrayBufferObject(BufferObjectHint hint)
			: base(ArrayBufferItem.GetArrayType(typeof(T)), hint)
		{
			
		}

		#endregion

		#region Array Buffer Data Definition

		/// <summary>
		/// Define this ArrayBufferObject by specifing an array of <typeparamref name="T"/>.
		/// </summary>
		/// <param name="items">
		/// An array that specify the contents of this ArrayBufferObject.
		/// </param>
		public void Define(T[] items)
		{
			if (items == null)
				throw new ArgumentNullException("items");
			if (items.Length == 0)
				throw new ArgumentException("zero items", "items");
			if ((Hint != BufferObjectHint.StaticCpuDraw) && (Hint != BufferObjectHint.DynamicCpuDraw))
				throw new InvalidOperationException(String.Format("conflicting hint {0}", Hint));

			// Store item count
			Create((uint)items.Length);
			// Copy the buffer
			Memory.MemoryCopy(ClientBufferAddress, items, ItemCount * ItemSize);
		}

		#endregion

		#region Array Buffer Access

		/// <summary>
		/// Accessor to ArrayBufferObject items.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public T this[uint index]
		{
			get
			{
				IntPtr clientBufferAddress = ClientBufferAddress;

				if (clientBufferAddress == IntPtr.Zero)
					throw new InvalidOperationException("no client buffer");
				if (index >= ItemCount)
					throw new ArgumentException("index out of bounds", "index");

				return ((T) Marshal.PtrToStructure(new IntPtr(clientBufferAddress.ToInt64() + (index * ItemSize)), typeof(T)));
			}
			set
			{
				IntPtr clientBufferAddress = ClientBufferAddress;

				if (clientBufferAddress == IntPtr.Zero)
					throw new InvalidOperationException("no client buffer");
				if (index >= ItemCount)
					throw new ArgumentException("index out of bounds", "index");

				Marshal.StructureToPtr(value, new IntPtr(clientBufferAddress.ToInt64() + (index * ItemSize)), false);
			}
		}

		#endregion

		#region To Array

		/// <summary>
		/// Convert this array buffer object in a strongly-typed array.
		/// </summary>
		/// <returns></returns>
		public new T[] ToArray()
		{
			IntPtr clientBufferAddress = ClientBufferAddress;

			if (clientBufferAddress == IntPtr.Zero)
				throw new InvalidOperationException("no client buffer");

			T[] array = new T[ItemCount];

			// Copy the buffer
			Memory.MemoryCopy(array, clientBufferAddress, ItemCount * ItemSize);

			return (array);
		}

		#endregion

		#region Array Buffer Mapping

		/// <summary>
		/// Set an item to this mapped BufferObject.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/>
		/// </param>
		/// <param name="value">
		/// A <typeparamref name="T"/> that specify the mapped BufferObject element.
		/// </param>
		/// <param name="offset">
		/// A <see cref="Int64"/> that specify the offset applied to the mapped BufferObject where <paramref name="value"/>
		/// is stored. The offset is expressed in number of items from the beginning of the array buffer object, indeed
		/// dependent on the <typeparamref name="T"/> size in bytes.
		/// </param>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this ArrayBufferObject is not mapped (<see cref="BufferObject.IsMapped"/>).
		/// </exception>
		public void MapSet(GraphicsContext ctx, T value, Int64 offset) { MapSet<T>(ctx, value, offset * ItemSize); }

		/// <summary>
		/// Get an element from this mapped BufferObject.
		/// </summary>
		/// <param name="ctx">
		/// A <see cref="GraphicsContext"/>
		/// </param>
		/// <param name="offset">
		/// A <see cref="Int64"/> that specify the offset applied to the mapped BufferObject. The offset is expressed
		/// in number of items from the beginning of the array buffer object, indeed dependent on the <typeparamref name="T"/>
		/// size in bytes.
		/// </param>
		/// <returns>
		/// It returns a structure of type <typeparamref name="T"/>, read from the mapped BufferObject
		/// </returns>
		/// <exception cref="InvalidOperationException">
		/// Exception thrown if this ArrayBufferObject is not mapped (<see cref="BufferObject.IsMapped"/>).
		/// </exception>
		public T MapGet(GraphicsContext ctx, Int64 offset) { return (MapGet<T>(ctx, offset * ItemSize)); }

		#endregion

		#region Join Multiple ArrayBufferObject

		/// <summary>
		/// Join a set of compatible buffers.
		/// </summary>
		/// <param name="buffers">
		/// An array of <see cref="ArrayBufferObject"/> instances to be packed sequentially on this ArrayBufferObject.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// Exception thrown if <paramref name="buffers"/> is null or any item in <paramref name="buffers"/> array
		/// is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Exception thrown if <paramref name="buffers"/> length is less than two (minimum number of ArrayBufferObject
		/// to join).
		/// </exception>
		/// <remarks>
		/// <para>
		/// This ArrayBufferObject will store sequentially all the items stored in <paramref name="buffers"/>.
		/// </para>
		/// </remarks>
		public void Join(params ArrayBufferObject<T>[] buffers)
		{
			if (buffers == null)
				throw new ArgumentNullException("buffers");
			if (buffers.Length <= 1)
				throw new ArgumentException("not enought buffers", "buffers");

			// Since T is the same for 'buffers' and 'this', they are compatible for every T, even if
			// T is a complex structure

			// Collect array buffer objects statistics
			uint bufferSize = 0, itemCount = 0;

			for (int i = 0; i < buffers.Length; i++) {
				if (buffers[i] == null)
					throw new ArgumentNullException("buffers[" + i + "]");

				bufferSize += buffers[i].BufferSize;
				itemCount += buffers[i].ItemCount;
			}

			// Allocate the array buffer object
			Allocate(bufferSize);

			// Join buffers into a single one
			bufferSize = 0;
			for (int i = 0; i < buffers.Length; i++) {
				IntPtr src = buffers[i].ClientBufferAddress;
				IntPtr dst = new IntPtr(ClientBufferAddress.ToInt64() + bufferSize);

				// Copy 'i'th buffer content
				Memory.MemoryCopy(dst, src, buffers[i].BufferSize);
				// Next buffer offset
				bufferSize += buffers[i].BufferSize;
			}
		}

		#endregion
	}
}