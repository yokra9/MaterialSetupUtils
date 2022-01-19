using System.IO;
using System.Xml.Serialization;

namespace Yokra9.MaterialSetupUtils
{
    /// <summary>
    /// マテリアルの設定をコピーするやつの設定
    /// </summary>
    [XmlType("Settings")]
    public class Settings
    {
        /// <summary>
        /// 添え字のプロパティをコピーするか？
        /// </summary>
        [XmlArray("IsCopyProperties")]
        [XmlArrayItem("IsCopyProperty")]
        public bool[] IsCopyProperty;

        /// <summary>
        /// 設定ファイルの保存先
        /// </summary>
        private const string PATH = "Assets/MaterialSetupUtils/Data/Settings.xml";

        /// <summary>
        /// デフォルトのコンストラクタ（デシリアライザ用）
        /// </summary>
        public Settings() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="isCopyProperty">添え字のプロパティをコピーするか？</param>
        public Settings(bool[] isCopyProperty)
        {
            IsCopyProperty = isCopyProperty;
        }

        /// <summary>
        /// 設定の読込関数
        /// </summary>
        public static Settings load()
        {
            Settings settings;
            using (var fs = new FileStream(PATH, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                settings = (Settings)serializer.Deserialize(fs);
            }

            return settings;
        }

        /// <summary>
        /// 設定の保存関数
        /// </summary>
        public void save()
        {
            using (var fs = new FileStream(PATH, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                serializer.Serialize(fs, this);
            }
        }
    }
}