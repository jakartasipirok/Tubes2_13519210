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
    public class filesAndFolderBFS
    {
        public string parent;
        public string direct;
        public string status;
        public int id;

        public filesAndFolder()
        {
            this.parent = "";
            this.direct = "";
            this.status = "queued";
            this.id = -999;
        }

        public filesAndFolder(string parent, string direct, string status, int id)
        {
            this.parent = parent;
            this.direct = direct;
            this.status = status;
            this.id = id;
        }

        public filesAndFolderBFS(filesAndFolderBFS fl)
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
    class BFS : filesAndFolderBFS
    {
        private Graph graph;
        public static string pathBFS = "";

        public Queue<filesAndFolder> nodeBFS = new Queue<filesAndFolder>(); // Queue buat output
        public Queue<filesAndFolder> nodeFound = new Queue<filesAndFolder>();
        public Queue<filesAndFolder> nodeChecked = new Queue<filesAndFolder>();
        public BFS()
        {
            this.graph = new Graph();
        }
        public void pathFound(filesAndFolder found)
        {
            int curr = found.id;
            while (curr != -1)
            {
                found.status = "found";
                string parentname = found.parent;
                found = getNodeByName(parentname);
                curr = found.id;
            }
            //foreach (filesAndFolder ortu in nodeBFS)
            //{
            //    foreach (filesAndFolder anak in nodeBFS)
            //    {
            //        if ((anak.parent == ortu.direct) && (curr == anak.direct))
            //        {
            //            anak.status = "found";
            //            ortu.status = "found";
            //            curr = anak.parent;
            //        }
            //    }
            //}
        }

        public void pathFalse(filesAndFolder found)
        {
            int curr = found.id;
            while (curr != -1 && found.status != "found")
            {
                found.status = "false";
                string parentname = found.parent;
                found = getNodeByName(parentname);
                curr = found.id;
            }
            //foreach (filesAndFolder ortu in nodeBFS)
            //{
            //    foreach (filesAndFolder anak in nodeBFS)
            //    {
            //        if ((anak.parent == ortu.direct) && (curr == anak.direct))
            //        {
            //            anak.status = "found";
            //            ortu.status = "found";
            //            curr = anak.parent;
            //        }
            //    }
            //}
        }
        filesAndFolder getNodeByName(string name)
        {
            foreach (filesAndFolder anak in nodeBFS)
            {
                if(anak.direct == name)
                {
                    return (anak);
                }
            }
            return null;
        }


        public void SearchBFS(string root, string filename, bool IsAllOccurences)
        {
            Queue<string> dirs_visited = new Queue<string>(10000);

            if (!System.IO.Directory.Exists(root))
            {
                throw new ArgumentException();
            }

            dirs_visited.Enqueue(root);
            nodeBFS.Enqueue(new filesAndFolder("", root, "queued", -1));
            int id = 1;

            while (dirs_visited.Count > 0)
            {
                string currentDir = dirs_visited.Dequeue();
                pathBFS = currentDir + "\\";

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
                    filesAndFolder proccess = new filesAndFolder(currentDir, file, "queued", id);
                    id++;
                    nodeBFS.Enqueue(proccess);
                    try
                    {
                        System.IO.FileInfo fi = new System.IO.FileInfo(file);
                        if (fi.Name == filename)
                        {
                            pathFound(proccess);
                            pathBFS += filename;
                            System.Console.WriteLine(pathBFS);
                            if (IsAllOccurences)
                            {
                                continue;
                            }
                            else
                            {
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

                string[] subDirs;
                try
                {
                    subDirs = System.IO.Directory.GetDirectories(currentDir);
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

                foreach (string str in subDirs)
                {
                    dirs_visited.Enqueue(str);
                    nodeBFS.Enqueue(new filesAndFolder(currentDir, str, "queued", id));
                    id++;
                }
            }
        }
        public void createGraphBFS(string namafile)
        {
            foreach (filesAndFolderBFS anak in nodeBFS)
            {
                foreach (filesAndFolderBFS ortu in nodeBFS)
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
                        else
                        {
                            graph.AddEdge(ortu.direct, anak.direct);
                            graph.FindNode(anak.parent).Label.Text = new DirectoryInfo(anak.parent).Name;
                            graph.FindNode(anak.direct).Label.Text = new DirectoryInfo(anak.direct).Name;
                            break;
                        }
                    }
                }
            }
        }
        public Graph getGraphBFS()
        {
            return this.graph;
        }
    }
}
