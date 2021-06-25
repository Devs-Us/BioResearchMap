using HarmonyLib;
using LevelImposter.Models;
using System;
using System.Collections.Generic;
using System.Text;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace LevelImposter.Harmony.Patches
{
    [HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
    public static class VersionPatch
    {
        public static void Postfix(VersionShower __instance)
        {
            byte[] logoData = Properties.Resources.logo;

            GameObject logo = new GameObject("LevelImposterLogo");
            logo.transform.position = __instance.text.transform.position;
            logo.transform.position += new Vector3(2f, -0.015f, 0);
            logo.transform.localScale = new Vector3(0.5f, 0.5f, 1.0f);
            logo.layer = (int)Layer.UI;

            Texture2D tex = new Texture2D(1, 1);
            ImageConversion.LoadImage(tex, logoData);
            SpriteRenderer spriteRenderer = logo.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

            __instance.text.text += $"\t\t\t   <size=65%>v{MainHarmony.VERSION}</size> | Bio Research Map v0.1.0-beta";
        }
    }

    [HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
    public static class PingTrackerPatch
    {
        private static void Postfix(PingTracker __instance)
        {
            if (MeetingHud.Instance) __instance.text.text = "";
            __instance.text.alignment = TMPro.TextAlignmentOptions.TopRight;
            if (AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Started)
            {
                if (!MeetingHud.Instance) __instance.text.text = $"<size=130%>Bio Research\n<size=0.8>Created by Devs Us</size>\nLatency: {AmongUsClient.Instance.Ping}ms";
                __instance.transform.localPosition = PlayerControl.LocalPlayer.Data.IsDead
                    ? new Vector3(3.45f, __instance.transform.localPosition.y, __instance.transform.localPosition.z)
                    : new Vector3(4.1f, __instance.transform.localPosition.y, __instance.transform.localPosition.z);
            }
            else
            {
                __instance.text.alignment = TMPro.TextAlignmentOptions.TopGeoAligned;
                __instance.text.text = $"Bio Research Map\nCreated by Devs Us\nPowered by <color=#399cbd>Level</color><color=#f70d1a>Imposter</color>\n<size=80%>{__instance.text.text}";
                __instance.transform.localPosition = new Vector3(__instance.transform.localPosition.x, 2.88f, __instance.transform.localPosition.z);
            }
        }
    }
}
