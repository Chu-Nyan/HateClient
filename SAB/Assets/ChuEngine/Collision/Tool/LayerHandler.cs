using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Chu.Collision
{
    [CreateAssetMenu(fileName = "LayerHandler", menuName = "Scriptable Objects/LayerHandler", order = 1)]
    public class LayerHandler : ScriptableObject
    {
        private const string _fileName = "NyanLayer.cs";
        private const string _outputPath = "Assets/ChuEngine/Collision"; // TODO : 경로 하드 코딩 제거

        [SerializeField]
        private string[] _layer = new string[16];

        public bool TryVerify(out string log)
        {
            var names = new HashSet<string>();
            bool isIntegrity = true;
            log = "저장 가능";

            for (int i = 0; i < _layer.Length; i++)
            {
                string name = _layer[i];

                if (string.IsNullOrEmpty(name))
                    continue;

                if (names.Add(name) == true)
                    continue;

                isIntegrity = false;
                log = $"중복 레이어 감지됨 : {i}번 : {name}";
            }

            return isIntegrity;
        }

        public string ConvertConstScript()
        {
            return string.Format(GetScriptTemplate(), GetField());
        }

        private string GetScriptTemplate()
        {
            var sb = new StringBuilder();
            sb.AppendLine("// NyanCollisionLayerHandler에 의해 자동 생성됨");
            sb.AppendLine("// 수동 수정 금지");
            sb.AppendLine();
            sb.AppendLine("namespace Chu.Collision");
            sb.AppendLine("{{");
            sb.AppendLine("\t public enum NyanLayer");
            sb.AppendLine("\t{{");
            sb.AppendLine("{0}");
            sb.AppendLine("\t}}");
            sb.AppendLine("}}");

            return sb.ToString();
        }

        private string GetField()
        {
            var lines = new List<string>();

            for (int i = 0; i < _layer.Length; i++)
            {
                if (string.IsNullOrEmpty(_layer[i]) == true)
                    continue;

                lines.Add($"\t\t{_layer[i]} = 1 << {i},");
            }

            return string.Join(Environment.NewLine, lines);
        }

        public bool GenerateFile(out string log)
        {
            try
            {
                if (Directory.Exists(_outputPath) == false)
                    Directory.CreateDirectory(_outputPath);

                var pathAndFile = Path.Combine(_outputPath, _fileName);
                File.WriteAllText(pathAndFile, ConvertConstScript());
                log = $"파일 생성 완료 \n경로 : {_outputPath} \n파일명 : {_fileName}";
                return true;
            }
            catch (Exception e)
            {
                log = $"파일 생성 실패\n{e.Message}";
                return false;
            }
        }
    }
}
