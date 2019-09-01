using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ipogonyshevNetTest.ViewModel
{
	public class LabelWindowViewModel : ViewModelBase, IDataErrorInfo
	{
		private readonly LabelViewModel _labelViewModel;
		private readonly List<LabelViewModel> _labelViewModels;
		private string _labelName;

		public LabelWindowViewModel(LabelViewModel labelViewModel, List<LabelViewModel> labelViewModels)
		{
			_labelViewModel = labelViewModel;
			_labelViewModels = labelViewModels;

			SaveCommand = new RelayCommand(Save, () => Validate() == null);

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

		/// <summary>
		/// Save new or changed label.
		/// </summary>
		public RelayCommand SaveCommand { get; set; }

		/// <summary>
		/// Validation: Processing input of invalid values.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public string this[string property] => Validate();

		public string Error { get; }


		private void Save()
		{
			_labelViewModel.Name = LabelName;
		}

		private string Validate()
		{
			var sameNameExists = _labelViewModels.Where(l => l.Id != _labelViewModel.Id)
												.Any(l => l.Name == LabelName);

			if (sameNameExists)
				return "Label name already exist";

			if (LabelName != null)
			{
				if (LabelName.Length >= 400)
					return "The label name you chosen is too long";
				
				if (LabelName.Length == 0)
					return "Label name cannot be empty";
			}

			return null;
		}
	}
}
