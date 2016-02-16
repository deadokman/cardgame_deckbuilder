using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;

using Duelyst.DeckConstructor.CardCatalog.Squad;

using Color = System.Drawing.Color;
using Image = System.Drawing.Image;
using Size = System.Drawing.Size;

namespace Duelyst.DeckConstructor
{
    public static class ToPictureProcessor
    {
        #region ImageGenerationConfigurationProps

        //TODO: Define global config for this:
        /// <summary>
        /// Интервал между строками карт
        /// </summary>
        public static int CardRowIntervalPx = 40;

        /// <summary>
        /// Интервал между столбцами
        /// </summary>
        public static int CardColIntervalPx = 20;

        /// <summary>
        /// Высота рамки по вертикали
        /// </summary>
        public static int BorderHLength = 60;

        /// <summary>
        /// Высота рамки по ширине
        /// </summary>
        public static int BorderWLength = 40;

        /// <summary>
        /// Число крат в строке
        /// </summary>
        public static int CardsInrow = 4;

        /// <summary>
        /// Карт в подложке слоя
        /// </summary>
        public static int CardDispLayer = 8;

        /// <summary>
        /// Декремент размера шрифта названия отряда
        /// </summary>
        public static int SquadFontScaleFactor = 3;

        /// <summary>
        /// Декремент размера шрифта названия отряда
        /// </summary>
        public static int OffsetSquadNameFactor = 10;

        /// <summary>
        /// Отображать количество карт
        /// </summary>
        public static bool DisplayCardCntOverlay = true;

        /// <summary>
        /// Размерность шрифта для подписи количества карт
        /// </summary>
        public static int OverlayCntFontSize = 25;

        /// <summary>
        /// Смещение надписи с числом карт относительно самой карты по высоте
        /// </summary>
        public static int OverCntOffsetH = 40;

        /// <summary>
        /// Смещение надписи с числом карт относительно самой карты по высоте
        /// </summary>
        public static int OverCntOffsetW = 20;

        /// <summary>
        /// Контроллер размерности логотипа
        /// </summary>
        public static int LogoScaleFactor = 50;

        #endregion

        private static int LogoScale;

        private static Image GetBgImage()
        {
            using (var st = Assembly.GetExecutingAssembly().GetManifestResourceStream("Duelyst.DeckConstructor.AdditionalResources.imgBg_downgraded_contrast.png"))
            {
                return new Bitmap(st);
            }
        }

        private static Image GetLogoImage()
        {
            using (var st = Assembly.GetExecutingAssembly().GetManifestResourceStream("Duelyst.DeckConstructor.AdditionalResources.kaban_logo.png"))
            {
                return new Bitmap(st);
            }
        }

        /// <summary>
        /// Отрисовать текстовые аттрибуты и доп. информацию по колоде
        /// </summary>
        /// <param name="squad">Отряд</param>
        /// <param name="g">Канвасы</param>
        /// <param name="w">Ширина картинки</param>
        /// <param name="h">Высота картинки</param>
        private static void DrawLables(Squad squad, Graphics g, int h, int w)
        {
            var defaultBrush = new SolidBrush(Color.White);
            var captionTextSize = BorderHLength / SquadFontScaleFactor;
            var pOffset = BorderHLength / OffsetSquadNameFactor;
            //Расчитать координаты для ссылки на сайт, названия отряда и отрисовать их
            DrawCustomText(g, String.Format("{0} - {1}",squad.SquadName, squad.SquadOwner.Name), defaultBrush, captionTextSize, BorderWLength, pOffset);
            //Поместить лого в подпись
            //Пересчитать размер лого, перерисовать и поместить на изображение
            var logoImg = GetLogoImage().ResizeImage(new Size(LogoScale, LogoScale), false);
            var wPos = w/2 - BorderWLength - logoImg.Size.Width/2;
            var hPos = h - BorderHLength - logoImg.Size.Height;
            g.DrawImage(logoImg, new Point(wPos, hPos));
        }

        private static void DrawCustomText(Graphics g, string text, SolidBrush colorBrush, int size, float px, float py, bool uppecase = true)
        {
            var drawFont = new Font("Arial", size, FontStyle.Bold);
            var drawPoint = new PointF(px, py);
            var drawFormat = new StringFormat();
            if (uppecase)
            {
                text = text.ToUpper();
            }

            g.DrawString(text, drawFont, colorBrush, drawPoint, drawFormat);
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

                if (i == 1 && DisplayCardCntOverlay)
                {
                    var cntTxt = String.Format("X{0}", instanceCount);
                    var posH = cptDown + heigth - OverCntOffsetH;
                    var posW = cptRight + width / 2 - OverCntOffsetW;
                    DrawCustomText(g, cntTxt, new SolidBrush(Color.White), OverlayCntFontSize, posW, posH);
                }
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
            LogoScale = BorderHLength + LogoScaleFactor;
            var resultImageH = Convert.ToInt32(ownerHlen * rows + CardRowIntervalPx * rows + BorderHLength * 2 + LogoScale);
            var resultImageW = Convert.ToInt32(ownerWlen * CardsInrow + CardColIntervalPx * CardsInrow + BorderWLength * 2);
            var resultImage = new Bitmap(resultImageW, resultImageH);
            var img = GetBgImage().ResizeImage(new Size(resultImageW, resultImageH), false);
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

            return resultImage;
        }
    }
}
