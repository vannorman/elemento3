using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Elemento.Spells;

namespace Elemento
{ 
    public interface ISpellEffectReceiver
    {
        void OnSpellAction(Spell spell, float forceAmount = 50f);
    }

}
