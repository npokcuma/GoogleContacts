using GalaSoft.MvvmLight;

namespace ipogonyshevNetTest
{
	internal class Contact : ViewModelBase
	{
		private string _id;
		private string _name;
		private string _phoneNumber;
		private string _emailAddress;

		public string Id
		{
			get => _id;
			set => Set(() => Id, ref _id, value);
		}

		public string Name
		{
			get => _name;
			set => Set(() => Name, ref _name, value);
		}

		public string PhoneNumber
		{
			get => _phoneNumber;
			set => Set(() => PhoneNumber, ref _phoneNumber, value);
		}

		public string EmailAddress
		{
			get => _emailAddress;
			set => Set(() => EmailAddress, ref _emailAddress, value);
		}
	}
}
