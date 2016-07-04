using System;
namespace NumberTracker
{
	public class TransactionViewModel : BaseViewModel
	{
		
		public TransactionViewModel(Transaction transaction)
		{
			id = transaction.transactionId;
			datetime = transaction.DateTimeAdded;
		}

		public string id
		{
			get;set;
		}

		public DateTimeOffset datetime
		{
			get; set;
		}
	}
}

