using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Misc")]

public class MiscClass : ItemClass
{
    //data specific to misc class
    public override void Use(PlayerControl caller)
    {
        //base.Use(caller);
    }

    public override MiscClass GetMisc() {return this;}
}
