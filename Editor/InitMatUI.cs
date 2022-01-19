using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yokra9.MaterialSetupUtils
{
    /// <summary>
    /// マテリアルの設定をコピーするやつの GUI
    /// </summary>
    public class InitMatUI : EditorWindow
    {

        /// <summary>
        /// コピー元マテリアル
        /// </summary>
        public Material Source = null;

        /// <summary>
        /// 添え字のプロパティをコピーするか？
        /// </summary>
        public bool[] IsCopyProperty = new bool[0];

        /// <summary>
        /// シェーダプロパティの数
        /// </summary>
        private int propertyCount = 0;

        /// <summary>
        /// スクロールバーの位置
        /// </summary>
        private Vector2 _scrollPosition = Vector2.zero;

        /// <summary>
        /// プロパティをフィルタする文字列
        /// </summary>
        private string _filterText = "";

        /// <summary>
        /// ウィンドウの表示
        /// </summary>
        [MenuItem("Tools/MaterialSetupUtils")]
        static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<InitMatUI>();
            window.titleContent = new GUIContent("MaterialSetupUtils");
        }

        /// <summary>
        /// レンダリングと GUI イベントのハンドリング
        /// </summary>
        void OnGUI()
        {
            /*
              ソースの指定
             */
            EditorGUILayout.LabelField("Source", EditorStyles.boldLabel);
            Source = (Material)EditorGUILayout.ObjectField("Source Material", Source, typeof(Material), true);
            if (Source == null) return;

            /*
              シェーダの取得
             */
            EditorGUILayout.LabelField("Shader", Source.shader.name, EditorStyles.helpBox);
            EditorGUILayout.Space();

            /*
              コピーするプロパティの指定
            */
            {
                EditorGUILayout.LabelField("Property", EditorStyles.boldLabel);

                // プロパティをフィルタする文字列を入力
                _filterText = EditorGUILayout.TextField("Filter (Prefix)", _filterText);

                // シェーダプロパティ数の確認
                propertyCount = Source.shader.GetPropertyCount();
                Array.Resize(ref IsCopyProperty, propertyCount);

                // シェーダプロパティ一覧
                using (new EditorGUILayout.VerticalScope("box"))
                {
                    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                    for (int i = 0; i < propertyCount; i++)
                    {
                        string propertyName = Source.shader.GetPropertyName(i);
                        string propertyDescription = Source.shader.GetPropertyDescription(i);
                        string label = propertyDescription == "" ? propertyName : propertyDescription;

                        // ラベルとフィルタ文字列が前方一致しなかったら表示をスキップ
                        if (!label.StartsWith(_filterText)) continue;

                        // シェーダプロパティをコピーするか？
                        IsCopyProperty[i] = EditorGUILayout.ToggleLeft(label, IsCopyProperty[i]);
                    }
                    EditorGUILayout.EndScrollView();
                }
            }

            /*
              便利ボタン
            */
            using (new GUILayout.HorizontalScope())
            {
                // 一括設定
                if (GUILayout.Button("All")) setAllProperty(true);
                if (GUILayout.Button("Clear")) setAllProperty(false);

                // 保存と読込
                if (GUILayout.Button("Save")) new Settings(IsCopyProperty).save();
                if (GUILayout.Button("Load")) IsCopyProperty = Settings.load().IsCopyProperty;
            }

            /*
              コピー実行
            */
            if (GUILayout.Button("Copy"))
            {
                InitMaterial initMaterial = new InitMaterial(Source, IsCopyProperty);

                // 選択中のマテリアルに対してコピー処理を実行
                foreach (var mat in Selection.objects.OfType<Material>())
                {
                    // コピー関数実行
                    initMaterial.SetUp(mat);
                }
            }
        }

        /// <summary>
        /// 一括設定関数
        /// </summary>
        /// <param name="isCopyProperty">プロパティをコピーするか？</param>
        private void setAllProperty(bool isCopyProperty)
        {
            for (int i = 0; i < propertyCount; i++)
            {
                IsCopyProperty[i] = isCopyProperty;
            }
        }
    }
}