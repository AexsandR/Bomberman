using Bomberman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            (new MainWindow()).Show();
            Close();
        }

        private void InformationСreator(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("работу сделал: Боровков Адександр Владимирович группа 23-ИСбо-1а\n управление:\n W - вверх\n S - вниз\n A - вправо\n D - влево\n L - поставить бомбу\n Цель игры: уничтожить нечесть и найти выход");
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MovingWin(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }
    }
}
