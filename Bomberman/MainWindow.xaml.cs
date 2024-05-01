using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;
using Bomberman.model;

namespace Bomberman.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private int cellSize = 39;
        private double ratioSpeed = 2;
        private double left = 0;
        private Random rdn = new(); 
        private List<string> diractions = new();
        private DispatcherTimer tick = new DispatcherTimer();
        private BoardGame pole = new BoardGame();
        private Player player;
        private Image playerImg;
        private List<Image> blocks = new();
        private Dictionary<Enemy, Image> enemyes = new Dictionary<Enemy, Image>();
        private Dictionary<Brick, Image> bricks = new Dictionary<Brick, Image>();

        public MainWindow()
        {
            InitializeComponent();
            CreatePole();
            tick.Tick += new EventHandler(Update);
            tick.Interval = new TimeSpan(0, 0, 0, 0, 16);
            tick.Start();

        }
        private void CreatePole()
        {
            int bottom = 461;
            int right = 761;
            
            for(int y = 0; y < pole.Map.Length; y++)
            {
                for(int x = 0; x < pole.Map[y].Length; x++)
                {
                    var item = new Image();
                    item.Width = cellSize;
                    item.Height = cellSize;
                    if (pole.Map[y][x] == '#')
                    {
                        item.Source = new BitmapImage(new Uri("/data/block/0.png", UriKind.Relative));
                        blocks.Add(item);
                        Panel.SetZIndex(item, 2);

                    }
                    if (pole.Map[y][x] == '@')
                    {
                        item.Source = new BitmapImage(new Uri("/data/brick/0.png", UriKind.Relative));
                        var bric = new Brick();
                        bricks.Add(bric, item);
                        Panel.SetZIndex(item, 2);
                    }
                    if (pole.Map[y][x] == '0')
                    {
                        item.Source = new BitmapImage(new Uri("/data/player/moveY/0.png", UriKind.Relative));
                        player = new(x * cellSize, y * cellSize, right - x * cellSize, bottom - y * cellSize, cellSize);
                        playerImg = item;
                        Panel.SetZIndex(item, 1);

                    }
                    if (pole.Map[y][x] == '1')
                    {
                        item.Source = new BitmapImage(new Uri("/data/enemy/Valcom/move/0.png", UriKind.Relative));
                        var enemy = new Enemy(x * cellSize, y * cellSize, right - x * cellSize, bottom - y * cellSize, cellSize);
                        diractions.Clear();
                        if (pole.Map[y - 1][x] == ' ' || pole.Map[y + 1][x] == ' ')
                        {
                            diractions.Add("down");
                            diractions.Add("up");
                        }
                        if (pole.Map[y][x - 1] == ' ' || pole.Map[y][x + 1] == ' ')
                        {
                            diractions.Add("right");
                            diractions.Add("left");
                        }
                        if(diractions.Count != 0)                        
                            enemy.Diraction = diractions[rdn.Next(diractions.Count)];
                        Panel.SetZIndex(item, 0);
                        enemyes.Add(enemy, item);

                    }

                    item.Stretch = Stretch.Fill;
                    item.Margin = new Thickness(x * cellSize, y * cellSize, right - x * cellSize, bottom - y * cellSize);
                    camera.Children.Add(item);
                }
            }
        }
        private void Update(object sender, EventArgs e)
        {
            foreach(var enemy in enemyes)
            {
                enemy.Key.move();
                enemy.Value.Source = new BitmapImage(new Uri(enemy.Key.Update(), UriKind.Relative));
                enemy.Value.Margin = new Thickness(enemy.Key.Left, enemy.Key.Top, enemy.Key.Right, enemy.Key.Bottom);
                foreach(var block in blocks)
                {
                    enemy.Key.CheckIntersection(block.Margin.Left, block.Margin.Top);
                }
                foreach (var block in bricks)
                {
                    enemy.Key.CheckIntersection(block.Value.Margin.Left, block.Value.Margin.Top);
                }
                enemy.Key.SwapRdnDiraction(pole.Map);


            }
        }
        private void Intersection()
        {

        }
        private void movePlayer(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    player.Diraction = "up";
                    break;
                case Key.S:
                    player.Diraction = "down";
                    break;
                case Key.A:
                    player.Diraction = "left";
                    playerImg.FlowDirection = FlowDirection.LeftToRight;
                    camera.Margin = new Thickness(left, 80, 0, -13);
                    if (player.Left >= left + Width / 2 - cellSize &&
                        0 > left && !(player.Left > pole.Map[0].Length * cellSize - Width / 2))
                        left += player.Speed * ratioSpeed;
                    break;
                case Key.D:
                    player.Diraction = "right";
                    playerImg.FlowDirection = FlowDirection.RightToLeft;
                    camera.Margin = new Thickness(left, 80, 0, -13);
                    if(player.Left >= left + Width / 2 + cellSize &&
                        (pole.Map[0].Length + 3) * cellSize - player.Speed * ratioSpeed > Math.Abs(left)  + Width / 2 &&
                        !(player.Left > (pole.Map[0].Length - 1 )* cellSize - Width / 2))
                        left -= player.Speed * ratioSpeed;
                    break;
            }
            player.move();
            playerImg.Source = new BitmapImage(new Uri(player.Update(), UriKind.Relative));
            playerImg.Margin = new Thickness(player.Left, player.Top, player.Right, player.Bottom);
        }
    }
}