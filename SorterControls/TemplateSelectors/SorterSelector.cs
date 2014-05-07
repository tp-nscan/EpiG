using System;
using System.Windows;
using System.Windows.Controls;
using SorterControls.ViewModel;

namespace SorterControls.TemplateSelectors
{
    public class SorterSelector : DataTemplateSelector
    {
        public DataTemplate UnstagedSorterTemplate { get; set; }

        public DataTemplate StagedSorterTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var screenVm = item as ISorterVm;

            if (screenVm != null)
            {
                switch (screenVm.SorterVmType)
                {
                    case SorterVmType.Unstaged:
                        return UnstagedSorterTemplate;
                    case SorterVmType.Staged:
                        return StagedSorterTemplate;
                    default:
                        throw new Exception("IScreenVm template not found in Clinical.Resources.ScreenSelector.SelectTemplate");
                }
            }

            return DefaultTemplate;
        }
    


    }
}
