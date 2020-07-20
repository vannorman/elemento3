using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elemento.Spells;

namespace Elemento
{ 
    public interface ISpellActionHandler
    {
        void OnSpellAction(HandPoseTracker handTracker, Spell spell);
    }

}
