﻿using APIs.Photon;
using HarmonyLib;
using Il2CppSystem;
using MelonLoader;
using System;
using System.Diagnostics;
using Enum = System.Enum;

//发送给服务器调用的api为Photon_Realtime_LoadBalancingClient__OpRaiseEvent ->
//      //向同一房间的其他玩家发送带有自定义代码/类型和任何内容的事件
//      bool __stdcall Photon_Realtime_LoadBalancingPeer__OpRaiseEvent(
//        Photon_Realtime_LoadBalancingPeer_o * this,
//        uint8_t eventCode,
//        Il2CppObject *customEventContent,
//        Photon_Realtime_RaiseEventOptions_o *raiseEventOptions,
//        ExitGames_Client_Photon_SendOptions_o sendOptions,
//        const MethodInfo *method)
//高一级有SendEventToPlugin

namespace GGD_Hack.Hook
{
    public class PhotonEventAPI_
    {
        /// <summary>
        /// 游戏开始
        /// </summary>
        [HarmonyPatch(typeof(PhotonEventAPI), nameof(PhotonEventAPI.OnEvent), typeof(ExitGames.Client.Photon.EventData))]
        class OnEvent_
        {
            static bool Prefix(ExitGames.Client.Photon.EventData __0)
            {
#if Developer
                bool shouldBlockEvent = false;
                int code = __0.Code;

                string eventName = "";

                //获取枚举的名字
                if (System.Enum.IsDefined(typeof(EventDataCodeEnum), code))
                {
                    eventName = Enum.GetName(typeof(EventDataCodeEnum), code);
                }

                //pass的事件
                switch (code)
                {
                    case (int)EventDataCodeEnum.UnreliableRead:
                    case (int)EventDataCodeEnum.ReliableRead:
                        return true;
                }

                MelonLogger.Msg("接收到事件: " + eventName);
                MelonLogger.Msg(__0.ToStringFull());

                //屏蔽事件
                switch (code)
                {   //反作弊
                    case (int)EventDataCodeEnum.AntiCheat:
                    case (int)EventDataCodeEnum.PropertiesChanged:
                        //测试
                        //case 226:
                        shouldBlockEvent = true;
                        break;
                }

                //打印追踪栈
                switch (code)
                {
                    //case (int)EventDataCodeEnum.PropertiesChanged:
                    case 666666:
                        StackTrace stackTrace = new StackTrace();
                        string stackTraceString = stackTrace.ToString();
                        MelonLogger.Warning(stackTraceString);
                        break;
                    default:
                        break;
                }


                //开始屏蔽事件
                if (shouldBlockEvent)
                {
                    MelonLogger.Warning("已屏蔽事件: " + eventName + '\n');
                    return false;
                }
                else
                {
                    return true;
                }
#else
                    return true;
#endif
            }
        }

        public enum EventDataCodeEnum // TypeDefIndex: 748
        {
            //enum1
            PLUGIN_MESSAGE = 0,
            COMPLETE_TASK = 1,
            EMERGENCY = 2,
            RECEIVE_KILL = 3,
            REPORT = 4,
            VOTE = 5,
            SABOTAGE = 6,
            VENT = 7,
            EXIT_VENT = 8,
            CONNECT_TO_TASK = 9,
            ASSIGN_TASK = 10,
            LOOK_AT_CAMERAS = 11,
            SPECIAL_KILL = 12,
            EVALUATE_IMAGES = 13,
            UNASSIGN_TASK = 14,
            BROADCAST = 15,
            FART = 16,
            KICK_PLAYER = 17,
            DECREASE_TIMER = 18,
            EAT = 19,
            SEND_KILL = 20,
            MORPH = 21,
            GAME_ENDED = 22,
            IS_CHATTING = 23,
            SILENCE = 24,
            LOVER_SUICIDE = 25,
            VOTE_STATE_CHANGE = 26,
            NOTIFY_BEHAVIOR_VOICE_BAN = 27,
            TELEPORT = 28,
            DECON = 29,
            MOVE_SHUTTLE = 30,
            DAMAGE_TELEPORTER = 31,
            INFECT = 32,
            RECEIVE_VOTE = 33,
            ASSASSINATE = 34,
            PROCEEDING_VOTE_STATE = 35,
            SPAWN_KEYS = 36,
            PICK_UP_KEY = 37,
            KNOCK_DOOR = 38,
            EXIT_DOOR = 39,
            SPECTATOR_INFO = 40,
            SPECTATOR_STATE = 41,
            SYNC_LOUNGE_MUSIC = 42,
            OPEN_DOOR = 43,
            CLOSE_DOOR = 44,
            TOGGLE_DND = 45,
            HELIUM = 46,
            WHISTLEBLOW_BOMB = 47,
            THROW_BOMB = 48,
            EXPLODE_BOMB = 49,
            SETTINGS_UPDATE = 50,
            FEED_GOD = 51,
            PLAYER_PROPERTIES_UPDATE = 52,
            LIGHT_TORCH = 53,
            SET_GOD = 54,
            DROP_BRIDGE = 55,
            CREATE_FOOD = 56,
            PICK_UP_FOOD = 57,
            TASK_COMPLETED = 58,
            GRAB_BODY = 59,
            UPDATE_TASKBAR = 60,
            DROP_BODY = 61,
            GENERATE_BOMB = 62,
            REQUEST_SYNC_LOUNGE_MUSIC = 63,
            INVESTIGATE = 64,
            REVEAL_ROLE = 65,
            CHAT_MESSAGE = 66,
            USED_ROLE_SPECIAL = 67,
            MORTICIAN_ABILITY = 68,
            ENABLE_BOUNTY_ICON = 69,
            LAST_PEASANT = 70,
            DUELING_DODO_REVEAL = 71,
            ENABLE_ROLE_BUTTON = 72,
            CELEBRITY_DIED = 73,
            SPAWN_SACRIFICE_BELL = 74,
            PICK_UP_SACRIFICE_BELL = 75,
            BASEMENT_TELEPORT_EFFECT = 76,
            TURN_INVISIBLE = 77,
            PELICAN_EAT = 78,
            PELICAN_KILL = 79,
            PELICAN_RELEASE = 80,
            MEETING_TURN_THRALL = 81,
            MORPH_INTO_MUMMY = 82,
            LATCH_ONTO_VILLAGER = 83,
            SEND_MONSTER = 84,
            SERIAL_KILLER_TARGET = 85,
            SERIAL_KILLER_SUCCESS = 86,
            DRAFT_SEND_ORDER = 87,
            DRAFT_CHOOSE = 88,
            DRAFT_TURN = 89,
            DRAFT_DISCONNECT = 90,
            DRAFT_FAIL = 91,
            DRAFT_DATA = 92,
            GGD_LOBBY_GAME_EVENT = 93,
            GGD_LOBBY_GAME_LIST = 94,
            GGD_LOBBY_GAME_CREATE = 95,
            GGD_LOBBY_GAME_DESTROY = 96,
            GGD_LOBBY_GAME_JOIN = 97,
            GGD_LOBBY_GAME_LEAVE = 98,
            SERVER_SEND_ROLE = 99,
            CLIENT_RECEIVE_ROLE = 100,
            KICK_PLAYER_AFK = 101,
            START_GAME = 102,
            SERVER_ABORT_GAME = 103,
            SEND_DEBUG_SETTINGS = 104,
            TASK_ACHIEVEMENT = 105,
            GAINED_ACHIEVEMENT = 106,
            GAME_SETTINGS = 107,
            CHANGE_MAP = 108,
            LOUNGE_JSON_VALIDATE = 109,
            LOUNGE_JSON_PROCESS = 110,
            TRANSFER_MASTER_RECIEVE = 111,
            TRANSFER_MASTER_RESPONSE = 112,
            REGISTER_ACK = 113,
            GENERATE_RANDOM_SPAWN = 114,
            SET_IDENTITY = 115,
            SET_TARGET = 116,
            WRONG_KILL = 117,
            SET_VIP = 118,
            DUELING_TO_DODO = 119,
            FORCE_EXIT_VENT = 120,
            MAKE_ADMIN = 121,
            ADMIN_VOICE_BAN = 122,
            ADMIN_SUPER_BAN = 123,
            SET_IN_ENDING_SCREEN = 124,
            RESET_MID_SPECTATORS = 125,
            RESEND_RECONNECTION_DATA = 126,
            ASSIGN_TASK_TIMER = 127,
            NOTIFY_MOD_VOICE_BAN = 128,
            SET_SANDSTORM_ACTIVE = 129,
            SPAWN_MUMMY = 130,
            MUMMY_MOVEMENT = 131,
            LOCUST_START = 132,
            TESTING_MIC = 133,
            DRAFT_RECONNECT = 134,
            SYNC_DESERT_BOAT = 135,
            UPDATE_FIRE_INFO = 136,
            GACHA_DATA = 137,
            GACHA_PLAY = 138,
            REJECT_VOTE = 139,
            STALKER_CHOOSE_TARGET = 140,
            ESPER_SPECTATE = 141,
            ESPER_BUTTON_CHANGE = 142,
            CHANGE_MAP_SKIN = 143,
            SEND_KILL_SWITCH_DATA = 144,
            PRECURSOR = 145,
            //enum2
            Rpc = 200,
            UnreliableRead = 201,
            Instantiate = 202,
            CloseConnection = 203,
            Destroy = 204,
            RemoveCachedRPCs = 205,
            ReliableRead = 206,
            DestroyPlayer = 207,
            SetMasterClient = 208,
            OwnershipRequest = 209,
            OwnershipTransfer = 210,
            VacantViewIds = 211,
            OwnershipUpdate = 212,
            Recall = 214,
            Debugger = 217,
            AntiCheat = 218,
            Admin = 219,
            //Photon.Realtime.EventCode
            AuthEvent = 223,
            LobbyStats = 224,
            AppStats = 226,
            Match = 227,
            QueueState = 228,
            GameListUpdate = 229,
            GameList = 230,
            CacheSliceChanged = 250,
            ErrorInfo = 251,
            PropertiesChanged = 253,
            Leave = 254,
            Join = 255
        }
    }
}
