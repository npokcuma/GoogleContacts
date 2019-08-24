using System.Collections.Generic;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	interface IContactService
	{
		List<Contact> GetAllContacts();

		bool Delete(Contact contact);

		bool Create(Contact contact);

		bool Update(Contact contact);
		List<Lable> GetAllLables();
		bool RenameLable(Lable lable);
		bool DeleteLable(Lable lable);

	}
}
