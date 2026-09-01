using PurrNet.Packing;
using PurrNet.Utils;
using System;

public static class RegisterType
{
    public static void RegisterEnum<T>() where T : Enum
    {
        Hasher.PrepareType<T>();
        Packer<T>.RegisterWriter(Write);
        Packer<T>.RegisterReader(Read);
    }

    private static void Write<T>(BitPacker packer, T value) where T : Enum
    {
        packer.Write(Convert.ToInt32(value));
    }

    private static void Read<T>(BitPacker packer, ref T value) where T : Enum
    {
        int rawValue = 0;
        packer.Read(ref rawValue);
        value = (T)Enum.ToObject(typeof(T), rawValue);
    }
}