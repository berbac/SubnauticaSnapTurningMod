﻿using SMLHelper.V2.Options;
using SMLHelper.V2.Utility;
using UnityEngine;

public class Options : ModOptions
{
    public const string PLAYER_PREF_KEY_TOGGLE_SNAP_TURNING = "SnapTurningTogglePlayerPrefKey";
    public const string PLAYER_PREF_KEY_TOGGLE_SEAMOTH = "SnapTurningToggleSeamoth";
    public const string PLAYER_PREF_KEY_TOGGLE_PRAWN = "SnapTurningTogglePrawn";
    public const string PLAYER_PREF_KEY_SNAP_ANGLE = "SnapAnglePlayerPrefKey";
    private const string TOGGLE_CHANGED_ID_SNAP_TURNING = "SnapTurningId";
    private const string TOGGLE_CHANGED_ID_SEAMOTH = "SeamothId";
    private const string TOGGLE_CHANGED_ID_PRAWN = "PrawnId";
    private const string CHOICE_CHANGED_ID_SNAP_ANGLE = "SnapAngleId";
    private const string CHOICE_CHANGED_ID_SEAMOTH_ANGLE = "SeamothAngleId";
    private const string CHOICE_CHANGED_ID_PRAWN_ANGLE = "PrawnAngleId";

    public Options() : base("Snap Turning")
    {
        ToggleChanged += Options_ToggleChanged;
        ChoiceChanged += Options_ChoiceChanged;
        KeybindChanged += Options_KeybindChanged;
    }

    public void Options_ToggleChanged(object sender, ToggleChangedEventArgs e)
    {
        switch (e.Id)
        {
            case TOGGLE_CHANGED_ID_SNAP_TURNING:
                SnapTurningConfig.Config.EnableSnapTurning = e.Value;
                PlayerPrefsExtra.SetBool(PLAYER_PREF_KEY_TOGGLE_SNAP_TURNING, e.Value);
                SnapTurningConfig.Config.Save();
                break;
            case TOGGLE_CHANGED_ID_SEAMOTH:
                SnapTurningConfig.Config.EnableSeamoth = e.Value;
                PlayerPrefsExtra.SetBool(PLAYER_PREF_KEY_TOGGLE_SEAMOTH, e.Value);
                SnapTurningConfig.Config.Save();
                break;
            case TOGGLE_CHANGED_ID_PRAWN:
                SnapTurningConfig.Config.EnablePrawn = e.Value;
                PlayerPrefsExtra.SetBool(PLAYER_PREF_KEY_TOGGLE_PRAWN, e.Value);
                SnapTurningConfig.Config.Save();
                break;
        }
    }

    public void Options_ChoiceChanged(object sender, ChoiceChangedEventArgs e)
    {
        switch (e.Id)
        {
            case CHOICE_CHANGED_ID_SNAP_ANGLE:
                SnapTurningConfig.Config.SnapAngleChoiceIndex = e.Index;
                PlayerPrefs.SetInt(PLAYER_PREF_KEY_SNAP_ANGLE, e.Index);
                SnapTurningConfig.Config.Save();
                break;
            case CHOICE_CHANGED_ID_SEAMOTH_ANGLE:
                SnapTurningConfig.Config.SeamothAngleChoiceIndex = e.Index;
                PlayerPrefs.SetInt(PLAYER_PREF_KEY_TOGGLE_SEAMOTH, e.Index);
                SnapTurningConfig.Config.Save();
                break;
            case CHOICE_CHANGED_ID_PRAWN_ANGLE:
                SnapTurningConfig.Config.PrawnAngleChoiceIndex = e.Index;
                PlayerPrefs.SetInt(PLAYER_PREF_KEY_TOGGLE_PRAWN, e.Index);
                SnapTurningConfig.Config.Save();
                break;
        }
    }

    public void Options_KeybindChanged(object sender, KeybindChangedEventArgs e)
    {
        if (e.Id == "exampleKeybindLeft")
        {
            SnapTurningConfig.Config.KeybindKeyLeft = e.Key;
            PlayerPrefsExtra.SetKeyCode("SMLHelperExampleModKeybindLeft", e.Key);
            SnapTurningConfig.Config.Save();
        }
        if (e.Id == "exampleKeybindRight")
        {
            SnapTurningConfig.Config.KeybindKeyRight = e.Key;
            PlayerPrefsExtra.SetKeyCode("SMLHelperExampleModKeybindRight", e.Key);
            SnapTurningConfig.Config.Save();
        }
    }

    public override void BuildModOptions()
    {
        SnapTurningConfig.Config.Load();
        AddToggleOption(TOGGLE_CHANGED_ID_SNAP_TURNING, "Enabled", SnapTurningConfig.Config.EnableSnapTurning);
        AddChoiceOption(CHOICE_CHANGED_ID_SNAP_ANGLE, "Angle", new string[] { "45", "90", "22.5" }, SnapTurningConfig.Config.SnapAngleChoiceIndex);

        if (!GameInput.IsPrimaryDeviceGamepad())
        {
            AddKeybindOption("exampleKeybindLeft", "Keyboard Left", GameInput.Device.Keyboard, SnapTurningConfig.Config.KeybindKeyLeft);
            AddKeybindOption("exampleKeybindRight", "Keyboard Right", GameInput.Device.Keyboard, SnapTurningConfig.Config.KeybindKeyRight);
        }

        AddToggleOption(TOGGLE_CHANGED_ID_SEAMOTH, "Seamoth", SnapTurningConfig.Config.EnableSeamoth);
        AddChoiceOption(CHOICE_CHANGED_ID_SEAMOTH_ANGLE, "Seamoth Angle", new string[] { "45", "90", "22.5" }, SnapTurningConfig.Config.SeamothAngleChoiceIndex);

        AddToggleOption(TOGGLE_CHANGED_ID_PRAWN, "Prawn Suit", SnapTurningConfig.Config.EnablePrawn);
        AddChoiceOption(CHOICE_CHANGED_ID_PRAWN_ANGLE, "Prawn Suit Angle", new string[] { "45", "90", "22.5" }, SnapTurningConfig.Config.PrawnAngleChoiceIndex);

    }
}