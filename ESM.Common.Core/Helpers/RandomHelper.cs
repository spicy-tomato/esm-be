using System.Security.Cryptography;

namespace ESM.Common.Core.Helpers;

public static class RandomHelper
{
    public static int Next(int maxValue = int.MaxValue)
    {
        var random = RandomNumberGenerator.Create();
        var data = new byte[4];

        random.GetBytes(data);
        var convertedRandomNumber = BitConverter.ToUInt16(data);

        return convertedRandomNumber % maxValue;
    }
}