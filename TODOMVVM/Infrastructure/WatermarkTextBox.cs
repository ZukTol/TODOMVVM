using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace TODOMVVM.Infrastructure {
    public class WatermarkTextBox : TextBox {
        static WatermarkTextBox() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkTextBox), new FrameworkPropertyMetadata(typeof(WatermarkTextBox)));
        }

        public static readonly DependencyProperty WatermarkTextProperty = DependencyProperty.Register(
            "WatermarkText", typeof (string), typeof (WatermarkTextBox), new PropertyMetadata(default(string)));

        public string WatermarkText {
            get { return (string) GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        public static readonly DependencyProperty WatermarkTemplateProperty = DependencyProperty.Register(
            "WatermarkTemplate", typeof (DataTemplate), typeof (WatermarkTextBox), new PropertyMetadata(default(DataTemplate)));

        public DataTemplate WatermarkTemplate {
            get { return (DataTemplate) GetValue(WatermarkTemplateProperty); }
            set { SetValue(WatermarkTemplateProperty, value); }
        }
    }
}
