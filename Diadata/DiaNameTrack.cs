//括弧やセミコロン、ダブルクォーテーション、breakの忘れなどの数等に注意する。
//構文に関するコメントを書く。
//また、switch構文以外を弄らないこと。

namespace TatehamaATS.Diadata
{
    internal class DiaNameTrack
    {
        public void ChengeTrack(string 列番)
        {
            switch (列番)
            {
                case "1017A":
                    //元シナリオ：　1107A 　Ｃ特１　大道寺　→館　浜　／内容：着線２番振替
                    TrainState.RouteDatabase.CircuitList[35].ChengeName("館浜下り場内1LB");
                    break;
                case "回1295":
                case "1295B":
                case "回1295X":
                case "1295BX":
                case "回1281":
                case "1281B":
                    //元シナリオ：回 607A 　回送　　江ノ原検→館　浜　／内容：着線２番振替 
                    TrainState.RouteDatabase.CircuitList[34].ChengeName("館浜下り場内1LB");
                    break;
                case "回1294":
                case "回1294X":
                case "回1280":
                case "1294KX":
                case "回7190":
                case "回7290":
                case "6981K":
                    //元シナリオ：館　浜　発車シナリオ共通　　　　　　／内容：着線２番振替    
                    TrainState.RouteDatabase.CircuitList[0].ChengeName("館浜下り場内1LB");
                    TrainState.RouteDatabase.CircuitList[1].ChengeName("館浜上り出発2R");
                    break;
            }
        }
    }
}