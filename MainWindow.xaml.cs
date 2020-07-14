using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace RoR2ProfileManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        UserProfile profile = null;
        String file = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers

        /// <summary>
        /// Event action to be performed when the user clicks the profile button.
        /// The user is prompted with a file dialog where they are to chose the profile to be managed.
        /// </summary>
        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "XML Documents (*.xml)|*.xml|All files (*.*)|*.*"
            };

            if (fileDialog.ShowDialog() == true)
            {
                file = fileDialog.FileName;
                try
                {
                    DeserializeProfileData(file);
                    AttributesComboBox.IsEnabled = true;
                    ProfileBlock.Text = profile.Name;
                    AttributesComboBox.SelectedIndex = 3;
                }
                catch(InvalidOperationException)
                {
                    Console.Out.WriteLine("The Profile Manager was unable to read the given file. Please try again.");
                    //TODO: Create a popup window with message "The Profile Manager was unable to read the given file. Please try again."
                }
            }

            
        }

        /// <summary>
        /// Event action to be performed when the user changes the selection of the attribures combo box.
        /// When the user chooses a different attribute, the proper attributes will be displayed in the
        /// options list box.
        /// </summary>
        private void AttributesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (ComboBoxItem)AttributesComboBox.SelectedItem;
            OptionsListBox.Items.Clear();
            //var sb = new StringBuilder();
            //sb.Append("{ \"\" , \"" + achievement.ToString() + "\"},\n");
            if (!(selectedItem.Content is string selected))
                selected = "";
            switch (selected)
            {    
                case "Achievements":
                    PopulateAttriburesComboBoxStringArray(profile.AchivevementsList, DataNameTranslator.Achievements);
                    break;

                case "Coins":
                    var coinsTextBox = new TextBox
                    {
                        Text = profile.Coins.ToString()
                    };
                    OptionsListBox.Items.Add(coinsTextBox);
                    break;

                case "Discovered Pickups":
                    PopulateAttriburesComboBoxStringArray(profile.DiscoveredPickups, DataNameTranslator.Achievements);
                    break;

                case "Unlockables":
                    profile.Stats.Unlock.Sort();
                    var unlockEnum = profile.Stats.Unlock.GetEnumerator();

                    if (profile.Stats.Unlock.FirstOrDefault() != null)
                        unlockEnum.MoveNext();

                    foreach (var key in DataNameTranslator.Unlocks.Keys)
                    {
                        try
                        {
                            var checkBoxItem = new CheckBox()
                            {
                                Content = key
                            };

                            if (unlockEnum.Current != null && unlockEnum.Current.Name.Equals(DataNameTranslator.Unlocks[key]))
                            {
                                checkBoxItem.IsChecked = true;
                                unlockEnum.MoveNext();
                            }

                            OptionsListBox.Items.Add(checkBoxItem);
                        }
                        catch (Exception ex)
                        {
                            Console.Out.WriteLine("Something went wrong when trying to display all of the unlockables.");
                            //TODO: Create a popup box with a more descriptive error message.
                        }
                    }
                    break;

                default:
                    Console.Out.Write("The combo box input is not implemented yet.");
                    //TODO: Add a popup box that better discribes the problem.
                    break;
            }
            SelectAllButton.Opacity = 1;
            SelectAllButton.IsEnabled = true;
            //OptionsListBox.Items.Add(new TextBox() { Text = sb.ToString() });
        }
        /// <summary>
        /// Event action to be performed when the user clicks the update button.
        /// This commits all of the changes the user made while using the manager.
        /// </summary>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var selected = (ComboBoxItem)AttributesComboBox.SelectedItem;
            switch (selected.Content)
            {
                case "Achievements":
                    var newAchievements = new StringBuilder();
                    foreach (var item in OptionsListBox.Items)
                    {
                        if (!(item is CheckBox boxItem))
                            continue;
                        if (boxItem.IsChecked == true)
                            newAchievements.Append(DataNameTranslator.Achievements[(string)boxItem.Content] + " ");
                    }
                    newAchievements.Remove(newAchievements.Length - 1, 1);
                    profile.AchivevementsList = newAchievements.ToString();
                    break;

                case "Coins":
                    foreach (var item in OptionsListBox.Items)
                    {
                        if (!(item is TextBox boxItem))
                            continue;
                        try
                        {
                            profile.Coins = Int32.Parse(boxItem.Text);
                        }
                        catch
                        (Exception ex)
                        {
                            Console.Out.WriteLine(ex);
                            //TODO: Have a popup box with a more descriptive message.
                        }
                    }
                    break;

                case "Unlockables":
                    var newUnlocks = new List<Unlock>();
                    foreach (var item in OptionsListBox.Items)
                    {
                        if (!(item is CheckBox boxItem))
                            continue;
                        if (boxItem.IsChecked == true)
                            newUnlocks.Add(new Unlock() { Name = DataNameTranslator.Unlocks[(string)boxItem.Content] });
                    }
                    profile.Stats.Unlock = newUnlocks;
                    break;

                default:
                    break;
            }

            SerializeProfileData();
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(var item in OptionsListBox.Items)
            {
                if (item is CheckBox checkbox)
                    checkbox.IsChecked = true;
            }
        }

        #endregion

        private void PopulateAttriburesComboBoxStringArray(string profileDict, Dictionary<string, string> dict)
        {
            var strArray = profileDict.Split(" ");
            Array.Sort(strArray);
            var i = 0;
            var arrayLen = strArray.Length;
            foreach (var key in dict.Keys)
            {
                try
                {
                    var checkBoxItem = new CheckBox()
                    {
                        Content = key
                    };

                    if (i < arrayLen && strArray[i].Equals(dict[key]))
                    {
                        checkBoxItem.IsChecked = true;
                        i++;
                    }

                    OptionsListBox.Items.Add(checkBoxItem);
                }
                catch (Exception ex)
                {
                    Console.Out.WriteLine("Something went wrong when trying to display all of the items.");
                    //TODO: Create a popup box with a more descriptive error message.
                }
            }
            
            
        }

        #region XML File Methods

        /// <summary>
        /// Deserializes the user profile xml into a UserProfile object.
        /// </summary>
        /// <param name="file">The user profile xml file.</param>
        private void DeserializeProfileData(string file)
        {
            var ser = new XmlSerializer(typeof(UserProfile));
            var reader = new StreamReader(file);
            profile = (UserProfile)ser.Deserialize(reader);
            reader.Close();
        }

        /// <summary>
        /// Serializes a UserProfile object into an xml file.
        /// </summary>
        private void SerializeProfileData()
        {
            var ser = new XmlSerializer(typeof(UserProfile));
            var writer = File.CreateText(file);
            ser.Serialize(writer, profile);
            writer.Close();
        }

        #endregion
    }
}
