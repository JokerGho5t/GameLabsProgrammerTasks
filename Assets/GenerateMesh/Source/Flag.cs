using System;
using UnityEngine;

namespace GenerateMesh
{
    public enum EMode
    {
        MonoBehaviour,
        Compute,
        GPUShader
    }
    
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Flag : MonoBehaviour
    {
        [SerializeField] private ComputeShader computeShader;
        [SerializeField] private Material flagMaterial;
        
        [Header("Option")]
        [SerializeField] private Vector2 size;
        [SerializeField] private Vector2Int resolution;
        [Space] 
        [SerializeField] private EMode mode;

        [SerializeField] private float amplitude, waveLength, speed;
        [Space]
        [SerializeField] private Vector2 textureMoveDir;
        [SerializeField] private float textureSpeed;

        private readonly PlaneGenerator m_Generator = new PlaneGenerator();
        private Mesh m_Mesh;
        private MeshRenderer m_Renderer;
        private Material currentMat;

        private ComputeBuffer m_VerticesBuffer;
        private ComputeBuffer m_NormalsBuffer;
        private ComputeBuffer m_UvsBuffer;
        private int m_KernelIndex;
        private uint m_ThreadGroupSize;

        private int m_BufferSize;

        private void Start()
        {
            m_Mesh = GetComponent<MeshFilter>().mesh;
            m_Renderer = GetComponent<MeshRenderer>();
            currentMat = m_Renderer.material;
            
            m_Generator.Generate(m_Mesh, size, resolution);

            m_BufferSize = m_Mesh.vertexCount;
            
            InitComputeShader();
        }

        private void InitComputeShader()
        {
            m_VerticesBuffer = new ComputeBuffer(m_BufferSize, sizeof(float) * 3);
            m_VerticesBuffer.SetData(m_Mesh.vertices);
            m_NormalsBuffer = new ComputeBuffer(m_BufferSize, sizeof(float) * 3);
            m_UvsBuffer = new ComputeBuffer(m_BufferSize, sizeof(float) * 2);
            m_UvsBuffer.SetData(m_Mesh.uv);

            m_KernelIndex = computeShader.FindKernel("SimulateFlag");
            computeShader.GetKernelThreadGroupSizes(m_KernelIndex, out m_ThreadGroupSize, out _, out _);
        }

        private void OnDestroy()
        {
            m_VerticesBuffer.Dispose();
            m_UvsBuffer.Dispose();
            m_NormalsBuffer.Dispose();
        }

        private void Update()
        {
            switch (mode)
            {
                case EMode.MonoBehaviour:
                    SimulateCPU();
                    break;
                case EMode.Compute:
                    if (m_Renderer.sharedMaterial != flagMaterial)
                        m_Renderer.sharedMaterial = flagMaterial;
                    SimulateCompute();
                    break;
            }

            if (mode == EMode.GPUShader && m_Renderer.sharedMaterial != flagMaterial)
            {
                m_Renderer.sharedMaterial = flagMaterial;
            }

            if (mode == EMode.GPUShader) return;
            
            if (m_Renderer.sharedMaterial != currentMat)
                m_Renderer.sharedMaterial = currentMat;
            
            m_Renderer.material.mainTextureOffset += textureMoveDir * (textureSpeed * Time.deltaTime);
        }

        private void SimulateCompute()
        {
            computeShader.SetBuffer(m_KernelIndex, "Vertices", m_VerticesBuffer);
            computeShader.SetBuffer(m_KernelIndex, "Normals", m_NormalsBuffer);
            computeShader.SetBuffer(m_KernelIndex, "UVs", m_UvsBuffer); 
            computeShader.SetFloat("Time", Time.time);
            computeShader.SetFloat("Amplitude", amplitude);
            computeShader.SetFloat("WaveLength", waveLength);
            computeShader.SetFloat("Speed", speed);

            var threadGroups = (int)(m_BufferSize / m_ThreadGroupSize);
            computeShader.Dispatch(m_KernelIndex, threadGroups, 1, 1);

            var temp = new Vector3[m_BufferSize];
            m_VerticesBuffer.GetData(temp);
            m_Mesh.vertices = temp;
            m_NormalsBuffer.GetData(temp);
            m_Mesh.normals = temp;
        }

        private void SimulateCPU()
        {
            var vertices = m_Mesh.vertices;
            var normals = m_Mesh.normals;
            
            for (var i = 0; i < vertices.Length; i++)
            {
                var k = 2 * Mathf.PI / waveLength;
                var f = k * (vertices[i].x - speed * Time.time);
                vertices[i].y = -m_Mesh.uv[i].x * amplitude * Mathf.Sin(f);

                var tangent = Vector3.Normalize(new Vector3(1, k * amplitude * Mathf.Cos(f), 0));
                normals[i] = new Vector3(-tangent.y, tangent.x, 0);
            }

            m_Mesh.vertices = vertices;
            m_Mesh.normals = normals;
        }
    }
}
