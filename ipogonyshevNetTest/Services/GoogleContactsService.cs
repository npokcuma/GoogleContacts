using System;
using System.Collections.Generic;
using System.Linq;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	internal class GoogleContactsService : IContactService
	{
		private static PeopleServiceService _service;
		private List<Contact> _listContacts;
		private List<Label> _listLabels;
		private IList<Person> _listPerson;

		public GoogleContactsService()
		{
			_service = Authorization.GetGooglePeopleService();
		}

		private void ReloadContacts()
		{
			var groupRequest = new ContactGroupsResource(_service).List();
			var response = groupRequest.Execute();

			var allGroups = response.ContactGroups.Select(group => new Label
			{
				Id = group.ResourceName,
				Name = group.FormattedName
			}).ToList();
			_listLabels = allGroups;

			var peopleRequest = _service.People.Connections.List("people/me");
			peopleRequest.PersonFields = "addresses,ageRanges,biographies,birthdays,braggingRights,coverPhotos,emailAddresses,events,genders,imClients,interests,locales,memberships,metadata,names,nicknames,occupations,organizations,phoneNumbers,photos,relations,relationshipInterests,relationshipStatuses,residences,sipAddresses,skills,taglines,urls,userDefined";
			peopleRequest.SortOrder = (PeopleResource.ConnectionsResource.ListRequest.SortOrderEnum)1;
			peopleRequest.PageSize = 1000;
			var personResponse = peopleRequest.Execute();

			_listPerson = personResponse.Connections;
			_listContacts = new List<Contact>();
			foreach (var person in _listPerson)
			{
				var contact = new Contact
				{
					FirstName = person.Names?[0]?.GivenName,
					Surname = person.Names?[0]?.FamilyName,
					MiddleName = person.Names?[0]?.MiddleName,
					PhoneNumber = Convert.ToString(person.PhoneNumbers?[0]?.CanonicalForm),
					EmailAddress = Convert.ToString(person.EmailAddresses?[0]?.Value),
					Id = person.ResourceName
				};
				_listContacts.Add(contact);

				foreach (var membership in person.Memberships)
				{
					_listLabels.First(l => l.Id == membership.ContactGroupMembership.ContactGroupResourceName).Contacts.Add(contact);
				}
			}
		}

		public List<Contact> GetAllContacts()
		{
			ReloadContacts();
			return _listContacts;
		}

		public bool DeleteContact(Contact contact)
		{
			return true;
		}

		public bool CreateContact(Contact contact)
		{
			return true;
		}

		public bool UpdateContact(Contact contact)
		{
			var person = _listPerson.First(p => p.ResourceName == contact.Id);
			if (person.Names == null)
			{
				person.Names = new List<Name> { new Name() };
			}
			if (person.Names[0] == null)
			{
				person.Names[0] = new Name();
			}
			person.Names[0].GivenName = contact.FirstName;
			person.Names[0].FamilyName = contact.Surname;
			person.Names[0].MiddleName = contact.MiddleName;

			if (person.EmailAddresses == null)
			{
				person.EmailAddresses = new List<EmailAddress> { new EmailAddress() };
			}
			if (person.EmailAddresses[0] == null)
			{
				person.EmailAddresses[0] = new EmailAddress();
			}
			person.EmailAddresses[0].Value = contact.EmailAddress;

			if (person.PhoneNumbers == null)
			{
				person.PhoneNumbers = new List<PhoneNumber> { new PhoneNumber() };
			}
			if (person.PhoneNumbers[0] == null)
			{
				person.PhoneNumbers[0] = new PhoneNumber();
			}
			person.PhoneNumbers[0].Value = contact.PhoneNumber;

			var peopleRequest = _service.People.UpdateContact(person, person.ResourceName);
				peopleRequest.UpdatePersonFields = "emailAddresses,memberships,names,phoneNumbers";
				peopleRequest.Execute();

			ReloadContacts();

			return true;
		}

		public List<Label> GetAllLabels()
		{
			ReloadContacts();
			return _listLabels;
		}

		public bool CreateLabel(Label label)
		{
			return true;
		}

		public bool RenameLabel(Label label)
		{
			return true;
		}

		public bool DeleteLabel(Label label)
		{
			return true;
		}

		public bool UpdateLabel(Label label)
		{
			return true;
		}
	}
}
