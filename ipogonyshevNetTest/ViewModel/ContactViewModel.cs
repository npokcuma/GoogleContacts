﻿using System;
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
		private string _phoneNumber;
		private string _emailAddress;
		private string _firstName;
		private string _middleName;
		private string _surname;

		/// <summary>
		/// Create a new contact and fill it with functionality.
		/// </summary>
		public ContactViewModel()
		{
			_contact = new Contact();
			Id = _contact.Id;
			FirstName = "New contact";
			IsNew = true;

			RemoveFromLabelCommand = new RelayCommand(RemoveFromLabel, () => true);
		}

		/// <summary>
		/// Add existing contacts.
		/// </summary>
		/// <param name="contact"></param>
		public ContactViewModel(Contact contact)
		{
			_contact = contact;
			Id = contact.Id;
			FirstName = contact.FirstName;
			MiddleName = contact.MiddleName;
			Surname = contact.Surname;
			PhoneNumber = contact.PhoneNumber;
			EmailAddress = contact.EmailAddress;
			IsNew = false;

			RemoveFromLabelCommand = new RelayCommand(RemoveFromLabel, () => true);
		}

		/// <summary>
		/// The event of removing a contact from the group.
		/// </summary>
		public event EventHandler<EventArgs> OnRemoveFromLabel;


		public string Id
		{
			get => _id;
			set
			{
				Set(() => Id, ref _id, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string DisplayName => $"{FirstName} {MiddleName} {Surname}";

		public string FirstName
		{
			get => _firstName;
			set
			{
				Set(() => FirstName, ref _firstName, value);
				RaisePropertyChanged(nameof(DisplayName));
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string MiddleName
		{
			get => _middleName;
			set
			{
				Set(() => MiddleName, ref _middleName, value);
				RaisePropertyChanged(nameof(DisplayName));
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string Surname
		{
			get => _surname;
			set
			{
				Set(() => Surname, ref _surname, value);
				RaisePropertyChanged(nameof(DisplayName));
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

		/// <summary>
		/// A contact is new.
		/// </summary>
		public bool IsNew
		{
			get => _isNew;
			set
			{
				Set(() => IsNew, ref _isNew, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		/// <summary>
		/// A contact had changed.
		/// </summary>
		public bool IsDirty
		{
			get
			{
				bool isDirty = IsNew;
				isDirty = isDirty || FirstName != _contact.FirstName;
				isDirty = isDirty || MiddleName != _contact.MiddleName;
				isDirty = isDirty || Surname != _contact.Surname;
				isDirty = isDirty || PhoneNumber != _contact.PhoneNumber;
				isDirty = isDirty || EmailAddress != _contact.EmailAddress;
				return isDirty;
			}
		}

		public RelayCommand RemoveFromLabelCommand { get; set; }


		public Contact GetContact()
		{
			var contact = new Contact
			{
				Id = Id,
				FirstName = FirstName,
				MiddleName = MiddleName,
				Surname = Surname,
				PhoneNumber = PhoneNumber,
				EmailAddress = EmailAddress
			};
			return contact;
		}

		public void Save()
		{
			_contact.Id = Id;
			_contact.FirstName = FirstName;
			_contact.MiddleName = MiddleName;
			_contact.Surname = Surname;
			_contact.PhoneNumber = PhoneNumber;
			_contact.EmailAddress = EmailAddress;
			IsNew = false;
		}

		/// <summary>
		/// Pass the event to MainWindowViewModel.
		/// </summary>
		private void RemoveFromLabel()
		{
			OnRemoveFromLabel?.Invoke(this, null);
		}
	}
}
