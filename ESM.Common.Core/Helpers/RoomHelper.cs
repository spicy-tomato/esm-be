namespace ESM.Common.Core.Helpers;

public static class RoomHelper
{
    public static string[] GetRoomsFromString(string? str)
    {
        if (str == null)
            return Array.Empty<string>();

        var arr = str.Split(",");
        for (var i = 0; i < arr.Length; i++)
        {
            var room = arr[i];
            room = room.Trim();
            if (room[0] == 'P' && char.IsDigit(room[1]))
                room = room[1..];
            arr[i] = room;
        }

        return arr;
    }
}