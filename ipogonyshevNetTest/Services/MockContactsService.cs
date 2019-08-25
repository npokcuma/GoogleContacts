using System.Collections.Generic;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	class MockContactsService : IContactService
	{
		private readonly List<Contact> _listContacts;
		private readonly List<Lable> _listLables;

		public MockContactsService()
		{

			_listContacts = new List<Contact>();
			var contact1 = new Contact
			{
				Id = "id1",
				EmailAddress = "email1@mail.ru",
				Name = "Иванов Иван Иванович",
				PhoneNumber = "8 800 555 35 35"
			};
			var contact2 = new Contact
			{
				Id = "id2",
				EmailAddress = "email2@mail.ru",
				Name = "Петров Петр Иванович",
				PhoneNumber = "8 999 666 11 22"
			};
			var contact3 = new Contact
			{
				Id = "id3",
				EmailAddress = "email3@mail.ru",
				Name = "Сидоров Сидор",
				PhoneNumber = "8 693 000 77 77"
			};
			_listContacts.Add(contact1);
			_listContacts.Add(contact2);
			_listContacts.Add(contact3);

			_listLables = new List<Lable>();
			var lable1 = new Lable
			{
				Id = "1",
				Name = "Lalka"
			};
			lable1.Contacts.Add(contact1);
			var lable2 = new Lable
			{
				Id = "2",
				Name = "MyBand"
			};
			lable2.Contacts.Add(contact2);
			_listLables.Add(lable1);
			_listLables.Add(lable2);
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

		public bool CreateContact(Contact contact)
		{
			_listContacts.Add(contact);
			return false;
		}

		public bool UpdateContact(Contact contact)
		{
			return true;
		}

		public List<Lable> GetAllLables()
		{
			return _listLables;
		}

		public bool CreateLable(Lable lable)
		{
			_listLables.Add(lable);
			return true;
		}

		public bool RenameLable(Lable lable)
		{
			return true;
		}

		public bool DeleteLable(Lable lable)
		{
			_listLables.Remove(lable);
			return true;
		}
	}
}
