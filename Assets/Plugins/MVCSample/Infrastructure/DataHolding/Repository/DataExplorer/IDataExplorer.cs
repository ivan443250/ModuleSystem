using UnityEngine;

namespace MVCSample.Infrastructure.DataHolding
{
    public interface IDataExplorer 
    {
        IGlobalDataExplorerContext GlobalDataSet { get; }
        ISaveCellExplorerContext SaveCellDataSet { get; }
        ISceneDataExplorerContext SceneDataSet { get; }
    }
}
