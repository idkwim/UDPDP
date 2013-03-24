﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;


namespace Decryptor
{
    public static class Decryptor
    {
        #region Decrypting DLL code.
                
        public static IDecryptor InitDecryptDll(string dll)
        {
            if (!File.Exists(dll))
            {
                Console.WriteLine("Unable to find {0}, are you sure the path is correct?", dll);
                return null;
            }
            if (IsManagedDll(dll))
            {
                return LoadManagedDll(dll);
            }
            return LoadUnmanagedDll(dll);
        }

        private static IDecryptor LoadManagedDll(string dll)
        {
            Assembly assembly = Assembly.LoadFile(dll);

            foreach (Type t in assembly.GetTypes())
            {
                if (typeof(IDecryptor).IsAssignableFrom(t))
                {

                    return Activator.CreateInstance(t) as IDecryptor;
                }
            }
            Console.WriteLine("Unable to get Decryptor type!");
            return null;
        }

        private static IDecryptor LoadUnmanagedDll(string dll)
        {
            UnManagedDecryptor umd = new UnManagedDecryptor();
            return (IDecryptor)umd;
        }

        private static bool IsManagedDll(string dll)
        {
            try
            {
                Assembly assembly = Assembly.LoadFile(dll);
                return true;
            }
            catch (BadImageFormatException)
            { }
            return false;
        }
        
        #endregion
    }
}