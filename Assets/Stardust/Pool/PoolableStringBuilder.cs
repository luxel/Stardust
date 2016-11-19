namespace Stardust
{
	using System;
	using System.Text;

	/// <summary>
	/// A DisposableStringBuilder will auto clears it and returns it back to the pool when disposing.
	/// </summary>
	public class PoolableStringBuilder : IPoolableObject<StringBuilder>
	{
		public StringBuilder Value { get; private set; }

		public PoolableStringBuilder()
		{
			Value = new StringBuilder();
		}
		
		public PoolableStringBuilder (StringBuilder value)
		{
			Value = value;
		}

		public virtual void Restore()
		{
			Value.Length = 0;
		}

		public virtual void Release()
		{
			Pools.StringBuilderPool.Return(this);
		}

		public void Dispose()
		{			
			Restore();
			Release();
		}
	}
}