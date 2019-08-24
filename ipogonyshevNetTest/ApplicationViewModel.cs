using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System.Threading;

namespace ipogonyshevNetTest
{
    class ApplicationViewModel : INotifyPropertyChanged
    {
        private ContactsData selectedContact;

        public ObservableCollection<ContactsData> ContactsList { get; set; }
        public ContactsData SelectedContact
        {
            get { return selectedContact; }
            set
            {
                selectedContact = value;
                OnPropertyChanged("SelectedContact");
            }
        }

        static PeopleServiceService service = Authorization.GetToken();
        List<GroupData> groupNames = GetGroupList.Getlist(service);
        List<ContactsData> contacts = GetContactsList.GetList(service);


        public ApplicationViewModel()
        {
            ContactsList = new ObservableCollection<ContactsData>();
            foreach (var person in contacts)
            {
                ContactsList.Add(new ContactsData() {Name=person.Name,PhoneNumber=person.PhoneNumber,EmailAddress=person.EmailAddress });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
