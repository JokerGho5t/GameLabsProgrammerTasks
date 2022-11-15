using System.Collections.Generic;

namespace Metro
{
        public class MetroMap
    {
        private readonly Dictionary<EMetroStation, List<Connection>> m_Stations;

        public MetroMap(Dictionary<EMetroStation, List<Connection>> stations) => m_Stations = stations;
        
        public (List<Connection>, int) GetPath(EMetroStation start, EMetroStation finish)
        {
            var queue = new Queue<EMetroStation>();
            var paths = new Dictionary<EMetroStation, List<Connection>>();
            EMetroStation current = start;

            queue.Enqueue(current);
            paths[current] = new List<Connection>();

            while (queue.Count != 0)
            {
                current = queue.Dequeue();
                if (paths.ContainsKey(finish))
                {
                    break;
                }

                foreach (var neighbour in m_Stations[current])
                {
                    var path = new List<Connection>(paths[current]) { neighbour };

                    if (neighbour.Station == finish)
                    {
                        if (!paths.ContainsKey(finish))
                        {
                            paths.Add(neighbour.Station, path);
                            continue;
                        }
                    }

                    if (paths.ContainsKey(neighbour.Station)) continue;
                    
                    paths.Add(neighbour.Station, path);
                    queue.Enqueue(neighbour.Station);
                }
            }

            return (paths[finish], CountTransfers(paths[finish]));
        }

        private static int CountTransfers(List<Connection> path)
        {
            if (path.Count < 2)
                return 0;

            int number = 0;
            for (int i = 1; i < path.Count; i++)
            {
                if (path[i - 1].Line != path[i].Line)
                {
                    number++;
                }
            }

            return number;
        }
    }
}

