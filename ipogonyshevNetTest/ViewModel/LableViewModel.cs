using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.ViewModel
{
	public class LableViewModel : ViewModelBase
	{
		private readonly Lable _lable;
		private bool _isNew;
		private string _id;
		private string _name;

		public event EventHandler<EventArgs> OnDelete;

		public LableViewModel()
		{
			_lable = new Lable();
			IsNew = true;

			DeleteCommand = new RelayCommand(Delete, () => true);
		}

		public LableViewModel(Lable lable)
		{
			_lable = lable;
			Id = lable.Id;
			Name = lable.Name;
			IsNew = false;

			DeleteCommand = new RelayCommand(Delete, () => true);
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

		public RelayCommand DeleteCommand { get; set; }

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

		public void Delete()
		{
			OnDelete?.Invoke(this, null);
		}

		
	}
}
