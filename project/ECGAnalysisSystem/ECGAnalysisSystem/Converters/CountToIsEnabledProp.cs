﻿using System;
using System.Globalization;
using System.Windows.Data;

namespace ECGAnalysisSystem.Converters
{
    /// <summary>
    /// Auxiliary class for button state controlling
    /// </summary>
    class CountToIsEnabledProp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) value == 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}