using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;

namespace Tubes_Stima_2
{
    class DFS
    {
        private Stack<string> queueOfPath; // Stack is used to apply DFS algorithm     
        private List<string> allFilesPathFound;
        private List<string> pathVisited;
        private Graph graph;

        public DFS()
        {
            this.queueOfPath = new Stack<string>();
            this.allFilesPathFound = new List<string>();
            this.pathVisited = new List<string>();
            this.graph = new Graph();
        }

        public Graph getGraph()
        {
            return this.graph;
        }

        public List<string> getRequestedFilePaths(string path, string filenameToFind, Boolean IsAllOccurences)
        {
            // F.S: allFilesPathFound are gotten, i.e., empty, a value, or couple of values.

            this.pathVisited.Add(path);

            if (this.isFile(path))
            {
                var filename = Path.GetFileName(path);
                if (filename.Equals(filenameToFind))
                {
                    this.allFilesPathFound.Add(path);
                    if (!IsAllOccurences)
                    {
                        return this.allFilesPathFound;
                    }
                }

                if (this.queueOfPath.Any())
                {
                    var nextPath = this.queueOfPath.Pop();
                    return this.getRequestedFilePaths(nextPath, filenameToFind, IsAllOccurences);
                }
                else
                {
                    return this.allFilesPathFound;
                }
            }

            else
            {
                this.pushAllObjectsWithinDirToQueue(path);
                if (this.queueOfPath.Any())
                {
                    var nextPath = this.queueOfPath.Pop();
                    return this.getRequestedFilePaths(nextPath, filenameToFind, IsAllOccurences);
                }
                else
                {
                    return this.allFilesPathFound;
                }
            }
        }

        public Boolean isFile(string path)
        {
            return !File.GetAttributes(path).HasFlag(FileAttributes.Directory);
        }

        private void pushAllObjectsWithinDirToQueue(string path)
        {
            // F.S: Files are on top of directories in queueOfPath

            var allDirsInPath = Directory.EnumerateDirectories(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var dirPath in allDirsInPath)
            {
                queueOfPath.Push(dirPath);
            }

            var allFilesInPath = Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in allFilesInPath)
            {
                queueOfPath.Push(filePath);
            }
        }

        public List<string> getFullPathsVisited()
        {
            return this.pathVisited;
        }

        public List<string> getFileSystemNameVisited()
        {
            // Filesystem consists of normal files and directories
            // Returning each file and directory name instead of its respective full path

            var filesAndDirsName = new List<string>();

            foreach (var path in this.pathVisited)
            {
                var fileOrDirName = Path.GetFileName(path);
                filesAndDirsName.Add(fileOrDirName);
            }

            return filesAndDirsName;
        }

        public List<string> getFullPathsInQueue()
        {
            return new List<string>(this.queueOfPath);
        }

        public List<string> getFileSystemNameInQueue()
        {
            // Filesystem consists of normal files and directories
            // Returning each file and directory name instead of its respective full path

            var filesAndDirsName = new List<string>();

            foreach (var path in this.queueOfPath)
            {
                var fileOrDirName = Path.GetFileName(path);
                filesAndDirsName.Add(fileOrDirName);
            }

            return filesAndDirsName;
        }

        // Methods below only for testing purposes //
        public void printQueue(Boolean isFileName)
        {
            var queue = new List<string>();

            if (isFileName)
            {
                queue = this.getFileSystemNameInQueue();
            }
            else
            {
                queue = this.getFullPathsInQueue();
            }

            if (queue.Any())
            {
                foreach (var path in queue)
                {
                    System.Console.WriteLine(path);
                }
            }
            else
            {
                System.Console.WriteLine("Queue is empty");
            }
        }

        public void printVisited(Boolean isFileName)
        {
            var visited = new List<string>();
            if (isFileName)
            {
                visited = this.getFileSystemNameVisited();
            }
            else
            {
                visited = this.getFullPathsVisited();
            }

            if (visited.Any())
            {
                foreach (var path in visited)
                {
                    System.Console.WriteLine(path);
                }
            }
            else
            {
                System.Console.WriteLine("No path has visited");
            }
        }

        public void printRequestedFilePaths()
        {
            if (this.allFilesPathFound.Any())
            {
                foreach (var path in this.allFilesPathFound)
                {
                    System.Console.WriteLine(path);
                }
            }
            else
            {
                System.Console.WriteLine("File is not found");
            }
        }

        public void makeGraph()
        {
            if (this.allFilesPathFound.Any())
            {
                for (int i = 0; i < allFilesPathFound.Count - 1; i++)
                {
                    string path = this.allFilesPathFound[i];
                    string next = this.allFilesPathFound[i + 1];
                    graph.AddEdge(path, next).Attr.Color = Microsoft.Msagl.Drawing.Color.Blue;
                }

                //graph.AddEdge(this.allFilesPathFound[this.allFilesPathFound.Count-2], this.allFilesPathFound[this.allFilesPathFound.Count - 1]);
            }
            else
            {
                graph.AddEdge("notfound", "nf");
                graph.AddEdge("nf", "stima");
            }
        }
    }
}
