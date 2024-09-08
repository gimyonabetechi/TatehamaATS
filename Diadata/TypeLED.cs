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

                //新だんじり
                case "回7181":
                case "回7281":
                case "回1195":
                case "回7283":
                case "回1181":
                case "回1181X":
                case "回1281":
                case "回1281X":
                    MainWindow.controlLED.overrideText = "回送-2";
                    break;
                case "7180C":
                case "7282C":
                case "1180C":
                case "1280C":
                    MainWindow.controlLED.overrideText = "だんじり準急" :
                    break;
                case "7281B":
                case "1195B":
                case "1181B":
                case "1281B":
                    MainWindow.controlLED.overrideText = "だんじり急行";
                    break;
                case "6981K":
                case "7280K":
                case "7185K":
                case "1194K":
                case "7083K":
                    MainWindow.controlLED.overrideText = "だんじり快急";
                    break;

                default:
                    MainWindow.controlLED.overrideText = null;
                    break;
            }
        }
    }
}
