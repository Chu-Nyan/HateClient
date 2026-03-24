using Chu.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GrassRenderer : MonoBehaviour
{
    [SerializeField]
    private Terrain _terrain;
    [SerializeField]
    private Material _grassMaterial;
    [SerializeField]
    private Mesh _grassMesh;
    [SerializeField]
    private string _seed;
    [SerializeField]
    private float _grassMinDistance;
    [SerializeField]
    [Range(1, 255)]
    private int _densityWeight;

    private ComputeBuffer _positionBuffer;
    private ComputeBuffer _rotationBuffer;
    private ComputeBuffer _gpuBuffer;

    private Bounds _drawArea;

    private void Awake()
    {
        // TODO : Compute Shader로 오브젝트 컬링
        var mapSize = new Vector2(_terrain.terrainData.size.x, _terrain.terrainData.size.z);
        var merge = GetGrassPosition(_seed, _grassMinDistance, mapSize, _terrain);
        var rotations = GenerateRandomRotation(_seed);
        uint[] args = new uint[] { _grassMesh.GetIndexCount(0), (uint)merge.Count, 0, 0, 0 };

        _gpuBuffer = new ComputeBuffer(1, args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
        _gpuBuffer.SetData(args);
        _positionBuffer = new(merge.Count, sizeof(float) * 3);
        _positionBuffer.SetData(merge);
        _rotationBuffer = new(rotations.Length, sizeof(float));
        _rotationBuffer.SetData(rotations);

        _grassMaterial.SetBuffer("PositionBuffer", _positionBuffer);
        _grassMaterial.SetBuffer("RotationBuffer", _rotationBuffer);

        _drawArea = new Bounds(Vector2.zero, _terrain.terrainData.size * 2);
    }

    private void OnRenderObject()
    {
        Graphics.DrawMeshInstancedIndirect(_grassMesh, 0, _grassMaterial, _drawArea, _gpuBuffer);
    }

    private List<Vector3> GetGrassPosition(string seed, float distance, Vector2 size, Terrain terrain)
    {
        var data = terrain.terrainData;
        var grassSampling = PoissonDiscSampling.GeneratePoints(seed, distance, size);
        var densityArr = data.GetDetailLayer(0, 0, data.detailWidth, data.detailHeight, 0); // 정보
        var result = new List<Vector3>(128);
        var random = new System.Random(seed.GetHashCode());

        for (int i = 0; i < grassSampling.Count; i++)
        {
            Vector2 sampling = grassSampling[i];
            int x = (int)(sampling.x / data.size.x * data.detailWidth);
            int z = (int)(sampling.y / data.size.z * data.detailHeight);

            if (densityArr[z, x] <= 0)
                continue;
            if (random.NextDouble() > densityArr[z, x] / _densityWeight)
                continue;

            Vector3 worldPos = new(sampling.x, 0f, sampling.y);
            worldPos.y = terrain.SampleHeight(worldPos);

            result.Add(worldPos);
        }

        return result;
    }

    private float[] GenerateRandomRotation(string seed)
    {
        var random = new System.Random(seed.GetHashCode());
        float[] rotations = new float[128];

        for (int i = 0; i < rotations.Length; i++)
        {
            rotations[i] = (float)random.NextDouble() * 360f;
        }

        return rotations;
    }

    private void OnDestroy()
    {
        _positionBuffer?.Release();
        _gpuBuffer?.Release();
    }
}
