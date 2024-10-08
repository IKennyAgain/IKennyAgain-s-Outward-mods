﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.TextCore;
using static MapMagic.ObjectPool;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace UIFitter
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class IKennyUIFitter : BaseUnityPlugin
    {
        public const string GUID = "ikennyagain.com.uifitter";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "UI Fitter";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "0.0.1";

        // For accessing your BepInEx Logger from outside of this class (eg Plugin.Log.LogMessage("");)
        internal static ManualLogSource Logging;

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;

        public static IKennyUIFitter Instance;

        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Logging = this.Logger;
            Instance = this;
            Logging.LogMessage($"Hello world from {NAME} {VERSION}!");
            // Harmony is for patching methods. If you're not patching anything, you can comment-out or delete this line.
            new Harmony(GUID).PatchAll();
        }

        public static void Log(object message)
        {
            IKennyUIFitter.Logging.LogMessage($"UIPluginFit :" + message);
        }

        public static void DelayDo(Action ToDo, float Delay = 2f)
        {
            IKennyUIFitter.Instance.StartCoroutine(DelayDoRoutine(ToDo, Delay));
        }

        public static IEnumerator DelayDoRoutine(Action ToDo, float Delay = 2f)
        {
            yield return new WaitForSeconds(Delay);
            ToDo?.Invoke();
            yield break;
        }
    }

    // This is an example of a Harmony patch.
    // If you're not using this, you should delete it.
    [HarmonyPatch(typeof(Character), nameof(Character.Start))]
    public class CharacterStartPatch
    {
        public const float ShopMenuXOffset = 900;
        public const float ShopMenuYOffset = 520;

        public const string PathToShopUI = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu";

        public const string PathToShopNameLabel = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/lblShopName";

        public const string PathToShopPlayerInvLabel = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/lblPlayerInventory";

        public const string PathToShopPlayerCurrency = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/PlayerCurrency";

        public const string PathToShopMerchantCurrency = "Canvas/GameplayPanels/Menus/ModalMenus/ShopMenu/TopPanel/Shop PanelTop/MerchantCurrency";

        public const string PathToChestUI = "Canvas/GameplayPanels/Menus/ModalMenus/Stash - Panel";

        public const string PathToChestLabel = "Canvas/GameplayPanels/Menus/ModalMenus/Stash - Panel/Content/TopPanel/Shop PanelTop/lblShopName";

        public const string PathToChestPlayerLabel = "Canvas/GameplayPanels/Menus/ModalMenus/Stash - Panel/Content/TopPanel/Shop PanelTop/lblPlayerInventory";

        public const string PathToPlayerStatus = "Canvas/GameplayPanels/HUD/StatusEffect - Panel";

        public const string PathToPlayerInventory = "Canvas/GameplayPanels/Menus/CharacterMenus/MainPanel";

        [HarmonyPostfix]
        public static void Postfix(Character __instance)
        {
            IKennyUIFitter.DelayDo(() =>
            {
                if (!__instance.IsAI && __instance.CharacterUI != null)
                {
                    ModifyRectSizeDeltaAtPath(__instance.CharacterUI, PathToShopUI, new Vector2(900, 520));
                    ///change the offset min and offset max of the Shop Name Label (lbl)
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopNameLabel, new Vector2(0, 6), new Vector2(500, 34));
                    ///change the offset min and offset max of the Player Inv Name Label (lbl)
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopPlayerInvLabel, new Vector2(-500, 6), new Vector2(0, 34));
                    ///change the offset min and offset max of the Player Currency Label (lbl)
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopPlayerCurrency, new Vector2(-500, -32), new Vector2(0, 18));
                    ///change the offset min and offset max of the Merchant Currency Label (lbl)
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopMerchantCurrency, new Vector2(0, -32), new Vector2(500, 18));
                    ///change the sizeDelta of the chest UI
                    ModifyRectSizeDeltaAtPath(__instance.CharacterUI, PathToChestUI, new Vector2(945, 520));
                    ///change the offset min and max of the chest UI
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToChestUI, default (Vector2), new Vector2(0, -20));
                    ///change the offset min and max of the chest name
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToChestLabel, new Vector2(0, -10), new Vector2(500, 28));
                    ///change the offset min and max of the player name
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToChestPlayerLabel, new Vector2(-500, -10), new Vector2(0, 28));
                    ///change the sizeDelta of the Status UI
                    ModifyRectSizeDeltaAtPath(__instance.CharacterUI, PathToPlayerStatus, new Vector2(140, 100));
                    ///change the offset min and max of the Status UI
                    ModifyRectOffsetAtPath(__instance.CharacterUI, PathToPlayerStatus, new Vector2(20, 300), new Vector2(160, 108));
                    




                    //IKennyUIFitter.Log($"{__instance.Name} IsLocalPlayer? {__instance.IsLocalPlayer} IsWorldHost? {__instance.IsWorldHost} ");
                    //if this is a local player BUT not the 'main' player, do specific UI changes
                    //if (__instance.IsLocalPlayer && !__instance.IsWorldHost)
                    //{
                    //ModifyRectOffsetAtPath(__instance.CharacterUI, "Canvas/GeneralPanels/MainScreen/VisualMainScreen/Options", default(Vector2), new Vector2(180, 94));
                    //}
                    //otherwise if is a local player AND is the world host, continue as normal and make 
                    //else if(__instance.IsLocalPlayer && __instance.IsWorldHost)
                    //{

                    //ModifyRectSizeDeltaAtPath(__instance.CharacterUI, PathToShopUI, new Vector2(900, 520));
                    ///change the offset min and offset max of the Shop Name Label (lbl)
                    //ModifyRectOffsetAtPath(__instance.CharacterUI, PathToShopNameLabel, new Vector2(0, 6), new Vector2(500, 34));
                    ///} 
                }

            }, 4f);

        }
        public static void ModifyRectSizeDeltaAtPath(CharacterUI CharacterUI, string RectTransformPath, Vector2 NewSize)
        {
            IKennyUIFitter.Log($"{CharacterUI.transform} root.");
            IKennyUIFitter.Log($"Attempting to find {CharacterUI.transform}/{RectTransformPath}.");
            RectTransform foundRectTransform = GetRectTransformAtPath(CharacterUI, RectTransformPath);
            if (foundRectTransform != null)
            {
                foundRectTransform.sizeDelta = NewSize;
            }
        }

        public static void ModifyRectOffsetAtPath(CharacterUI CharacterUI, string RectTransformPath, Vector2 Min = default(Vector2), Vector2 Max = default(Vector2))
        {
            IKennyUIFitter.Log($"{CharacterUI.transform} root.");
            IKennyUIFitter.Log($"Attempting to find {CharacterUI.transform}/{RectTransformPath}.");
            RectTransform foundRectTransform = GetRectTransformAtPath(CharacterUI, RectTransformPath);
            if (foundRectTransform != null)
            {
                if (Min != default(Vector2))
                {
                    foundRectTransform.offsetMin = Min;
                }

                if (Max != default(Vector2))
                {
                    foundRectTransform.offsetMax = Max;
                }                    
            }
        }

        public static RectTransform GetRectTransformAtPath(CharacterUI CharacterUI, string RectTransformPath)
        {
            RectTransform foundRectTransform = CharacterUI.transform.Find(RectTransformPath).GetComponent<RectTransform>();
            if (foundRectTransform != null)
            {
                IKennyUIFitter.Log($"Found RectTransform");
                return foundRectTransform;
            }
            else
            {
                IKennyUIFitter.Log($"Did not find RectTransform");
                return null;
            }
        }
    }

    [HarmonyPatch(typeof(SplitScreenManager), nameof(SplitScreenManager.AssignPlayerToUI))]
    public class SplitScreenManagerStartPatch
    {
        [HarmonyPostfix]
        public static void Postfix(SplitScreenManager __instance, Character _character)
        {
            IKennyUIFitter.DelayDo(() =>
            {
                RectTransform foundRectTransform = _character.CharacterUI.transform.Find("GeneralPanels/MainScreen/VisualMainScreen/Options").GetComponent<RectTransform>();
                if (foundRectTransform != null)
                {
                    foundRectTransform.offsetMax = new Vector2(180, 94);
                }
                else
                {
                    IKennyUIFitter.Log($"MenuManagerStartPatch : Did not find RectTransform");
                }

                }, 4f);

        }
    }

}
