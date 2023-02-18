﻿using Handlers.GameHandlers.SpecialBehaviour;
using Il2CppSystem.Collections.Generic;
using UnhollowerRuntimeLib;
using UnityEngine;
using UnityEngine.PlayerLoop;
using HarmonyLib;

using IntPtr = System.IntPtr;
using MelonLoader;
using Handlers.GameHandlers.PlayerHandlers;
using static MelonLoader.MelonLogger;
using Handlers.LobbyHandlers;
using Il2CppSystem.Threading.Tasks;

namespace GGD_Hack.Features
{
    [RegisterTypeInIl2Cpp]
    public class MinimapESP : MonoBehaviour
    {
        private static MinimapESP instance = null;

        //玩家userId对应的GameObject，表示地图上的点
        private static Dictionary<string, GameObject> playersOnMinimap = new Dictionary<string, GameObject>();
        private static bool instantiatedAllPlayers = false;

        //通过hook更新
        public static MiniMapHandler miniMapHandler = null;

        public MinimapESP(IntPtr ptr) : base(ptr) { }

        // Optional, only used in case you want to instantiate this class in the mono-side
        // Don't use this on MonoBehaviours / Components!
        public MinimapESP() : base(ClassInjector.DerivedConstructorPointer<MinimapESP>()) => ClassInjector.DerivedConstructorBody(this);


        public static void Init()
        {
            GameObject ML_Manager = GameObject.Find("ML_Manager");
            if (ML_Manager == null)
            {
                ML_Manager = new GameObject("ML_Manager");
                DontDestroyOnLoad(ML_Manager);
            }

            if (ML_Manager.GetComponent<MinimapESP>() == null)
            {
                instance = ML_Manager.AddComponent<MinimapESP>();
            }
        }


        /// <summary>
        /// 初始化所有玩家在地图上的点位（除了本地玩家）
        /// </summary>
        private static void InstantiateAllPlayers()
        {
            MelonLogger.Msg("初始化所有玩家的minimap点位");
            //清空之前的玩家列表
            MinimapESP.playersOnMinimap.Clear();

            //准备克隆本地玩家的点位
            GameObject targetMe = Utils.GameInstances.FindGameObjectByPath("Canvas/MiniMap/Panel/Target Me");

            if (targetMe == null)
            {
                MelonLogger.Warning("[MinimapESP] 未找到Target Me对象，初始化所有玩家点位失败!");
                return;
            }

            //遍历
            foreach (var entry in PlayerController.playersList)
            {
                var playerController = entry.Value;

                //跳过null和本地玩家
                if (playerController == null || playerController.isLocal) continue;

                GameObject clone = Object.Instantiate(targetMe, targetMe.transform.parent);

                Transform childTransform = clone.transform.Find("You/TMP UI SubObject [JunkDog SDF Material]");

                if (childTransform == null)
                {
                    MelonLogger.Warning("找不到 You/TMP UI SubObject [JunkDog SDF Material]");
                }

                //修改文字
                if (childTransform != null)
                {
                    TMPro.TMP_SubMeshUI tmp_SubMeshUI = childTransform.gameObject.GetComponent<TMPro.TMP_SubMeshUI>();
                    if (tmp_SubMeshUI != null)
                    {
                        tmp_SubMeshUI.textComponent.SetText(playerController.nickname, true);
                    }
                }

                //修改颜色

                playersOnMinimap.Add(playerController.userId, clone);
            }

            instantiatedAllPlayers = true;
        }

        /// <summary>
        /// 更新所有gameObjects的坐标，相对于LocalPlayer
        /// </summary>
        private void Update()
        {
            if (miniMapHandler == null)
            {
                return;
            }

            if (!LobbySceneHandler.Instance.gameStarted)
            {
                if (MinimapESP.playersOnMinimap.Count != 0)
                {
                    MelonLogger.Msg("游戏结束，清除所有玩家的minimap点位");
                    foreach(var entry in MinimapESP.playersOnMinimap)
                    {
                        if(entry.Value != null)
                        {
                            Destroy(entry.Value);
                        }
                    }

                    MinimapESP.playersOnMinimap.Clear();
                }

                instantiatedAllPlayers = false;

                return;
            }

            if(!instantiatedAllPlayers)
            {
                MinimapESP.InstantiateAllPlayers();
            }

            foreach (var player in MinimapESP.playersOnMinimap)
            {
                string userId = player.Key;
                GameObject gameObject = player.Value;

                if (gameObject == null) continue;

                if (!PlayerController.playersList.ContainsKey(userId)) continue;

                //删除幽灵点位
                if(PlayerController.playersList[userId].isGhost)
                {
                    Destroy(gameObject);
                    gameObject = null;
                    continue;
                }

                //获取PlayerController对应的坐标
                Vector3 position = PlayerController.playersList[userId].JGEIABDOLNO;

                //根据PlayerController的坐标计算出GameObject的坐标
                gameObject.transform.localPosition = new Vector3(
                    (float)(miniMapHandler.xFactor * position.x) + miniMapHandler.xOffset,
                    (float)(miniMapHandler.yFactor * position.y) + miniMapHandler.yOffset,
                    0.0f
                );
            }

            /*
             * this是MiniMapHandler
            v180.fields.x = (float)(this->fields.xFactor * this->fields.PFMBPLBLHNN.fields.x) + this->fields.xOffset;
            v180.fields.y = (float)(this->fields.yFactor * this->fields.PFMBPLBLHNN.fields.y) + this->fields.yOffset;
            v180.fields.z = 0.0;
            */
        }
    }


    [HarmonyPatch(typeof(MiniMapHandler), "Update")]
    class MiniMapHandlerUpdateHook
    {
        //更新实例
        static void Postfix(MiniMapHandler __instance)
        {
            if (MinimapESP.miniMapHandler == null)
            {
                MelonLogger.Msg("已成功Hook获取到MinimapHandler");
                MinimapESP.miniMapHandler = __instance;
            }
        }
    }
}