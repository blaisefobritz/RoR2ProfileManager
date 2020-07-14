using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RoR2ProfileManager
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("UserProfile")]
    public class UserProfile
    {
        [System.Xml.Serialization.XmlElement("achievementsList")]
        public string AchivevementsList { get; set; }

        [System.Xml.Serialization.XmlElement("coins")]
        public int Coins { get; set; }

        [System.Xml.Serialization.XmlElement("discoveredPickups")]
        public string DiscoveredPickups { get; set; }

        [System.Xml.Serialization.XmlElement("gamepadVibrationScale")]
        public int GamepadVibrationScale { get; set; }

        [System.Xml.Serialization.XmlElement("joystickMap")]
        public string JoystickMap { get; set; }

        [System.Xml.Serialization.XmlElement("keyboardMap")]
        public string KeyboardMap { get; set; }

        [System.Xml.Serialization.XmlElement("mouseLookInvertX")]
        public int MouseLookInvertX { get; set; }

        [System.Xml.Serialization.XmlElement("mouseLookInvertY")]
        public int MouseLookInvertY { get; set; }

        [System.Xml.Serialization.XmlElement("mouseLookScaleX")]
        public int MouseLookScaleX { get; set; }

        [System.Xml.Serialization.XmlElement("mouseLookScaleY")]
        public int MouseLookScaleY { get; set; }

        [System.Xml.Serialization.XmlElement("mouseLookSensitivity")]
        public double MouseLookSensitivity { get; set; }

        [System.Xml.Serialization.XmlElement("mouseMap")]
        public string MouseMap { get; set; }

        [System.Xml.Serialization.XmlElement("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlElement("screenShakeScale")]
        public int ScreenShakeScale { get; set; }

        [System.Xml.Serialization.XmlElement("stickLookInvertX")]
        public int StickLookInvertX { get; set; }

        [System.Xml.Serialization.XmlElement("stickLookInvertY")]
        public int StickLookInvertY { get; set; }

        [System.Xml.Serialization.XmlElement("stickLookScaleX")]
        public int StickLookScaleX { get; set; }

        [System.Xml.Serialization.XmlElement("stickLookScaleY")]
        public int StickLookScaleY { get; set; }

        [System.Xml.Serialization.XmlElement("stickLookSensitivity")]
        public double StickLookSensitivity { get; set; }

        [System.Xml.Serialization.XmlElement("totalAliveSeconds")]
        public int TotalAliveSeconds { get; set; }

        [System.Xml.Serialization.XmlElement("totalCollectedCoins")]
        public int TotalCollectedCoins { get; set; }

        [System.Xml.Serialization.XmlElement("totalLoginSeconds")]
        public int TotalLoginSeconds { get; set; }

        [System.Xml.Serialization.XmlElement("totalRunCount")]
        public int TotalRunCount { get; set; }

        [System.Xml.Serialization.XmlElement("totalRunSeconds")]
        public int TotalRunSeconds { get; set; }

        [System.Xml.Serialization.XmlElement("unviewedAchievementsList")]
        public string ViewedAchievementsList { get; set; }

        [System.Xml.Serialization.XmlElement("version")]
        private double Version { get; set; }

        [System.Xml.Serialization.XmlElement("viewedUnlockablesList")]
        public string ViewedUnlockablesList { get; set; }

        [System.Xml.Serialization.XmlElement("viewedViewables")]
        public string ViewedViewables { get; set; }

        [System.Xml.Serialization.XmlElement("stats")]
        public Stats Stats { get; set; }

        [System.Xml.Serialization.XmlElement("tutorialDifficulty")]
        public int TutorialDifficulty { get; set; }

        [System.Xml.Serialization.XmlElement("tutorialEquipment")]
        public int TutorialEquipment { get; set; }

        [System.Xml.Serialization.XmlElement("tutorialSprint")]
        public int TutorialSprint { get; set; }

        [System.Xml.Serialization.XmlElement("loadout")]
        public Loadout Loadout { get; set; }
    }
    
    public class Stats
    {
        [System.Xml.Serialization.XmlElement("stat")]
        public List<Stat> Stat { get; set; }

        [System.Xml.Serialization.XmlElement("unlock")]
        public List<Unlock> Unlock { get; set; }
    }


    public class Stat
    {
        [System.Xml.Serialization.XmlAttribute("name")]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlText]
        public double Value { get; set; }
    }

    public class Unlock : IComparable<Unlock>
    {
        [System.Xml.Serialization.XmlText]
        public string Name { get; set; }

        public int CompareTo([AllowNull] Unlock other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public class Loadout
    {
        [System.Xml.Serialization.XmlArray("BodyLoadouts")]
        public List<BodyLoadout> BodyLoadouts { get; set; }
    }

    public class BodyLoadout
    {
        [System.Xml.Serialization.XmlAttribute("bodyName")]
        public string BodyName { get; set; }

        [System.Xml.Serialization.XmlElement("Skin")]
        public int Skin { get; set; }

        [System.Xml.Serialization.XmlElement("SkillPreference")]
        public List<SkillPreference> SkillPreference { get; set; }
    }

    public class SkillPreference
    {
        [System.Xml.Serialization.XmlAttribute("skillFamily")]
        public string SkillFamily { get; set; }

        [System.Xml.Serialization.XmlText]
        public string SkillName { get; set; }
    }
}
