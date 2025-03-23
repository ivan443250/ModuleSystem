using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MVCSample.Infrastructure.DataHolding
{
    public class DefaultFileSystem : IFileSystem
    {
        private IDataConverter _dataConverter;

        public DefaultFileSystem(IDataConverter dataConverter)
        {
            _dataConverter = dataConverter;
        }

        public async Task Save(object objectToSave, params string[] folders)
        {
            IFolderFilesCollection filesCollection = await GetFilesCollection(true, folders);

            await filesCollection.Reset(objectToSave);
        }

        public async Task<object> Load(Type objectType, params string[] folders)
        {
            return await (await GetFilesCollection(false, folders)).Get(objectType);
        }

        public async Task<Dictionary<Type, object>> LoadAll(params string[] folders)
        {
            return await (await GetFilesCollection(false, folders)).GetAll();
        }

        private async Task<IFolderFilesCollection> GetFilesCollection(bool pathMustExist, params string[] folders)
        {
            string path = GetFullPath(pathMustExist, folders);

            return await FolderMetadata.GetFilesCollection(path, _dataConverter);
        }

        private string GetFullPath(bool pathMustExist, params string[] folders)
        {
            StringBuilder checkedPath = new(Application.persistentDataPath);

            foreach (string folder in folders)
            {
                checkedPath.Append("/");
                checkedPath.Append(folder);

                if (pathMustExist && (Directory.Exists(checkedPath.ToString()) == false))
                    Directory.CreateDirectory(checkedPath.ToString());
            }

            if (Directory.Exists(checkedPath.ToString()) == false)
                return null;

            return checkedPath.ToString();
        }
    }
}
