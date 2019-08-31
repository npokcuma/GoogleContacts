using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using ipogonyshevNetTest.Model;

namespace ipogonyshevNetTest.ViewModel
{
	public class LabelViewModel : ViewModelBase
	{
		private readonly Label _label;
		private bool _isNew;
		private string _id;
		private string _name;


		public LabelViewModel()
		{
			_label = new Label();
			Id = _label.Id;
			IsNew = true;



			DeleteCommand = new RelayCommand(Delete, () => true);
			EditCommand=new RelayCommand(Edit,()=>true);
		}

		public LabelViewModel(Label label)
		{
			_label = label;
			Id = label.Id;
			Name = label.Name;
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
				isDirty = isDirty || Name != _label.Name;

				return isDirty;
			}
		}

		public ObservableCollection<ContactViewModel> Contacts { get; set; } = new ObservableCollection<ContactViewModel>();

		public RelayCommand DeleteCommand { get; set; }
		public RelayCommand EditCommand { get; set; }

		public Label Entity => _label;


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
			_label.Id = Id;
			_label.Name = Name;
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
