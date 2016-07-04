using System;
using Realms;

namespace NumberTracker
{
	public class Transaction : RealmObject
	{
		public Transaction()
		{
		}

		public string transactionId { get; set; }
		public DateTimeOffset DateTimeAdded { get; set; }
	}
}

