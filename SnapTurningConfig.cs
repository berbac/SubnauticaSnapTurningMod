using SMLHelper.V2.Utility;
using SMLHelper.V2.Json;
using UnityEngine;

public class SnapTurningConfig : ConfigFile
{
    public bool EnableSnapTurning = true;
    public bool EnableSeamoth = false;
    public bool EnablePrawn = false;
    public int SnapAngleChoiceIndex = 0;
    public int SeamothAngleChoiceIndex = 0;
    public int PrawnAngleChoiceIndex = 0;
    public float[] SnapAngles = { 45, 90, 22.5f };
    public KeyCode KeybindKeyLeft = KeyCode.LeftArrow;
    public KeyCode KeybindKeyRight = KeyCode.RightArrow;

    public static SnapTurningConfig Config { get; } = new SnapTurningConfig();

    public static void Settings()
    {
        Config.EnableSnapTurning = PlayerPrefsExtra.GetBool(Options.PLAYER_PREF_KEY_TOGGLE_SNAP_TURNING, true);
        Config.EnableSeamoth = PlayerPrefsExtra.GetBool(Options.PLAYER_PREF_KEY_TOGGLE_SEAMOTH, false);
        Config.EnablePrawn = PlayerPrefsExtra.GetBool(Options.PLAYER_PREF_KEY_TOGGLE_PRAWN, false);
        Config.SnapAngleChoiceIndex = GetSnapAngleChoiceIndex(SnapType.Default);
        Config.SeamothAngleChoiceIndex = GetSnapAngleChoiceIndex(SnapType.Seamoth);
        Config.PrawnAngleChoiceIndex = GetSnapAngleChoiceIndex(SnapType.Prawn);
        Config.KeybindKeyLeft = PlayerPrefsExtra.GetKeyCode("SMLHelperExampleModKeybindLeft", KeyCode.LeftArrow);
        Config.KeybindKeyRight = PlayerPrefsExtra.GetKeyCode("SMLHelperExampleModKeybindRight", KeyCode.RightArrow);
    }

    private static int GetSnapAngleChoiceIndex(SnapType snapType)
    {
        int result = GetChoiceIndexForSnapType(snapType);
        if (result > Config.SnapAngles.Length)
        {
            result = 0;
        }

        return result;
    }

    private static int GetChoiceIndexForSnapType(SnapType snapType)
    {
        int result = 0;
        if (snapType == SnapType.Default)
        {
            result = PlayerPrefs.GetInt(Options.PLAYER_PREF_KEY_SNAP_ANGLE, 0);
        }
        else if (snapType == SnapType.Seamoth)
        {
            result = PlayerPrefs.GetInt(Options.PLAYER_PREF_KEY_TOGGLE_SEAMOTH, 0);
        }
        else if (snapType == SnapType.Prawn)
        {
            result = PlayerPrefs.GetInt(Options.PLAYER_PREF_KEY_TOGGLE_PRAWN, 0);
        }

        return result;
    }
}

public enum SnapType
{
    Default,
    Seamoth,
    Prawn
}