using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;



namespace Encryptor_RSA

{
    public class File_Cryptor
    {
        public readonly BigInteger n;
        public readonly BigInteger d;
        public readonly BigInteger e_Bigint;
        private readonly Action<string> log; //  делегат вывода
        public File_Cryptor(BigInteger n, BigInteger d, BigInteger e_Bigint, Action<string> log)
        {
            this.n = n;
            this.d = d;
            this.e_Bigint = e_Bigint;
            this.log = log;
        }

        public void CryptFile(string file_name)
        {
            byte[] data = File.ReadAllBytes(file_name);

            byte[] nBytes = n.ToByteArray();
            
            //длина  n в байтах.если последний байт 0,он отбрасывается
            int keySizeInBytes = nBytes[nBytes.Length - 1] == 0 ? nBytes.Length - 1 : nBytes.Length;
            //размер блока данных
            int blockSize = keySizeInBytes - 1; // -1 для padding 
            List<BigInteger> encryptedBlocks = new List<BigInteger>();

            for (int offset = 0; offset < data.Length; offset += blockSize)
            {
                int currentBlockSize = Math.Min(blockSize, data.Length - offset);//размер реальных данных в блоке
                byte[] block = new byte[blockSize]; 

                // padding: заполняем блок нулями, потом копируем данные в конец блока
                Array.Copy(data, offset, block, blockSize - currentBlockSize, currentBlockSize);
                //перевод в Little-endian(кодировка)
                byte[] blockBE = new byte[block.Length];
                Array.Copy(block, blockBE, block.Length);
                Array.Reverse(blockBE); 
                //добавляем нулевой байт для положит.числа
                byte[] blockWithSign = new byte[blockBE.Length + 1];
                Array.Copy(blockBE, 0, blockWithSign, 0, blockBE.Length);
                //блок в число BigInteger
                BigInteger m = new BigInteger(blockWithSign);
                //RSA шифрование блока
                BigInteger c = BigInteger.ModPow(m, e_Bigint, n);
                // добавление его к остальным
                encryptedBlocks.Add(c);
            }

            using (BinaryWriter bw = new BinaryWriter(File.Open(file_name + ".enc", FileMode.Create)))
            {
                //кол-во блоков
                bw.Write(encryptedBlocks.Count);
                //исходный размер файла
                bw.Write(data.Length);
                //поблочная запись  зашифрованных данных в файл
                foreach (var block in encryptedBlocks)
                {
                    byte[] bytes = block.ToByteArray();
                    bw.Write(bytes.Length);
                    bw.Write(bytes);
                }
            }
            log($"Файл {file_name} зашифрован.Находится в директории с исходным файлом.");
            return;
        }

    }
}
