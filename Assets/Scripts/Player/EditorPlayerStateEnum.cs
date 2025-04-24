using Player;
using UnityEditor;

public class PlayerStateEnumFlagsField : EditorWindow
{
    PlayerStateEnums.PlayerState m_Flags;

    [MenuItem("Player/PlayerStateEnumFlagsField")]
    static void OpenWindow()
    {
        GetWindow<PlayerStateEnumFlagsField>().Show();
    }

    void OnGUI()
    {
        m_Flags = (PlayerStateEnums.PlayerState)EditorGUILayout.EnumFlagsField(m_Flags);
    }
}
