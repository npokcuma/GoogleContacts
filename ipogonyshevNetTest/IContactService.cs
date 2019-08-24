using System.Collections.Generic;

namespace ipogonyshevNetTest
{
	interface IContactService
	{
		List<Contact> GetAllContacts();

		bool Delete(Contact contact);

		bool Create(Contact contact);

		bool Update(Contact contact);
	}
}
