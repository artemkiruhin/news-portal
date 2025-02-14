using System.Security.Cryptography;
using System.Text;

namespace Backend.Core.Services.Security.Hash;

public class SHA256Hasher : IHasher
{
    public string Hash(string content)
    {
        if (string.IsNullOrEmpty(content))
            throw new ArgumentException("Строка для хеширования не может быть пустой", nameof(content));

        var data = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(data);
        return Convert.ToHexString(hash).ToLower();
    }
}