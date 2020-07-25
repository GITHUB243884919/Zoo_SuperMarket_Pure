
using UFrame;
using UnityEngine;

namespace CrossRoadGame
{
    public class StateCrossRoadAnimalIdle : FSMState
    {
        public StateCrossRoadAnimalIdle(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        public override void AddAllConvertCond()
        {
        }

        public override void Tick(int deltaTimeMS)
        {
        }
    }
}
