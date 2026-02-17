using System.Numerics;
using System.Windows;
using System;
using WpfApp_Dialogs;
using WpfApp_ModInv;
using Encryptor_RSA;
using Encrypt_Info;

namespace WpfApp_Encrypt
	{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
		{
		private readonly DefaultDialogService _dialogService;
		public MainWindow()
			{
				InitializeComponent();
				_dialogService = new DefaultDialogService();
			}
		private void button_SelectFile_Click(object sender, RoutedEventArgs e)
			{
			if (_dialogService.OpenFileDialog())
				{
					textBox_selectFile.Text = _dialogService.FilePath;
				}
			}
        private void AddLog(string message)
        {
            Dispatcher.Invoke(() =>
            {
                textBox_Output.Text += message;
            });
        }
		private void button_Info_Click(object sender, RoutedEventArgs e)
		{
			Info.ShowInfo();
			return;
		}

        private void button_EncryptFile_Click(object sender, RoutedEventArgs e)
		{
				textBox_Output.Clear();	
				//параметры для шифрования(p,q,e)
				
				String p_s = textBox_p_val.Text;
				String q_s = textBox_q_val.Text;
				String e_s = textBox_e_val.Text;
				if(string.IsNullOrEmpty(textBox_p_val.Text) || (string.IsNullOrEmpty(textBox_q_val.Text)) || (string.IsNullOrEmpty(textBox_e_val.Text)))
				{
					MessageBox.Show("Введены не все параметры шифрования RSA!", "Предупреждение", MessageBoxButton.OK);
					return;
				}
				if (!ulong.TryParse(p_s, out ulong p_ul) || !ulong.TryParse(q_s, out ulong q_ul) || !ulong.TryParse(e_s, out ulong e_ul))
				{
					MessageBox.Show(
					 "Параметры RSA должны быть целыми положительными числами!",
					"Ошибка ввода",
					 MessageBoxButton.OK);
					return;

				}
				BigInteger p = BigInteger.Parse(p_s);
				BigInteger q = BigInteger.Parse(q_s);
				BigInteger e_Bigint = BigInteger.Parse(e_s);

				BigInteger n = p * q;
				BigInteger phi = (p - 1) * (q - 1);
				BigInteger d = ModInverse.ModInverse_Proc(e_Bigint, phi);
				
				textBox_n_val.Text = n.ToString();
				textBox_phi_val.Text = phi.ToString();
				textBox_d_val.Text = d.ToString();

				

				String file_name = textBox_selectFile.Text;
				if(string.IsNullOrEmpty(textBox_selectFile.Text))
				{
					MessageBox.Show("Файл для шифрования не выбран!", "Предупреждение", MessageBoxButton.OK);
					return;
				}
				else
				{
					textBox_Output.Text += "Параметры шифрования RSA:\n";
					textBox_Output.Text += "p = " + p_s + "\n";
					textBox_Output.Text += "q = " + q_s + "\n";
					textBox_Output.Text += "e = " + e_s + "\n";
					textBox_Output.Text += "n = " + n.ToString() + "\n";
					textBox_Output.Text += "φ = " + phi.ToString() + "\n";
					textBox_Output.Text += "d = " + d.ToString() + "\n";
					textBox_Output.Text += "Сформирована пара ключей.\n";
					textBox_Output.Text += $"Публичный ключ: ({e_s},{n.ToString()})\n";
					textBox_Output.Text += $"Закрытый ключ: ({d.ToString()},{n.ToString()})\n";
				}

			//шифрование
				File_Cryptor cryptor = new File_Cryptor(n, d, e_Bigint, AddLog);
				cryptor.CryptFile(file_name);

				
		}

	}
}
