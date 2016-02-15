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
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace Duelyst.DeckConstructor
{
    public static class ToPictureProcessor
    {
        //TODO: Define global config for this:
        private static int _cardRowIntervalPx = 40;

        private static int _cardColIntervalPx = 20;

        private static int _borderHLength = 60;

        private static int _borderWLength = 40;

        private static int _cardsInrow = 4;

        private static int _cardDispLayer = 8;

        private static Image GetBgImage()
        {
            var names = Assembly.GetExecutingAssembly().GetManifestResourceNames();
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
                var cptRight = pRight - _cardDispLayer * i;
                var cptDown = pDown - _cardDispLayer * i;
                g.DrawImage(toDraw, new Rectangle(cptRight, cptDown, width, heigth));
            }
        }

        public static Image SquadToImage(Squad squad)
        {
            //Предполагается, что все карты одного разрешения
            //Получить разрешение карты владельца отряда
            var ownerHlen = Convert.ToInt32(squad.SquadOwner.Image.Height);
            var ownerWlen = Convert.ToInt32(squad.SquadOwner.Image.Width);
            //Получить общее количество уникальных карт в отряде
            var uniqueCardList = squad.SquadCards.Skip(1).Distinct().ToArray();
            var uniqueCardsCount = uniqueCardList.Count();
            var rows = Convert.ToInt32(Math.Ceiling((decimal)uniqueCardsCount / _cardsInrow));
            //результирующая высота картинки
            var resultImageH = Convert.ToInt32(ownerHlen * rows + _cardRowIntervalPx * rows + _borderHLength * 2);
            var resultImageW = Convert.ToInt32(ownerWlen * _cardsInrow + _cardColIntervalPx * _cardsInrow + _borderWLength * 2);
            var resultImage = new Bitmap(resultImageW, resultImageH);
            var img = GetBgImage().ResizeImage(new Size(resultImageW, resultImageH));;
            using (var canvas = Graphics.FromImage(resultImage))
            {
                canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //Нарисовать подложку, определить множитель увеличения по высоте
                //и добавить к ширине
                canvas.DrawImage(img, new Rectangle(0, 0, resultImageW, resultImageH));
                for (int col = 0; col < _cardsInrow; col++)
                {
                    var rightStepOver = Convert.ToInt32((col * ownerWlen) + _cardColIntervalPx) + _borderWLength;
                    for (int row = 0; row < rows; row++)
                    {
                        var idx = row * _cardsInrow + col;
                        if (idx < uniqueCardsCount)
                        {
                            var card = uniqueCardList[idx];
                            var downStepOver = Convert.ToInt32((row * ownerHlen) + _cardRowIntervalPx) + _borderHLength;
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
