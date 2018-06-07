﻿using AE_Xamarin.Forms.Controls;
using AE_Xamarin.Misc;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class CityEntry : ValidationEntry
    {
        //Store Origional Color
        private Color? TextBaseColor = null;

        public CityEntry() : base(BasicRegex.RegexPatters.CityNameValidation)
        {
            //Set Default Info
            Keyboard = Keyboard.Plain;

            //Add Event To Validate The Number
            Unfocused += CityEntry_Unfocused;
        }
        private void CityEntry_Unfocused(object sender, FocusEventArgs e)
        {
            //Check Validity
            if (!IsValid)
            {
                //Save The Color
                TextBaseColor = TextColor;
                TextColor = Color.Red;
            }

            //Make Sure We Changed It In The First Place
            else if (TextBaseColor.HasValue)
            {
                TextColor = TextBaseColor.Value;
            }
        }
    }
}
