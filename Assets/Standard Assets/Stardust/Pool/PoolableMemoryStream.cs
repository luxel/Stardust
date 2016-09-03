namespace Stardust.Pool
{
	using System;
	using System.IO;

	public class PoolableMemoryStream : IPoolableObject<MemoryStream> {

		public MemoryStream Value { get; private set; }

		public PoolableMemoryStream()
		{
			Value = new MemoryStream();
		}

		public PoolableMemoryStream (MemoryStream value)
		{
			Value = value;
		}

		public virtual void Restore()
		{
			Value.Position = 0;
		}

		public virtual void Release()
		{
			Pools.MemoryStreamPool.Return(this);
		}

		public void Dispose()
		{			
			Restore();
			Release();
		}
	}
}