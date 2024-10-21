using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{

    [SerializeField] private Button button; // インスペクタへの表示をするためSerializeFieldを付加

    private void Start()
    {
        button.onClick.AddListener(OnClick); // ボタンクリック時に実行される関数を指定
    }

    public void OnClick()
    {
        SceneManager.LoadScene(SceneNames.HOME); // ホーム画面への遷移
    }
}
