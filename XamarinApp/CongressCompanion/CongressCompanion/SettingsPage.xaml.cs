﻿using AE_Xamarin.Forms;
using CongressCompanion.ClassObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            //Load The Colors And Set Trigger
            UpdateThemeColors();
            AppThemeManager.Instance.AppThemeChange += Instance_AppThemeChange;

            //Set The Save Location Switch
            SaveLocationSwitch.IsToggled = AppManager.Instance.ShouldSaveLocation;

            //Load Theme Names
            foreach(string ThemeName in AppThemeManager.Instance.ThemeNames)
            {
                ThemePicker.Items.Add(ThemeName);
            }

            //Select The Current Theme
            ThemePicker.SelectedIndex = ThemePicker.Items.IndexOf(AppThemeManager.Instance.CurrentThemeName);
        }

        private async void EditLocationBTN_Clicked(object sender, EventArgs e)
        {
            //Load The Location Page
            await Navigation.PushAsync(new LocationPage(true));
        }
        private void SaveLocationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            //Set The Visibility Of The Location Bar
            BoarderLayout.IsVisible = e.Value;

            //Change The Boolean Value Of Should Save Location
            AppManager.Instance.ShouldSaveLocation = e.Value;

            //Save The New Data
            AppManager.Instance.SaveUserData();
        }
        private void ThemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Load The Correct Theme
            AppThemeManager.Instance.SelectTheme(ThemePicker.SelectedItem.ToString());

            //Save The Data
            AppManager.Instance.SaveUserData();
        }

        public void UpdateThemeColors()
        {
            ThemeLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            ThemePicker.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            ResetDataBTN.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            LocationEntry.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            SaveLocationLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            ResetDataBTN.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;
            LocationEntry.PlaceholderColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            MainLayout.BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;
            EditLocationBTN.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;
            SavedLocationLayout.BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;

            //Add More Margin Space For UWP
            if (Device.RuntimePlatform == Device.UWP)
            {
                BoarderLayout.Margin = new Thickness(45, 0);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            //Stop From Going Back To Start Page
            return true;
        }
        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            //Update The Text Based On Location Type
            if (AppManager.Instance.IsZipcode)
            {
                LocationEntry.Placeholder = $"Zipcode: {AppManager.Instance.UserLocationInfo}";
            }
            else
            {
                LocationEntry.Placeholder = AppManager.Instance.UserLocationInfo;
            }
        }
        private void Instance_AppThemeChange(object sender, ThemeChangeArgs e)
        {
            UpdateThemeColors();
        }

        private async void ResetDataBTN_Clicked(object sender, EventArgs e)
        {
            //Ask The User If They Are Sure
            if(await DisplayAlert("Erase User Data?", "Are You Sure You Want To Erase All User Saved Data?", "Yes", "No"))
            {
                //Reset Data
                AppManager.Instance.ResetSaveData();

                //Set The Save Location Switch
                SaveLocationSwitch.IsToggled = AppManager.Instance.ShouldSaveLocation;

                //Update The Text Based On Location Type
                if (AppManager.Instance.IsZipcode)
                {
                    LocationEntry.Placeholder = $"Zipcode: {AppManager.Instance.UserLocationInfo}";
                }
                else
                {
                    LocationEntry.Placeholder = AppManager.Instance.UserLocationInfo;
                }

                //Select The Current Theme
                ThemePicker.SelectedIndex = ThemePicker.Items.IndexOf(AppThemeManager.Instance.CurrentThemeName);
            }
        }
    }
}