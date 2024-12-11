using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineUpgradeSystem : UpgradeSystem<UpgradeType>
{
    protected override void OnUpgradeApplied(UpgradeConfig<UpgradeType> config, float newEffectValue)
    {
        Events.OnMachineUpgrade?.Invoke(config.type);
    }
}