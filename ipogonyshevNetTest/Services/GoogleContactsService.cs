using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.Services
{
	/// <summary>
	/// Here we use google libraries to work on the API.
	/// </summary>
	internal class GoogleContactsService : IContactService
	{
		/// <summary>
		/// For authorization, you must obtain ClientId and ClientSecret in advance for our application.
		/// We will use this to get a token. It is necessary to work on the API with Google.
		/// </summary>
		private const string ClientId = "1062889216556-s7mbmuqve920v01a0c48d4l9atfgde2b.apps.googleusercontent.com";
		private const string ClientSecret = "pgKMMKqoItVymM2M3XP0p3HL";

		private static PeopleServiceService _service;
		private List<Contact> _listContacts = new List<Contact>();
		private List<Label> _listLabels = new List<Label>();
		private IList<Person> _listPerson;
		private IList<ContactGroup> _listGroups;

		/// <summary>
		/// We go through authorization, get a token for working with GoogleContacts.
		/// </summary>
		public GoogleContactsService()
		{
			Authorize();
		}


		public void Authorize()
		{
			var userCredential = GetUserCredential();

			if (userCredential.Token.IsExpired(SystemClock.Default))
			{
				var refresh = userCredential.RefreshTokenAsync(CancellationToken.None).Result;
			}

			var service = new PeopleServiceService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = userCredential,
				ApplicationName = "NetTest",
			});

			_service = service;
		}

		public void LogOut()
		{
			if (_service != null)
			{
				var credential = GetUserCredential();

				if (credential.Token.IsExpired(SystemClock.Default))
				{
					var refresh = credential.RefreshTokenAsync(CancellationToken.None).Result;
				}
				var revoke = credential.RevokeTokenAsync(CancellationToken.None).Result;
			}

			_service = null;
		}

		public bool IsLoggedIn()
		{
			return _service != null;
		}

		/// <summary>
		/// Get a list of contacts and groups from google account.
		/// </summary>
		public List<Contact> GetAllContacts()
		{
			ReloadContacts();
			return _listContacts;
		}

		public string CreateContact(Contact contact)
		{
			if (!IsLoggedIn())
				return null;

			var person = new Person
			{
				Names = new List<Name>
				{
					new Name()
				}
			};
			person.Names[0].GivenName = contact.FirstName;
			person.Names[0].FamilyName = contact.Surname;
			person.Names[0].MiddleName = contact.MiddleName;

			person.EmailAddresses = new List<EmailAddress> { new EmailAddress() };
			person.EmailAddresses[0].Value = contact.EmailAddress;

			person.PhoneNumbers = new List<PhoneNumber> { new PhoneNumber() };
			person.PhoneNumbers[0].Value = contact.PhoneNumber;

			var peopleRequest = _service.People.CreateContact(person);
			person = peopleRequest.Execute();

			ReloadContacts();

			return person.ResourceName;
		}

		public bool DeleteContact(Contact contact)
		{
			if (!IsLoggedIn())
				return false;

			var person = _listPerson.FirstOrDefault(p => p.ResourceName == contact.Id);
			while (person == null)
			{
				ReloadContacts();
				person = _listPerson.FirstOrDefault(p => p.ResourceName == contact.Id);
				Thread.Sleep(1000);
			}

			try
			{
				var peopleRequest = _service.People.DeleteContact(person.ResourceName);
				peopleRequest.Execute();
			}
			catch (GoogleApiException ex)
			{
				Console.WriteLine(ex);

				// Contact was previously deleted from Google Contacts
				if (ex.Error.Code != 404)
					return false;
			}

			_listPerson.Remove(person);

			return true;
		}

		public bool UpdateContact(Contact contact)
		{
			if (!IsLoggedIn())
				return false;

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

		public string CreateLabel(Label label)
		{
			if (!IsLoggedIn())
				return null;

			var createContactGroupRequest = new CreateContactGroupRequest
			{
				ContactGroup = new ContactGroup
				{
					Name = label.Name
				}
			};

			var groupRequest = _service.ContactGroups.Create(createContactGroupRequest);
			var response = groupRequest.Execute();

			ReloadContacts();

			return response.ResourceName;
		}

		public bool DeleteLabel(Label label)
		{
			if (!IsLoggedIn())
				return false;

			var group = _listGroups.First(g => g.ResourceName == label.Id);

			try
			{
				var groupRequest = _service.ContactGroups.Delete(group.ResourceName);
				groupRequest.Execute();
			}
			catch (GoogleApiException ex)
			{
				Console.WriteLine(ex);

				// Contact was previously deleted from Google Contacts
				if (ex.Error.Code != 404)
					return false;
			}

			_listGroups.Remove(group);

			return true;
		}

		public bool UpdateLabel(Label label)
		{
			if (!IsLoggedIn())
				return false;

			var group = _listGroups.First(g => g.ResourceName == label.Id);
			group.Name = label.Name;
			var updateContactGroupRequest = new UpdateContactGroupRequest
			{
				ContactGroup = group
			};

			var groupRequest = _service.ContactGroups.Update(updateContactGroupRequest, group.ResourceName);
			groupRequest.Execute();

			ReloadContacts();

			return true;
		}

		public bool AddLabelToContact(Contact contact, Label label)
		{
			if (!IsLoggedIn())
				return false;

			var modifyContactGroupMembersRequest = new ModifyContactGroupMembersRequest
			{
				ResourceNamesToAdd = new List<string>
				{
					contact.Id
				}
			};
			var peopleRequest = _service.ContactGroups.Members.Modify(modifyContactGroupMembersRequest, label.Id);
			peopleRequest.Execute();

			ReloadContacts();

			return true;
		}

		public bool RemoveLabelFromContact(Contact contact, Label label)
		{
			if (!IsLoggedIn())
				return false;

			var modifyContactGroupMembersRequest = new ModifyContactGroupMembersRequest
			{
				ResourceNamesToRemove = new List<string>
				{
					contact.Id
				}
			};
			var peopleRequest = _service.ContactGroups.Members.Modify(modifyContactGroupMembersRequest, label.Id);
			peopleRequest.Execute();

			ReloadContacts();

			return true;
		}


		private static UserCredential GetUserCredential()
		{
			var userCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(
				new ClientSecrets { ClientId = ClientId, ClientSecret = ClientSecret },
				new[] { "profile", "https://www.google.com/m8/feeds/contacts/default/full" },
				"me",
				CancellationToken.None).Result;

			return userCredential;
		}

		private void ReloadContacts()
		{
			if (!IsLoggedIn())
				return;

			var groupRequest = _service.ContactGroups.List();
			var response = groupRequest.Execute();

			var listGroups = response.ContactGroups;
			_listGroups = listGroups.Where(g => !(g.ResourceName == "contactGroups/chatBuddies" ||
																		 g.ResourceName == "contactGroups/all" ||
																		 g.ResourceName == "contactGroups/friends" ||
																		 g.ResourceName == "contactGroups/family" ||
																		 g.ResourceName == "contactGroups/coworkers" ||
																		 g.ResourceName == "contactGroups/blocked"))
																		.ToList();

			var allGroups = _listGroups.Select(group => new Label
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

			_listContacts = new List<Contact>();
			if (personResponse.Connections == null)
				return;

			_listPerson = personResponse.Connections;
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

	}
}
