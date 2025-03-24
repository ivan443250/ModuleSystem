using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCSample.Infrastructure.DataHolding
{
    public class DefaultDataExplorer : IDataExplorer, 
        IGlobalDataExplorerContext, ISaveCellExplorerContext, ISceneDataExplorerContext
    {
        private readonly IFileSystem _fileSystem;

        private string _sceneName;

        private int _saveCellIndex;

        private Dictionary<Type, object> _globalContext;
        private Dictionary<Type, object> _cellContext;
        private Dictionary<Type, object> _sceneContext;

        #region APIs To Get Context

        public IGlobalDataExplorerContext GlobalDataSet => this;

        public ISaveCellExplorerContext SaveCellDataSet
        {
            get
            {
                if (_saveCellIndex == -1)
                    return null;

                return this;
            }
        }

        public ISceneDataExplorerContext SceneDataSet
        {
            get
            {
                if (SaveCellDataSet == null)
                    return null;

                if (_sceneName == null)
                    return null;

                return this;
            }
        }

        #endregion

        public DefaultDataExplorer(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            _saveCellIndex = -1;
        }

        #region IGlobalDataExplorerContext API

        async Task<ISaveCellExplorerContext> IGlobalDataExplorerContext.OpenSaveCell(int cellIndex)
        {
            if (cellIndex < 0)
                throw new ArgumentOutOfRangeException("Save Cell Index can not be less than 0");

            _globalContext = await _fileSystem.LoadAll();

            _saveCellIndex = cellIndex;

            _cellContext = await _fileSystem.LoadAll(cellIndex.ToString());

            return this;
        }

        T IGlobalDataExplorerContext.GetData<T>() where T : class
        {
            return GetDataInternal<T>(_globalContext);
        }

        async Task IGlobalDataExplorerContext.Save<T>(T data) where T : class
        {
            await _fileSystem.Save(data);
        }

        #endregion

        #region ISaveCellExplorerContext API    

        async Task<ISceneDataExplorerContext> ISaveCellExplorerContext.OpenScene(string sceneName)
        {
            _sceneName = sceneName;

            _sceneContext = await _fileSystem.LoadAll(_saveCellIndex.ToString(), sceneName);

            return this;
        }

        T ISaveCellExplorerContext.GetData<T>() where T : class
        {
            return GetDataInternal<T>(_cellContext);
        }

        async Task ISaveCellExplorerContext.Save<T>(T data) where T : class
        {
            await _fileSystem.Save(data, _saveCellIndex.ToString());
        }

        #endregion

        #region ISceneDataExplorerContext API

        T ISceneDataExplorerContext.GetData<T>()
        {
            return GetDataInternal<T>(_sceneContext);
        }

        async Task ISceneDataExplorerContext.Save<T>(T data) where T : class
        {
            await _fileSystem.Save(data, _saveCellIndex.ToString(), _sceneName);
        }

        #endregion

        private T GetDataInternal<T>(Dictionary<Type, object> context) where T : class
        {
            if (context.ContainsKey(typeof(T)) == false)
                return null;

            return context[typeof(T)] as T;
        }
    }

    public interface IGlobalDataExplorerContext
    {
        Task<ISaveCellExplorerContext> OpenSaveCell(int cellIndex);

        T GetData<T>() where T : class;
        Task Save<T>(T data) where T : class;
    }

    public interface ISaveCellExplorerContext
    {
        Task<ISceneDataExplorerContext> OpenScene(string sceneName);

        T GetData<T>() where T : class;
        Task Save<T>(T data) where T : class;
    }

    public interface ISceneDataExplorerContext
    {
        T GetData<T>() where T : class;
        Task Save<T>(T data) where T : class;
    }
}
