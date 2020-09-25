using System;
using System.Windows.Controls;

namespace UpdatesClient.UI.Controllers
{
    /// <summary>
    /// Логика взаимодействия для NotifyList.xaml
    /// </summary>
    public partial class NotifyList : UserControl
    {
        public static NotifyList NotifyPanel;

        public NotifyList()
        {
            InitializeComponent();
            if (NotifyPanel != null) throw new Exception("Допускается один элемент");
            NotifyPanel = this;
        }
    }
}
