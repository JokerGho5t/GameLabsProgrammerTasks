using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Metro
{
    public class MetroView : MonoBehaviour
    {
        private readonly Dictionary<EMetroStation, List<Connection>> m_Stations = new Dictionary<EMetroStation, List<Connection>>()
        { 
        {
            EMetroStation.A,
            new List<Connection> 
            {
                new Connection(EMetroStation.B, EMetroLine.Red)
            } 
        },
        { EMetroStation.B,
            new List<Connection> 
            {
                new Connection(EMetroStation.A, EMetroLine.Red),
                new Connection(EMetroStation.C, EMetroLine.Red),
                new Connection(EMetroStation.H, EMetroLine.Black),
            }
        },
        { EMetroStation.C,
            new List<Connection> 
            {
                new Connection(EMetroStation.B, EMetroLine.Red),
                new Connection(EMetroStation.D, EMetroLine.Red),
                new Connection(EMetroStation.J, EMetroLine.Green),
                new Connection(EMetroStation.K, EMetroLine.Green),
            }
        },
        { EMetroStation.D,
            new List<Connection> 
            {
                new Connection(EMetroStation.C, EMetroLine.Red),
                new Connection(EMetroStation.E, EMetroLine.Red),
                new Connection(EMetroStation.L, EMetroLine.Blue),
                new Connection(EMetroStation.J, EMetroLine.Blue),
            }
        },
        { EMetroStation.E,
            new List<Connection> 
            {
                new Connection(EMetroStation.D, EMetroLine.Red),
                new Connection(EMetroStation.F, EMetroLine.Red),
                new Connection(EMetroStation.M, EMetroLine.Green),
                new Connection(EMetroStation.J, EMetroLine.Green),
            }
        },
        { EMetroStation.F,
            new List<Connection> 
            {
                new Connection(EMetroStation.E, EMetroLine.Red),
                new Connection(EMetroStation.J, EMetroLine.Black),
                new Connection(EMetroStation.G, EMetroLine.Black),
            }
        },
        { EMetroStation.G,
            new List<Connection> 
            {
                new Connection(EMetroStation.F, EMetroLine.Black),
            }
        },
        { EMetroStation.H,
            new List<Connection> 
            {
                new Connection(EMetroStation.B, EMetroLine.Black),
                new Connection(EMetroStation.J, EMetroLine.Black),
            }
        },
        { EMetroStation.J,
            new List<Connection> 
            {
                new Connection(EMetroStation.O, EMetroLine.Blue),
                new Connection(EMetroStation.F, EMetroLine.Black),
                new Connection(EMetroStation.E, EMetroLine.Green),
                new Connection(EMetroStation.D, EMetroLine.Blue),
                new Connection(EMetroStation.C, EMetroLine.Green),
                new Connection(EMetroStation.H, EMetroLine.Black),
            }
        },
        { EMetroStation.K,
            new List<Connection> 
            {
                new Connection(EMetroStation.C, EMetroLine.Green),
                new Connection(EMetroStation.L, EMetroLine.Green),
            }
        },
        { EMetroStation.L,
            new List<Connection> 
            {
                new Connection(EMetroStation.K, EMetroLine.Green),
                new Connection(EMetroStation.M, EMetroLine.Green),
                new Connection(EMetroStation.D, EMetroLine.Blue),
                new Connection(EMetroStation.N, EMetroLine.Blue),
            }
        },
        { EMetroStation.M,
            new List<Connection> 
            {
                new Connection(EMetroStation.K, EMetroLine.Green),
                new Connection(EMetroStation.M, EMetroLine.Green),
                new Connection(EMetroStation.D, EMetroLine.Blue),
                new Connection(EMetroStation.N, EMetroLine.Blue),
            }
        },
        { EMetroStation.N,
            new List<Connection> 
            {
                new Connection(EMetroStation.L, EMetroLine.Blue),
            }
        },
        { EMetroStation.O,
            new List<Connection> 
            {
                new Connection(EMetroStation.J, EMetroLine.Blue),
            }
        },
        };

        public string GetColor(EMetroLine line) =>
            line switch
            {
                EMetroLine.Red => ColorUtility.ToHtmlStringRGB(Color.red),
                EMetroLine.Blue => ColorUtility.ToHtmlStringRGB(Color.blue),
                EMetroLine.Green => ColorUtility.ToHtmlStringRGB(Color.green),
                EMetroLine.Black => ColorUtility.ToHtmlStringRGB(Color.black),
                _ => throw new ArgumentOutOfRangeException(nameof(line), line, null)
            };
            

        [SerializeField] private Dropdown startDropdown;
        [SerializeField] private Dropdown endDropdown;
        [SerializeField] private Text log;
        [SerializeField] private Button btn;

        private EMetroStation m_StartStation;
        private EMetroStation m_EndStation;
        private MetroMap m_MetroMap;
        
        private void Start()
        {
            m_MetroMap = new MetroMap(m_Stations);

            log.text = "";
            
            InitDropdown(startDropdown);
            InitDropdown(endDropdown);
            
            startDropdown.onValueChanged.AddListener(OnChangeStartPoint);
            endDropdown.onValueChanged.AddListener(OnChangeEndPoint);
            
            btn.onClick.AddListener(OnGetPath);
        }

        private void InitDropdown(Dropdown dropdown)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(Enum.GetNames(typeof(EMetroStation)).ToList());
        }

        private void OnChangeStartPoint(int index)
        {
            m_StartStation = (EMetroStation)index;
        }
        
        private void OnChangeEndPoint(int index)
        {
            m_EndStation = (EMetroStation)index;
        }
        
        private void OnGetPath()
        {
            var (path, transfers) = m_MetroMap.GetPath(m_StartStation, m_EndStation);

            log.text = $"{m_StartStation} ";

            for (var i = 0; i < path.Count; i++)
            {
                log.text += $"<color=#{GetColor(path[i].Line)}>---</color> {path[i].Station} ";
            }

            log.text += $"\nNumber of transfer: {transfers}";
        }
    }
}