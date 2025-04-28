using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "ScriptableObjects/LevelSettings", order = 1)]
public class LevelSettings : ScriptableObject
{
    public List<BlockConfig> blockConfigs;
}

[Serializable]
public class BlockConfig{
    public int blockNumber;
    public List<LevelConfig> levelConfigs;
}

[Serializable]
public class LevelConfig{
    public int levelNumber;
    public List<string> windowIds;
}