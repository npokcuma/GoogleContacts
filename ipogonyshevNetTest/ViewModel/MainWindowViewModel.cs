﻿using System;
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

			var labels = _contactService.GetAllLabels();
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

			AddContactCommand = new RelayCommand(AddContact, () => true);
			DeleteContactCommand = new RelayCommand(DeleteContact, IsAnyContactSelected);
			SaveContactCommand = new RelayCommand(SaveContact, () => true);
			AddLabelForContactCommand = new RelayCommand(AddLabelForContact, IsAnyContactSelected);
			AddLabelCommand = new RelayCommand(AddLabel, () => true);
			ShowAllContactsCommand = new RelayCommand(SelectNoLabel, () => true);
		}


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

		public ObservableCollection<LabelViewModel> Labels { get; set; } = new ObservableCollection<LabelViewModel>();

		public ContactViewModel SelectedContact
		{
			get => _selectedContact;
			set
			{
				Set(() => SelectedContact, ref _selectedContact, value);
				DeleteContactCommand.RaiseCanExecuteChanged();
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

		public LabelViewModel SelectedLabelForContact { get; set; }

		public RelayCommand AddContactCommand { get; set; }

		public RelayCommand SaveContactCommand { get; set; }

		public RelayCommand DeleteContactCommand { get; set; }

		public RelayCommand AddLabelForContactCommand { get; set; }

		public RelayCommand AddLabelCommand { get; set; }

		public RelayCommand ShowAllContactsCommand { get; set; }


		private void AddContact()
		{
			var contactViewModel = new ContactViewModel();
			Contacts.Insert(0, contactViewModel);
			if (SelectedLabel != null)
			{
				_contacts.Insert(0, contactViewModel);
			}
			SelectedContact = contactViewModel;
		}

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
				foreach (var label in Labels)
				{
					if (label.Contacts.Contains(SelectedContact))
					{
						label.Contacts.Remove(SelectedContact);
					}
				}

				SelectedContact = Contacts.FirstOrDefault();
			}
		}


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


		private void AddLabelForContact()
		{
			if (!SelectedLabelForContact.Contacts.Contains(SelectedContact))
			{
				SelectedLabelForContact.Contacts.Add(SelectedContact);
			}
		}

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


		private void AddContactToList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLabel += ContactViewModel_OnRemoveFromLabel;
			_contacts.Add(contactViewModel);
		}

		private void RemoveContactFromList(ContactViewModel contactViewModel)
		{
			contactViewModel.OnRemoveFromLabel -= ContactViewModel_OnRemoveFromLabel;
			_contacts.Remove(contactViewModel);
		}

		private void AddLabelToList(LabelViewModel labelViewModel)
		{
			labelViewModel.OnDelete += LabelViewModel_OnDelete;
			labelViewModel.OnEdit += LabelViewModel_OnEdit;
			Labels.Add(labelViewModel);
		}

		private void RemoveLabelFromList(LabelViewModel labelViewModel)
		{
			labelViewModel.OnDelete -= LabelViewModel_OnDelete;
			labelViewModel.OnEdit -= LabelViewModel_OnEdit;
			Labels.Remove(labelViewModel);
		}
	}
}
