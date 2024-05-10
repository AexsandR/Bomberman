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
using static System.Formats.Asn1.AsnWriter;


namespace Bomberman.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        private bool statusMoveCamera = true;
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
                    item.Width = Setting.CellSize;
                    item.Height = Setting.CellSize;
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
                        player = new(x * Setting.CellSize, y * Setting.CellSize, right - x * Setting.CellSize, bottom - y * Setting.CellSize);
                        playerImg = item;
                        Panel.SetZIndex(item, 1);

                    }
                    if (pole.Map[y][x] == '1')
                    {
                        item.Source = new BitmapImage(new Uri("/data/enemy/Valcom/move/0.png", UriKind.Relative));
                        var enemy = new Enemy(x * Setting.CellSize, y * Setting.CellSize, right - x * Setting.CellSize, bottom - y * Setting.CellSize);
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
                    item.Margin = new Thickness(x * Setting.CellSize, y * Setting.CellSize, right - x * Setting.CellSize, bottom - y * Setting.CellSize);
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
                if (enemy.Key.Diraction == "right")
                    enemy.Value.FlowDirection = FlowDirection.LeftToRight;
                else
                    enemy.Value.FlowDirection = FlowDirection.RightToLeft;
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
            foreach (var enemy in enemyes)
            {
                if (player.CheckIntersection(enemy.Key.Left, enemy.Key.Top, true))
                {
                    player.Alive = false;

                }
            }
            if (!player.Alive)
                playerImg.Source = new BitmapImage(new Uri(player.Dead(), UriKind.Relative));
            
        }
        private void MoveCamera(string diracation)
        {
            if (diracation == "left" && player.Left >= left + Width / 2 - Setting.CellSize &&
                    0 > left && !(player.Left > pole.Map[0].Length * Setting.CellSize - Width / 2))
                left += player.Speed * ratioSpeed ;
            if (diracation == "right" && player.Left >= left + Width / 2 + Setting.CellSize &&
                    (pole.Map[0].Length + 3) * Setting.CellSize - player.Speed * ratioSpeed > Math.Abs(left) + Width / 2 &&
                    !(player.Left > (pole.Map[0].Length - 1) * Setting.CellSize - Width / 2))
                left -= player.Speed * ratioSpeed;
            //тут есть баг
            camera.Margin = new Thickness(left, 80, 0, -13);
        }
        private void intersectionPlayer()
        {
            foreach (var block in blocks)
            {
                if (player.CheckIntersection(block.Margin.Left, block.Margin.Top))
                    statusMoveCamera = false;
            }
            foreach (var bric in bricks)
            {
                if (player.CheckIntersection(bric.Value.Margin.Left, bric.Value.Margin.Top))
                    statusMoveCamera = false;
            }

            if (statusMoveCamera)
                MoveCamera(player.Diraction);
            statusMoveCamera = true;
        }
        private void MovePlayer(object sender, KeyEventArgs e)
        {
            if (!player.Alive)
                return;
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
                    break;
                case Key.D:
                    player.Diraction = "right";
                    playerImg.FlowDirection = FlowDirection.RightToLeft;
                    break;
                default: return;
            }
            player.move();
            playerImg.Source = new BitmapImage(new Uri(player.Update(), UriKind.Relative));
            intersectionPlayer();
            playerImg.Margin = new Thickness(player.Left, player.Top, player.Right, player.Bottom);




        }
    }
}