using System;
using System.Collections.Generic;
using System.Text;

namespace Faust.Base.Global
{
    public class Enums
    {
        public enum worlds : uint
        {
            Dev = 0,
            Marilith = 404,
            Halicarnassus = 406
        }

        public enum TiltOrigin : byte
        {
            Ground = 0,
            Center = 1,
            Waist = 2,
            UpperBody = 3,
        }
    }
}
