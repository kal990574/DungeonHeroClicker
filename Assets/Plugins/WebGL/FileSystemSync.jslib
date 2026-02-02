var FileSystemSyncPlugin = {
    SyncFileSystem: function () {
        FS.syncfs(false, function (err) {
            if (err) {
                console.error("IndexedDB sync error: " + err);
            }
        });
    }
};

mergeInto(LibraryManager.library, FileSystemSyncPlugin);
