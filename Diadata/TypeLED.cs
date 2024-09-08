//括弧やセミコロン、ダブルクォーテーション、breakの忘れなどの数等に注意する。
//構文に関するコメントを書く。
//また、switch構文以外を弄らないこと。

namespace TatehamaATS.Diadata
{
    internal class TypeLED
    {
        public void ChengeTypeLED(string 列番)
        {
            switch (列番)
            {
                //通常列車
                case "1110A":
                    MainWindow.controlLED.overrideText = "C特1";
                    break;
                case "1017A":
                    MainWindow.controlLED.overrideText = "C特2-2";
                    break;
                case "回7191":
                case "回7291":
                case "回7190":
                case "回7290":
                    MainWindow.controlLED.overrideText = "回送-2";
                    break;

                //旧だんじり
                case "回1295":
                case "回1295X":
                case "回1281":
                case "回1294":
                case "回1294X":
                case "回1280":
                    MainWindow.controlLED.overrideText = "回送-2";
                    break;
                case "1295B":
                case "1295BX":
                case "1281B":
                    MainWindow.controlLED.overrideText = "だんじり急行";
                    break;
                case "1294KX":
                    MainWindow.controlLED.overrideText = "だんじり快急";
                    break;
                default:
                    MainWindow.controlLED.overrideText = null;
                    break;
            }
        }
    }
}
