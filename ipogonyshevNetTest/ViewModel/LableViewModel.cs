using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
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
		private Brush _background;




		public LableViewModel()
		{
			_lable = new Lable();
			Id = _lable.Id;
			IsNew = true;
			Background = new SolidColorBrush(Colors.LightGreen);


			DeleteCommand = new RelayCommand(Delete, () => true);
			EditCommand=new RelayCommand(Edit,()=>true);
		}

		public LableViewModel(Lable lable)
		{
			_lable = lable;
			Id = lable.Id;
			Name = lable.Name;
			IsNew = false;

			DeleteCommand = new RelayCommand(Delete, () => true);
			EditCommand = new RelayCommand(Edit, () => true);
		}

		public event EventHandler<EventArgs> OnDelete;
		public event EventHandler<EventArgs> OnEdit;

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

		public ObservableCollection<ContactViewModel> Contacts { get; set; } = new ObservableCollection<ContactViewModel>();

		public RelayCommand DeleteCommand { get; set; }
		public RelayCommand EditCommand { get; set; }

		public Lable Entity => _lable;

		public Brush Background
		{
			get => _background;
			set
			{
				Set(() => Background, ref _background, value);
				RaisePropertyChanged(nameof(IsDirty));
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
			Background = new SolidColorBrush(Colors.White);
			IsNew = false;
		}

		public void Delete()
		{
			OnDelete?.Invoke(this, null);
		}

		public void Edit()
		{
			OnEdit?.Invoke(this, null);
		}

	}
}
