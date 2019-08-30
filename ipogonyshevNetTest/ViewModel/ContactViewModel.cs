using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.ViewModel
{
	public class ContactViewModel : ViewModelBase
	{
		private readonly Contact _contact;
		private bool _isNew;
		private string _id;
		private string _name;
		private string _phoneNumber;
		private string _emailAddress;

		public ContactViewModel()
		{
			_contact = new Contact();
			Name = "Новый контакт";
			IsNew = true;

			RemoveFromLableCommand = new RelayCommand(RemoveFromLable, () => true);
		}

		public ContactViewModel(Contact contact)
		{
			_contact = contact;
			Id = contact.Id;
			Name = contact.Name;
			PhoneNumber = contact.PhoneNumber;
			EmailAddress = contact.EmailAddress;
			IsNew = false;

			RemoveFromLableCommand = new RelayCommand(RemoveFromLable, () => true);
		}

		public event EventHandler<EventArgs> OnRemoveFromLable;


		public string Id
		{
			get => _id;
			set
			{
				Set(() => Id, ref _id, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				Set(() => Name, ref _name, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string PhoneNumber
		{
			get => _phoneNumber;
			set
			{
				Set(() => PhoneNumber, ref _phoneNumber, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string EmailAddress
		{
			get => _emailAddress;
			set
			{
				Set(() => EmailAddress, ref _emailAddress, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public ObservableCollection<LableViewModel> Lables { get; set; }

		public bool IsNew
		{
			get => _isNew;
			set
			{
				Set(() => IsNew, ref _isNew, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public bool IsDirty
		{
			get
			{
				bool isDirty = IsNew;
				isDirty = isDirty || Name != _contact.Name;
				isDirty = isDirty || PhoneNumber != _contact.PhoneNumber;
				isDirty = isDirty || EmailAddress != _contact.EmailAddress;
				return isDirty;
			}
		}

		public RelayCommand RemoveFromLableCommand { get; set; }


		public Contact GetContact()
		{
			var contact = new Contact
			{
				Id = Id,
				Name = Name,
				PhoneNumber = PhoneNumber,
				EmailAddress = EmailAddress
			};
			return contact;
		}

		public void Save()
		{
			_contact.Id = Id;
			_contact.Name = Name;
			_contact.PhoneNumber = PhoneNumber;
			_contact.EmailAddress = EmailAddress;

			IsNew = false;
		}

		private void RemoveFromLable()
		{
			OnRemoveFromLable?.Invoke(this, null);
		}
	}
}
