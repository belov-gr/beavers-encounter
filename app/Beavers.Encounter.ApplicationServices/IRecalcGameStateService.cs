using System;

namespace Beavers.Encounter.ApplicationServices
{
    public interface IRecalcGameStateService
    {
        void RecalcGameState(DateTime recalcDateTime);
    }
}
