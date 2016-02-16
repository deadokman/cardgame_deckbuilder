using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Duelyst.DeckConstructor.CardCatalog.Squad;

using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace Duelyst.DeckConstructor
{
    public static class ToPictureProcessor
    {
        //TODO: Define global config for this:
        public static int CardRowIntervalPx = 40;

        public static int CardColIntervalPx = 20;

        public static int BorderHLength = 60;

        public static int BorderWLength = 40;

        public static int CardsInrow = 4;

        public static int CardDispLayer = 8;

        private static Image GetBgImage()
        {
            using (var st = Assembly.GetExecutingAssembly().GetManifestResourceStream("Duelyst.DeckConstructor.AdditionalResources.imgBg_downgraded_contrast.png"))
            {
                return new Bitmap(st);
            }
        }

        private static void DrawLables(Squad squad, Graphics g, int w, int h)
        {
            //Расчитать координаты для ссылки на сайт, названия отряда и отрисовать их
            g.DrawString(squad.SquadName, new Font("Segoe UI", 10), new SolidBrush(Color.White), new PointF(0, 0));
        }

        private static void DrawImageCycled(
            Graphics g,
            Image toDraw,
            int pRight,
            int pDown,
            int width,
            int heigth,
            int instanceCount)
        {
            for (var i = instanceCount; i > 0; i--)
            {
                var cptRight = pRight - CardDispLayer * i;
                var cptDown = pDown - CardDispLayer * i;
                g.DrawImage(toDraw, new Rectangle(cptRight, cptDown, width, heigth));
            }
        }

        public static Bitmap SquadToImage(Squad squad)
        {
            //Предполагается, что все карты одного разрешения
            //Получить разрешение карты владельца отряда
            var ownerHlen = Convert.ToInt32(squad.SquadOwner.Image.Height);
            var ownerWlen = Convert.ToInt32(squad.SquadOwner.Image.Width);
            //Получить общее количество уникальных карт в отряде
            var uniqueCardList = squad.SquadCards.Skip(1).Distinct().ToArray();
            var uniqueCardsCount = uniqueCardList.Count();
            var rows = Convert.ToInt32(Math.Ceiling((decimal)uniqueCardsCount / CardsInrow));
            //результирующая высота картинки
            var resultImageH = Convert.ToInt32(ownerHlen * rows + CardRowIntervalPx * rows + BorderHLength * 2);
            var resultImageW = Convert.ToInt32(ownerWlen * CardsInrow + CardColIntervalPx * CardsInrow + BorderWLength * 2);
            var resultImage = new Bitmap(resultImageW, resultImageH);
            var img = GetBgImage().ResizeImage(new Size(resultImageW, resultImageH));;
            using (var canvas = Graphics.FromImage(resultImage))
            {
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //Нарисовать подложку, определить множитель увеличения по высоте
                //и добавить к ширине
                canvas.DrawImage(img, new Rectangle(0, 0, resultImageW, resultImageH));
                for (int col = 0; col < CardsInrow; col++)
                {
                    var rightStepOver = Convert.ToInt32((col * ownerWlen) + CardColIntervalPx * col) + BorderWLength;
                    for (int row = 0; row < rows; row++)
                    {
                        var idx = row * CardsInrow + col;
                        if (idx < uniqueCardsCount)
                        {
                            var card = uniqueCardList[idx];
                            var downStepOver = Convert.ToInt32((row * ownerHlen) + CardRowIntervalPx * row) + BorderHLength;
                            DrawImageCycled(
                                canvas,
                                new Bitmap(card.Image.StreamSource),
                                rightStepOver,
                                downStepOver,
                                ownerWlen,
                                ownerHlen,
                                squad.CardSquadCount[card.CardId]);
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                DrawLables(squad, canvas, resultImageH, resultImageW);
                canvas.Save();
            }
#if DEBUG
            resultImage.Save("test.bmp");
#endif
            return resultImage;
        }
    }
}
