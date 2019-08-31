using System.Collections.Generic;
using System.Linq;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	class MockContactsService : IContactService
	{
		private readonly List<Contact> _listContacts;
		private readonly List<Label> _listLabels;

		public MockContactsService()
		{

			_listContacts = new List<Contact>();
			var contact1 = new Contact
			{
				Id = "id1",
				EmailAddress = "email1@mail.ru",
				FirstName = "Иван",
				Surname = "Иванов",
				MiddleName = "Иванович",
				PhoneNumber = "8 800 555 35 35"
			};
			var contact2 = new Contact
			{
				Id = "id2",
				EmailAddress = "email2@mail.ru",
				FirstName = "Петр",
				Surname = "Петров",
				MiddleName = "Петрович",
				PhoneNumber = "8 999 666 11 22"
			};
			var contact3 = new Contact
			{
				Id = "id3",
				EmailAddress = "email3@mail.ru",
				FirstName = "Сидор",
				Surname = "Сидоров",
				PhoneNumber = "8 693 000 77 77"
			};
			_listContacts.Add(contact1);
			_listContacts.Add(contact2);
			_listContacts.Add(contact3);

			_listLabels = new List<Label>();
			var label1 = new Label
			{
				Id = "1",
				Name = "Lalka"
			};
			label1.Contacts.Add(contact1);
			var label2 = new Label
			{
				Id = "2",
				Name = "MyBand"
			};
			label2.Contacts.Add(contact2);
			_listLabels.Add(label1);
			_listLabels.Add(label2);
		}


		public List<Contact> GetAllContacts()
		{
			return _listContacts;
		}

		public bool DeleteContact(Contact contact)
		{
			_listContacts.Remove(contact);
			return true;
		}

		public string CreateContact(Contact contact)
		{
			_listContacts.Add(contact);
			return contact.Id;
		}

		public bool UpdateContact(Contact contact)
		{
			return true;
		}

		public List<Label> GetAllLabels()
		{
			return _listLabels;
		}

		public string CreateLabel(Label label)
		{
			_listLabels.Add(label);
			return label.Id;
		}

		public bool DeleteLabel(Label label)
		{
			_listLabels.Remove(label);
			return true;
		}

		public bool UpdateLabel(Label label)
		{
			return true;
		}

		public bool AddLabelToContact(Contact contact, Label label)
		{
			var storedLabel = _listLabels.First(l => l.Id == label.Id);
			var storedContact = _listContacts.First(c => c.Id == contact.Id);

			storedLabel.Contacts.Remove(storedContact);
			return true;
		}

		public bool RemoveLabelFromContact(Contact contact, Label label)
		{
			var storedLabel = _listLabels.First(l => l.Id == label.Id);
			var storedContact = _listContacts.First(c => c.Id == contact.Id);

			storedLabel.Contacts.Add(storedContact);
			return true;
		}
	}
}
