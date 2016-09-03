namespace Stardust
{
	using System;

	public class StardustException : Exception 
	{
		public StardustException(string message) : base(message)
		{
		}

		public StardustException(string message, Exception innerException) : base(message, innerException)
		{
		}
	}
}