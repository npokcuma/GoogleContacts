using System.Collections.Generic;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	public interface IContactService
	{
		List<Contact> GetAllContacts();

		bool DeleteContact(Contact contact);

		string CreateContact(Contact contact);

		bool UpdateContact(Contact contact);

		List<Label> GetAllLabels();

		bool CreateLabel(Label label);

		bool RenameLabel(Label label);

		bool DeleteLabel(Label label);

		bool UpdateLabel(Label label);
	}
}
