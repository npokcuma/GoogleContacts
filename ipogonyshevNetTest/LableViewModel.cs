using GalaSoft.MvvmLight;

namespace ipogonyshevNetTest
{
	class LableViewModel : ViewModelBase
	{
		private readonly Lable _lable;
		private bool _isNew;
		private string _id;
		private string _name;
		private string _phoneNumber;
		private string _emailAddress;

		public LableViewModel()
		{
			_lable = new Lable();
			IsNew = true;
		}

		public LableViewModel(Lable lable)
		{
			_lable = lable;
			Id = lable.Id;
			Name = lable.Name;
			IsNew = false;
		}

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


		public bool IsNew
		{
			get => _isNew;
			set
			{
				Set(() => IsNew, ref _isNew, value);
				RaisePropertyChanged(nameof(IsDirty));
			}
		}

		public bool IsDirty
		{
			get
			{
				bool isDirty = IsNew;
				isDirty = isDirty || Name != _lable.Name;

				return isDirty;
			}
		}


		public Lable GetLable()
		{
			var lable = new Lable
			{
				Id = Id,
				Name = Name,
			};
			return lable;
		}

		public void Save()
		{
			_lable.Id = Id;
			_lable.Name = Name;


			IsNew = false;
		}
	}
}

