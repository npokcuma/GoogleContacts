using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight.Command;

namespace ipogonyshevNetTest
{
	class MainWindowViewModel : ViewModelBase
	{
		private readonly IContactService _contactService;
		private ContactViewModel _selectedContact;

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
				Lables.Add(lableViewModel);
			}

			AddContactCommand = new RelayCommand(AddContact, () => true);
			DeleteContactCommand = new RelayCommand(DeleteContact, CanDeleteContact);
			SaveContactCommand = new RelayCommand(SaveContact, () => true);
			AddLableCommand = new RelayCommand(AddLable, () => true);
		}


		public ObservableCollection<ContactViewModel> Contacts { get; set; } = new ObservableCollection<ContactViewModel>();
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

		public RelayCommand AddContactCommand { get; set; }

		public RelayCommand DeleteContactCommand { get; set; }

		public RelayCommand SaveContactCommand { get; set; }

		public RelayCommand AddLableCommand { get; set; }


		private void AddContact()
		{
			var contactViewModel = new ContactViewModel();
			Contacts.Add(contactViewModel);
			SelectedContact = contactViewModel;
		}

		private void DeleteContact()
		{
			var result = _contactService.Delete(SelectedContact.GetContact());
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
				var result = _contactService.Create(SelectedContact.GetContact());
				if (result)
				{
					SelectedContact.Save();
				}
			}
			else
			{
				var result = _contactService.Update(SelectedContact.GetContact());
				if (result)
				{
					SelectedContact.Save();
				}
			}
		}

		private void AddLable()
		{
			var window = new LableWindow();
			window.ShowDialog();
		}
	}
}
