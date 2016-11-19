namespace Stardust
{
	using System;

	public interface IObjectWithId<T>
	{
		T Id { get; }
	}
}