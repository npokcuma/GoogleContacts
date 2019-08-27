using System.Collections.Generic;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	public interface IContactService
	{
		List<Contact> GetAllContacts();

		bool DeleteContact(Contact contact);

		bool CreateContact(Contact contact);

		bool UpdateContact(Contact contact);

		List<Lable> GetAllLables();

		bool CreateLable(Lable lable);

		bool RenameLable(Lable lable);

		bool DeleteLable(Lable lable);
		bool UpdateLable(Lable lable);
	}
}
