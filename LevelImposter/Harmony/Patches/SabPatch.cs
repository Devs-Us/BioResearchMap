using HarmonyLib;
using UnityEngine;

namespace LevelImposter.Harmony.Patches
{
    [HarmonyPatch(typeof(ReactorTask), nameof(ReactorTask.Awake))]
    public static class ReactorPatch
    {
        public static void Prefix(ReactorTask __instance)
        {
            __instance.StartAt = SystemTypes.Laboratory;
        }
    }

    [HarmonyPatch(typeof(ReactorTask), nameof(ReactorTask.Complete))]
    public static class SabArrowFix1
    {
        public static void Postfix()
        {
            GameObject.Find("SabManager").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("SabManager").transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    [HarmonyPatch(typeof(ElectricTask), nameof(ElectricTask.Complete))]
    public static class SabArrowFix2
    {
        public static void Postfix()
        {
            GameObject.Find("SabManager").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("SabManager").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(HudOverrideTask), nameof(HudOverrideTask.Complete))]
    public static class SabArrowFix3
    {
        public static void Postfix()
        {
            GameObject.Find("SabManager").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("SabManager").transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.AddSystemTask))]
    public static class SabPatch
    {
        public static bool Prefix(SystemTypes system)
        {
            // Get Task
            PlayerTask playerTask = null;
            for (int i = 0; i < ShipStatus.Instance.SpecialTasks.Length; i++)
            {
                PlayerTask task = ShipStatus.Instance.SpecialTasks[i];
                if (task.StartAt == system)
                {
                    playerTask = task;
                    break;
                }
            }

            // Check
            if (playerTask == null)
            {
                LILogger.LogError($"Player has been given invalid System Task: {system} | {playerTask}");
                return false;
            }
            else
            {
                LILogger.LogInfo($"Player has been given a valid System Task: {system} | {playerTask}");
            }

            // Provide
            PlayerTask playerTask2 = Object.Instantiate(playerTask, PlayerControl.LocalPlayer.transform);
            playerTask2.Id = 255u;
            playerTask2.Owner = PlayerControl.LocalPlayer;
            playerTask2.Initialize();
            PlayerControl.LocalPlayer.myTasks.Add(playerTask2);
            return false;
        }
    }
}
