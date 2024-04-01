// RansomYard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// RansomYard.RansomYardStart
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;

[StandardModule]
internal sealed class RansomYardStart
{
	[STAThread]
	public static void main()
	{
        // I moved all encrypted file to the Desktop that why I got rid of the other paths
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
		// string path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\";
		// string path3 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\";
		// string path4 = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
		// string path5 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\";
		// RegistryKey currentUser = Registry.CurrentUser;
		// string[] subKeyNames = currentUser.GetSubKeyNames();
		// currentUser.OpenSubKey(subKeyNames[3]);

        // I hardcodded the registries keys and values
		string[] array = new string[3] { "Path", "TEMP", "TMP" };
		string[] array2 = new string[3] { "C:\\Users\\Admin\\AppData\\Local\\Microsoft\\WindowsApps;", "C:\\Users\\Admin\\AppData\\Local\\Temp", "C:\\Users\\Admin\\AppData\\Local\\Temp" };
		
        string text = "{";
		string text2 = "";
		string[] array3 = array;
		foreach (string text3 in array3)
		{
			text = text + GetHash(text3) + "-";
			text2 = text2 + text3 + "$";
		}
		int num = 0;
		string[] array4 = text2.Split('$');
		for (int j = 0; j < array4.Length; j++)
		{
			if (array4[j].Length > 0)
			{
				text = text + GetHash(Conversions.ToString(array2[num])) + "-";
				num++;
			}
		}
		text += "}";
		Encryptor(path, text + "1");
		Encryptor(path, text + "2");
		Encryptor(path, text + "3");
		Encryptor(path, text + "4");
		Encryptor(path, text + "5");
	}

    // Nothing changed here
	public static string GetHash(string theInput)
	{
		checked
		{
			using MD5 mD = MD5.Create();
			byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(theInput));
			StringBuilder stringBuilder = new StringBuilder();
			int num = array.Length - 1;
			for (int i = 0; i <= num; i++)
			{
				stringBuilder.Append(array[i].ToString("X2"));
			}
			return stringBuilder.ToString();
		}
	}

    // Nothing changed here
	public static string[] GetFiles(string path)
	{
		return Directory.GetFiles(path);
	}

    // We edited this function to get the before encryption names
	public static void WriteFile(string path, string data)
	{
		int num = path.LastIndexOf(".");
		string path2 = "";
		if (num >= 0)
		{
			path2 = path.Substring(0, num);
		}
		File.WriteAllText(path2, data);
	}

	public static void Encryptor(string path, string key)
	{
		try
		{
			string[] files = GetFiles(path);
			foreach (string text in files)
			{
                // self-explanatory
				if (text.EndsWith(".flagy"))
				{
					byte[] array = Encrypt(File.ReadAllBytes(text), key);
					if (array != null)
					{
						WriteFile(text, array);
					}
				}
			}
		}
		catch (Exception)
		{
		}
	}

    // Well we changed this function to decrypt instead of encrypt :D
	public static byte[] Encrypt(byte[] bytData, string strPass)
	{
		// To take rid of flagy{
        byte[] array = new byte[bytData.Length - 6];
		Array.Copy(bytData, 6, array, 0, array.Length);
		string @string = Encoding.UTF8.GetString(array);
		byte[] array2 = new byte[@string.Length / 2];

		for (int i = 0; i < @string.Length; i += 2)
		{
            // self-explanatory
			array2[i / 2] = Convert.ToByte(@string.Substring(i, 2), 16);
		}
		try
		{
			using RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.KeySize = 256;
			rijndaelManaged.Key = GeKey(strPass);
			rijndaelManaged.IV = GetIV(strPass);
			using MemoryStream stream = new MemoryStream(array2);
            // decrypt instead of encrypt
			using CryptoStream cryptoStream = new CryptoStream(stream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Read);
			using MemoryStream memoryStream = new MemoryStream();
			cryptoStream.CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return null;
		}
	}

    // Nothing changed
	private static byte[] GeKey(string strPass)
	{
		byte[] salt = new byte[8] { 3, 1, 3, 3, 3, 3, 3, 7 };
		byte[] password;
		using (SHA256Managed sHA256Managed = new SHA256Managed())
		{
			string text = Convert.ToBase64String(sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(strPass)));
			string s = strPass + text;
			password = sHA256Managed.ComputeHash(Encoding.UTF8.GetBytes(s));
			sHA256Managed.Clear();
		}
		return new Rfc2898DeriveBytes(password, salt, 1000).GetBytes(32);
	}

    // Nothing changed
	private static byte[] GetIV(string strPass)
	{
		_ = new byte[8] { 3, 1, 3, 3, 3, 3, 3, 7 };
		using MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
		string text = Convert.ToBase64String(mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(strPass)));
		string s = strPass + text;
		byte[] result = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(s));
		mD5CryptoServiceProvider.Clear();
		return result;
	}

    // We didn't use it
	public static string My_th(byte[] ba)
	{
		StringBuilder stringBuilder = new StringBuilder(checked(ba.Length * 2 + 7));
		stringBuilder.Append("flagy{");
		foreach (byte b in ba)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}

	public static void WriteFile(string path, byte[] data)
	{
		int num = path.LastIndexOf(".");
		string path2 = "";
		if (num >= 0)
		{
			path2 = path.Substring(0, num);
		}
		File.WriteAllBytes(path2, data);
	}
}
