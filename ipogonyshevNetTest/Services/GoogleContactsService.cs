using System.Collections.Generic;
using Google.Apis.PeopleService.v1;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	class GoogleContactsService : IContactService
	{

		static PeopleServiceService service = Authorization.GetToken();

		//List<GroupData> groupNames = GetGroupList.Getlist(service);

		//List<Contact> contacts = GetContactsList.GetList(service);


		private readonly List<Contact> _listContacts;
		private readonly List<Label> _listLabels;

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
			_listLabels = new List<Label>
			{
				new Label
				{
					Id = "1",
					Name = "Lalka"
				},
				new Label
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

		public bool DeleteContact(Contact contact)
		{
			_listContacts.Remove(contact);
			return true;
		}

		public bool CreateContact(Contact contact)
		{
			_listContacts.Add(contact);
			return true;
		}

		public bool UpdateContact(Contact contact)
		{
			return true;
		}

		public List<Label> GetAllLabels()
		{
			throw new System.NotImplementedException();
		}

		public bool CreateLabel(Label label)
		{
			throw new System.NotImplementedException();
		}

		public bool RenameLabel(Label label)
		{
			throw new System.NotImplementedException();
		}

		public bool DeleteLabel(Label label)
		{
			throw new System.NotImplementedException();
		}

		public bool UpdateLabel(Label label)
		{
			return true;
		}
	}
}
