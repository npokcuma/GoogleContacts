using System.Collections.Generic;

namespace ipogonyshevNetTest
{
	class MockContactsService : IContactService
	{
		private readonly List<Contact> _listContacts;
		private readonly List<Lable> _listLables;

		public MockContactsService()
		{
			_listContacts = new List<Contact>
			{
				new Contact
				{
					Id = "id1",
					EmailAddress = "email1@mail.ru",
					Name = "Иванов Иван Иванович",
					PhoneNumber = "8 800 555 35 35"
				},
				new Contact
				{
					Id = "id2",
					EmailAddress = "email2@mail.ru",
					Name = "Петров Петр Иванович",
					PhoneNumber = "8 999 666 11 22"
				}
			};

			_listLables = new List<Lable>
			{
				new Lable
				{
					Id = "1",
					Name = "Lalka"
				},
				new Lable
				{
					Id="2",
					Name = "MyBand"
				}
			};
		}


		public List<Contact> GetAllContacts()
		{
			return _listContacts;
		}

		public bool Delete(Contact contact)
		{
			_listContacts.Remove(contact);
			return true;
		}

		public bool Create(Contact contact)
		{
			_listContacts.Add(contact);
			return false;
		}

		public bool Update(Contact contact)
		{
			return true;
		}

		public List<Lable> GetAllLables()
		{
			return _listLables;
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
