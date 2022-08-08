using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using WebApiAutores.DTOs;

namespace WebApiAutores.Servicios
{
    public class HashService
    {
        public ResultadoHash Hash(string textoPlano)
        {
            var sal =new byte[16];
            RSA rsa = RSA.Create();
            using(var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(sal);
            }

            return Hash(textoPlano, sal);
        }

        private ResultadoHash Hash(string textoPlano, byte[] sal)
        {
            var llaveDerivada = KeyDerivation.Pbkdf2(
                textoPlano,
                salt: sal, 
                prf: KeyDerivationPrf.HMACSHA1, 
                iterationCount: 10000, 
                numBytesRequested: 32);

            var hash = Convert.ToBase64String(llaveDerivada);

            return new ResultadoHash
            {
                Hash = hash,
                Sal = sal
            };
        }
    }
}
