using UnityEngine;
using UnityEngine.Rendering;

namespace Yokra9.MaterialSetupUtils
{
    /// <summary>
    /// マテリアルの設定をコピーするやつ
    /// </summary>
    public class InitMaterial
    {
        /// <summary>
        /// コピー元マテリアル
        /// </summary>
        private Material Source;

        /// <summary>
        /// 添え字のプロパティをコピーするか？
        /// </summary>
        private bool[] IsCopyProperty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="source">コピー元マテリアル</param>
        /// <param name="isCopyProperty">添え字のプロパティをコピーするか？</param>
        public InitMaterial(Material source, bool[] isCopyProperty)
        {
            Source = source;
            IsCopyProperty = isCopyProperty;
        }

        /// <summary>
        /// マテリアルの設定をコピーする関数
        /// </summary>
        /// <param name="mat">対象マテリアル</param>
        public Material SetUp(Material mat)
        {
            // コピー元と同じシェーダを設定
            mat.shader = Source.shader;

            // シェーダプロパティのコピー
            for (int i = 0; i < Source.shader.GetPropertyCount(); i++)
            {
                if (IsCopyProperty[i])
                {
                    string propertyName = Source.shader.GetPropertyName(i);
                    ShaderPropertyType propertyType = Source.shader.GetPropertyType(i);

                    switch (propertyType)
                    {
                        case ShaderPropertyType.Color:
                            mat.SetColor(propertyName, Source.GetColor(propertyName));
                            break;
                        case ShaderPropertyType.Vector:
                            mat.SetVector(propertyName, Source.GetVector(propertyName));
                            break;
                        case ShaderPropertyType.Float:
                            mat.SetFloat(propertyName, Source.GetFloat(propertyName));
                            break;
                        case ShaderPropertyType.Range:
                            // holds a floating number value in a certain range.
                            mat.SetFloat(propertyName, Source.GetFloat(propertyName));
                            break;
                        case ShaderPropertyType.Texture:
                            mat.SetTexture(propertyName, Source.GetTexture(propertyName));
                            break;
                    }
                }
            }

            return mat;
        }
    }
}
