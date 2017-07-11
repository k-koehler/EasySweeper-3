using System;

namespace EasyMap
{
    [Flags]
    public enum RoomType
    {
        NotOpened,
        N = 1,
        S = 2,
        E = 4,
        W = 8,
        Home = 16,
        Boss = 32
    }
}
