using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSDApp.com.rta.vsd.hh.encryption
{
   public interface IEncryptionDecryptionManager
    {
        byte[] Encrypt(string plainText, byte[] Key, byte[] IV);
        string Decrypt(byte[] cipherText, byte[] Key, byte[] IV);

    }
}
