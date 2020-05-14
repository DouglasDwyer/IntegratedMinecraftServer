using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace IMS_Library
{
    public static class Encryption
    {
        private static SHA512 EncryptionProvider = SHA512.Create();

        /// <summary>
        /// Retrieves a securely-generated list of bytes for use in cryptographic operations.
        /// </summary>
        /// <param name="size">The size of the byte array to return.</param>
        /// <returns></returns>
        public static byte[] GetRandomBytes(byte size)
        {
            byte[] randValues = new byte[size];
            new RNGCryptoServiceProvider().GetBytes(randValues);
            return randValues;
        }

        /// <summary>
        /// Hashes a series of bytes using the SHA512 encryption algorithm.
        /// </summary>
        /// <param name="toEncrypt">The series of bytes to encrypt.</param>
        /// <returns></returns>
        public static byte[] HashBytes(byte[] toEncrypt)
        {
            return EncryptionProvider.ComputeHash(toEncrypt);
        }

        /// <summary>
        /// Hashes a series of bytes, appended with a nonce, using the SHA512 encryption algorithm.
        /// </summary>
        /// <param name="toEncrypt">The series of bytes to encrypt.</param>
        /// <param name="nonce">The random sequence to append.</param>
        /// <returns></returns>
        public static byte[] HashBytes(byte[] toEncrypt, byte[] nonce)
        {
            List<byte> encrypted = new List<byte>(toEncrypt);
            encrypted.AddRange(nonce);
            return HashBytes(encrypted.ToArray());
        }

        /// <summary>
        /// Generates an asymmetric keypair for encryption.
        /// </summary>
        /// <returns>A Tuple which contains the public encryption information, and then the private decryption information.</returns>
        public static Tuple<RSAParameters, RSAParameters> GenerateAsymmetricKeyPair()
        {
            RSAParameters toReturnPublic = default(RSAParameters);
            RSAParameters toReturnPrivate = default(RSAParameters);
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                toReturnPublic = provider.ExportParameters(false);
                toReturnPrivate = provider.ExportParameters(true);
            }
            return new Tuple<RSAParameters, RSAParameters>(toReturnPublic, toReturnPrivate);
        }

        /// <summary>
        /// Decrypts a series of bytes using a key.
        /// </summary>
        /// <param name="data">The byte array to decrypt.</param>
        /// <param name="key">The key to use in decryption.</param>
        /// <returns>The decrypted byte array.</returns>
        public static byte[] DecryptBytesAsymmetric(byte[] data, RSAParameters key)
        {
            using(RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.ImportParameters(key);
                return provider.Decrypt(data, false);
            }
        }

        /// <summary>
        /// Encrypts a series of bytes using a key.
        /// </summary>
        /// <param name="data">The data to encrypt.</param>
        /// <param name="key">The encryption key to use.</param>
        /// <returns>The encrypted byte array.</returns>
        public static byte[] EncryptBytesAsymmetric(byte[] data, RSAParameters key)
        {
            using (RSACryptoServiceProvider provider = new RSACryptoServiceProvider())
            {
                provider.ImportParameters(key);
                return provider.Encrypt(data, false);
            }
        }
    }
}