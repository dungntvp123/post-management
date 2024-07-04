using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace PRN_Project.Utils
{
	public class PasswordEncoder
	{
		public static string Encode(string password) => BCrypt.Net.BCrypt.HashPassword(password);

		public static bool Match(string password, string hash) => BCrypt.Net.BCrypt.Verify(password, hash); 
	}
}
