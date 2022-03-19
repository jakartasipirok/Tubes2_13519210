class DFS{
    private Stack<string> pathQueue; // Stack is used to apply DFS algorithm     
    private List<string> requestedFilesPath;
    private List<string> pathVisited;

    public DFS(){
        this.pathQueue = new Stack<string>();
        this.requestedFilesPath = new List<string>();
        this.pathVisited = new List<string>();
    }

    public List<string> getFilePaths(string path, string filenameToFind, Boolean IsAllOccurences){
        // F.S: requestedFilesPath are gotten, i.e., either empty, a string, or list of string.

        this.pathVisited.Add(path);

        if (this.isFile(path)){
            var filename = Path.GetFileName(path);
            if (filename.Equals(filenameToFind)){
                this.requestedFilesPath.Add(path);
                if (!IsAllOccurences){
                    return this.requestedFilesPath;
                }
            }

            if (this.pathQueue.Any()){
                var nextPath = this.pathQueue.Pop();
                return this.getFilePaths(nextPath, filenameToFind, IsAllOccurences);
            } else {
                return this.requestedFilesPath;
            }
        }

        else {
            this.pushAllObjectsWithinDirToQueue(path);
            if (this.pathQueue.Any()){
                var nextPath = this.pathQueue.Pop();
                return this.getFilePaths(nextPath, filenameToFind, IsAllOccurences);
            } else {
                return this.requestedFilesPath;
            }
        }
    }

    private Boolean isFile(string path){
        return File.GetAttributes(path).HasFlag(FileAttributes.Normal);
    }

    private void pushAllObjectsWithinDirToQueue(string path){
        // F.S: Files are on top of directories in pathQueue

        var allDirsPath = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
        foreach (var dirPath in allDirsPath){
            pathQueue.Push(dirPath);
        }

        var allFilePaths = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
        foreach (var filePath in allFilePaths){
            pathQueue.Push(filePath);
        }
    }

    public List<string> getFullPathsVisited(){
        return this.pathVisited;
    }

    public List<string> getFileSystemNameVisited(){
        // Filesystem consists of normal files and directories
        // Returning each file and directory name instead of its respective full path

        var filesAndDirsName = new List<string>();

        foreach (var path in this.pathVisited){
            var fileOrDirName = Path.GetFileName(path);
            filesAndDirsName.Add(fileOrDirName);
        }

        return filesAndDirsName;
    }



    public List<string> getFullPathsInQueue(){
        var container = new List<string>();
        foreach (var element in this.pathQueue){
            container.Add(element);
        }

        return container;
    }

    public List<string> getFileSystemNameInQueue(){
        // Filesystem consists of normal files and directories
        // Returning each file and directory name instead of its respective full path

        var filesAndDirsName = new List<string>();

        foreach (var path in this.pathQueue){
            var fileOrDirName = Path.GetFileName(path);
            filesAndDirsName.Add(fileOrDirName);
        }

        return filesAndDirsName;
    }

    // Methods below for testing purposes //
    public void printQueue(Boolean isFileName){
        var container = new List<string>();

        if (isFileName){
            container = this.getFileSystemNameInQueue();
        }
        else{
            container = this.getFullPathsInQueue();
        }

        foreach (var element in container){
            System.Console.WriteLine(element);
        }
    }

    public void printVisited(Boolean isFileName){
        var container = new List<string>();

        if (isFileName){
            container = this.getFileSystemNameVisited();
        }
        else{
            container = this.getFullPathsVisited();
        }

        foreach (var element in container){
            System.Console.WriteLine(element);
        }
    }

    public void printRequestedFilePaths(){
        foreach (var path in this.requestedFilesPath){
            System.Console.WriteLine(path);
        }
    }
}