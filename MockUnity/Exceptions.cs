using System;
using System.Runtime.Serialization;

namespace UnityEngine
{
	[Serializable ]
	public class UnityException : SystemException
	{
		private const int Result = -2147467261;
		public UnityException () : base ("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}
		public UnityException (string message) : base (message)
		{
			base.HResult = -2147467261;
		}
		public UnityException (string message, Exception innerException) : base (message, innerException)
		{
			base.HResult = -2147467261;
		}
		protected UnityException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
		}
	}
    
	[Serializable ]
	public class MissingComponentException : SystemException
	{
		private const int Result = -2147467261;
		public MissingComponentException () : base ("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}
		public MissingComponentException (string message) : base (message)
		{
			base.HResult = -2147467261;
		}
		public MissingComponentException (string message, Exception innerException) : base (message, innerException)
		{
			base.HResult = -2147467261;
		}
		protected MissingComponentException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
		}
	}

	[Serializable ]
	public class MissingReferenceException : SystemException
	{
		private const int Result = -2147467261;
		public MissingReferenceException () : base ("A Unity Runtime error occurred!")
		{
			base.HResult = -2147467261;
		}
		public MissingReferenceException (string message) : base (message)
		{
			base.HResult = -2147467261;
		}
		public MissingReferenceException (string message, Exception innerException) : base (message, innerException)
		{
			base.HResult = -2147467261;
		}
		protected MissingReferenceException (SerializationInfo info, StreamingContext context) : base (info, context)
		{
		}
	}
    
}

