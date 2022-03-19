string path = "./";

var dfs = new DFS();

dfs.getFilePaths(path, "DFS.cs", false);
dfs.printRequestedFilePaths();
dfs.printQueue(false);
dfs.printVisited(false);