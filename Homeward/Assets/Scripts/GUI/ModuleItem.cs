using UnityEngine;
using System.Collections;

public enum ModuleItemName
{
    HabitatModule,
    ConnectorModule,
    FoodModule,
    HealthStaminaModule,
    ModuleControlModule,
    PowerModule,
    RadarModule,
    RefineryModule,
    RobotModule,
    StorageModule,
    AirlockModule
}

[System.Serializable]
public class ModuleItem : MonoBehaviour {

    public ModuleItemName itemName;
    public string itemDescription;
    public Sprite spriteNeutral;
    public Sprite spriteHighLighted;

}
