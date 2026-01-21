using System.Numerics;
using System.Windows;
using System;
using System.IO;
using WpfApp_Dialogs;
using WpfApp_ModInv;

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

		private void button_EncryptFile_Click(object sender, RoutedEventArgs e)
			{
				//параметры для шифрования(p,q,e)
				String p_s = textBox_p_val.Text;
				String q_s = textBox_q_val.Text;
				String e_s = textBox_e_val.Text;
				if(string.IsNullOrEmpty(textBox_p_val.Text) || (string.IsNullOrEmpty(textBox_q_val.Text)) || (string.IsNullOrEmpty(textBox_e_val.Text)))
				{
					MessageBox.Show("Введены не все параметры шифрования RSA!", "Предупреждение", MessageBoxButton.OK);
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
				byte[] data = File.ReadAllBytes(file_name);
				textBox_Output.Text += ($"\nЗагружено {data.Length} байт.\n");
				BigInteger[] encrypted = new BigInteger[data.Length];

				for (int i = 0; i < data.Length; i++)
					encrypted[i] = BigInteger.ModPow(data[i], e_Bigint, n);

				using (BinaryWriter bw = new BinaryWriter(File.Open(String.Concat(file_name, ".enc"), FileMode.Create)))
				{
				foreach (var value in encrypted)
					{
						byte[] bytes = value.ToByteArray();
						bw.Write(bytes.Length);
						bw.Write(bytes);
					}
				}
				textBox_Output.Text += ($"\nФайл {file_name}  успешно зашифрован.");
				textBox_Output.Text += "Находится в папке с исходным файлом.";
		}

	}
}
