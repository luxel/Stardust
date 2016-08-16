namespace Stardust.Services.Tickable
{
	using System;

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	internal class IntervaledTickAttribute : Attribute
	{
		public float interval;

		public IntervaledTickAttribute(float intervalTime)
		{
			interval = intervalTime;
		}
	}
}
