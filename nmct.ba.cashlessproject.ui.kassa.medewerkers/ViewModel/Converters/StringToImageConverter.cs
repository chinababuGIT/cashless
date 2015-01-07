﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace nmct.ba.cashlessproject.ui.kassa.ViewModel
{
    public class StringToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                byte[] input = (byte[])value;
                if (input.Length <= 1)
                    return new BitmapImage(new Uri(@"pack://application:,,,/nmct.ba.cashlessproject.ui.kassa;component/Themes/images/empty.png"));
                else
                    return BitmapImageFromBytes(input);
            }
            else
                return new BitmapImage(new Uri(@"pack://application:,,,/nmct.ba.cashlessproject.ui.kassa;component/Themes/images/empty.png"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }

        private BitmapImage BitmapImageFromBytes(byte[] bytes)
        {
            int max=bytes.Max();
            if (max == 0) { return new BitmapImage(new Uri(@"pack://application:,,,/nmct.ba.cashlessproject.ui;component/Themes/images/empty.png")); }
            BitmapImage image = null;
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                stream.Seek(0, SeekOrigin.Begin);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                image = new BitmapImage();
                image.BeginInit();
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.StreamSource.Seek(0, SeekOrigin.Begin);
                image.EndInit();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Close();
                stream.Dispose();
            }
            return image;
        }
    }
}
