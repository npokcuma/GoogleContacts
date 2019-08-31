using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.ViewModel
{
	public class LabelViewModel : ViewModelBase
	{
		private bool _isNew;
		private string _id;
		private string _name;


		public LabelViewModel()
		{
			Entity = new Label();
			Id = Entity.Id;
			IsNew = true;

			DeleteCommand = new RelayCommand(Delete, () => true);
			EditCommand=new RelayCommand(Edit,()=>true);
		}

		public LabelViewModel(Label label)
		{
			Entity = label;
			Id = label.Id;
			Name = label.Name;
			IsNew = false;

			DeleteCommand = new RelayCommand(Delete, () => true);
			EditCommand = new RelayCommand(Edit, () => true);
		}


		public event EventHandler<EventArgs> OnDelete;

		public event EventHandler<EventArgs> OnEdit;


		public Label Entity { get; }

		public ObservableCollection<ContactViewModel> Contacts { get; set; } = new ObservableCollection<ContactViewModel>();

		public string Id
		{
			get => _id;
			set
			{
				Set(() => Id, ref _id, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				Set(() => Name, ref _name, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public bool IsDirty
		{
			get
			{
				bool isDirty = IsNew;
				isDirty = isDirty || Name != Entity.Name;

				return isDirty;
			}
		}

		public RelayCommand DeleteCommand { get; set; }

		public RelayCommand EditCommand { get; set; }

		private bool IsNew
		{
			get => _isNew;
			set
			{
				Set(() => IsNew, ref _isNew, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}



		public Label GetLabel()
		{
			var label = new Label
			{
				Id = Id,
				Name = Name,
			};
			return label;
		}

		public void Save()
		{
			Entity.Id = Id;
			Entity.Name = Name;
			IsNew = false;
		}

		private void Delete()
		{
			OnDelete?.Invoke(this, null);
		}

		private void Edit()
		{
			OnEdit?.Invoke(this, null);
		}
	}
}
