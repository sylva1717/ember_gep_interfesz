using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HCI.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace HCI
{
    class GridTemplateSelector : DataTemplateSelector
    {
        public DataTemplate PlusTemplate { get; set; }
        public DataTemplate MinusTemplate { get; set; }

        protected override Windows.UI.Xaml.DataTemplate SelectTemplateCore(object item, Windows.UI.Xaml.DependencyObject container)
        {
            var dataItem = item as Finances;
            var uiElement = container as UIElement;

            if (dataItem.Amount < 0) // Couldn't come up with a better logic :)
            {
                return MinusTemplate;
            }
            else
            {
                return PlusTemplate;
            }
            
        }
    }
}
