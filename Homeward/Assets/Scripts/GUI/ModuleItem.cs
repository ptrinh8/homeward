using UnityEngine;
using System.Collections;

public enum ModuleItemName
{
    HabitatModule,
    ConnectorModule,
    FoodModule,
    AirlockModule,
    HealthStaminaModule,
    ModuleControlModule,
    PowerModule,
    RadarModule,
    RefineryModule,
    RobotModule,
    StorageModule
}

[System.Serializable]
public class ModuleItem : MonoBehaviour {

    public ModuleItemName itemName;
    public string itemDescription;
    public Sprite spriteNeutral;
    public Sprite spriteHighLighted;

}
