using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ITVisions.Security
{

 public class HashResult
 {
  public byte[] Salt;
  public string HashedText;

 }

 public class Hashing
 {



  public static HashResult HashPassword(string password, byte[] salt = null)
  {

   var result = new HashResult();

   if (salt == null)
   {
    // generate a 128-bit salt using a secure PRNG
    salt = new byte[128 / 8];
    using (var rng = RandomNumberGenerator.Create())
    {
     rng.GetBytes(salt);
    }
   }

   result.Salt = salt;

   // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
   string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
       password: password,
       salt: salt,
       prf: KeyDerivationPrf.HMACSHA1,
       iterationCount: 10000,
       numBytesRequested: 256 / 8));

   result.HashedText = hashed;
   return result;
  }
 }
}
