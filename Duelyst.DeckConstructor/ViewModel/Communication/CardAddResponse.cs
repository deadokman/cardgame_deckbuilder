using System.Collections.Generic;

namespace Duelyst.DeckConstructor.ViewModel.Communication
{
    public class CardAddResponse
    {
        private static Dictionary<EResponseType, string> _respinonses = new Dictionary<EResponseType, string>();

        static CardAddResponse()
        {
            _respinonses.Add(EResponseType.CardInstanceLimit, "Достигнуто максимальное число карт этого типа в колоде");
            _respinonses.Add(EResponseType.SquadLimit, "Достигнут лимит колоды");
            _respinonses.Add(EResponseType.OwnerError, "Карта не может принадлежать выбранному генералу");
            _respinonses.Add(EResponseType.SquadSaveUnavaileble, "Не возможно сохранить отряд из-за недостаточного числа карт");
            _respinonses.Add(EResponseType.None, "");
        }

        public CardAddResponse()
        {
            ResponseType = EResponseType.None;
        }

        public EResponseType ResponseType { get; set; }

        public string ResponseMessage
        {
            get { return _respinonses[ResponseType]; }
        }


    }
}
