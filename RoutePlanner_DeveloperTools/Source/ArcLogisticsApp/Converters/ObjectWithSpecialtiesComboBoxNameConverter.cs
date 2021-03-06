﻿/*
 | Version 10.1.84
 | Copyright 2013 Esri
 |
 | Licensed under the Apache License, Version 2.0 (the "License");
 | you may not use this file except in compliance with the License.
 | You may obtain a copy of the License at
 |
 |    http://www.apache.org/licenses/LICENSE-2.0
 |
 | Unless required by applicable law or agreed to in writing, software
 | distributed under the License is distributed on an "AS IS" BASIS,
 | WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 | See the License for the specific language governing permissions and
 | limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Documents;
using System.Globalization;
using System.Diagnostics;
using System.Text.RegularExpressions;
using ESRI.ArcLogistics.DomainObjects;
using ESRI.ArcLogistics.App.Pages;
using System.Windows.Media;
using ESRI.ArcLogistics.Data;
using System.Collections;

namespace ESRI.ArcLogistics.App.Converters
{
    /// <summary>
    /// Converts object's (route or vehicle) specialties collection to a set of Inlines.
    /// </summary>
    [ValueConversion(typeof(object), typeof(ICollection<Inline>))]
    internal class ObjectWithSpecialtiesComboBoxNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            List<Inline> inlines = new List<Inline>();

            if (value != null && !string.IsNullOrEmpty(value.ToString()))
            {
                Vehicle inputVehicle;
                Driver inputDriver;

                if (value is Vehicle)
                {
                    inputVehicle = (Vehicle)value;
                    inlines.Add(new Run(inputVehicle.Name + " "));
                    _CreateListOfSpecialties(inlines, inputVehicle);
                }

                else if (value is Driver)
                {
                    inputDriver = (Driver)value;
                    inlines.Add(new Run(inputDriver.Name + " "));
                    _CreateListOfSpecialties(inlines, inputDriver);
                }            
            }

            return inlines;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        /// <summary>
        /// Method creates collection of specialties an add it to the end of object name
        /// </summary>
        /// <param name="inlines"></param>
        /// <param name="specialties"></param>
        private void _CreateListOfSpecialties(List<Inline> inlines, object parentObject)
        {
            string collectionOfSpecialties = null;
            StringBuilder sb = new StringBuilder();

            if (parentObject is Vehicle)
                foreach (VehicleSpecialty specialty in ((Vehicle)parentObject).Specialties)
                {
                    if (0 < sb.Length)
                        sb.Append(", ");
                    sb.Append(specialty.Name);
                }
            else if (parentObject is Driver)
                foreach (DriverSpecialty specialty in ((Driver)parentObject).Specialties)
                {
                    if (0 < sb.Length)
                        sb.Append(", ");
                    sb.Append(specialty.Name);
                }

            collectionOfSpecialties += sb.ToString();

            if (!string.IsNullOrEmpty(collectionOfSpecialties))
            {
                collectionOfSpecialties = string.Format((string)App.Current.FindResource("SpecialtiesCellText"), collectionOfSpecialties);
                Italic noOrders = new Italic(new Run(collectionOfSpecialties));
                noOrders.Foreground = new SolidColorBrush(Colors.Gray);
                noOrders.FontSize = (double)App.Current.FindResource("StandartHelpFontSize");
                inlines.Add(noOrders);
            }
        }
    }
}
