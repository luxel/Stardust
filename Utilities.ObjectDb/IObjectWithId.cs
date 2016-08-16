namespace Stardust.Utilities
{
	using System;

	public interface IObjectWithId<T>
	{
		T Id { get; }
	}
}