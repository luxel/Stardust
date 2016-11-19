namespace Stardust
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// A generic object pool which can store objects which can be reused later.
	/// </summary>
	public class ObjectPool<T> where T: new()
	{
		/// <summary>
		/// The collection which stores the objects.
		/// </summary>
		private Stack<T> pool;

		/// <summary>
		/// Initializes a new instance of the <see cref="Stardust.ObjectPool`1"/> class, with one instance created initially
		/// </summary>
		public ObjectPool () : this(1)
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Stardust.ObjectPool`1"/> class with specified amount of objects.
		/// </summary>
		/// <param name="count">Count.</param>
		public ObjectPool (int count)
		{
			pool = new Stack<T>();

			for (int i = 0; i < count; i ++)
			{
				Return(CreateNewInstance());
			}
		}

		public int Count
		{
			get 
			{
				return pool.Count;
			}
		}

		/// <summary>
		/// Gets an object from the pool.
		/// </summary>
		public T Borrow()
		{
			if (pool.Count > 0)
			{
				return pool.Pop();
			}
			else
			{
				return CreateNewInstance();
			}
		}

		/// <summary>
		/// Returns an object to the pool
		/// </summary>
		public void Return(T t)
		{
			pool.Push(t);
		}

		/// <summary>
		/// Creates a new instance. Any child class must implement this method.
		/// </summary>
		protected virtual T CreateNewInstance()
		{
			return new T();
		}
	}
}