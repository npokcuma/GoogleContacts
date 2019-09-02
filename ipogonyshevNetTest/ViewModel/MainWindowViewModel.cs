using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Services;
using ipogonyshevNetTest.View;

namespace ipogonyshevNetTest.ViewModel
{
	public class MainWindowViewModel : ViewModelBase
	{
		private readonly IContactService _contactService;
		private ContactViewModel _selectedContact;
		private ObservableCollection<ContactViewModel> _contacts = new ObservableCollection<ContactViewModel>();
		private LabelViewModel _selectedLabel;
		private LabelViewModel _selectedLabelForContact;

		public MainWindowViewModel()
		{
		}

		/// <summary>
		/// Create a viewmodel of our application and fill it with functionality
		/// </summary>
		/// <param name="contactService"></param>
		public MainWindowViewModel(IContactService contactService)
		{
			_contactService = contactService;

			if (_contactService.IsLoggedIn())
			{
				RefreshContacts();
			}

			AuthorizeCommand = new RelayCommand(Authorize, () => true);
			LogOutCommand = new RelayCommand(LogOut, () => true);

			AddContactCommand = new RelayCommand(AddContact, () => true);
			DeleteContactCommand = new RelayCommand(DeleteContact, IsAnyContactSelected);
			SaveContactCommand = new RelayCommand(SaveContact, IsAnyContactSelected);
			AddLabelForContactCommand = new RelayCommand(AddLabelForContact, () => IsAnyContactSelected() &&
																				   SelectedLabelForContact != null);
			AddLabelCommand = new RelayCommand(AddLabel, () => true);
			ShowAllContactsCommand = new RelayCommand(SelectNoLabel, () => true);
		}

		/// <summary>
		/// Collection of contacts. In the case of a highlighted group, shows its members.
		/// </summary>
		public ObservableCollection<ContactViewModel> Contacts
		{
			get
			{
				if (SelectedLabel != null)
				{
					return SelectedLabel.Contacts;
				}
				return _contacts;
			}
			set => _contacts = value;
		}

		public int ContactsCount => _contacts.Count;

		public ObservableCollection<LabelViewModel> Labels { get; set; } = new ObservableCollection<LabelViewModel>();

		public bool IsAuthorized => _contactService.IsLoggedIn();

		/// <summary>
		/// If the contact is highlighted, make active the delete, save, add a contact to the group buttons.
		/// </summary>
		public ContactViewModel SelectedContact
		{
			get => _selectedContact;
			set
			{
				Set(() => SelectedContact, ref _selectedContact, value);
				DeleteContactCommand.RaiseCanExecuteChanged();
				SaveContactCommand.RaiseCanExecuteChanged();
				AddLabelForContactCommand.RaiseCanExecuteChanged();
			}
		}

		public LabelViewModel SelectedLabel
		{
			get => _selectedLabel;
			set
			{
				Set(() => SelectedLabel, ref _selectedLabel, value);
				RaisePropertyChanged(nameof(Contacts));
			}
		}

		public LabelViewModel SelectedLabelForContact
		{
			get => _selectedLabelForContact;
			set
			{
				Set(() => SelectedLabelForContact, ref _selectedLabelForContact, value);
				AddLabelForContactCommand.RaiseCanExecuteChanged();
			}
		}

		/// <summary>
		/// We go through the authorization procedure in Google and get a token for working on API.
		/// </summary>
		public RelayCommand AuthorizeCommand { get; set; }

		/// <summary>
		/// We exit from the Google account.
		/// </summary>
		public RelayCommand LogOutCommand { get; set; }

		/// <summary>
		/// Adding a new contact.
		/// </summary>
		public RelayCommand AddContactCommand { get; set; }

		/// <summary>
		/// Saving a new contact.
		/// </summary>
		public RelayCommand SaveContactCommand { get; set; }

		/// <summary>
		/// Deleting a new contact.
		/// </summary>
		public RelayCommand DeleteContactCommand { get; set; }

		/// <summary>
		/// Adding a contact to a group.
		/// </summary>
		public RelayCommand AddLabelForContactCommand { get; set; }


		/// <summary>
		/// Adding a new label.
		/// </summary>
		public RelayCommand AddLabelCommand { get; set; }

		/// <summary>
		/// Show all contacts.
		/// </summary>
		public RelayCommand ShowAllContactsCommand { get; set; }


		private void Authorize()
		{
			_contactService.Authorize();
			RefreshContacts();
		}

		private void LogOut()
		{
			_contactService.LogOut();
			ClearContacts();
		}

		/// <summary>
		/// We get an updated list of contacts and groups by API.
		/// </summary>
		private void RefreshContacts()
		{
			var contacts = _contactService.GetAllContacts();
			_contacts = new ObservableCollection<ContactViewModel>();
			foreach (var contact in contacts)
			{
				var contactViewModel = new ContactViewModel(contact);
				AddContactToList(contactViewModel);
			}

			var labels = _contactService.GetAllLabels();
			Labels = new ObservableCollection<LabelViewModel>();
			foreach (var label in labels)
			{
				var labelViewModel = new LabelViewModel(label);
				foreach (var contact in labelViewModel.Entity.Contacts)
				{
					var contactViewModel = Contacts.First(c => c.Id == contact.Id);
					labelViewModel.Contacts.Add(contactViewModel);
				}
				AddLabelToList(labelViewModel);
			}

			RaisePropertyChanged(nameof(IsAuthorized));
			RaisePropertyChanged(nameof(Contacts));
			RaisePropertyChanged(nameof(ContactsCount));
			RaisePropertyChanged(nameof(Labels));
		}

		/// <summary>
		/// Create an empty list of contacts and groups.
		/// </summary>
		private void ClearContacts()
		{
			_contacts = new ObservableCollection<ContactViewModel>();
			Labels = new ObservableCollection<LabelViewModel>();
			RaisePropertyChanged(nameof(IsAuthorized));
			RaisePropertyChanged(nameof(Contacts));
			RaisePropertyChanged(nameof(ContactsCount));
			RaisePropertyChanged(nameof(Labels));
		}

		/// <summary>
		/// Create a new contact. Check that the group is not selected. Subscribe to the event.
		/// </summary>
		private void AddContact()
		{
			var contactViewModel = new ContactViewModel();
			Contacts.Insert(0, contactViewModel);
			if (SelectedLabel != null)
			{
				_contacts.Insert(0, contactViewModel);
			}
			SelectedContact = contactViewModel;
			contactViewModel.OnRemoveFromLabel += ContactViewModel_OnRemoveFromLabel;
			RaisePropertyChanged(nameof(ContactsCount));
		}

		/// <summary>
		/// Save a new or update a changed contact.
		/// </summary>
		private void SaveContact()
		{
			if (SelectedContact != null)
			{
				if (SelectedContact.IsNew)
				{
					var result = _contactService.CreateContact(SelectedContact.GetContact());
					if (result != null)
					{
						SelectedContact.Id = result;
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

		}

		/// <summary>
		/// Delete the contact.
		/// Check if it is new and wait for confirmation from Google.
		/// Also remove it from the groups.
		/// </summary>
		private void DeleteContact()
		{
			var confirm = MessageBox.Show("Are you really want delete contact?",
											"Delete contact",
											MessageBoxButton.YesNo,
											MessageBoxImage.Warning);
			if (confirm != MessageBoxResult.Yes)
				return;

			var result = SelectedContact.IsNew ||
						 _contactService.DeleteContact(SelectedContact.GetContact());
			if (result)
			{
				var removedContact = SelectedContact;
				RemoveContactFromList(SelectedContact);
				foreach (var label in Labels)
				{
					if (label.Contacts.Contains(removedContact))
					{
						label.Contacts.Remove(removedContact);
					}
				}

				SelectedContact = Contacts.FirstOrDefault();
				RaisePropertyChanged(nameof(ContactsCount));
			}
		}

		/// <summary>
		/// Add a new group and check if it matches the current groups.
		/// </summary>
		private void AddLabel()
		{
			var labelViewModel = new LabelViewModel();
			var labelWindowViewModel = new LabelWindowViewModel(labelViewModel, Labels.ToList())
			{
				Title = "Add label"
			};
			var window = new LabelWindow(labelWindowViewModel);
			if (window.ShowDialog() == true)
			{
				var result = _contactService.CreateLabel(labelViewModel.GetLabel());
				if (result != null)
				{
					labelViewModel.Id = result;
					labelViewModel.Save();
					AddLabelToList(labelViewModel);
				}
			};
		}

		/// <summary>
		/// Change the name of the group.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelViewModel_OnEdit(object sender, EventArgs e)
		{
			var labelViewModel = (LabelViewModel)sender;
			var labelWindowViewModel = new LabelWindowViewModel(labelViewModel, Labels.ToList())
			{
				Title = "Edit label"
			};
			var window = new LabelWindow(labelWindowViewModel);
			if (window.ShowDialog() == true)
			{
				if (labelViewModel.IsDirty)
				{
					var result = _contactService.UpdateLabel(labelViewModel.GetLabel());
					if (result)
					{
						labelViewModel.Save();
					}
				}
			};
		}

		/// <summary>
		/// Delete the group from the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LabelViewModel_OnDelete(object sender, EventArgs e)
		{
			var labelViewModel = (LabelViewModel)sender;
			var confirm = MessageBox.Show("Are you really want delete label?",
				"Delete label",
				MessageBoxButton.YesNo,
				MessageBoxImage.Warning);
			if (confirm != MessageBoxResult.Yes)
				return;

			var result = _contactService.DeleteLabel(labelViewModel.GetLabel());
			if (result)
			{
				RemoveLabelFromList(labelViewModel);
			}
		}

		/// <summary>
		/// Add a group to the contact.
		/// </summary>
		private void AddLabelForContact()
		{
			if (SelectedContact.IsNew)
			{
				var resultId = _contactService.CreateContact(SelectedContact.GetContact());
				if (resultId != null)
				{
					SelectedContact.Id = resultId;
				}
			}
			var result = _contactService.AddLabelToContact(SelectedContact.GetContact(), SelectedLabelForContact.Entity);
			if (result)
			{
				if (!SelectedLabelForContact.Contacts.Contains(SelectedContact))
				{
					SelectedLabelForContact.Contacts.Add(SelectedContact);
				}
			}
		}

		/// <summary>
		/// Delete the contact from the group.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ContactViewModel_OnRemoveFromLabel(object sender, EventArgs e)
		{
			var contactViewModel = (ContactViewModel)sender;
			var result = _contactService.RemoveLabelFromContact(contactViewModel.GetContact(), SelectedLabel.Entity);
			if (result)
			{
				SelectedLabel.Contacts.Remove(contactViewModel);
			}
		}

		private bool IsAnyContactSelected()
		{
			return SelectedContact != null;
		}

		private void SelectNoLabel()
		{
			SelectedLabel = null;
		}

		/// <summary>
		/// Add the contact to the local list.
		/// Subscribe to events.
		/// </summary>
		/// <param name="contactViewModel"></param>
		private void AddContactToList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLabel += ContactViewModel_OnRemoveFromLabel;
			_contacts.Add(contactViewModel);
		}

		/// <summary>
		/// Delete the contact from the local list.
		/// Unsubscribe from events.
		/// </summary>
		/// <param name="contactViewModel"></param>
		private void RemoveContactFromList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLabel -= ContactViewModel_OnRemoveFromLabel;
			_contacts.Remove(contactViewModel);
		}

		/// <summary>
		/// Add the group to the local list.
		/// Subscribe to events.
		/// </summary>
		/// <param name="labelViewModel"></param>
		private void AddLabelToList(LabelViewModel labelViewModel)
		{
			labelViewModel.OnDelete += LabelViewModel_OnDelete;
			labelViewModel.OnEdit += LabelViewModel_OnEdit;
			Labels.Add(labelViewModel);
		}

		/// <summary>
		/// Delete the group from the local list.
		/// Unsubscribe from events.
		/// </summary>
		/// <param name="labelViewModel"></param>
		private void RemoveLabelFromList(LabelViewModel labelViewModel)
		{
			labelViewModel.OnDelete -= LabelViewModel_OnDelete;
			labelViewModel.OnEdit -= LabelViewModel_OnEdit;
			Labels.Remove(labelViewModel);
		}
	}
}
