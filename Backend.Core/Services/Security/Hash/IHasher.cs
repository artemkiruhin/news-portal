namespace Backend.Core.Services.Security.Hash;

public interface IHasher
{
    string Hash(string content);
}