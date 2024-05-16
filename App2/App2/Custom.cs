using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
namespace App2
{
    public class Custom : Entry
    {
        private string _mask = ""; // Маска по умолчанию

        public string Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }
        public static readonly BindableProperty BorderColorProperty =
               BindableProperty.Create(
                   nameof(BorderColor),
                   typeof(Color),
                   typeof(Custom),
                   Color.Gray);

        // Gets or sets BorderColor value
        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create(
            nameof(BorderWidth),
            typeof(int),
            typeof(Custom),
            Device.OnPlatform<int>(1, 2, 2));

        // Gets or sets BorderWidth value
        public int BorderWidth
        {
            get { return (int)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(double),
            typeof(Custom),
            Device.OnPlatform<double>(6, 7, 7));

        // Gets or sets CornerRadius value
        public double CornerRadius
        {
            get { return (double)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(
            nameof(IsCurvedCornersEnabled),
            typeof(bool),
            typeof(Custom),
            true);

        // Gets or sets IsCurvedCornersEnabled value
        public bool IsCurvedCornersEnabled
        {
            get { return (bool)GetValue(IsCurvedCornersEnabledProperty); }
            set { SetValue(IsCurvedCornersEnabledProperty, value); }
        }
        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            var text = e.NewTextValue;

            if (!string.IsNullOrEmpty(_mask)) // Если маска установлена
            {
                // Применяем маску, заменяя символы, если они отличаются от X
                for (int i = 0; i < text.Length; i++)
                {
                    if (_mask.Length > i && _mask[i] != 'X')
                    {
                        text = text.Insert(i, _mask[i].ToString());
                    }
                }

                // Удаляем символы, которые выходят за пределы маски
                if (text.Length > _mask.Length)
                {
                    text = text.Substring(0, _mask.Length);
                }
            }

            entry.Text = text; // Устанавливаем отформатированный текст
        }
    }
}