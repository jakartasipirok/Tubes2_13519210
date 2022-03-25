using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Diagnostics;


namespace Tubes_Stima_2
{
    public class filesAndFolderDFS
    {
        public string parent;
        public string direct;
        public string status;
        public int id;

        public filesAndFolderDFS()
        {
            this.parent = "";
            this.direct = "";
            this.status = "queued";
            this.id = -999;
        }

        public filesAndFolderDFS(string parent, string direct, string status, int id)
        {
            this.parent = parent;
            this.direct = direct;
            this.status = status;
            this.id = id;
        }

        public filesAndFolderDFS(filesAndFolder fl)
        {
            this.parent = fl.parent;
            this.direct = fl.direct;
            this.status = fl.status;
            this.id = fl.id;
        }

        public void setvalue(string parent, string direct)
        {
            this.parent = parent;
            this.direct = direct;
        }
    }
    public class DFS : filesAndFolderDFS
    {
        private Graph graph;
        private Stopwatch stopwatch;
        public static string pathDFS = "";
        private List<string> listPathDFS;
        
        public Queue<filesAndFolderDFS> nodeDFS = new Queue<filesAndFolderDFS>();
        
        public DFS()
        {
            this.graph = new Graph();
            this.stopwatch = new Stopwatch();
            this.listPathDFS = new List<string>(10000);
        }

        public void pathFound(filesAndFolderDFS found)
        {
            int curr = found.id;
            while (curr != -1)
            {
                found.status = "found";
                string parentname = found.parent;
                found = getNodeByNameDFS(parentname);
                curr = found.id;
            }
        }

        public void pathFalse(filesAndFolderDFS found)
        {
            int curr = found.id;
            while (curr != -1 && found.status != "found")
            {
                found.status = "false";
                string parentname = found.parent;
                found = getNodeByNameDFS(parentname);
                curr = found.id;
            }
        }

        public List<string> getListPathDFS()
        {
            return this.listPathDFS;
        }
        filesAndFolderDFS getNodeByNameDFS(string name)
        {
            foreach (filesAndFolderDFS anak in nodeDFS)
            {
                if (anak.direct == name)
                {
                    return (anak);
                }
            }
            return null;
        }

        public string getTimeElapsed()
        {
            TimeSpan ts = this.stopwatch.Elapsed;

            return (ts.ToString(@"mm\:ss\.ffff"));
        }

        public void SearchDFS(string root, string filename, bool IsAllOccurences)
        {
            this.stopwatch.Start();
            Stack<string> dirs_visited = new Stack<string>(10000);
        

            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }
            int id = 1;
            dirs_visited.Push(root);
            nodeDFS.Enqueue(new filesAndFolderDFS("", root, "false", -1));

            while (dirs_visited.Count > 0)
            {
                string currentDir = dirs_visited.Pop();
                pathDFS = currentDir + "\\";

                string[] files = null;
                try
                {
                    files = System.IO.Directory.GetFiles(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {

                    System.Console.WriteLine(e.Message);
                    continue;
                }

                catch (System.IO.DirectoryNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }
                foreach (string file in files)
                {
                    filesAndFolderDFS proccess = new filesAndFolderDFS(currentDir, file, "queued", id);
                    nodeDFS.Enqueue(proccess);
                    id++;
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.Name == filename)
                        {
                            pathFound(proccess);
                            this.listPathDFS.Add(pathDFS);
                            System.Console.WriteLine(pathDFS);
                            if (IsAllOccurences)
                            {
                                continue;
                            }
                            else
                            {
                                this.stopwatch.Stop();
                                return;
                            }
                        }
                        else
                        {
                            pathFalse(proccess);
                        }
                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        System.Console.WriteLine(e.Message);
                        continue;
                    }
                }

                string[] subdirs;
                try
                {
                    subdirs = System.IO.Directory.GetDirectories(currentDir);
                }

                catch (UnauthorizedAccessException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }
                catch (System.IO.DirectoryNotFoundException e)
                {
                    System.Console.WriteLine(e.Message);
                    continue;
                }

                foreach (string str in subdirs)
                {
                    dirs_visited.Push(str);
                    nodeDFS.Enqueue(new filesAndFolderDFS(currentDir, str, "queued", id));
                    id++;
                }
            }
            this.stopwatch.Stop();
        }
        public void createGraphDFS()
        {
            foreach (filesAndFolderDFS anak in nodeDFS)
            {
                foreach (filesAndFolderDFS ortu in nodeDFS)
                {
                    if (anak.parent == ortu.direct)
                    {

                        if (anak.status == "found")
                        {
                            graph.AddEdge(ortu.direct, anak.direct).Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
                            graph.FindNode(anak.direct).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                            graph.FindNode(anak.parent).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                            graph.FindNode(anak.parent).Label.Text = new DirectoryInfo(anak.parent).Name;
                            graph.FindNode(anak.direct).Label.Text = new DirectoryInfo(anak.direct).Name;
                            break;
                        }
                        else if (anak.status == "false")
                        {
                            graph.AddEdge(ortu.direct, anak.direct).Attr.Color = Microsoft.Msagl.Drawing.Color.Red;
                            graph.FindNode(anak.direct).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Red;
                            graph.FindNode(anak.parent).Label.Text = new DirectoryInfo(anak.parent).Name;
                            graph.FindNode(anak.direct).Label.Text = new DirectoryInfo(anak.direct).Name;
                            break;
                        }

                    }
                }
            }
        }
        public Graph getGraphDFS()
        {
            return this.graph;
        }
    }
}
