namespace Stardust.Pool
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// Reference to some useful and frequently used object pools.
	/// 用于访问一些常用的对象池
	/// </summary>
	public static class Pools
	{
		/// <summary>
		/// Pools of System.Text.StringBuilder objects.
		/// </summary>
		public static ObjectPool<PoolableStringBuilder> StringBuilderPool { get; private set; }
		/// <summary>
		/// Pools of System.IO.Memory objects.
		/// </summary>
		public static ObjectPool<PoolableMemoryStream> MemoryStreamPool { get; private set; }

		static Pools()
		{
			// Pre-create some object pools.
			StringBuilderPool = new ObjectPool<PoolableStringBuilder>();
			MemoryStreamPool = new ObjectPool<PoolableMemoryStream>();
		}
	}
}