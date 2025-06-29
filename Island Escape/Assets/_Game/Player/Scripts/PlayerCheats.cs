using IngameDebugConsole;
using UnityEngine;

namespace Core.Players
{
    public class PlayerCheats : MonoBehaviour
    {
        private void Start()
        {
            DebugLogConsole.AddCommand<string>("Player.Local.ChangeNickname", "Change nickname for local player", ChangeNickname);
        }

        private void ChangeNickname(string newNickname)
        {
            Player.Local.Nickname = newNickname;

            Debug.Log($"Nickname was set to {newNickname}");
        }
    }
}
