using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VersusPlayerButton : MonoBehaviour
{
    [SerializeField] private Button button; // インスペクタへの表示をするためSerializeFieldを付加

    void Start()
    {
        button.onClick.AddListener(this.onClicked); // ボタンクリック時に実行される関数を指定
    }

    void onClicked()
    {
        SceneManager.LoadScene(SceneNames.GAME); // ゲーム画面への遷移
    }
}
