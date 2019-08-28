using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;
using ipogonyshevNetTest.Services;
using ipogonyshevNetTest.View;
using Microsoft.Win32;

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
				AddContactToList(contactViewModel);
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
			DeleteContactCommand = new RelayCommand(DeleteContact, IsAnyContactSelected);
			SaveContactCommand = new RelayCommand(SaveContact, () => true);
			AddLableForContactCommand = new RelayCommand(AddLableForContact, IsAnyContactSelected);
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
				AddLableForContactCommand.RaiseCanExecuteChanged();
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

		public LableViewModel SelectedLableForContact { get; set; }

		public RelayCommand AddContactCommand { get; set; }

		public RelayCommand DeleteContactCommand { get; set; }

		public RelayCommand SaveContactCommand { get; set; }

		public RelayCommand AddLableForContactCommand { get; set; }

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
			var confirm = MessageBox.Show("Are you really want delete contact?",
											"Delete contact",
											MessageBoxButton.YesNo,
											MessageBoxImage.Warning);
			if (confirm != MessageBoxResult.Yes)
				return;

			var result = _contactService.DeleteContact(SelectedContact.GetContact());
			if (result)
			{
				RemoveContactFromList(SelectedContact);
				foreach (var lable in Lables)
				{
					if (lable.Contacts.Contains(SelectedContact))
					{
						lable.Contacts.Remove(SelectedContact);
					}
				}

				SelectedContact = Contacts.FirstOrDefault();
			}
		}

		private bool IsAnyContactSelected()
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

		private void AddLableForContact()
		{
			if (!SelectedLableForContact.Contacts.Contains(SelectedContact))
			{
				SelectedLableForContact.Contacts.Add(SelectedContact);
			}
		}




		private void ShowAllContacts()
		{
			SelectedLable = null;
		}

		private void LableViewModel_OnDelete(object sender, System.EventArgs e)
		{
			var lableViewModel = (LableViewModel)sender;
			RemoveLableFromList(lableViewModel);
		}

		private void ContactViewModel_OnRemoveFromLable(object sender, EventArgs e)
		{
			var contactViewModel = (ContactViewModel)sender;
			SelectedLable.Contacts.Remove(contactViewModel);
		}
		private void LableViewModel_OnEdit(object sender, System.EventArgs e)
		{
			var lableViewModel = (LableViewModel)sender;
			EditLableOnList(lableViewModel);
		}

		private void AddContactToList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLable += ContactViewModel_OnRemoveFromLable;
			_contacts.Add(contactViewModel);
		}

		private void RemoveContactFromList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLable -= ContactViewModel_OnRemoveFromLable;
			_contacts.Remove(contactViewModel);
		}

		private void AddLableToList(LableViewModel lableViewModel)
		{
			lableViewModel.OnDelete += LableViewModel_OnDelete;
			lableViewModel.OnEdit += LableViewModel_OnEdit;
			Lables.Add(lableViewModel);
		}

		private void RemoveLableFromList(LableViewModel lableViewModel)
		{

			var confirm = MessageBox.Show("Are you really want delete lable?",
				"Delete lable",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);
			if (confirm != MessageBoxResult.Yes)
				return;
			lableViewModel.OnDelete -= LableViewModel_OnDelete;
			Lables.Remove(lableViewModel);
		}

		private void AddLable()
		{
			var lableViewModel = new LableViewModel();
			var labelWindowViewModel = new LableWindowViewModel(lableViewModel, Lables.ToList())
			{
				Title = "Add label"
			};
			var window = new LableWindow(labelWindowViewModel);
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

		private void EditLableOnList(LableViewModel lableViewModel)
		{
			var labelWindowViewModel = new LableWindowViewModel(lableViewModel, Lables.ToList())
			{
				Title = "Edit label"
			};
			var window = new LableWindow(labelWindowViewModel);
			if (window.ShowDialog() == true)
			{
				if (lableViewModel.IsDirty)
				{
					var result = _contactService.UpdateLable(lableViewModel.GetLable());
					if (result)
					{
						lableViewModel.Save();
					}
				}
			};
		}
	}
}
