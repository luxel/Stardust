namespace Stardust
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	/// <summary>
	/// Provides utility methods for collections.
	/// </summary>
	public static class CollectionUtility 
	{
		/// <summary>
		/// Shuffles the specified list to make it having a random order.
		/// </summary>
		public static void Shuffle<T>(IList<T> list)
		{
			Random rand = new Random();
			for (int i=list.Count-1; i > 0; i--)				
			{
				int swapIndex = i;
				while (swapIndex == i ) {
					swapIndex = rand.Next(0, list.Count);
				}

				if (swapIndex != i)
				{
					T tmp = list[swapIndex];
					list[swapIndex] = list[i];
					list[i] = tmp;
				}
			}
		}
	}
}