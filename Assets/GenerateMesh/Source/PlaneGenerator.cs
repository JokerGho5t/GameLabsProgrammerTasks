using UnityEngine;

namespace GenerateMesh
{
    public class PlaneGenerator
    {
        private Mesh m_Mesh;
        private Vector2 m_Size;
        private Vector2Int m_Resolution;
        private Vector3[] m_Vertices;
        private int[] m_Triangles;
        private Vector2[] m_Uvs;
        private Vector4[] m_Tangents;

        public void Generate(Mesh mesh, Vector2 size, Vector2Int resolution)
        {
            m_Mesh = mesh;
            m_Size = size;
            m_Resolution = resolution;

            CreateMesh();
        }

        private void CreateMesh()
        {
            CreateVertices();
            CreateTriangles();

            m_Mesh.vertices = m_Vertices;
            m_Mesh.triangles = m_Triangles;
            m_Mesh.uv = m_Uvs;
            m_Mesh.tangents = m_Tangents;

            m_Mesh.RecalculateNormals();
        }

        private void CreateVertices()
        {
            var stepX = m_Size.x / m_Resolution.x;
            var stepY = m_Size.y / m_Resolution.y;

            m_Vertices = new Vector3[(m_Resolution.x + 1) * (m_Resolution.y + 1)];
            m_Uvs = new Vector2[m_Vertices.Length];
            m_Tangents = new Vector4[m_Vertices.Length];
            var tangent = new Vector4(1f, 0f, 0f, -1f);

            for (int i = 0, y = 0; y <= m_Resolution.y; y++)
            {
                for (int x = 0; x <= m_Resolution.x; x++, i++)
                {
                    m_Vertices[i] = new Vector3(x * stepX, 0, y * stepY);
                    m_Uvs[i] = new Vector2((float)x / m_Resolution.x, (float)y / m_Resolution.y);
                    m_Tangents[i] = tangent;
                }
            }
        }

        private void CreateTriangles()
        {
            m_Triangles = new int[m_Resolution.x * m_Resolution.y * 6];
            for (int ti = 0, vi = 0, y = 0; y < m_Resolution.y; y++, vi++)
            {
                for (int x = 0; x < m_Resolution.x; x++, ti += 6, vi++)
                {
                    m_Triangles[ti] = vi;
                    m_Triangles[ti + 1] = m_Triangles[ti + 4] = vi + m_Resolution.x + 1;
                    m_Triangles[ti + 2] = m_Triangles[ti + 3] = vi + 1;
                    m_Triangles[ti + 5] = vi + m_Resolution.x + 2;
                }
            }
        }
    }
}