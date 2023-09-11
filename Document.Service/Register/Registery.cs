using Document.Service.Exceptions;
using Microsoft.Win32;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Document.Service.Register;

public class Registery
{
    private class RegisterFieldItem
    {
        public RegistryHive Root { get; set; }

        public RegistryView View { get; set; }

        public string SubKey { get; set; }

        public string Key { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            try
            {
                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(GetType());
                MemoryStream memoryStream = new MemoryStream();
                dataContractJsonSerializer.WriteObject((Stream)memoryStream, (object?)this);
                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
            catch (Exception innerException)
            {
                string text = "Can not serialise object " + GetType().Name + "!";
                return text;
            }
        }
    }

    private readonly RegisterFieldItem[] Items = new RegisterFieldItem[0];

    public void Run()
    {
        RegisterFieldItem[] items = Items;
        foreach (RegisterFieldItem registerFieldItem in items)
        {
            try
            {
                RegistryKey registryKey = RegistryKey.OpenBaseKey(registerFieldItem.Root, registerFieldItem.View);
                string[] array = registerFieldItem.SubKey.Split('\\');
                foreach (string text in array)
                {
                    registryKey = registryKey.OpenSubKey(text, writable: true);
                    if (registryKey == null)
                    {
                        registryKey = registryKey.CreateSubKey(text, RegistryKeyPermissionCheck.ReadWriteSubTree);
                    }
                }

                registryKey.SetValue(registerFieldItem.Key, registerFieldItem.Value);
            }
            catch (Exception ex)
            {
                throw new ExceptionRegisterError($"Can not register Key {registerFieldItem}",ex);
            }
        }

        Console.WriteLine("");
    }
}