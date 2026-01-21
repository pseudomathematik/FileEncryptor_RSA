using Microsoft.Win32;
using System;
using System.Windows;

namespace WpfApp_Dialogs
{
	public class DefaultDialogService
	{
		public string FilePath { get; private set; }

		public bool OpenFileDialog()
		{
			try
			{
				OpenFileDialog dialog = new OpenFileDialog
				{
					Title = "Выберите файл",
					Filter = "All files (*.*)|*.*",
					Multiselect = false
				};

				if (dialog.ShowDialog() == true)
				{
					FilePath = dialog.FileName;
					return true;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(
					ex.Message,
					"Ошибка",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
			}

			return false;
		}
	}
}
