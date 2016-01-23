using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace Duelyst.DeckConstructor.ViewModel.Ifaces
{
    public interface IResizeableViewModel
    {
        double PageWidth { get; set; }
        double PageHeight { get; set; }
        double PageHeightExtended { get; set; }

        void MouseMove(double newPosx, double newPosy, double centerPosX, double centerPosY);
    }
}
