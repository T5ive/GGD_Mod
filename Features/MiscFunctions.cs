﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GGD_Hack.Features
{
    public class MiscFunctions
    {
        /// <summary>
        /// 控制连接殖民地中的飞船启动
        /// </summary>
        public static void MoveShuttle()
        {
            SendEventToPlugin.MOVE_SHUTTLE();
        }
    }
}

public static class SendEventToPlugin
{
    //控制连接殖民地中的飞船启动
    public static void MOVE_SHUTTLE()
    {
        APIs.Photon.PhotonEventAPI.SendEventToPlugin(30, null, false);
    }

    public static void SPECIAL_KILL(int type)
    {
        string[] killedType = new string[1];
        killedType[0] = type.ToString();

        APIs.Photon.PhotonEventAPI.SendEventToPlugin(12, fromStringArray(killedType, 1), false);
    }

    public static void RECEIVE_KILL()
    {
        //TODO: 无效
        //string[] killedType = new string[1];
        //killedType[0] = type.ToString();

        APIs.Photon.PhotonEventAPI.SendEventToPlugin(3, null, false);
    }

    public static void MorphIntoMummy()
    {
        APIs.Photon.PhotonEventAPI.SendEventToPlugin(82, null, false);
    }

    public static Il2CppSystem.Object fromStringArray(System.String[] stringArr, int size)
    {
        UnhollowerBaseLib.Il2CppStringArray strs = new UnhollowerBaseLib.Il2CppStringArray(size);

        for (int i = 0; i < size; i++)
        {
            strs[i] = stringArr[i];
        }

        Il2CppSystem.Object obj = new Il2CppSystem.Object(strs.Pointer);

        return obj;
    }
}