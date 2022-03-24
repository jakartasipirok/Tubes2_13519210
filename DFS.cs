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
    public class filesAndFolderDFS
    {
        public string parent;
        public string direct;

        public filesAndFolderDFS()
         {
             this.parent = "";
             this.direct = "";
         }

        public filesAndFolderDFS(string parent, string direct)
        {
            this.parent = parent;
            this.direct = direct;
        }

        public filesAndFolderDFS(filesAndFolderDFS fl)
        {
            this.parent = fl.parent;
            this.direct = fl.direct;
        }

        public void setvalue(string parent,string direct)
        {
            this.parent = parent;
            this.direct = direct;
        }
    }
    public class DFS : filesAndFolderDFS
    {
        private Graph graph;
        
        public static string pathDFS = "";
        
        public Queue<filesAndFolderDFS> nodeDFS = new Queue<filesAndFolderDFS>();
        
        public DFS()
        {
            this.graph = new Graph();
        }
        
        public void SearchDFS(string root, string filename, bool IsAllOccurences)
        {
            Stack<string> dirs_visited = new Stack<string>(10000);
        

            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            dirs_visited.Push(root);
            nodeDFS.Enqueue(new filesAndFolderDFS("", root));

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
                    nodeDFS.Enqueue(new filesAndFolderDFS(currentDir, file));
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.Name == filename)
                        {
                            pathDFS += filename;
                            System.Console.WriteLine(pathDFS);
                            if (IsAllOccurences)
                            {
                                continue;
                            }
                            else
                            {
                                return;
                            }
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
                    nodeDFS.Enqueue(new filesAndFolderDFS(currentDir, str));
                }
            }
        }
        public void createGraphDFS(string namafile)
        {
            foreach (filesAndFolderDFS anak in nodeDFS)
            {
                foreach (filesAndFolderDFS ortu in nodeDFS)
                {
                    if (anak.parent == ortu.direct)
                    {
                        graph.AddEdge(ortu.direct, anak.direct);
                        graph.FindNode(anak.parent).Label.Text = new DirectoryInfo(anak.parent).Name;
                        graph.FindNode(anak.direct).Label.Text = new DirectoryInfo(anak.direct).Name;
                        break;
                    }
                }
                if (String.Compare(Path.GetFileName(anak.direct), namafile) == 0)
                {
                    graph.FindNode(anak.direct).Attr.FillColor = Microsoft.Msagl.Drawing.Color.Green;
                }
            }
        }
        public Graph getGraphDFS()
        {
            return this.graph;
        }
    }
}
