﻿using System.Windows;

namespace YUtils
{
    public class MsgUtils
    {
        public static MessageBoxResult MessageBox(string message, MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information, params object[] parameters)
        {
            string msg = message;
            if (parameters.Length > 0)
            {
                msg = string.Format(message, parameters);
            }
            return System.Windows.MessageBox.Show(msg, "YUtils", button, icon);
        }

    }
}