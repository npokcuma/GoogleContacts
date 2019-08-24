using System.Collections.Generic;
using Google.Apis.PeopleService.v1;

namespace ipogonyshevNetTest
{
	class GoogleContactsService : IContactService
	{

		static PeopleServiceService service = Authorization.GetToken();

		//List<GroupData> groupNames = GetGroupList.Getlist(service);

		//List<Contact> contacts = GetContactsList.GetList(service);


		private readonly List<Contact> _listContacts;

		public GoogleContactsService()
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
			return true;
		}

		public bool Update(Contact contact)
		{
			return true;
		}
	}
}