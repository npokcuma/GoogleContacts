using System;
using GalaSoft.MvvmLight;

namespace ipogonyshevNetTest.Model
{
	public class Contact : ViewModelBase
	{
		/// <summary>
		/// Create a unique id.
		/// </summary>
		public Contact()
		{
			Id = Guid.NewGuid().ToString();
		}

		public string Id { get; set; }

		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string Surname { get; set; }

		public string PhoneNumber { get; set; }

		public string EmailAddress { get; set; }
	}
}
