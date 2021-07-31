using System;
using System.Windows.Input;

namespace MyExplorer
{
    public static class Keys
    {
        /// <summary>
        /// アプリケーションで有効な、キーによる操作内容
        /// </summary>
        public enum KeyEventType
        {
            FolderBack,
            FolderForward,
            Else
        }

        /// <summary>
        /// キー入力内容をアプリケーションイベント内容に変換する
        /// </summary>
        /// <param name="key"></param>
        /// <param name="systemKey"></param>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public static KeyEventType ToKeyEventType(Key key, Key systemKey, ModifierKeys modifier)
        {
            // 単押しの場合                  → キー情報は e.Key に入る
            // System キーとの同時押しの場合 → キー情報は e.SystemKey に入る
            KeyEventType keyEventType = ToKeyEventTypeConbination(systemKey, modifier);
            if (keyEventType != KeyEventType.Else)
            {
                return keyEventType;
            }

            return ToKeyEventTypeOneKey(key);
        }

        private static KeyEventType ToKeyEventTypeConbination(Key key, ModifierKeys modifier)
        {
            // Ctrl + Shift + 何か
            if (modifier == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                // nothing
            }

            // Ctrl + 何か
            if (modifier == ModifierKeys.Control)
            {
                // nothing
            }

            // Shift + 何か
            if (modifier == ModifierKeys.Shift)
            {
                // nothing
            }

            // Alt + 何か
            if (modifier == ModifierKeys.Alt)
            {
                // nothing
            }

            return KeyEventType.Else;
        }

        private static KeyEventType ToKeyEventTypeOneKey(Key key)
        {
            if (key == Key.Left)
            {
                return KeyEventType.FolderBack;
            }
            else if (key == Key.Right)
            {
                return KeyEventType.FolderForward;
            }

            return KeyEventType.Else;
        }
    }
}
