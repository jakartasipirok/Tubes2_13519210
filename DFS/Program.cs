// The use example of DFS class
string rootPath = Directory.GetCurrentDirectory();

var dfs = new DFS();
dfs.getRequestedFilePaths(rootPath, ".NETCoreApp,Version=v6.0.AssemblyAttributes.cs", true);

var queue_onlyNames = new List<string>(dfs.getFileSystemNameInQueue());
var queue_fullPaths = new List<string>(dfs.getFullPathsInQueue());

var visited_onlyNames = new List<string>(dfs.getFileSystemNameVisited());
var visited_fullPaths = new List<string>(dfs.getFileSystemNameVisited());

dfs.printRequestedFilePaths();
System.Console.WriteLine();

dfs.printQueue(true);
System.Console.WriteLine("-----------");
dfs.printQueue(false);
System.Console.WriteLine();

dfs.printVisited(true);
System.Console.WriteLine("-----------");
dfs.printVisited(false);
