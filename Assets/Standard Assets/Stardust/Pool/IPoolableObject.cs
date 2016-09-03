namespace Stardust.Pool
{
	using System;
	using System.Collections;

	public interface IPoolableObject<T> : IDisposable
	{
		/// <summary>
		/// Gets the underlying instance.
		/// </summary>
		/// <value>The value.</value>
		T Value { get; }
		/// <summary>
		/// Clean up this instance for reuse.
		/// </summary>
		/// Restore methods should clean up the instance sufficiently to remove prior state.
		void Restore ();

		/// <summary>
		/// Release this instance back to the pool.
		/// </summary>
		/// Release methods should clean up the instance sufficiently to remove prior state.
		void Release();
	}
}