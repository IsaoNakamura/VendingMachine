using System;
using System.Linq;

namespace VendingMachine
{
    /**
    * @brief 自動販売機アプリケーションクラス
    */
    class VendingMachineApp
    {
        /**
        * @brief メイン関数
        */
        static void Main(string[] args)
        {
            // 自動販売機を製造する
            VendingMachine machine = null;
            if (VendingMachine.createInstance(out machine) != 0)
            {
                Console.WriteLine("自動販売機の製造時に致命的なエラーが発生しました。");
                return;
            }

            // ドリンク入れる本数
            const int stock_num = 3;

            // 製品ラインナップ (ドリンク種類,価格)
            Tuple<string, int>[] lineup = {
                    new Tuple<string, int>("水", 100),
                    new Tuple<string, int>("コーラ", 150),
                    new Tuple<string, int>("お茶", 130)
                };

            // サポートする貨幣を設定
            int[] supportMoneyType = { 500, 100, 10, 50 };

            // 自動販売機を整備する
            if (machine.maintain(stock_num, lineup, supportMoneyType, false, false) != 0)
            {
                Console.WriteLine("自動販売機の整備時に致命的なエラーが発生しました。");
                return;
            }

            // 自動販売機を運用する
            if (machine.mainLoop() != 0)
            {
                Console.WriteLine("自動販売機の運用時に致命的なエラーが発生しました。");
                return;
            }

            Console.WriteLine("自動販売機を拡張します。");

            // 自動販売機を工場出荷状態に戻す
            if (machine.reset() != 0)
            {
                Console.WriteLine("自動販売機を工場出荷状態に戻す際に致命的なエラーが発生しました。");
                return;
            }

            // サポートする貨幣に1000円を追加
            int[] supportMoneyTypeEx = {1000, 500, 100, 10, 50 };

            // 自動販売機を拡張整備する(おみくじ機能と追加購入機能をON)
            if (machine.maintain(stock_num, lineup, supportMoneyTypeEx, true, true) != 0)
            {
                Console.WriteLine("自動販売機の拡張整備時に致命的なエラーが発生しました。");
                return;
            }

            // 自動販売機を拡張運用する
            if (machine.mainLoop() != 0)
            {
                Console.WriteLine("自動販売機の拡張運用時に致命的なエラーが発生しました。");
                return;
            }

        }

    }
}
