// MyGrassLogic.hlsl
#ifndef MY_GRASS_LOGIC_INCLUDED
#define MY_GRASS_LOGIC_INCLUDED

StructuredBuffer<float3> PositionBuffer;
StructuredBuffer<float3> RotationBuffer;
StructuredBuffer<float3> ScaleBuffer;

void GetInstancedPosition_float(float instanceID, out float3 OutPosition)
{
    // 입력받은 instanceID를 정수로 변환
    uint id = (uint) round(instanceID);
    
    // 데이터 가져오기
    OutPosition = PositionBuffer[id];
}
#endif