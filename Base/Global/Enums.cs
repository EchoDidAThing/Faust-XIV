using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

        public enum LightType : byte
        {
            Mount = 0,
            Equipment = 1,
            Weapon = 2,
            Companion = 3,
            FollowMount = 4,
            Ornament = 5,
            BgObject = 6
        }
    }
}
