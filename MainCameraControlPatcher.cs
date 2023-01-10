using HarmonyLib;
using SMLHelper.V2.Utility;
using UnityEngine;
using UnityEngine.XR;

namespace SubnauticaSnapTurningMod
{
    [HarmonyPatch(typeof(MainCameraControl))]
    internal class MainCameraControlPatcher
    {
        private static float SnapAngle => SnapTurningConfig.Config.SnapAngles[SnapTurningConfig.Config.SnapAngleChoiceIndex];
        private static float SeamothSnapAngle => SnapTurningConfig.Config.SnapAngles[SnapTurningConfig.Config.SeamothAngleChoiceIndex];
        private static float PrawnSnapAngle => SnapTurningConfig.Config.SnapAngles[SnapTurningConfig.Config.PrawnAngleChoiceIndex];
        private static bool IsInPrawnSuit => Player.main.inExosuit;
        private static bool IsInSeamoth => Player.main.inSeamoth;
        public static bool ShouldResetControllerHAxis;

        [HarmonyPatch(typeof(MainCameraControl), nameof(MainCameraControl.OnUpdate))]
        [HarmonyPrefix]
        public static bool Prefix()
        {
            var isIgnoringSeamoth = IsInSeamoth && !SnapTurningConfig.Config.EnableSeamoth;
            var isIgnoringPrawn = IsInPrawnSuit && !SnapTurningConfig.Config.EnablePrawn;
            if (!SnapTurningConfig.Config.EnableSnapTurning || isIgnoringSeamoth || isIgnoringPrawn)
            {
                ShouldResetControllerHAxis = false;
                return true; //Enter vanilla method
            }
            
            var didLookRight = GameInput.GetButtonDown(GameInput.Button.LookRight) || KeyCodeUtils.GetKeyDown(SnapTurningConfig.Config.KeybindKeyRight);
            var didLookLeft = GameInput.GetButtonDown(GameInput.Button.LookLeft) || KeyCodeUtils.GetKeyDown(SnapTurningConfig.Config.KeybindKeyLeft);
            //var isLookingLeft = GameInput.GetButtonHeld(GameInput.Button.LookLeft);
            //var isLookingRight = GameInput.GetButtonHeld(GameInput.Button.LookRight);
            var isLooking = didLookLeft || didLookRight;// || isLookingLeft || isLookingRight;
            var shouldSnapTurn = XRSettings.enabled && isLooking; 
            if (shouldSnapTurn)
            {
                UpdatePlayerOrVehicleRotation(didLookRight, didLookLeft);
                ShouldResetControllerHAxis = true;
                return false; //Don't enter vanilla method if we snap turn
            }
            return true;
        }

        private static void UpdatePlayerOrVehicleRotation(bool didLookRight, bool didLookLeft)
        {
            var newEulerAngles = GetNewEulerAngles(didLookRight, didLookLeft);

            if (IsInSeamoth)
            {
                Player.main.currentMountedVehicle.transform.localRotation = Quaternion.Euler(newEulerAngles);
            }
            else if (IsInPrawnSuit)
            {
                Player.main.currentMountedVehicle.transform.localRotation = Quaternion.Euler(newEulerAngles);
            }
            else
            {
                Player.main.transform.localRotation = Quaternion.Euler(newEulerAngles);
            }
        }

        private static Vector3 GetNewEulerAngles(bool didLookRight, bool didLookLeft)
        {
            var isInVehicle = IsInSeamoth || IsInPrawnSuit;
            var newEulerAngles = isInVehicle
                ? Player.main.currentMountedVehicle.transform.localRotation.eulerAngles
                : Player.main.transform.localRotation.eulerAngles;

            var angle = GetSnapAngle();

            if (didLookRight)
            {
                newEulerAngles.y += angle;
            }
            else if (didLookLeft)
            {
                newEulerAngles.y -= angle;
            }

            return newEulerAngles;
        }

        private static float GetSnapAngle()
        {
            var snapAngle = SnapAngle;
            if (IsInSeamoth)
            {
                snapAngle = SeamothSnapAngle;
            }
            else if (IsInPrawnSuit)
            {
                snapAngle = PrawnSnapAngle;
            }

            return snapAngle;
        }
    }
    [HarmonyPatch(typeof(GameInput), nameof(GameInput.Update))]
    internal class GameInputPatcher
    {
        [HarmonyPostfix]
        public static void ClearControllerHorizontalInput(ref float[] ___axisValues)
        {
            if (MainCameraControlPatcher.ShouldResetControllerHAxis)
            {
                ___axisValues[0] = 0f;
            }
        }
    }
}
