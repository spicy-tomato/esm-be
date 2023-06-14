namespace ESM.Application.Common.Helpers;

public static class StringHelper
{
    public static string RandomEmail() => new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
       .Select(s => s[RandomHelper.Next(s.Length)]).ToArray()) + "@com";
}