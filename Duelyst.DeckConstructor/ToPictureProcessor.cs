using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Duelyst.DeckConstructor.CardCatalog.Squad;

namespace Duelyst.DeckConstructor
{
    public static class ToPictureProcessor
    {
        //TODO: Define global config for this:
        private static int _cardRowIntervalPx = 20;

        private static int _cardColIntervalPx = 10;

        private static int _borderHLength = 60;

        private static int _borderWLength = 40;

        private static int _cardsInrow = 4;

        public static Image SquadToImage(Squad squad)
        {
            //Предполагается, что все карты одного разрешения
            //Получить разрешение карты владельца отряда
            var ownerHlen = squad.SquadOwner.Image.Height;
            var ownerWlen = squad.SquadOwner.Image.Width;
            //Получить общее количество уникальных карт в отряде
            var uniqueCardList = squad.SquadCardsList.Distinct();
            var uniqueCardsCount = uniqueCardList.Count();
            var rows = Convert.ToInt32(Math.Ceiling((decimal)uniqueCardsCount / _cardsInrow));
            //результирующая высота картинки
            var resultImageH = Convert.ToInt32(ownerHlen * (rows) 
                + (_cardRowIntervalPx * rows) + (_borderHLength * 2));
            var resultImageW = Convert.ToInt32(ownerWlen * _cardsInrow 
                + (_cardColIntervalPx * _cardsInrow) + (_borderWLength * 2));
            using (var resultImage = new Bitmap(resultImageH, resultImageW))
            {
                using (var canvas = Graphics.FromImage(resultImage))
                {
                    canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    for (int col = 0; col < _cardsInrow; col++)
                    {
                        for (int row = 0; row < rows; row++)
                        {
                            
                            //canvas.DrawImage();
                        }
                    }
                }

                return resultImage;
            }
        }
    }
}
