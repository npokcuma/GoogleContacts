using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace ipogonyshevNetTest.ViewModel
{
	public class LabelWindowViewModel : ViewModelBase, IDataErrorInfo
	{
		private LabelViewModel _labelViewModel;
		private List<LabelViewModel> _labelViewModels;
		private string _labelName;

		public LabelWindowViewModel(LabelViewModel labelViewModel, List<LabelViewModel> labelViewModels)
		{
			_labelViewModel = labelViewModel;
			_labelViewModels = labelViewModels;

			SaveCommand = new RelayCommand(Save, IsExecuteCommand);

			LabelName = labelViewModel.Name;
		}

		public string Title { get; set; }

		public string LabelName
		{
			get => _labelName;
			set
			{
				Set(() => LabelName, ref _labelName, value);
				SaveCommand.RaiseCanExecuteChanged();
			}
		}

		public RelayCommand SaveCommand { get; set; }


		public string this[string property]
		{
			get
			{
				if (!IsValid())
				{
					return "Label name already exist";
				}

				if (!IsValidNameLength())
				{
					return "The label name you chosen is too long";

				}

				return null;
			}
		}



		public string Error { get; }

		private void Save()
		{
			_labelViewModel.Name = LabelName;
		}

		private bool IsValid()
		{
			var result = _labelViewModels.Where(l => l.Id != _labelViewModel.Id).All(l => l.Name != LabelName);
			return result;
		}
		private bool IsValidNameLength()
		{
			if (LabelName != null)
			{
				var result = LabelName.Length <= 400;
				return result;
			}

			return true;
		}

		private bool IsExecuteCommand()
		{
			if (IsValid() && IsValidNameLength())
			{
				return true;
			}

			return false;
		}
	}
}
