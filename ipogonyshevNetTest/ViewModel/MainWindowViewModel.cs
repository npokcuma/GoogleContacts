﻿using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;
using ipogonyshevNetTest.Services;
using ipogonyshevNetTest.View;

namespace ipogonyshevNetTest.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly IContactService _contactService;
		private ContactViewModel _selectedContact;
		private ObservableCollection<ContactViewModel> _contacts = new ObservableCollection<ContactViewModel>();
		private LableViewModel _selectedLable;

		public MainWindowViewModel()
		{
		}

		public MainWindowViewModel(IContactService contactService)
		{
			_contactService = contactService;

			var contacts = _contactService.GetAllContacts();
			foreach (var contact in contacts)
			{
				var contactViewModel = new ContactViewModel(contact);
				Contacts.Add(contactViewModel);
			}

			var lables = _contactService.GetAllLables();
			foreach (var lable in lables)
			{
				var lableViewModel = new LableViewModel(lable);
				foreach (var contact in lableViewModel.Entity.Contacts)
				{
					var contactViewModel = Contacts.First(c => c.Id == contact.Id);
					lableViewModel.Contacts.Add(contactViewModel);
				}
				AddLableToList(lableViewModel);
			}

			AddContactCommand = new RelayCommand(AddContact, () => true);
			DeleteContactCommand = new RelayCommand(DeleteContact, CanDeleteContact);
			SaveContactCommand = new RelayCommand(SaveContact, () => true);
			AddLableCommand = new RelayCommand(AddLable, () => true);
			ShowAllContactsCommand = new RelayCommand(ShowAllContacts, () => true);
		}


		public ObservableCollection<ContactViewModel> Contacts
		{
			get
			{
				if (SelectedLable != null)
				{
					return SelectedLable.Contacts;
				}
				return _contacts;
			}
			set => _contacts = value;
		}

		public ObservableCollection<LableViewModel> Lables { get; set; } = new ObservableCollection<LableViewModel>();

		public ContactViewModel SelectedContact
		{
			get => _selectedContact;
			set
			{
				Set(() => SelectedContact, ref _selectedContact, value);
				DeleteContactCommand.RaiseCanExecuteChanged();
			}
		}

		public LableViewModel SelectedLable
		{
			get => _selectedLable;
			set
			{
				Set(() => SelectedLable, ref _selectedLable, value);
				RaisePropertyChanged(nameof(Contacts));
			}
		}

		public RelayCommand AddContactCommand { get; set; }

		public RelayCommand DeleteContactCommand { get; set; }

		public RelayCommand SaveContactCommand { get; set; }

		public RelayCommand AddLableCommand { get; set; }

		public RelayCommand ShowAllContactsCommand { get; set; }


		private void AddContact()
		{
			var contactViewModel = new ContactViewModel();
			Contacts.Add(contactViewModel);
			SelectedContact = contactViewModel;
		}

		private void DeleteContact()
		{
			var result = _contactService.DeleteContact(SelectedContact.GetContact());
			if (result)
			{
				Contacts.Remove(SelectedContact);
				SelectedContact = Contacts.FirstOrDefault();
			}
		}

		private bool CanDeleteContact()
		{
			return SelectedContact != null;
		}

		private void SaveContact()
		{
			if (SelectedContact.IsNew)
			{
				var result = _contactService.CreateContact(SelectedContact.GetContact());
				if (result)
				{
					SelectedContact.Save();
				}
			}
			else
			{
				var result = _contactService.UpdateContact(SelectedContact.GetContact());
				if (result)
				{
					SelectedContact.Save();
				}
			}
		}

		private void AddLable()
		{
			var lableViewModel = new LableViewModel();
			var window = new LableWindow(lableViewModel);
			if (window.ShowDialog() == true)
			{
				var result = _contactService.CreateLable(lableViewModel.GetLable());
				if (result)
				{
					lableViewModel.Save();
					AddLableToList(lableViewModel);
				}
			};
		}

		private void ShowAllContacts()
		{
			SelectedLable = null;
		}

		private void LableViewModel_OnDelete(object sender, System.EventArgs e)
		{
			var lableViewModel = (LableViewModel) sender;
			RemoveLableFromList(lableViewModel);
		}

		private void AddLableToList(LableViewModel lableViewModel)
		{
			lableViewModel.OnDelete += LableViewModel_OnDelete;
			Lables.Add(lableViewModel);
		}

		private void RemoveLableFromList(LableViewModel lableViewModel)
		{
			lableViewModel.OnDelete -= LableViewModel_OnDelete;
			Lables.Remove(lableViewModel);
		}
	}
}