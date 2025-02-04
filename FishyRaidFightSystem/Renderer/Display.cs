using FishyRaidFightSystem.Logic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FishyRaidFightSystem.Renderer
{
    internal class Display : FrameworkElement
    {
        Size Area; //Háttér méret
        static Random R = new Random();
        IGameModel model;

        public void SetupSizes(Size area)
        {
            this.Area = area;
            this.InvalidateVisual(); //Újrarajzolás
        }

        public void SetupModel(IGameModel model)
        {
            this.model = model;
            this.model.Changed += (sender, eventargs) => this.InvalidateVisual();
        }

        public Brush MapBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "arenakep.png"), UriKind.RelativeOrAbsolute)));
            }
        }

        public Brush FishBrush
        {
            get
            {
                return new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "fishmodel.png"), UriKind.RelativeOrAbsolute)));
            }
        }

        [Obsolete]
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Area.Width > 0 && Area.Height > 0 && model != null)
            {
                drawingContext.DrawRectangle(MapBrush, null, new Rect(0, 0, Area.Width, Area.Height));

                FormattedText formattedText = new FormattedText(
        "Wave " + model.Palyaszam + "/" + model.MaxPalyaszam,
        CultureInfo.GetCultureInfo("en-us"),
        FlowDirection.LeftToRight,
        new Typeface("Verdana"),
        64,
        Brushes.Yellow);
                FormattedText halstatusz = new FormattedText(
        "ENERGY: " + model.Jatekos.Energy + "   SPELLS: " + model.KovetkezoHal+" kor:"+model.Korszam,
        CultureInfo.GetCultureInfo("en-us"),
        FlowDirection.LeftToRight,
        new Typeface("Verdana"),
        32,
        Brushes.Yellow);
                drawingContext.DrawText(formattedText, new Point(Area.Width / 2 - 200, 0));
                drawingContext.DrawText(halstatusz, new Point(Area.Width / 24, Area.Height / 1.1));

                foreach (var item in model.Jatekos.FishesInFight)
                {
                    if (item.sorszam == 1)
                    {
                        item.poziciokezel();
                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 4 - 100;
                            item.y = (double)Area.Height / 2 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {

                            if (model.Korszam == 0 && model.Jatekos.FishesInFight[0].meghalt == false && model.Enemy.FishesInFight[0].tamad == false && model.Enemy.FishesInFight[1].tamad == false && model.Enemy.FishesInFight[2].tamad == false)
                            {
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "leftarrow.png"), UriKind.RelativeOrAbsolute))), null, new Rect(item.x + 200, item.y + 100 + item.pozicio, 70, 40));
                            }

                            int szazalek = (int)Math.Round((double)(100 * item.Elet) / item.Maxhp);

                            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 3.9 - 100, Area.Height / 2 - 100 + item.pozicio, szazalek * 1.5, 10));

                        }
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.Eleresiut, UriKind.RelativeOrAbsolute))), null, new Rect(item.x, item.y + item.pozicio, 200, 200));



                        if (item.lovedeke.aktiv == true)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri( item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }
                        else
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }

                    }
                    else if (item.sorszam == 2)
                    {
                        item.poziciokezel();

                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 15 - 100;
                            item.y = (double)Area.Height / 8 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {

                            if (model.Korszam == 2 && model.Jatekos.FishesInFight[1].meghalt == false && model.Enemy.FishesInFight[0].tamad == false && model.Enemy.FishesInFight[1].tamad == false && model.Enemy.FishesInFight[2].tamad == false)
                            {
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "leftarrow.png"), UriKind.RelativeOrAbsolute))), null, new Rect(item.x + 200, item.y + 100 + item.pozicio, 70, 40));
                            }
                            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 14 - 100, Area.Height / 8 - 100 + item.pozicio, item.Elet * 1.5, 10));
                        }

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.Eleresiut, UriKind.RelativeOrAbsolute))), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        if (item.lovedeke.aktiv)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }
                        else
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }
                    }
                    else if (item.sorszam == 3)
                    {
                        item.poziciokezel();

                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 15 - 100;
                            item.y = (double)Area.Height / 1.3 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {

                            if (model.Korszam == 4 && model.Jatekos.FishesInFight[2].meghalt == false && model.Enemy.FishesInFight[0].tamad == false && model.Enemy.FishesInFight[1].tamad == false && model.Enemy.FishesInFight[2].tamad == false)
                            {
                                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(System.IO.Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.Parent.FullName + "/Images", "leftarrow.png"), UriKind.RelativeOrAbsolute))), null, new Rect(item.x + 200, item.y + 100 + item.pozicio, 70, 40));
                            }
                            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 14 - 100, Area.Height / 1.3 - 100 + item.pozicio, item.Elet * 1.5, 10));
                        }

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.Eleresiut, UriKind.RelativeOrAbsolute))), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        if (item.lovedeke.aktiv)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }
                        if (item.lovedeke.aktiv == false)
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }
                    }

                    if (item.Buff != null)
                    {
                        if (item.Buff.mutat)
                        {
                            FormattedText szovege = new FormattedText(
            item.Buff.message,
            CultureInfo.GetCultureInfo("en-us"),
            FlowDirection.LeftToRight,
            new Typeface("Verdana"),
            80,
            Brushes.Yellow);

                            if (item.Buff.mutatasleszedve == false)
                            {
                                item.Buff.SzovegLeszed();

                            }


                            drawingContext.DrawText(szovege, new Point(Area.Width / 2 - 400, Area.Height / 2 - 200));
                        }
                    }
                }
                foreach (var item in model.Enemy.FishesInFight)
                {
                    if (item.sorszam == 1)
                    {
                        item.poziciokezel();

                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 1.4 - 100;
                            item.y = (double)Area.Height / 2 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {
                            int szazalek = (int)Math.Round((double)(100 * item.Elet) / item.Maxhp);



                            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 1.35 - 100, Area.Height / 2 - 100 + item.pozicio, szazalek * 1.5, 10));
                        }

                        BitmapImage enemy1 = new BitmapImage(new Uri(Path.Combine("Images", item.Eleresiut), UriKind.RelativeOrAbsolute));
                        TransformedBitmap tenemy1 = new TransformedBitmap();
                        tenemy1.BeginInit();
                        tenemy1.Source = enemy1;
                        tenemy1.Transform = new ScaleTransform(-1, 1);
                        tenemy1.EndInit();

                        if (model.Gamemode == "arena")
                        {
                            drawingContext.DrawRectangle(new ImageBrush(tenemy1), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }
                        else
                        {
                            drawingContext.DrawRectangle(new ImageBrush(enemy1), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }

                        //Itt tartunk
                        
                        

                        if (item.lovedeke.aktiv == false)
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }


                        if (item.lovedeke.aktiv)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }
                    }
                    else if (item.sorszam == 2)
                    {
                        item.poziciokezel();

                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 1.2 - 100;
                            item.y = (double)Area.Height / 8 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {
                            int szazalek = (int)Math.Round((double)(100 * item.Elet) / item.Maxhp);
 

                             drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 1.159 - 100, Area.Height / 8 - 100 + item.pozicio, szazalek * 1.5, 10));
                        }

                        BitmapImage enemy2 = new BitmapImage(new Uri(Path.Combine("Images", item.Eleresiut), UriKind.RelativeOrAbsolute));
                        TransformedBitmap tenemy2 = new TransformedBitmap();
                        tenemy2.BeginInit();
                        tenemy2.Source = enemy2;
                        tenemy2.Transform = new ScaleTransform(-1, 1);
                        tenemy2.EndInit();


                        if (model.Gamemode == "arena")
                        {
                            drawingContext.DrawRectangle(new ImageBrush(tenemy2), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }
                        else
                        {
                            drawingContext.DrawRectangle(new ImageBrush(enemy2), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }

                        if (item.lovedeke.aktiv == false)
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }

                        if (item.lovedeke.aktiv)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }
                    }
                    else if (item.sorszam == 3)
                    {
                        item.poziciokezel();

                        if (!item.tamad)
                        {
                            item.x = (double)Area.Width / 1.2 - 100;
                            item.y = (double)Area.Height / 1.3 - 100;
                        }

                        if (item.csikmutat && item.Elet > 0)
                        {
                            int szazalek = (int)Math.Round((double)(100 * item.Elet) / item.Maxhp);

                            drawingContext.DrawRectangle(Brushes.Red, null, new Rect(Area.Width / 1.159 - 100, Area.Height / 1.3 - 100 + item.pozicio, szazalek * 1.5, 10));
                        }

                        BitmapImage enemy3 = new BitmapImage(new Uri(Path.Combine("Images", item.Eleresiut), UriKind.RelativeOrAbsolute));
                        TransformedBitmap tenemy3 = new TransformedBitmap();
                        tenemy3.BeginInit();
                        tenemy3.Source = enemy3;
                        tenemy3.Transform = new ScaleTransform(-1, 1);
                        tenemy3.EndInit();

                        if (model.Gamemode == "arena")
                        {
                            drawingContext.DrawRectangle(new ImageBrush(tenemy3), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }
                        else
                        {
                            drawingContext.DrawRectangle(new ImageBrush(enemy3), null, new Rect(item.x, item.y + item.pozicio, 200, 200));
                        }

                        //Lovedék kirajzolás

                        if (item.lovedeke.aktiv == false)
                        {
                            item.lovedeke.x = item.x;
                            item.lovedeke.y = item.y;
                        }

                        if (item.lovedeke.aktiv)
                        {
                            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(item.lovedeke.eleres, UriKind.RelativeOrAbsolute))), null, new Rect(item.lovedeke.x, item.lovedeke.y + item.pozicio - 5, 100, 100));
                        }


                    }
                }
            }

        }
    }
}
